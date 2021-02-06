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
        public bool RemainPositions { get; private set; }


        public GraphLayout(Graph<V, E> graph)
        {
            LayoutAlgorithmType = "Tree";
            OverlapRemovalAlgorithmType = "FSA";
            Graph = graph;
            AnimationLength = new TimeSpan(0);
            LayoutUpdated += GraphLayout_LayoutUpdated;
        }


        public void SetRemainPositions(bool remainPositions)
        {
            RemainPositions = remainPositions;
        }

        private void GraphLayout_LayoutUpdated(object sender, EventArgs e)
        {
            foreach(V vertex in Graph.Vertices)
            {
                vertex.TargetCoordinatesChanged += Vertex_TargetCoordinatesChanged;
                GraphLayout.AddPositionChangedHandler(GetVertexControl(vertex), (s, ea) =>
                {
                    vertex.CurrentCoordinates = GetPosition(vertex);
                });

            }
        }

        private void Vertex_TargetCoordinatesChanged(object sender, EventArgs e)
        {
            V vertex = sender as V;
            var vertexControl = GetVertexControl(vertex);

            if (!double.IsNaN(vertex.TargetCoordinates.X) && !double.IsNaN(vertex.TargetCoordinates.Y))
            {
                GraphCanvas.SetX(vertexControl, vertex.TargetCoordinates.X);
                GraphCanvas.SetY(vertexControl, vertex.TargetCoordinates.Y);
            }

            vertex.SetTargetCoordinates(new Point(double.NaN, double.NaN));
        }

        public Point GetPosition(V v)
        {
            var vertexControl = GetVertexControl(v);
            return new Point(GraphCanvas.GetX(vertexControl), GraphCanvas.GetY(vertexControl));
        }
    }
}
