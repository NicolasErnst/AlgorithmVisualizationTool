using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphAlgorithmPlugin
{
    public interface IVertex
    {
        string VertexName { get; set; }
        string VertexContent { get; set; }
        Point CurrentCoordinates { get; set; }
        Point TargetCoordinates { get; }
        void Emphasize();
        void Deemphasize();
        void OnInitialize(IDictionary<string, string> properties);
        string ToString();
        event EventHandler TargetCoordinatesChanged;
        void SetTargetCoordinates(Point targetCoordinates, bool fireEventOnUpdate = true);
    }
}
