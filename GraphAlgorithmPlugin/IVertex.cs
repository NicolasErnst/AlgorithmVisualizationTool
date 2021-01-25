using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public interface IVertex
    {
        string VertexName { get; set; }
        string VertexContent { get; set; }
        void Emphasize();
        void Deemphasize();
        void OnInitialize(IDictionary<string, string> properties);
        string ToString();
    }
}
