using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GraphAlgorithmPlugin;

namespace StrongConnectedComponentsPlugin
{
    public class SCCPlugin : GraphAlgorithmPlugin<SCCVertex, Edge<SCCVertex>>
    {
        public override string AlgorithmName => "Strong Connected Components";

        public override GraphDirectionType CompatibleGraphDirections => GraphDirectionType.Directed;

        private readonly ExposableList publicL = new ExposableList("L");
        private readonly ExposableList publicQ = new ExposableList("Q");

        protected override async Task RunAlgorithm(SCCVertex startVertex)
        {
            ExposedLists.Clear();
            publicQ.Clear();
            publicL.Clear();
            ExposedLists.Add(publicQ);
            ExposedLists.Add(publicL);

            try
            {
                Stack<SCCVertex> l = new Stack<SCCVertex>();
                int u = 0;
                Progress = 0;
                ProgressText = "Initializing...";

                u = await RunDFS(startVertex, u, l);
                foreach (SCCVertex s in Graph.Vertices)
                {
                    if (!s.Marked)
                    {
                        u = await RunDFS(s, u, l);
                    }
                }

                List<Edge<SCCVertex>> edges = new List<Edge<SCCVertex>>(Graph.Edges);
                GraphLayout.KeepPositions = true;
                await MakeAlgorithmStep(() =>
                {
                    foreach (SCCVertex s in Graph.Vertices)
                    {
                        s.VertexContent = "";
                        s.Marked = false;
                        Graph.ClearEdges(s);
                    }
                    Graph.AddEdgeRange(edges.Select(x => new Edge<SCCVertex>(x.Target, x.Source)));
                    ProgressText = "Invert edges...";
                    Progress = ((2 * Graph.Vertices.Count() + 1) / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                }, () =>
                {
                    GraphLayout.KeepPositions = true;
                    foreach (SCCVertex s in Graph.Vertices)
                    {
                        s.UpdateLayout();
                        s.Marked = true;
                        s.UpdateContent();
                        Graph.ClearEdges(s);
                    }
                    Graph.AddEdgeRange(edges);
                    ProgressText = "DFS...";
                    Progress = (2 * Graph.Vertices.Count() / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                });
                GraphLayout.KeepPositions = false;

                int sccID = 1;
                u = 0;
                while (l.Count > 0)
                {
                    SCCVertex s = l.Pop();

                    if (!s.Marked)
                    {
                        u = await IdentifySCCs(s, sccID, u);
                        sccID += 1;
                    } else
                    {
                        await MakeAlgorithmStep(() =>
                        {
                            publicL.Remove(s);
                        }, () =>
                        {
                            publicL.Insert(0, s);
                        });
                    }
                }
            }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
        }

        private void GetDescendants(SCCVertex vertex, Dictionary<SCCVertex, HashSet<SCCVertex>> descendants)
        {
            if (!descendants.ContainsKey(vertex))
            {
                descendants.Add(vertex, new HashSet<SCCVertex>());
            }
            foreach (SCCVertex descendantVertex in Graph.OutEdges(vertex).Select(x => x.Target))
            {
                descendants[vertex].Add(descendantVertex);
            }
        }

        private async Task<int> RunDFS(SCCVertex startVertex, int u, Stack<SCCVertex> l)
        {
            Stack<SCCVertex> q = new Stack<SCCVertex>();
            Dictionary<SCCVertex, HashSet<SCCVertex>> descendantVertices = new Dictionary<SCCVertex, HashSet<SCCVertex>>();
            u += 1;

            await MakeAlgorithmStep(() =>
            {
                q.Push(startVertex);
                publicQ.Insert(0, startVertex);
                startVertex.Marked = true;
                GetDescendants(startVertex, descendantVertices);
                startVertex.PushTime = u;
                Progress = (u / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                ProgressText = "DFS...";
            }, () =>
            {
                q.Pop();
                publicQ.Remove(startVertex);
                startVertex.Marked = false;
                descendantVertices.Remove(startVertex);
                startVertex.PushTime = 0;
                Progress = 0;
                ProgressText = "Initializing...";
            });

            while (q.Count > 0)
            {
                SCCVertex v = q.Peek();
                if (descendantVertices.ContainsKey(v) && descendantVertices[v].Count > 0)
                {
                    SCCVertex w = descendantVertices[v].First();
                    descendantVertices[v].Remove(w);
                    if (!w.Marked)
                    {
                        await MakeAlgorithmStep(() =>
                        {
                            q.Push(w);
                            publicQ.Insert(0, w);
                            w.Marked = true;
                            GetDescendants(w, descendantVertices);
                            u += 1;
                            w.PushTime = u;
                            Progress = (u / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                        }, () =>
                        {
                            q.Pop();
                            publicQ.Remove(w);
                            w.Marked = false;
                            descendantVertices.Remove(w);
                            u -= 1;
                            w.PushTime = 0;
                            Progress = (u / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                        });
                    }
                }
                else
                {
                    await MakeAlgorithmStep(() =>
                    {
                        q.Pop();
                        publicQ.Remove(v);
                        l.Push(v);
                        publicL.Insert(0, v);
                        u += 1;
                        v.PopTime = u;
                        Progress = (u / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                    }, () =>
                    {
                        q.Push(v);
                        publicQ.Insert(0, v);
                        if (l.Count > 0)
                            l.Pop();
                        publicL.Remove(v);
                        u -= 1;
                        v.PopTime = 0;
                        Progress = (u / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                    });
                }
            }

            return u;
        }

        private async Task<int> IdentifySCCs(SCCVertex startVertex, int sccID, int u)
        {
            Stack<SCCVertex> q = new Stack<SCCVertex>();
            Dictionary<SCCVertex, HashSet<SCCVertex>> descendantVertices = new Dictionary<SCCVertex, HashSet<SCCVertex>>();
            u += 1;

            await MakeAlgorithmStep(() =>
            {
                q.Push(startVertex);
                publicQ.Insert(0, startVertex);
                publicL.Remove(startVertex);
                startVertex.Marked = true;
                GetDescendants(startVertex, descendantVertices);
                startVertex.SccID = sccID;
                ProgressText = "Identify SCCs...";
                Progress = ((2 * Graph.Vertices.Count() + 1 + u) / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
            }, () =>
            {
                if (q.Count > 0)
                    q.Pop();
                publicQ.Remove(startVertex);
                publicL.Insert(0, startVertex);
                startVertex.Marked = false;
                descendantVertices.Remove(startVertex);
                startVertex.SccID = 0;
                startVertex.VertexContent = "";
                ProgressText = "Invert edges...";
                Progress = ((2 * Graph.Vertices.Count() + u) / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
            });

            while (q.Count > 0)
            {
                SCCVertex v = q.Peek();
                if (descendantVertices.ContainsKey(v) && descendantVertices[v].Count > 0)
                {
                    SCCVertex w = descendantVertices[v].First();
                    descendantVertices[v].Remove(w);
                    if (!w.Marked)
                    {
                        await MakeAlgorithmStep(() =>
                        {
                            q.Push(w);
                            publicQ.Insert(0, w);
                            w.Marked = true;
                            GetDescendants(w, descendantVertices);
                            w.SccID = sccID;
                            u += 1;
                            Progress = ((2 * Graph.Vertices.Count() + 1 + u) / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                        }, () =>
                        {
                            q.Pop();
                            publicQ.Remove(w);
                            w.Marked = false;
                            descendantVertices.Remove(w);
                            w.SccID = 0;
                            w.VertexContent = "";
                            u -= 1;
                            Progress = ((2 * Graph.Vertices.Count() + 1 + u) / (3.0 * Graph.Vertices.Count() + 1.0)) * 100;
                        });
                    }
                }
                else
                {
                    await MakeAlgorithmStep(() =>
                    {
                        q.Pop();
                        publicQ.Remove(v);
                    }, () =>
                    {
                        q.Push(v);
                        publicQ.Insert(0, v);
                    });
                }
            }

            return u;
        }
    }
}
