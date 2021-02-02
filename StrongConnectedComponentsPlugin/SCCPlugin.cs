using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphAlgorithmPlugin;

namespace StrongConnectedComponentsPlugin
{
    public class SCCPlugin : GraphAlgorithmPlugin<Vertex, Edge<Vertex>>
    {
        public override string AlgorithmName => "Strong Connected Components";

        public override GraphDirectionType CompatibleGraphDirections => GraphDirectionType.Directed; 

        protected override async Task RunAlgorithm(Vertex startVertex)
        {
            // TODO
        }
    }
}

