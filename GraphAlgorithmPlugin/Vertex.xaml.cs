using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphAlgorithmPlugin
{
    /// <summary>
    /// Interaktionslogik für Vertex.xaml
    /// </summary>
    public partial class Vertex : UserControl, IVertex
    {
        public string VertexName { get => VertexNameBlock.Text; set => VertexNameBlock.Text = value; }
        public string VertexContent { get => VertexContentBlock.Text; set => VertexContentBlock.Text = value; }
        public Brush VertexBorderBrush { get => VertexBorder.BorderBrush; set => VertexBorder.BorderBrush = value; }


        public Vertex() : this("")
        {
            InitializeComponent();
        }

        public Vertex(string vertexName) : this(vertexName, "")
        {
            InitializeComponent();
        }

        public Vertex(string vertexName, string vertexContent) : this(vertexName, vertexContent, Brushes.Black)
        {
            InitializeComponent();
        }

        public Vertex(string vertexName, string vertexContent, Brush vertexBorderBrush)
        {
            InitializeComponent();
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
