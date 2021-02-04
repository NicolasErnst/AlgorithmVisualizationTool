using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphAlgorithmPlugin;

namespace StrongConnectedComponentsPlugin
{
    public class SCCPlugin : GraphAlgorithmPlugin<SCCVertex, Edge<SCCVertex>>
    {
        public override string AlgorithmName => "Strong Connected Components";

        public override GraphDirectionType CompatibleGraphDirections => GraphDirectionType.Directed;

        protected override async Task RunAlgorithm(SCCVertex startVertex)
        {
            try
            {
                Progress = 0;
                ProgressText = "Initializing...";
                Stack<SCCVertex> l = new Stack<SCCVertex>();
                int u = 0;

                u = await RunDFS(startVertex, l, u);
                foreach (SCCVertex s in Graph.Vertices)
                {
                    if (!s.Marked)
                    {
                        u = await RunDFS(s, l, u);
                    }
                }

                List<Edge<SCCVertex>> edges = new List<Edge<SCCVertex>>(Graph.Edges);
                await MakeAlgorithmStep(() =>
                {
                    foreach (SCCVertex s in Graph.Vertices)
                    {
                        s.Marked = false;
                        s.VertexContent = "";
                        Graph.ClearEdges(s);
                    }
                    foreach (Edge<SCCVertex> edge in edges)
                    {
                        Graph.AddEdge(new Edge<SCCVertex>(edge.Target, edge.Source));
                    }
                }, () =>
                {
                    foreach (SCCVertex s in Graph.Vertices)
                    {
                        s.Marked = true;
                    }
                    foreach (Edge<SCCVertex> edge in edges)
                    {
                        Graph.AddEdge(edge);
                    }
                });

                int sccCounter = 1;
                while (l.Count > 0)
                {
                    SCCVertex s = l.Pop();
                    if (!s.Marked)
                    {
                        await IdentifyStrongConnectedComponents(s, sccCounter);
                        sccCounter++;
                    }
                }

                Progress = 100;
                ProgressText = "Finished!";
            }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
        }

        private async Task IdentifyStrongConnectedComponents(SCCVertex startVertex, int sccCounter)
        {
            Stack<SCCVertex> q = new Stack<SCCVertex>();
            Dictionary<SCCVertex, List<SCCVertex>> descendantNodes = new Dictionary<SCCVertex, List<SCCVertex>>();

            await MakeAlgorithmStep(() =>
            {
                startVertex.SccID = sccCounter;
                q.Push(startVertex);
                startVertex.Marked = true;
                GetDescendantNodes(startVertex, descendantNodes);
            }, () =>
            {
                startVertex.SccID = 0; 
                q.Pop();
                startVertex.Marked = false;
                if (descendantNodes.ContainsKey(startVertex))
                {
                    descendantNodes.Remove(startVertex);
                }
            });

            while (q.Count > 0)
            {
                SCCVertex v = q.Peek();
                if (descendantNodes.ContainsKey(v) && descendantNodes[v].Count > 0)
                {
                    SCCVertex w = descendantNodes[v].First();
                    bool wMarkedState = w.Marked;

                    await MakeAlgorithmStep(() =>
                    {
                        descendantNodes[v].Remove(w);
                        if (!wMarkedState)
                        {
                            q.Push(w);
                            w.Marked = true;
                            GetDescendantNodes(w, descendantNodes);
                            w.SccID = sccCounter;
                        }
                    }, () =>
                    {
                        if (!descendantNodes[v].Contains(w))
                        {
                            descendantNodes[v].Add(w);
                        }
                        if (!wMarkedState)
                        {
                            q.Pop();
                            w.Marked = false;
                            if (descendantNodes.ContainsKey(w))
                            {
                                descendantNodes.Remove(w);
                            }
                            w.SccID = 0;
                        }
                    });
                }
                else
                {
                    await MakeAlgorithmStep(() =>
                    {
                        q.Pop();
                    }, () =>
                    {
                        q.Push(v);
                    });
                }
            }
        }

        private async Task<int> RunDFS(SCCVertex startVertex, Stack<SCCVertex> l, int u)
        {
            Stack<SCCVertex> q = new Stack<SCCVertex>();
            Dictionary<SCCVertex, List<SCCVertex>> descendantNodes = new Dictionary<SCCVertex, List<SCCVertex>>();
            u += 1;

            await MakeAlgorithmStep(() =>
            {
                startVertex.PushTime = u;
                q.Push(startVertex);
                startVertex.Marked = true;
                GetDescendantNodes(startVertex, descendantNodes);
                //Progress = (u / (Graph.VertexCount * 2.0)) * 100.0; ;
                //ProgressText = "Executing...";
            }, () =>
            {
                startVertex.PushTime = 0;
                q.Pop();
                startVertex.Marked = false;
                if (descendantNodes.ContainsKey(startVertex))
                {
                    descendantNodes.Remove(startVertex);
                }
                //Progress = 0;
                //ProgressText = "Initializing";
            });

            while (q.Count > 0)
            {
                SCCVertex v = q.Peek();
                if (descendantNodes.ContainsKey(v) && descendantNodes[v].Count > 0)
                {
                    SCCVertex w = descendantNodes[v].First();
                    bool wMarkedState = w.Marked;

                    await MakeAlgorithmStep(() =>
                    {
                        descendantNodes[v].Remove(w);
                        if (!wMarkedState)
                        {
                            q.Push(w);
                            w.Marked = true;
                            GetDescendantNodes(w, descendantNodes);
                            u += 1;
                            w.PushTime = u;
                            //Progress = (u / (Graph.VertexCount * 2.0)) * 100.0; ;
                        }
                    }, () =>
                    {
                        if (!descendantNodes[v].Contains(w))
                        {
                            descendantNodes[v].Add(w);
                        }
                        if (!wMarkedState)
                        {
                            q.Pop();
                            w.Marked = false;
                            if (descendantNodes.ContainsKey(w))
                            {
                                descendantNodes.Remove(w);
                            }
                            u -= 1;
                            w.PushTime = 0;
                            //Progress = (u / (Graph.VertexCount * 2.0)) * 100.0; ;
                        }
                    });
                }
                else
                {
                    await MakeAlgorithmStep(() =>
                    {
                        q.Pop();
                        l.Push(v);
                        u += 1;
                        v.PopTime = u;
                        //Progress = (u / (Graph.VertexCount * 2.0)) * 100.0;
                    }, () =>
                    {
                        q.Push(v);
                        l.Pop();
                        u -= 1;
                        v.PopTime = 0;
                        //Progress = (u / (Graph.VertexCount * 2.0)) * 100.0;
                    });
                }
            }

            return u; 

            //Progress = 100;
            //ProgressText = "Finished!";
        }

        private void GetDescendantNodes(SCCVertex vertex, Dictionary<SCCVertex, List<SCCVertex>> descendantNodes)
        {
            if (!descendantNodes.ContainsKey(vertex))
            {
                descendantNodes.Add(vertex, new List<SCCVertex>());
            }
            descendantNodes[vertex].AddRange(Graph.OutEdges(vertex).Select(x => x.Target));
        }
    }
}
