using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public class DOTGraphConverter<V, E> where V : class, IVertex, new() where E : Edge<V>, new()
    {
        public static DOTParsingResult Parse(Graph<V, E> graph, IEnumerable<string> statements)
        {
            graph.DirectionType = GraphDirectionType.None;

            for (int i = 0; i < statements.Count(); i++)
            {
                string statement = statements.ElementAt(i).Trim();
                if (DOTParser.IsFullMatch(statement, DOTParser.Statement()))
                {
                    statement = Regex.Match(statement, @"[^;]*").Value;
                    IDictionary<string, string> propertyList = new Dictionary<string, string>();
                    if (Regex.Match(statement, DOTParser.PropertyList()).Success)
                    {
                        propertyList = ParsePropertyList(statement);
                        statement = statement.Replace(Regex.Match(statement, DOTParser.PropertyList()).Value, "");
                    }

                    if (DOTParser.IsFullMatch(statement, DOTParser.VertexDefinition()))
                    {
                        try
                        {
                            IList<V> vertices = ParseVertices(graph, statement, false);
                            foreach (V vertex in vertices)
                            {
                                vertex.OnInitialize(propertyList);
                                graph.AddVertex(vertex);
                            }
                        }
                        catch (Exception e)
                        {
                            return new DOTParsingResult(false, e.Message, i + 1);
                        }
                    }
                    else if (DOTParser.IsFullMatch(statement, DOTParser.EdgeDefintion()))
                    {
                        string delimiter = "";
                        bool isUndirectedEdge = false; 

                        if (DOTParser.IsFullMatch(statement, DOTParser.UndirectedEdgeDefinition()))
                        {
                            if (graph.DirectionType != GraphDirectionType.Directed)
                            {
                                graph.DirectionType = GraphDirectionType.Undirected;
                            }
                            isUndirectedEdge = true;  
                            delimiter = "--";
                        }
                        else if (DOTParser.IsFullMatch(statement, DOTParser.DirectedEdgeDefinition()))
                        {
                            graph.DirectionType = GraphDirectionType.Directed; 
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

                                    if (isUndirectedEdge)
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
                    return new DOTParsingResult(false, statement, i + 1);
                }
            }

            return new DOTParsingResult(true);
        }

        public static IDictionary<string, string> ParsePropertyList(string statement)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            Match propertyListMatch = Regex.Match(statement, DOTParser.PropertyList());
            if (propertyListMatch.Success)
            {
                string propertyList = propertyListMatch.Value;
                Match keyValuePairs = Regex.Match(propertyList, DOTParser.KeyValuePairs());
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

            MatchCollection vertexNames = Regex.Matches(statement, DOTParser.VertexName());
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
