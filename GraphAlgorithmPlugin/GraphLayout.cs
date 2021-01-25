using GraphShape.Controls;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public class GraphLayout<V, E> : GraphLayout<V, E, Graph<V, E>> where V : class, IVertex, new() where E : Edge<V>, new()
    {
        public GraphLayout(Graph<V, E> graph)
        {
            LayoutAlgorithmType = "Tree";
            OverlapRemovalAlgorithmType = "FSA";
            Graph = graph;
        }
    }
}
