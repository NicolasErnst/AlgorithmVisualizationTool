using GraphAlgorithmPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFSPlugin
{
    public class DFSPlugin : GraphAlgorithmPlugin<Vertex, Edge<Vertex>>
    {
        public override void RunAlgorithm()
        {
            Graph.AddVertex(new Vertex("ABC"));
            ExposedLists.ElementAt(0).Add("DEF"); 
        }
    }
}
