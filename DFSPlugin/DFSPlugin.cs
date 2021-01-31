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
        public override string GetAlgorithmName()
        {
            return "Depth First Search"; 
        }

        public override GraphDirectionType GetSupportedGraphDirections()
        {
            return GraphDirectionType.Both;
        }

        protected override async Task RunAlgorithm(DFSVertex startVertex)
        {
            try
            {
                Stack<DFSVertex> q = new Stack<DFSVertex>();
                ExposableList qList = new ExposableList("q");
                ExposedLists.Add(qList);
                Dictionary<DFSVertex, List<DFSVertex>> descendantNodes = new Dictionary<DFSVertex, List<DFSVertex>>(); 
                int u = 1;

                await MakeAlgorithmStep(() =>
                {
                    startVertex.PushTime = u; 
                    q.Push(startVertex);
                    qList.Insert(0, startVertex);
                    startVertex.Marked = true;
                    GetDescendantNodes(startVertex, descendantNodes);
                }, () =>
                {
                    startVertex.PushTime = 0; 
                    q.Pop();
                    qList.Remove(startVertex);
                    startVertex.Marked = false; 
                    if (descendantNodes.ContainsKey(startVertex))
                    {
                        descendantNodes.Remove(startVertex); 
                    }
                }); 

                while (q.Count > 0)
                {
                    DFSVertex v = q.Peek(); 
                    if (descendantNodes.ContainsKey(v) && descendantNodes[v].Count > 0)
                    {
                        DFSVertex w = descendantNodes[v].First();
                        bool wMarkedState = w.Marked;

                        await MakeAlgorithmStep(() =>
                        {
                            descendantNodes[v].Remove(w);
                            if (!wMarkedState)
                            {
                                q.Push(w);
                                qList.Insert(0, w);
                                w.Marked = true;
                                GetDescendantNodes(w, descendantNodes);
                                u += 1;
                                w.PushTime = u;
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
                                qList.Remove(w);
                                w.Marked = false;
                                if (descendantNodes.ContainsKey(w))
                                {
                                    descendantNodes.Remove(w);
                                }
                                u -= 1;
                                w.PushTime = 0; 
                            }
                        });
                    }
                    else
                    {
                        await MakeAlgorithmStep(() =>
                        {
                            q.Pop();
                            qList.Remove(v);
                            u += 1;
                            v.PopTime = u;
                        }, () =>
                        {
                            q.Push(v);
                            qList.Insert(0, v);
                            u -= 1;
                            v.PopTime = 0; 
                        }); 
                    }
                }
            } 
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
        }

        private void GetDescendantNodes(DFSVertex vertex, Dictionary<DFSVertex, List<DFSVertex>> descendantNodes)
        {
            if (!descendantNodes.ContainsKey(vertex))
            {
                descendantNodes.Add(vertex, new List<DFSVertex>()); 
            }
            descendantNodes[vertex].AddRange(Graph.OutEdges(vertex).Select(x => x.Target));
        }
    }
}
