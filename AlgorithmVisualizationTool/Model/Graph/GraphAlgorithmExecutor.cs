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
                GenerateFromDot();
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

        #region DOTDescription 

        private string dotDescription = "";

        /// <summary>
        /// 
        /// </summary>
        public string DOTDescription
        {
            get
            {
                return dotDescription;
            }
            set
            {
                if (dotDescription == value)
                {
                    return;
                }

                dotDescription = value;

                RaisePropertyChanged();

                GenerateFromDot();
            }
        }

        #endregion


        public GraphAlgorithmExecutor()
        {
            SelectedGraphAlgorithm = new DFSPlugin.DFSPlugin();
        }


        public void GenerateFromDot()
        {
            string[] statements = DOTDescription.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> dotStatements = statements.Select(x => x.Trim().Replace(Environment.NewLine, "") + ";").ToList();
            DOTParsingResult result = SelectedGraphAlgorithm?.GenerateFromDot(dotStatements);

            if (!result.Success)
            {
                System.Windows.MessageBox.Show("The specified DOT description of the graph could not be parsed.\r\n\r\nLine of error: " + result.ErrorLine + "\r\nError message:" + result.ErrorMessage, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error); 
            }
        }

        public void Start()
        {
            AlgorithmState = GraphAlgorithmState.Started;

            Task.Run(() => SelectedGraphAlgorithm.RunAlgorithm());

            // SelectedGraphAlgorithm.RunAlgorithm();
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
