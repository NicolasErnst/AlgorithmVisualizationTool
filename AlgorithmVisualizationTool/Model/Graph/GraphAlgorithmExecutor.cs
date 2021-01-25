using AlgorithmVisualizationTool.Model.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using GraphAlgorithmPlugin;
using QuikGraph;
using System.Collections.ObjectModel;

namespace AlgorithmVisualizationTool.Model.Graph
{
    class GraphAlgorithmExecutor : ViewModelBase, IGraphAlgorithmExecutor
    {
        private readonly EventWaitHandle StepHandle = new ManualResetEvent(false);
        private readonly AlgorithmStepStack StepStack = new AlgorithmStepStack();

        #region AlgorithmState

        private GraphAlgorithmState algorithmState = GraphAlgorithmState.Finished;

        /// <summary>
        /// 
        /// </summary>
        public GraphAlgorithmState AlgorithmState
        {
            get
            {
                return algorithmState;
            }
            set
            {
                if (algorithmState == value)
                {
                    return;
                }

                algorithmState = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region Delay

        private int delay = 0;

        /// <summary>
        /// 
        /// </summary>
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                if (delay == value)
                {
                    return;
                }

                delay = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region Progress

        private double progress = 0.0;

        /// <summary>
        /// 
        /// </summary>
        public double Progress
        {
            get
            {
                return progress;
            }
            set
            {
                if (progress == value)
                {
                    return;
                }

                progress = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region ProgressText

        private string progressText = "";

        /// <summary>
        /// 
        /// </summary>
        public string ProgressText
        {
            get
            {
                return progressText;
            }
            set
            {
                if (progressText == value)
                {
                    return;
                }

                progressText = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region Graph 

        public object Graph
        {
            get
            {
                return SelectedGraphAlgorithm?.Graph;
            }
        }

        #endregion

        #region GraphLayout

        public object GraphLayout
        {
            get
            {
                return SelectedGraphAlgorithm?.GraphLayout;
            }
        }

        #endregion

        #region SelectedGraphAlgorithm 

        private IGraphAlgorithmPlugin selectedGraphAlgorithm = null;

        /// <summary>
        /// 
        /// </summary>
        public IGraphAlgorithmPlugin SelectedGraphAlgorithm
        {
            get
            {
                return selectedGraphAlgorithm;
            }
            set
            {
                if (selectedGraphAlgorithm == value)
                {
                    return;
                }

                selectedGraphAlgorithm = value;

                RaisePropertyChanged();

                // TODO: generate graph
                SelectedGraphAlgorithm.GraphAlgorithmExecutor = this;
                RaisePropertyChanged("Graph");
                RaisePropertyChanged("GraphLayout");
                RaisePropertyChanged("ExposedLists");
            }
        }

        #endregion

        #region MadeAlgorithmSteps

        private int madeAlgorithmSteps = 0;

        /// <summary>
        /// 
        /// </summary>
        public int MadeAlgorithmSteps
        {
            get
            {
                return madeAlgorithmSteps;
            }
            set
            {
                if (madeAlgorithmSteps == value)
                {
                    return;
                }

                madeAlgorithmSteps = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        //#region ExposedLists

        //private ObservableCollection<ExposableList> exposedLists = new ObservableCollection<ExposableList>();

        ///// <summary>
        ///// 
        ///// </summary>
        //public ObservableCollection<ExposableList> ExposedLists
        //{
        //    get
        //    {
        //        return exposedLists;
        //    }
        //    set
        //    {
        //        if (exposedLists == value)
        //        {
        //            return;
        //        }

        //        exposedLists = value;

        //        RaisePropertyChanged();
        //    }
        //}

        //#endregion


        public GraphAlgorithmExecutor()
        {
            SelectedGraphAlgorithm = new DFSPlugin.DFSPlugin();
            SelectedGraphAlgorithm.GenerateFromDot(new List<string> { "KACKHAUFEN" });
            // TODO: this is going to be generated by DOT parser
            //var a = new VertexBase("A", "", new Dictionary<string, string> { { "color", "blue" } });
            //var b = new VertexBase("B");
            //var c = new VertexBase("C");
            //Graph.AddVertex(a);
            //Graph.AddVertex(b);
            //Graph.AddVertex(c);
            //Graph.AddEdge(new Edge<VertexBase>(a, b));
            //Graph.AddEdge(new Edge<VertexBase>(a, c));
            // TODO: after DOT parser generated new graph, set it to the plugin 
        }


        public void Start()
        {
            //SelectedGraphAlgorithm = new DepthFirstSearch();
            //SelectedGraphAlgorithm.SetGraphAlgorithmExecutor(this);
            //// TODO : Create graph from DOT 
            //SelectedGraphAlgorithm.CreateGraphFromDot("");
            

            //if (SelectedGraphAlgorithm == null)
            //{
            //    throw new NullReferenceException("No graph algorithm was selected.");
            //}
            //AlgorithmState = GraphAlgorithmState.Started;
            SelectedGraphAlgorithm.RunAlgorithm();
        }

        public void StepForward()
        {
            if (StepStack.RedoCount > 0)
            {
                StepStack.Redo();
                MadeAlgorithmSteps += 1;
            } 
            else
            {
                StepHandle.Set();
            }
        }

        public void StepBackward()
        {
            MadeAlgorithmSteps -= 1;
            StepStack.Undo();
        }

        public void Stop()
        {
            AlgorithmState = GraphAlgorithmState.Stopped;
            StepHandle.Reset();
        }

        public void Reset()
        {
            Stop(); 
            Progress = 0;
            ProgressText = "";
            StepStack.Reset();
        }

        public void MakeAlgorithmStep(Action doAction, Action undoAction)
        {
            StepStack.Do(new AlgorithmStep(doAction, undoAction)); 

            MadeAlgorithmSteps += 1;

            if (AlgorithmState == GraphAlgorithmState.Stopped)
            {
                StepHandle.Reset();
                StepHandle.WaitOne();
            } 
            else if (AlgorithmState == GraphAlgorithmState.Started)
            {
                Thread.Sleep(Delay);
            }
        }
    }
}
