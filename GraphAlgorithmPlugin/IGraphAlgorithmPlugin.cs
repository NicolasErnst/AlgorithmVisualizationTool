using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public interface IGraphAlgorithmPlugin
    { 
        IGraphAlgorithmExecutor GraphAlgorithmExecutor { set; }
        object Graph { get; }
        object GraphLayout { get; }
        double Progress { get; set; }
        string ProgressText { get; set; }
        ExposableListContainer ExposedLists { get; }
        DOTParsingResult GenerateFromDot(IEnumerable<string> dotStatements);
        void RunAlgorithm(CancellationToken cancellationToken, string startVertexName);
        List<string> GetAllVertexNames();
        string AlgorithmName { get; }
        GraphDirectionType CompatibleGraphDirections { get; }
    }

    public interface IGraphAlgorithmPlugin<V, E> : IGraphAlgorithmPlugin where V : class, IVertex, new() where E : Edge<V>, new()
    {
        new Graph<V, E> Graph { get; }
        new GraphLayout<V, E> GraphLayout { get; }
    }
}
