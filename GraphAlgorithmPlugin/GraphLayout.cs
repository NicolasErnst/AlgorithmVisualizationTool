using GraphShape.Controls;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphAlgorithmPlugin
{
    public class GraphLayout<V, E> : GraphLayout<V, E, Graph<V, E>> where V : class, IVertex, new() where E : Edge<V>, new()
    {
        public GraphLayout(Graph<V, E> graph)
        {
            LayoutAlgorithmType = "Tree";
            OverlapRemovalAlgorithmType = "FSA";
            Graph = graph;
            AnimationLength = new TimeSpan(0);
        }

        
        public void UpdatePosition(V v, Point coordinates)
        {
            var vertexControl = GetVertexControl(v);
            GraphCanvas.SetX(vertexControl, coordinates.X);
            GraphCanvas.SetY(vertexControl, coordinates.Y);
        }

        public Point GetPosition(V v)
        {
            var vertexControl = GetVertexControl(v);
            return new Point(GraphCanvas.GetX(vertexControl), GraphCanvas.GetY(vertexControl));
        }
    }
}
