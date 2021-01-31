using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public class Graph<V, E> : BidirectionalGraph<V, E> where V : class, IVertex, new() where  E : Edge<V>, new()
    {
        public GraphDirectionType DirectionType { get; set; }


        public Graph()
        {
            DirectionType = GraphDirectionType.None;
        }
    }
}
