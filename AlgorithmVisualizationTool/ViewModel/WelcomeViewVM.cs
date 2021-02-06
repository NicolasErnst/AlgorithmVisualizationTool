using AlgorithmVisualizationTool.Model;
using AlgorithmVisualizationTool.Model.Extensions;
using AlgorithmVisualizationTool.Model.Graph;
using AlgorithmVisualizationTool.Model.MVVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgorithmVisualizationTool.ViewModel
{
    class WelcomeViewVM : DisplayableViewModel
    {
        #region RecentGraphs

        private ObservableCollection<GraphFile> recentGraphs = new ObservableCollection<GraphFile>();

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<GraphFile> RecentGraphs
        {
            get
            {
                return recentGraphs;
            }
            set
            {
                if (recentGraphs == value)
                {
                    return;
                }

                recentGraphs = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region FilteredRecentGraphs

        private ObservableCollection<GraphFile> filteredRecentGraphs = new ObservableCollection<GraphFile>();

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<GraphFile> FilteredRecentGraphs
        {
            get
            {
                return filteredRecentGraphs;
            }
            set
            {
                if (filteredRecentGraphs == value)
                {
                    return;
                }

                filteredRecentGraphs = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region SelectedRecentGraph

        private GraphFile selectedRecentGraph = null;

        /// <summary>
        /// 
        /// </summary>
        public GraphFile SelectedRecentGraph
        {
            get
            {
                return selectedRecentGraph;
            }
            set
            {
                if (selectedRecentGraph == value)
                {
                    return;
                }

                selectedRecentGraph = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region RecentGraphsSearchText

        private string recentGraphsSearchText = "";

        /// <summary>
        /// 
        /// </summary>
        public string RecentGraphsSearchText
        {
            get
            {
                return recentGraphsSearchText;
            }
            set
            {
                if (recentGraphsSearchText == value)
                {
                    return;
                }

                recentGraphsSearchText = value;

                RaisePropertyChanged();

                FilteredRecentGraphs = new ObservableCollection<GraphFile>(
                    RecentGraphs.Where(x => x.Name.Contains(recentGraphsSearchText) ||
                                            x.FilePath.Contains(recentGraphsSearchText) ||
                                            x.LastOpened.ToDateTimeString().Contains(recentGraphsSearchText)
                ));
            }
        }

        #endregion

        #region GraphTemplates

        private ObservableCollection<GraphTemplate> graphTemplates = new ObservableCollection<GraphTemplate>();

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<GraphTemplate> GraphTemplates
        {
            get
            {
                return graphTemplates;
            }
            set
            {
                if (graphTemplates == value)
                {
                    return;
                }

                graphTemplates = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region SelectedGraphTemplate

        private GraphTemplate selectedGraphTemplate = null;

        /// <summary>
        /// 
        /// </summary>
        public GraphTemplate SelectedGraphTemplate
        {
            get
            {
                return selectedGraphTemplate;
            }
            set
            {
                if (selectedGraphTemplate == value)
                {
                    return;
                }

                selectedGraphTemplate = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region CreateGraphCommand

        private RelayCommand createGraphCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand CreateGraphCommand
        {
            get
            {
                return createGraphCommand ?? (createGraphCommand = new RelayCommand(CreateGraphExe, CreateGraphCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool CreateGraphCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void CreateGraphExe(object param)
        {
            ShowCreateGraphDialog();
        }

        #endregion

        #region OpenGraphCommand

        private RelayCommand openGraphCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand OpenGraphCommand
        {
            get
            {
                return openGraphCommand ?? (openGraphCommand = new RelayCommand(OpenGraphExe, OpenGraphCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool OpenGraphCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void OpenGraphExe(object param)
        {
            ShowOpenGraphOrImportProjectDialog();
        }

        #endregion

        #region RecentGraphsDoubleClickedCommand

        private RelayCommand recentGraphsDoubleClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand RecentGraphsDoubleClickedCommand
        {
            get
            {
                return recentGraphsDoubleClickedCommand ?? (recentGraphsDoubleClickedCommand = new RelayCommand(RecentGraphsDoubleClickedExe, RecentGraphsDoubleClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool RecentGraphsDoubleClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void RecentGraphsDoubleClickedExe(object param)
        {
            OpenGraph(SelectedRecentGraph);
        }

        #endregion

        #region GraphTemplatesDoubleClickedCommand

        private RelayCommand graphTemplatesDoubleClickedCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand GraphTemplatesDoubleClickedCommand
        {
            get
            {
                return graphTemplatesDoubleClickedCommand ?? (graphTemplatesDoubleClickedCommand = new RelayCommand(GraphTemplatesDoubleClickedExe, GraphTemplatesDoubleClickedCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool GraphTemplatesDoubleClickedCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void GraphTemplatesDoubleClickedExe(object param)
        {
            ShowCreateGraphDialog(SelectedGraphTemplate.DOTDescription);
        }

        #endregion 


        public WelcomeViewVM()
        {
            string recentFilesSetting = Properties.Settings.Default["RecentFiles"].ToString();
            List<GraphFile> recentGraphs = new List<GraphFile>();
            if (!string.IsNullOrWhiteSpace(recentFilesSetting))
            {
                recentGraphs = JsonConvert.DeserializeObject<List<GraphFile>>(recentFilesSetting);
            }
            if (recentGraphs.Count > 0)
            {
                recentGraphs.RemoveAll(x => string.IsNullOrEmpty(x.FilePath) || !File.Exists(x.FilePath));
                recentGraphs = recentGraphs.GroupBy(x => x.FilePath).Select(y => y.OrderByDescending(r => r.LastOpened).First()).ToList();
                Properties.Settings.Default["RecentFiles"] = JsonConvert.SerializeObject(recentGraphs);
                Properties.Settings.Default.Save();
            }
            RecentGraphs = new ObservableCollection<GraphFile>(recentGraphs);
            FilteredRecentGraphs = new ObservableCollection<GraphFile>(RecentGraphs);
            OpenTemplates();
        }

        private void OpenTemplates()
        {
            if (Directory.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates")))
            {
                string[] files = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates"), "*.template", SearchOption.AllDirectories);
                foreach (string templateFilePath in files)
                {
                    GraphTemplate template = JsonConvert.DeserializeObject<GraphTemplate>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", templateFilePath)));
                    template.SetTemplateFilePath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", templateFilePath));
                    GraphTemplates.Add(template);
                }
            }

            if (GraphTemplates.Count > 0)
            {
                RaisePropertyChanged("GraphTemplates");
                SelectedGraphTemplate = GraphTemplates.ElementAt(0); 
            }
        }
    }
}
