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
using System.IO;

namespace AlgorithmVisualizationTool.Model.Graph
{
    class GraphAlgorithmExecutor : ViewModelBase, IGraphAlgorithmExecutor
    {
        private readonly static EventWaitHandle StepHandle = new ManualResetEvent(false);
        private readonly AlgorithmStepStack StepStack = new AlgorithmStepStack();
        private CancellationTokenSource AlgorithmExecutionCTS = new CancellationTokenSource();

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

                SelectedGraphAlgorithm.GraphAlgorithmExecutor = this;
                RefreshGraphVisualization();
            }
        }

        #endregion

        #region AvailableGraphAlgorithms

        private ObservableCollection<IGraphAlgorithmPlugin> availableGraphAlgorithms = new ObservableCollection<IGraphAlgorithmPlugin>();

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<IGraphAlgorithmPlugin> AvailableGraphAlgorithms
        {
            get
            {
                return availableGraphAlgorithms;
            }
            set
            {
                if (availableGraphAlgorithms == value)
                {
                    return;
                }

                availableGraphAlgorithms = value;

                RaisePropertyChanged();
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

        private IEnumerable<string> dotStatements = null;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> DOTStatements
        {
            get
            {
                return dotStatements;
            }
            set
            {
                if (dotStatements == value)
                {
                    return;
                }

                dotStatements = value;

                RaisePropertyChanged();

                RefreshGraphVisualization();
            }
        }

        #endregion

        #region AvailableUndos 

        public int AvailableUndos
        {
            get
            {
                return StepStack.UndoCount;
            }
        }

        #endregion

        #region GraphVertexNames 

        public ObservableCollection<string> GraphVertexNames
        {
            get
            {
                if (SelectedGraphAlgorithm != null)
                {
                    return new ObservableCollection<string>(SelectedGraphAlgorithm.GetAllVertexNames());
                }
                return new ObservableCollection<string>();
            }
        }

        #endregion

        #region SelectedStartVertexName

        private string selectedStartVertexName = "";

        /// <summary>
        /// 
        /// </summary>
        public string SelectedStartVertexName
        {
            get
            {
                return selectedStartVertexName;
            }
            set
            {
                if (selectedStartVertexName == value || value == null)
                {
                    return;
                }

                selectedStartVertexName = value;

                RaisePropertyChanged();
            }
        }

        #endregion 

        #region StartAlgorithmText

        private string startAlgorithmText = "Start";

        /// <summary>
        /// 
        /// </summary>
        public string StartAlgorithmText
        {
            get
            {
                return startAlgorithmText;
            }
            set
            {
                if (startAlgorithmText == value)
                {
                    return;
                }

                startAlgorithmText = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        private int TargetAlgorithmsSteps = 0; 


        public void GenerateFromDot()
        {
            DOTParsingResult result = SelectedGraphAlgorithm?.GenerateFromDot(DOTStatements);
            if (result != null && !result.Success)
            {
                System.Windows.MessageBox.Show("The specified DOT description of the graph could not be parsed.\r\n\r\nNumber of statement: " + result.ErrorLine + "\r\nStatement: " + result.ErrorMessage, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void Start()
        {
            if (AlgorithmState == GraphAlgorithmState.Finished)
            {
                ResetGraphExecution();
                AlgorithmExecutionCTS = new CancellationTokenSource();
                SelectedGraphAlgorithm?.RunAlgorithm(AlgorithmExecutionCTS.Token, SelectedStartVertexName);
            }
            if (AlgorithmState != GraphAlgorithmState.Started)
            {
                AlgorithmState = GraphAlgorithmState.Started;
                StepHandle.Set();
            }
        }

        public void StepForward()
        {
            if (AlgorithmState == GraphAlgorithmState.Finished)
            {
                ResetGraphExecution();
                AlgorithmExecutionCTS = new CancellationTokenSource();
                SelectedGraphAlgorithm?.RunAlgorithm(AlgorithmExecutionCTS.Token, SelectedStartVertexName);
            }
            if (AlgorithmState != GraphAlgorithmState.Started)
            {
                AlgorithmState = GraphAlgorithmState.Stopped;
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
            ResetGraphExecution();
            AlgorithmState = GraphAlgorithmState.Finished;
        }

        private void ResetGraphExecution()
        {
            AlgorithmExecutionCTS?.Cancel();
            StepHandle.Set();
            Progress = 0;
            ProgressText = "";
            MadeAlgorithmSteps = 0;
            StepStack.Reset();
            StepHandle.Reset();
            SelectedGraphAlgorithm?.ExposedLists.Clear();
            RefreshGraphVisualization();
        }

        private void RefreshGraphVisualization()
        {
            if (DOTStatements != null)
            {
                GenerateFromDot();
                RaisePropertyChanged("Graph");
                RaisePropertyChanged("GraphLayout");
                RaisePropertyChanged("ExposedLists");
                RaisePropertyChanged("GraphVertexNames");
            }
            if (!GraphVertexNames.Contains(SelectedStartVertexName))
            {
                SelectedStartVertexName = "";
            }
        }

        public async Task MakeAlgorithmStep(Action doAction, Action undoAction, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (TargetAlgorithmsSteps == 0)
            {
                await Task.Run(async () => await AlgorithmContinuation());
            }
            
            
            while (StepStack.RedoCount > 0)
            {
                StepStack.Redo();
                MadeAlgorithmSteps++;
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Run(async () => await AlgorithmContinuation());
            }

            cancellationToken.ThrowIfCancellationRequested();
            StepStack.Do(new AlgorithmStep(doAction, undoAction));
            MadeAlgorithmSteps++;

            if (MadeAlgorithmSteps >= TargetAlgorithmsSteps)
            {
                TargetAlgorithmsSteps = 0;
            }
        }

        private async Task AlgorithmContinuation()
        {
            if (MadeAlgorithmSteps > 0 && StepHandle.WaitOne())
            {
                if (AlgorithmState == GraphAlgorithmState.Stopped)
                {
                    StepHandle.Reset();
                    StepHandle.WaitOne();
                }
                else if (AlgorithmState == GraphAlgorithmState.Started)
                {
                    await Task.Delay(Delay);
                    StepHandle.WaitOne();
                }
            }
        }

        public void StartClicked()
        {
            if (AlgorithmState == GraphAlgorithmState.Stopped || AlgorithmState == GraphAlgorithmState.Finished)
            {
                Start();
                StartAlgorithmText = "Stop";

            }
            else
            {
                Stop();
                StartAlgorithmText = "Start";
            }
        }

        public void FinishedAlgorithm(bool success)
        {
            if (success)
            {
                System.Windows.MessageBox.Show("The algorithm was executed successfully.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            AlgorithmState = GraphAlgorithmState.Finished;
            StartAlgorithmText = "Start";
        }

        public void SelectGraphAlgorithmByFileName(string fileName, string startVertex)
        {
            foreach(IGraphAlgorithmPlugin plugin in AvailableGraphAlgorithms)
            {
                if (Path.GetFileName(plugin.FileName).Equals(fileName))
                {
                    SelectedGraphAlgorithm = plugin;
                    break; 
                }
            }
            SelectedStartVertexName = startVertex;
        }

        public void SetAlgorithmToState(int algorithmSteps)
        {
            TargetAlgorithmsSteps = algorithmSteps;
            while (TargetAlgorithmsSteps > 0)
            {
                StepForward();
            }
            TargetAlgorithmsSteps = 0;
        }
    }
}
