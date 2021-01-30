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
    }
}
