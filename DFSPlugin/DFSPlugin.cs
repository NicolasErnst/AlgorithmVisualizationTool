using GraphAlgorithmPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFSPlugin
{
    public class DFSPlugin : GraphAlgorithmPlugin<DFSVertex, Edge<DFSVertex>>
    {
        public override string AlgorithmName => "Depth First Search";

        public override GraphDirectionType CompatibleGraphDirections => GraphDirectionType.Both;

        protected override async Task RunAlgorithm(DFSVertex startVertex)
        {
            try
            {
                Stack<DFSVertex> q = new Stack<DFSVertex>();
                Dictionary<DFSVertex, HashSet<DFSVertex>> descendantVertices = new Dictionary<DFSVertex, HashSet<DFSVertex>>();
                int u = 1;
                ExposableList exposedQ = new ExposableList("Q");
                ExposedLists.Add(exposedQ);

                await MakeAlgorithmStep(() =>
                {
                    q.Push(startVertex);
                    exposedQ.Insert(0, startVertex);
                    startVertex.Marked = true;
                    GetDescendants(startVertex, descendantVertices);
                    startVertex.PushTime = u;
                    Progress = (u / (Graph.VertexCount * 2.0)) * 100;
                    ProgressText = "Executing...";
                }, () =>
                {
                    q.Pop();
                    exposedQ.Remove(startVertex);
                    startVertex.Marked = false; 
                    descendantVertices.Remove(startVertex);
                    startVertex.PushTime = 0;
                    Progress = 0;
                    ProgressText = "Initializing...";
                });

                while(q.Count > 0)
                {
                    DFSVertex v = q.Peek(); 
                    if (descendantVertices.ContainsKey(v) && descendantVertices[v].Count > 0)
                    {
                        DFSVertex w = descendantVertices[v].First();
                        descendantVertices[v].Remove(w);
                        if (!w.Marked)
                        {
                            await MakeAlgorithmStep(() =>
                            {
                                q.Push(w);
                                exposedQ.Insert(0, w);
                                w.Marked = true;
                                GetDescendants(w, descendantVertices);
                                u += 1;
                                w.PushTime = u;
                                Progress = (u / (Graph.VertexCount * 2.0)) * 100;
                            }, () => 
                            {
                                q.Pop();
                                exposedQ.Remove(w);
                                w.Marked = false;
                                descendantVertices.Remove(w);
                                u -= 1;
                                w.PushTime = 0;
                                Progress = (u / (Graph.VertexCount * 2.0)) * 100;
                            });
                        }
                    }
                    else
                    {
                        await MakeAlgorithmStep(() =>
                        {
                            q.Pop();
                            exposedQ.Remove(v);
                            u += 1;
                            v.PopTime = u;
                            Progress = (u / (Graph.VertexCount * 2.0)) * 100;
                        }, () =>
                        {
                            q.Push(v);
                            exposedQ.Insert(0, v);
                            u -= 1;
                            v.PopTime = 0;
                            Progress = (u / (Graph.VertexCount * 2.0)) * 100;
                        });
                    }
                }
            } 
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
        }

        private void GetDescendants(DFSVertex vertex, Dictionary<DFSVertex, HashSet<DFSVertex>> descendants)
        {
            if (!descendants.ContainsKey(vertex))
            {
                descendants.Add(vertex, new HashSet<DFSVertex>()); 
            }
            foreach(DFSVertex descendantVertex in Graph.OutEdges(vertex).Select(x => x.Target))
            {
                descendants[vertex].Add(descendantVertex);
            }
        }
    }
}
