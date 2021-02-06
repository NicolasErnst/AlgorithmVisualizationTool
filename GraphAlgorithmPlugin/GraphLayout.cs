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
        public bool KeepPositions { get; set; }
        private static Dictionary<V, Point> LastKnownPositions = new Dictionary<V, Point>();
        private static bool IsCallback = false;

        public GraphLayout(Graph<V, E> graph)
        {
            LayoutAlgorithmType = "Tree";
            OverlapRemovalAlgorithmType = "FSA";
            Graph = graph;
            AnimationLength = new TimeSpan(0);
            LayoutUpdated += GraphLayout_LayoutUpdated;
        }


        private void GraphLayout_LayoutUpdated(object sender, EventArgs e)
        {
            foreach(V vertex in Graph.Vertices)
            {
                vertex.TargetCoordinatesChanged += Vertex_TargetCoordinatesChanged;
                GraphLayout.AddPositionChangedHandler(GetVertexControl(vertex), (s, ea) =>
                {
                    if (IsCallback)
                    {
                        return; 
                    }

                    var vertexControl = GetVertexControl(vertex); 

                    if (KeepPositions)
                    {
                        SetPosition(vertex, LastKnownPositions[vertex]); 
                        vertex.CurrentCoordinates = LastKnownPositions[vertex]; 
                    }
                    else
                    {
                        if (!double.IsNaN(vertex.TargetCoordinates.X) && !double.IsNaN(vertex.TargetCoordinates.Y))
                        {
                            SetPosition(vertex, vertex.TargetCoordinates);
                            vertex.CurrentCoordinates = vertex.TargetCoordinates;
                            if (!LastKnownPositions.ContainsKey(vertex))
                            {
                                LastKnownPositions.Add(vertex, new Point());
                            }
                            LastKnownPositions[vertex] = vertex.TargetCoordinates;
                            vertex.SetTargetCoordinates(new Point(double.NaN, double.NaN), false);
                        }
                        else
                        {
                            vertex.CurrentCoordinates = GetPosition(vertex); 
                        }
                    }
                });

            }
        }

        private void Vertex_TargetCoordinatesChanged(object sender, EventArgs e)
        {
            V vertex = sender as V;
            if (!double.IsNaN(vertex.TargetCoordinates.X) && !double.IsNaN(vertex.TargetCoordinates.Y))
            {
                SetPosition(vertex, vertex.TargetCoordinates);
            }
            vertex.SetTargetCoordinates(new Point(double.NaN, double.NaN));
        }

        public void SetPosition(V vertex, Point coordinates)
        {
            var vertexControl = GetVertexControl(vertex);
            IsCallback = true;
            GraphLayout.SetX(vertexControl, coordinates.X);
            GraphLayout.SetY(vertexControl, coordinates.Y);
            IsCallback = false;
        }

        public Point GetPosition(V v)
        {
            var vertexControl = GetVertexControl(v);
            return new Point(GraphCanvas.GetX(vertexControl), GraphCanvas.GetY(vertexControl));
        }
    }
}
