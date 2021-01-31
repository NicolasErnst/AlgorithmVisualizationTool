using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public static class DOTParser<V, E> where V : class, IVertex, new() where E : Edge<V>, new()
    {
        public static string VertexName()
        {
            return @"([A-Za-z0-9_][A-Za-z0-9_\s]*)";
        }

        public static string VertexGroup()
        {
            return @"{\s*" + VertexName() + @"(,\s*" + VertexName() + @")*}"; 
        }

        public static string PropertyList()
        {
            return "\\[\\s*(\\w+\\s*=\\s*\"[^\"]*\"(,\\s*)?)*\\s*\\]";
        }

        public static string KeyValuePairs()
        {
            return "(\\w+)\\s*=\\s*\"([^\"]*)\"";
        }

        public static string VertexDefinition()
        {
            return @"(" + VertexName() + @"|" + VertexGroup() + @")\s*(" + PropertyList() + @")?\s*"; 
        }

        public static string DirectedEdgeDefinition()
        {
            return @"(" + VertexName() + @"|" + VertexGroup() + @")\s*->\s*(" + VertexName() + @"|" + VertexGroup() + @")(\s*->\s*(" + VertexName() + @"|" + VertexGroup() + @"))*";
        }

        public static string UndirectedEdgeDefinition()
        {
            return @"(" + VertexName() + @"|" + VertexGroup() + @")\s*--\s*(" + VertexName() + @"|" + VertexGroup() + @")(\s*--\s*(" + VertexName() + @"|" + VertexGroup() + @"))*";
        }

        public static string EdgeDefintion()
        {
            return @"(" + DirectedEdgeDefinition() + @"|" + UndirectedEdgeDefinition() + @")(" + PropertyList() + @")?";
        }

        public static string Statement()
        {
            return @"^(" + VertexDefinition() + @"|" + EdgeDefintion() + @")?\s*;$";
        }

        public static DOTParsingResult Parse(Graph<V, E> graph, IEnumerable<string> statements)
        { 
            graph.DirectionType = GraphDirectionType.None;
            bool onlyUndirectedEdgeDefinitions = true; 

            for (int i = 0; i < statements.Count(); i++)
            {
                string statement = statements.ElementAt(i).Trim(); 
                if (IsFullMatch(statement, Statement()))
                {
                    statement = Regex.Match(statement, @"[^;]*").Value;
                    IDictionary<string, string> propertyList = new Dictionary<string, string>();
                    if (Regex.Match(statement, PropertyList()).Success)
                    {
                        propertyList = ParsePropertyList(statement);
                        statement = statement.Replace(Regex.Match(statement, PropertyList()).Value, "");
                    }

                    if (IsFullMatch(statement, VertexDefinition()))
                    {
                        try
                        {
                            IList<V> vertices = ParseVertices(graph, statement, false); 
                            foreach(V vertex in vertices)
                            {
                                vertex.OnInitialize(propertyList);
                                graph.AddVertex(vertex);
                            }
                        } catch (Exception e)
                        {
                            return new DOTParsingResult(false, e.Message, i + 1); 
                        }
                    }
                    else if (IsFullMatch(statement, EdgeDefintion()))
                    {
                        string delimiter = "";

                        if (IsFullMatch(statement, UndirectedEdgeDefinition()))
                        {
                            delimiter = "--"; 
                        }
                        else if (IsFullMatch(statement, DirectedEdgeDefinition()))
                        {
                            onlyUndirectedEdgeDefinitions = false;
                            delimiter = "->";
                        }
                        else
                        {
                            return new DOTParsingResult(false, "The specified edge definition is neither directed nor undirected.", i + 1); 
                        }

                        string[] verticesOfEdge = statement.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < verticesOfEdge.Length - 1; j++)
                        {
                            IList<V> sourceVertices = ParseVertices(graph, verticesOfEdge[j], true);
                            IList<V> targetVertices = ParseVertices(graph, verticesOfEdge[j + 1], true);

                            foreach (V sourceVertex in sourceVertices)
                            {
                                if (!graph.Vertices.Contains(sourceVertex))
                                {
                                    sourceVertex.OnInitialize(new Dictionary<string, string>());
                                    graph.AddVertex(sourceVertex); 
                                }

                                foreach (V targetVertex in targetVertices)
                                {
                                    if (!graph.Vertices.Contains(targetVertex))
                                    {
                                        targetVertex.OnInitialize(new Dictionary<string, string>());
                                        graph.AddVertex(targetVertex);
                                    }

                                    E existingEdge = graph.Edges.Where(x => x.Source == sourceVertex && x.Target == targetVertex).FirstOrDefault();
                                    if (existingEdge == null)
                                    {
                                        E edge = (E)Activator.CreateInstance(typeof(E), sourceVertex, targetVertex);
                                        edge.OnInitialize(propertyList);
                                        graph.AddEdge(edge);
                                    }

                                    if (!graph.IsDirected)
                                    {
                                        E invertedExistingEdge = graph.Edges.Where(x => x.Source == targetVertex && x.Target == sourceVertex).FirstOrDefault();
                                        if (invertedExistingEdge == null)
                                        {
                                            E edge = (E)Activator.CreateInstance(typeof(E), targetVertex, sourceVertex);
                                            edge.OnInitialize(propertyList);
                                            graph.AddEdge(edge);
                                        }
                                    }
                                }
                            }
                        }
                    }
                } 
                else
                {
                    if (graph.VertexCount > 0)
                    {
                        if (onlyUndirectedEdgeDefinitions)
                        {
                            graph.DirectionType = GraphDirectionType.Undirected;
                        }
                        else
                        {
                            graph.DirectionType = GraphDirectionType.Directed;
                        }
                    }
                    return new DOTParsingResult(false, statement, i + 1); 
                }
            }

            if (graph.VertexCount > 0)
            {
                if (onlyUndirectedEdgeDefinitions)
                {
                    graph.DirectionType = GraphDirectionType.Undirected;
                }
                else
                {
                    graph.DirectionType = GraphDirectionType.Directed;
                }
            }
            return new DOTParsingResult(true);
        }

        public static bool IsFullMatch(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern); 
            if (match.Success && match.Value.Length == input.Length)
            {
                return true; 
            }

            return false;
        }

        public static IDictionary<string, string> ParsePropertyList(string statement)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>(); 

            Match propertyListMatch = Regex.Match(statement, PropertyList());
            if (propertyListMatch.Success)
            {
                string propertyList = propertyListMatch.Value;
                Match keyValuePairs = Regex.Match(propertyList, KeyValuePairs());
                for (int i = 0; i < keyValuePairs.Groups.Count - 1; i += 3)
                {
                    string key = keyValuePairs.Groups[i + 1].Value.Trim();
                    string value = keyValuePairs.Groups[i + 2].Value.Trim();
                    properties.Add(key, value);
                }
            }

            return properties; 
        }

        public static IList<V> ParseVertices(Graph<V, E> graph, string statement, bool allowForReferencing)
        {
            List<V> vertices = new List<V>();
            DOTParsingResult result = new DOTParsingResult(true); 

            MatchCollection vertexNames = Regex.Matches(statement, VertexName());
            foreach (Match vertexNameMatch in vertexNames)
            {
                string vertexName = vertexNameMatch.Value.Trim();
                V existingVertex = graph.Vertices.Where(x => x.VertexName.Equals(vertexName)).FirstOrDefault();
                if (existingVertex != null && allowForReferencing)
                {
                    vertices.Add(existingVertex); 
                }
                else if (existingVertex != null && !allowForReferencing)
                {
                    throw new Exception("A vertex with the name " + vertexName + " already exists.");
                }
                else
                {
                    V vertex = new V() { VertexName = vertexName };
                    vertices.Add(vertex);
                }               
            }

            return vertices;
        }
    }
}
