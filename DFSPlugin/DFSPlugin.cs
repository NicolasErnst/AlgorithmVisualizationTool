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
        public override async void RunAlgorithm()
        {
            for(int i = 0; i < 100; i++)
            {
                await MakeAlgorithmStep(() =>
                {
                    Vertex first = Graph.Vertices.FirstOrDefault();
                    if (first != null)
                    {
                        first.VertexContent = (i + 1).ToString();
                    }
                }, () =>
                {
                    Vertex first = Graph.Vertices.FirstOrDefault();
                    if (first != null)
                    {
                        first.VertexContent = (i).ToString();
                    }
                }); 
            }
            
        }
    }
}
