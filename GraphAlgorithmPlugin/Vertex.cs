using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphAlgorithmPlugin
{
    public class Vertex : UserControl, IVertex
    {
        private readonly TextBlock VertexNameBlock = new TextBlock() 
        { 
            Text = "Name", 
            FontSize = 16, 
            FontWeight = FontWeights.Bold, 
            Margin = new Thickness(10, 20, 10, 10),
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center
        };
        private readonly TextBlock VertexContentBlock = new TextBlock() 
        { 
            Text = "Content", 
            FontSize = 14,
            Margin = new Thickness(10, 0, 10, 20),
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center
        };
        private readonly Border VertexBorder = new Border() 
        { 
            BorderBrush = Brushes.Black, 
            BorderThickness = new Thickness(1), 
            CornerRadius = new CornerRadius(10000), 
            MinHeight = 100, MinWidth = 100 
        }; 

        public string VertexName { get => VertexNameBlock.Text; set => VertexNameBlock.Text = value; }
        public string VertexContent { get => VertexContentBlock.Text; set => VertexContentBlock.Text = value; }
        public Brush VertexBorderBrush { get => VertexBorder.BorderBrush; set => VertexBorder.BorderBrush = value; }

        #region Marked 

        private bool marked = false;

        /// <summary>
        /// 
        /// </summary>
        public bool Marked
        {
            get
            {
                return marked;
            }
            set
            {
                if (marked == value)
                {
                    return;
                }

                marked = value;

                if (marked)
                {
                    Emphasize();
                }
                else
                {
                    Deemphasize();
                }
            }
        }

        #endregion

        #region CurrentCoordinates

        private Point currentCoordinates = new Point(double.NaN,double.NaN);

        /// <summary>
        /// 
        /// </summary>
        public Point CurrentCoordinates
        {
            get
            {
                return currentCoordinates;
            }
            set
            {
                if (currentCoordinates == value)
                {
                    return;
                }

                currentCoordinates = value;
            }
        }

        #endregion

        #region TargetCoordinates

        public event EventHandler TargetCoordinatesChanged;

        private Point targetCoordinates = new Point(double.NaN, double.NaN);

        /// <summary>
        /// 
        /// </summary>
        public Point TargetCoordinates
        {
            get
            {
                return targetCoordinates;
            }
            set
            {
                if (targetCoordinates == value)
                {
                    return;
                }

                targetCoordinates = value;

                if (!double.IsNaN(targetCoordinates.X) && !double.IsNaN(targetCoordinates.Y))
                {
                    EventHandler handler = TargetCoordinatesChanged;
                    handler?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #endregion


        public Vertex() : this("")
        {
            // Nothing to do here
        }

        public Vertex(string vertexName) : this(vertexName, "")
        {
            // Nothing to do here
        }

        public Vertex(string vertexName, string vertexContent) : this(vertexName, vertexContent, Brushes.Black)
        {
            // Nothing to do here
        }

        public Vertex(string vertexName, string vertexContent, Brush vertexBorderBrush)
        {
            StackPanel stack = new StackPanel();
            stack.Children.Add(VertexNameBlock);
            stack.Children.Add(VertexContentBlock);
            VertexBorder.Child = stack;
            AddChild(VertexBorder);

            Background = Brushes.Transparent; 
            VertexName = vertexName;
            VertexContent = vertexContent;
            VertexBorderBrush = vertexBorderBrush;
            Marked = false;
        }


        public virtual void Deemphasize()
        {
            VertexBorderBrush = Brushes.Black;
        }

        public virtual void Emphasize()
        {
            VertexBorderBrush = Brushes.Red;
        }

        public virtual void OnInitialize(IDictionary<string, string> properties)
        {
            if (properties.ContainsKey("label"))
            {
                VertexContent = properties["label"];
            }
        }

        public override string ToString()
        {
            return VertexName;
        }

        public void SetTargetCoordinates(Point targetCoordinates, bool fireEventOnUpdate = true)
        {
            if (fireEventOnUpdate)
            {
                TargetCoordinates = targetCoordinates;
            }
            else
            {
                this.targetCoordinates = targetCoordinates;
            }
        }
    }
}
