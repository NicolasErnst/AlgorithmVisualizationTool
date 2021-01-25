using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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


        public GraphAlgorithmPlugin()
        {
            Graph = new Graph<V, E>();
            GraphLayout = new GraphLayout<V, E>(Graph);
            ExposedLists = new ExposableListContainer();
        }

        public void GenerateFromDot(List<string> dotStatements)
        {
            // TODO
            ExposedLists.Add(new ExposableList("Test", new List<object> { "ABC" }));
        }

        public abstract void RunAlgorithm();
    }
}
