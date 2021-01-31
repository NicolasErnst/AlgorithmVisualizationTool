using GraphAlgorithmPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFSPlugin
{
    public class DFSPlugin : GraphAlgorithmPlugin<DFSVertex, Edge<DFSVertex>>
    {
        protected override async void RunAlgorithm()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    var index = i;
                    await MakeAlgorithmStep(() =>
                    {
                        Vertex first = Graph.Vertices.FirstOrDefault();
                        if (first != null)
                        {
                            first.VertexContent = (index + 1).ToString();
                        }
                    }, () =>
                    {
                        Vertex first = Graph.Vertices.FirstOrDefault();
                        if (first != null)
                        {
                            first.VertexContent = (int.Parse(first.VertexContent) - 1).ToString();
                        }
                    });
                }
            } 
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }
        }
    }
}
