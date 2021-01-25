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
            return @"(" + VertexName() + @"|" + VertexGroup() + @")(" + PropertyList() + @")?"; 
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
            return @"^(" + VertexDefinition() + @"|" + EdgeDefintion() + @")\s*;$";
        }

        public static bool Parse(Graph<V, E> graph, IEnumerable<string> statements)
        {
            foreach (string statement in statements)
            {
                if (!IsFullMatch(statement, Statement()))
                {
                    return false; 
                }
            }

            // TODO: parse statements to graph structure 

            return true;
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
    }
}
