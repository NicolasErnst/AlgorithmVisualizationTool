using AlgorithmVisualizationTool.Controls;
using AlgorithmVisualizationTool.Model.Extensions;
using AlgorithmVisualizationTool.Model.Graph;
using AlgorithmVisualizationTool.Model.MVVM;
using GraphAlgorithmPlugin;
using Microsoft.Win32;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AlgorithmVisualizationTool.ViewModel
{
    class GraphViewVM : DisplayableViewModel
    {
        #region NewClickedCommand

        private RelayCommand newClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand NewClickedCommand
        {
            get
            {
                return newClickedCommand ?? (newClickedCommand = new RelayCommand(NewClickedExe, NewClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool NewClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void NewClickedExe(object param)
        {
            KeyNewCommand?.Invoke();
        }

        #endregion

        #region OpenClickedCommand

        private RelayCommand openClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand OpenClickedCommand
        {
            get
            {
                return openClickedCommand ?? (openClickedCommand = new RelayCommand(OpenClickedExe, OpenClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool OpenClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void OpenClickedExe(object param)
        {
            KeyOpenCommand?.Invoke();
        }

        #endregion 

        #region KeySaveCommand

        public override Action KeySaveCommand => new Action(() =>
        {
            Graph.Save();
            UnsavedChanges = false;
        });

        #endregion

        #region SaveClickedCommand 

        private RelayCommand saveClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand SaveClickedCommand
        {
            get
            {
                return saveClickedCommand ?? (saveClickedCommand = new RelayCommand(SaveClickedExe, SaveClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool SaveClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void SaveClickedExe(object param)
        {
            KeySaveCommand?.Invoke();
        }

        #endregion

        #region ExitClickedCommand

        private RelayCommand exitClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand ExitClickedCommand
        {
            get
            {
                return exitClickedCommand ?? (exitClickedCommand = new RelayCommand(ExitClickedExe, ExitClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool ExitClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void ExitClickedExe(object param)
        {
            KeyExitCommand?.Invoke();
        }

        #endregion 

        #region SaveAsClickedCommand

        private RelayCommand saveAsClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand SaveAsClickedCommand
        {
            get
            {
                return saveAsClickedCommand ?? (saveAsClickedCommand = new RelayCommand(SaveAsClickedExe, SaveAsClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool SaveAsClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void SaveAsClickedExe(object param)
        {
            Graph.OpenSaveAsDialog();
        }

        #endregion 

        #region KeyExitCommand

        public override Action KeyExitCommand => new Action(() =>
        {
            if (UnsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Do you want to save them before exiting the application?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Graph.Save();
                    UnsavedChanges = false;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            Environment.Exit(0);
        });

        #endregion

        #region InfoClickedCommand

        private RelayCommand infoClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand InfoClickedCommand
        {
            get
            {
                return infoClickedCommand ?? (infoClickedCommand = new RelayCommand(InfoClickedExe, InfoClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool InfoClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void InfoClickedExe(object param)
        {
            ShowingView?.KeyInfoCommand?.Invoke();
        }

        #endregion

        #region UnsavedChanges

        private bool unsavedChanges = false;

        /// <summary>
        /// 
        /// </summary>
        public bool UnsavedChanges
        {
            get
            {
                return unsavedChanges;
            }
            set
            {
                if (unsavedChanges == value)
                {
                    return;
                }

                unsavedChanges = value;

                if (!unsavedChanges)
                {
                    graph.Name = graph.Name.Trim();
                    UpdateOverviewTab();
                }

                UpdateWindowTitle();
            }
        }

        #endregion

        #region Graph 

        private GraphFile graph = null;

        /// <summary>
        /// 
        /// </summary>
        public GraphFile Graph
        {
            get
            {
                return graph;
            }
            set
            {
                if (graph == value)
                {
                    return;
                }

                graph = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region GraphName

        public string GraphName
        {
            get
            {
                return graph.Name;
            }
            set
            {
                if (graph.Name == value)
                {
                    return;
                }

                graph.Name = value;
                RaisePropertyChanged();

                UpdateWindowTitle();
                UnsavedChanges = true;
            }
        }

        #endregion

        #region AlgorithmMaxDelay

        private int algorithmMaxDelay = 10000;

        /// <summary>
        /// 
        /// </summary>
        public int AlgorithmMaxDelay
        {
            get
            {
                return algorithmMaxDelay;
            }
            set
            {
                if (algorithmMaxDelay == value)
                {
                    return;
                }

                algorithmMaxDelay = value;

                RaisePropertyChanged();

                if (AlgorithmExecutor.Delay > AlgorithmMaxDelay)
                {
                    AlgorithmExecutor.Delay = AlgorithmMaxDelay;
                }
            }
        }

        #endregion

        #region  AlgorithmMaxDelayDoubleClickedCommand

        private RelayCommand algorithmMaxDelayDoubleClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand AlgorithmMaxDelayDoubleClickedCommand
        {
            get
            {
                return algorithmMaxDelayDoubleClickedCommand ?? (algorithmMaxDelayDoubleClickedCommand = new RelayCommand(AlgorithmMaxDelayDoubleClickedExe, AlgorithmMaxDelayDoubleClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool AlgorithmMaxDelayDoubleClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void AlgorithmMaxDelayDoubleClickedExe(object param)
        {
            // TODO: pause algorithm 

            NumberInputDialog dialog = new NumberInputDialog("Enter a new maximum delay...", AlgorithmMaxDelay);
            if (dialog.ShowDialog() == true && dialog.Result == MessageBoxResult.OK)
            {
                AlgorithmMaxDelay = dialog.Value;
            }

            // TODO: continue algorithm 
        }

        #endregion

        #region AlgorithmDelayDoubleClickedCommand

        private RelayCommand algorithmDelayDoubleClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand AlgorithmDelayDoubleClickedCommand
        {
            get
            {
                return algorithmDelayDoubleClickedCommand ?? (algorithmDelayDoubleClickedCommand = new RelayCommand(AlgorithmDelayDoubleClickedExe, AlgorithmDelayDoubleClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool AlgorithmDelayDoubleClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void AlgorithmDelayDoubleClickedExe(object param)
        {
            // TODO: pause algorithm 

            NumberInputDialog dialog = new NumberInputDialog("Enter a new delay...", AlgorithmMaxDelay);
            if (dialog.ShowDialog() == true && dialog.Result == MessageBoxResult.OK)
            {
                AlgorithmExecutor.Delay = dialog.Value;
            }

            // TODO: continue algorithm 
        }

        #endregion

        #region AlgorithmExecutor

        private GraphAlgorithmExecutor algorithmExecutor = new GraphAlgorithmExecutor();

        /// <summary>
        /// 
        /// </summary>
        public GraphAlgorithmExecutor AlgorithmExecutor
        {
            get
            {
                return algorithmExecutor;
            }
            set
            {
                if (algorithmExecutor == value)
                {
                    return;
                }

                algorithmExecutor = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region StartAlgorithmClickedCommand

        private RelayCommand startAlgorithmClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand StartAlgorithmClickedCommand
        {
            get
            {
                return startAlgorithmClickedCommand ?? (startAlgorithmClickedCommand = new RelayCommand(StartAlgorithmClickedExe, StartAlgorithmClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool StartAlgorithmClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void StartAlgorithmClickedExe(object param)
        {
            AlgorithmExecutor?.StartClicked();
        }

        #endregion

        #region StepForwardClickedCommand

        private RelayCommand stepForwardClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand StepForwardClickedCommand
        {
            get
            {
                return stepForwardClickedCommand ?? (stepForwardClickedCommand = new RelayCommand(StepForwardClickedExe, StepForwardClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool StepForwardClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void StepForwardClickedExe(object param)
        {
            AlgorithmExecutor?.StepForward();
        }

        #endregion

        #region StepBackwardClickedCommand

        private RelayCommand stepBackwardClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand StepBackwardClickedCommand
        {
            get
            {
                return stepBackwardClickedCommand ?? (stepBackwardClickedCommand = new RelayCommand(StepBackwardClickedExe, StepBackwardClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool StepBackwardClickedCanExe(object param)
        {
            if (AlgorithmExecutor?.AvailableUndos == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void StepBackwardClickedExe(object param)
        {
            AlgorithmExecutor?.StepBackward();
        }

        #endregion 

        #region ResetClickedCommand

        private RelayCommand resetClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand ResetClickedCommand
        {
            get
            {
                return resetClickedCommand ?? (resetClickedCommand = new RelayCommand(ResetClickedExe, ResetClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool ResetClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void ResetClickedExe(object param)
        {
            AlgorithmExecutor?.Reset();
        }

        #endregion

        #region DOTDescription

        public string DOTDescription
        {
            get
            {
                return graph.DOTDescription;
            }
            set
            {
                if (graph.DOTDescription == value)
                {
                    return;
                }

                graph.DOTDescription = value;

                RaisePropertyChanged();

                UnsavedChanges = true; 

                if (AlgorithmExecutor != null)
                {
                    IEnumerable<string> statements = GetDOTStatements();
                    dotInputValidation = DOTParser.IsDotStatement(statements);
                    if (dotInputValidation.Success)
                    {
                        AlgorithmExecutor.DOTStatements = statements;
                        RaisePropertyChanged("GraphDirectionType");
                    }
                    RaisePropertyChanged("DOTInputValidation"); 
                }
            }
        }

        #endregion

        #region DOTDescriptionFocusLostCommand

        private RelayCommand dotDescriptionFocusLostCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand DOTDescriptionFocusLostCommand
        {
            get
            {
                return dotDescriptionFocusLostCommand ?? (dotDescriptionFocusLostCommand = new RelayCommand(DOTDescriptionFocusLostExe, DOTDescriptionFocusLostCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool DOTDescriptionFocusLostCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void DOTDescriptionFocusLostExe(object param)
        {
            if (dotInputValidation != null && !dotInputValidation.Success)
            {
                System.Windows.MessageBox.Show("The specified DOT description of the graph could not be parsed.\r\n\r\nNumber of statement: " + dotInputValidation.ErrorLine + "\r\nStatement: " + dotInputValidation.ErrorMessage, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        #endregion 

        #region GraphDirectionType 

        public string GraphDirectionType { 
            get
            {
                GraphDirectionType direction = DOTParser.DetermineGraphDirection(GetDOTStatements());
                GraphDirectionTypeToStringConverter converter = new GraphDirectionTypeToStringConverter();
                return converter.Convert(direction, typeof(string), null, null) as string; 
            }
        }

        #endregion

        #region DOTInputValidation

        private DOTParsingResult dotInputValidation = null;

        /// <summary>
        /// 
        /// </summary>
        public string DOTInputValidation
        {
            get
            {
                if (dotInputValidation == null || dotInputValidation.Success)
                {
                    return null;
                }
                return "Error in statement " + dotInputValidation.ErrorLine + ": " + dotInputValidation.ErrorMessage;
            }
        }

        #endregion

        #region ImportClickedCommand

        private RelayCommand importClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand ImportClickedCommand
        {
            get
            {
                return importClickedCommand ?? (importClickedCommand = new RelayCommand(ImportClickedExe, ImportClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool ImportClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void ImportClickedExe(object param)
        {
            ShowImportProjectDialog();
        }

        #endregion

        #region ExportClickedCommand

        private RelayCommand exportClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand ExportClickedCommand
        {
            get
            {
                return exportClickedCommand ?? (exportClickedCommand = new RelayCommand(ExportClickedExe, ExportClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool ExportClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void ExportClickedExe(object param)
        {
            ExportProjectDialog exportDialog = new ExportProjectDialog(Graph, AlgorithmExecutor?.SelectedStartVertexName, AlgorithmExecutor?.MadeAlgorithmSteps ?? 0, AlgorithmExecutor?.SelectedGraphAlgorithm, AlgorithmExecutor?.AvailableGraphAlgorithms);
            if (exportDialog.ShowDialog() == true)
            {
                ExportProjectFile(exportDialog.FileToExport, exportDialog.FileName); 
            }
        }

        #endregion 


        public GraphViewVM(GraphFile graphFile, string selectedAlgorithm = null, int madeAlgorithmSteps = 0, string startVertex = null)
        {
            graph = graphFile;
            UpdateOverviewTab();
            UpdateWindowTitle();
            LoadAvailableAlgorithms();

            FileSystemWatcher algorithmDirectoryWatcher = new FileSystemWatcher()
            {
                Path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms"),
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.dll",
                EnableRaisingEvents = true
            };
            algorithmDirectoryWatcher.Changed += (s, e) => { LoadAvailableAlgorithms(); };

            if (AlgorithmExecutor != null)
            {
                AlgorithmExecutor.DOTStatements = GetDOTStatements();
                if (selectedAlgorithm != null)
                {
                    AlgorithmExecutor.SelectGraphAlgorithmByFileName(selectedAlgorithm, startVertex);

                    if (madeAlgorithmSteps > 0)
                    {
                        AlgorithmExecutor.SetAlgorithmToState(madeAlgorithmSteps);
                    }
                }
            }
        }


        private void UpdateWindowTitle()
        {
            if (unsavedChanges)
            {
                SetWindowTitle(GraphName + "*");
            }
            else
            {
                SetWindowTitle(GraphName);
            }
        }

        private void UpdateOverviewTab()
        {
            RaisePropertyChanged("GraphName");
            RaisePropertyChanged("GraphLastModified");
            RaisePropertyChanged("GraphPath");
        }

        private IEnumerable<string> GetDOTStatements()
        {
            string[] statements = Regex.Split(DOTDescription.Replace(Environment.NewLine, ""), @"(?<=;)");
            return statements.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x) && !x.Equals(";")); 
        }

        private void LoadAvailableAlgorithms()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms")))
                {
                    Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms"));
                }

                AlgorithmExecutor?.AvailableGraphAlgorithms.Clear();
                string[] files = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms"), "*.dll");
                foreach (string file in files)
                {
                    var DLL = Assembly.LoadFile(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms", file));
                    foreach (Type type in DLL.GetExportedTypes())
                    {
                        if (typeof(IGraphAlgorithmPlugin).IsAssignableFrom(type))
                        {
                            IGraphAlgorithmPlugin algorithmPlugin = Activator.CreateInstance(type) as IGraphAlgorithmPlugin;
                            algorithmPlugin.FileName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms", file);
                            AlgorithmExecutor?.AvailableGraphAlgorithms.Add(algorithmPlugin);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("The algorithm plugins could not be loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
