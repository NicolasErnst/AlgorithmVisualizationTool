using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public abstract class GraphAlgorithmPlugin<V, E> : IGraphAlgorithmPlugin<V, E> where V : class, IVertex, new() where E : Edge<V>, new()
    {
        public Graph<V, E> Graph { get; private set; }

        object IGraphAlgorithmPlugin.Graph => Graph;

        public GraphLayout<V, E> GraphLayout { get; private set; }

        object IGraphAlgorithmPlugin.GraphLayout => GraphLayout;

        public IGraphAlgorithmExecutor GraphAlgorithmExecutor { private get; set; }

        public double Progress
        {
            get => (GraphAlgorithmExecutor == null) ? 0.0 : GraphAlgorithmExecutor.Progress;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("The progress value must be between 0 and 100.");
                }
                if (GraphAlgorithmExecutor == null)
                {
                    throw new NullReferenceException("The provided object of type IGraphAlgorithmExecutor is null.");
                }

                GraphAlgorithmExecutor.Progress = value;
            }
        }

        public string ProgressText
        {
            get => (GraphAlgorithmExecutor == null) ? "" : GraphAlgorithmExecutor.ProgressText;
            set
            {
                if (GraphAlgorithmExecutor == null)
                {
                    throw new NullReferenceException("The provided object of type IGraphAlgorithmExecutor is null.");
                }
                GraphAlgorithmExecutor.ProgressText = value;
            }
        }

        public ExposableListContainer ExposedLists { get; set; }

        private CancellationToken CancellationToken { get; set; }

        public abstract string AlgorithmName { get; }

        public abstract GraphDirectionType CompatibleGraphDirections { get; }

        public string FileName { get; set; }


        public GraphAlgorithmPlugin()
        {
            Graph = new Graph<V, E>();
            GraphLayout = new GraphLayout<V, E>(Graph);
            ExposedLists = new ExposableListContainer();
        }


        public DOTParsingResult GenerateFromDot(IEnumerable<string> dotStatements, bool refreshOnly)
        {
            return DOTGraphConverter<V, E>.Parse(Graph, dotStatements, refreshOnly); 
        }

        public async void RunAlgorithm(CancellationToken cancellationToken, string startVertexName)
        {
            V startVertex = null; 
            if (!string.IsNullOrWhiteSpace(startVertexName))
            {
                startVertex = Graph.Vertices.FirstOrDefault(x => x.VertexName.Equals(startVertexName)); 
            }
            CancellationToken = cancellationToken;
            ExposedLists.Clear();

            Progress = 0;
            ProgressText = "Initializing..."; 

            await RunAlgorithm(startVertex);

            Progress = 100;
            ProgressText = "Finished!"; 

            if (cancellationToken.IsCancellationRequested)
            {
                GraphAlgorithmExecutor?.FinishedAlgorithm(false);
            }
            else 
            {
                GraphAlgorithmExecutor?.FinishedAlgorithm(true);
            }
        }

        protected abstract Task RunAlgorithm(V startVertex);

        protected Task MakeAlgorithmStep(Action doAction)
        {
            return MakeAlgorithmStep(doAction, new Action(() => { }));
        }

        protected Task MakeAlgorithmStep(Action doAction, Action undoAction)
        {
            return GraphAlgorithmExecutor?.MakeAlgorithmStep(doAction, undoAction, CancellationToken);
        }

        public List<string> GetAllVertexNames()
        {
            List<string> names = new List<string>();
            names = Graph?.Vertices.Select(x => x.VertexName).ToList();
            return names;
        }
    }
}
