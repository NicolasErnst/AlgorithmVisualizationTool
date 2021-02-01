using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public static class DOTParser
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

        public static bool IsFullMatch(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern); 
            if (match.Success && match.Value.Length == input.Length)
            {
                return true; 
            }

            return false;
        }

        public static DOTParsingResult IsDotStatement(string statement)
        {
            return IsDotStatement(new List<string> { statement }); 
        }

        public static DOTParsingResult IsDotStatement(IEnumerable<string> statements)
        {
            for (int i = 0; i < statements.Count(); i++)
            {
                string statement = statements.ElementAt(i).Trim();
                if (!DOTParser.IsFullMatch(statement, DOTParser.Statement()))
                {
                    return new DOTParsingResult(false, statement, i + 1);
                }
            }

            return new DOTParsingResult(true);
        }

        public static GraphDirectionType DetermineGraphDirection(IEnumerable<string> statements)
        {
            bool edgeDefinitionsAvailable = false;
            bool onlyUndirectedEdgeDefinitions = true;

            for (int i = 0; i < statements.Count(); i++)
            {
                string statement = statements.ElementAt(i).Trim();
                if (DOTParser.IsFullMatch(statement, DOTParser.Statement()))
                {
                    statement = Regex.Match(statement, @"[^;]*").Value;
                    if (Regex.Match(statement, DOTParser.PropertyList()).Success)
                    {
                        statement = statement.Replace(Regex.Match(statement, DOTParser.PropertyList()).Value, "");
                    }

                    if (DOTParser.IsFullMatch(statement, DOTParser.EdgeDefintion()))
                    {
                        edgeDefinitionsAvailable = true;
                        if (DOTParser.IsFullMatch(statement, DOTParser.DirectedEdgeDefinition()))
                        {
                            onlyUndirectedEdgeDefinitions = false;
                            break;
                        }
                    }
                }
            }

            if (edgeDefinitionsAvailable)
            {
                if (onlyUndirectedEdgeDefinitions)
                {
                    return GraphDirectionType.Undirected; 
                }
                else
                {
                    return GraphDirectionType.Directed; 
                }
            }
            return GraphDirectionType.None; 
        }
    }
}
