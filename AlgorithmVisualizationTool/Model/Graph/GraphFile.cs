using AlgorithmVisualizationTool.Model.Extensions;
using AlgorithmVisualizationTool.Model.MVVM;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace AlgorithmVisualizationTool.Model.Graph
{
    public class GraphFile : ViewModelBase
    {
        #region Name

        private string name = "";

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region FilePath

        private string filePath = "";

        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                if (filePath == value)
                {
                    return;
                }

                filePath = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region LastOpened

        private DateTime lastOpened = DateTime.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastOpened
        {
            get
            {
                return lastOpened;
            }
            set
            {
                if (lastOpened == value)
                {
                    return;
                }

                lastOpened = value;

                RaisePropertyChanged();
                RaisePropertyChanged("LastOpenedToString");
            }
        }

        #endregion

        #region LastOpenedToString

        public string LastOpenedToString
        {
            get
            {
                return lastOpened.ToDateTimeString();
            }
        }

        #endregion

        #region LastModified

        private DateTime lastModified = DateTime.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return lastModified;
            }
            set
            {
                if (lastModified == value)
                {
                    return;
                }

                lastModified = value;

                RaisePropertyChanged();
                RaisePropertyChanged("LastModifiedToString"); 
            }
        }

        #endregion

        #region LastModifiedToString

        public string LastModifiedToString
        {
            get
            {
                return LastModified.ToDateTimeString();
            }
        }

        #endregion

        #region LastModifier

        private string lastModifier = "";

        /// <summary>
        /// 
        /// </summary>
        public string LastModifier
        {
            get
            {
                return lastModifier;
            }
            set
            {
                if (lastModifier == value)
                {
                    return;
                }

                lastModifier = value;

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
            }
        }

        #endregion


        public GraphFile()
        {
            Name = "";
            FilePath = "";
            LastOpened = DateTime.Now;
            LastModified = DateTime.Now;
            LastModifier = Environment.UserName;
            DOTDescription = "";
        }

        public GraphFile(string name, string filePath, DateTime lastOpened, DateTime lastModified, string lastModifier, string dotDescription)
        {
            Name = name;
            FilePath = filePath;
            LastOpened = lastOpened;
            LastModified = lastModified;
            LastModifier = lastModifier;
            DOTDescription = dotDescription;
        }

        public GraphFile(string name, string dotDescription)
        {
            Name = name;
            FilePath = "";
            LastOpened = DateTime.Now;
            LastModified = DateTime.Now;
            LastModifier = Environment.UserName;
            DOTDescription = dotDescription; 
        }


        public void Save()
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                SaveAs(FilePath);
            }
            else
            {
                OpenSaveAsDialog();
            }   
        }

        public void OpenSaveAsDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Save graph file as...",
                Filter = "Graph Files (*.graph)|*.graph",
            };

            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;

                if (string.IsNullOrWhiteSpace(FilePath))
                {
                    FilePath = fileName;
                    SaveAs(fileName);
                }
                else
                {
                    SaveAs(fileName);
                    MessageBoxResult result = MessageBox.Show("Do you want to open the newly saved file?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        ShowingView?.OpenGraph(fileName);
                    }
                }
            }
        }

        public async void SaveAs(string saveFilePath, bool updateModification = true)
        {
            if (updateModification)
            {
                UpdateModification();
            }
            string recentFilesSetting = Properties.Settings.Default["RecentFiles"].ToString();
            List<GraphFile> recentGraphs = new List<GraphFile>();
            if (!string.IsNullOrWhiteSpace(recentFilesSetting))
            {
                recentGraphs = JsonConvert.DeserializeObject<List<GraphFile>>(recentFilesSetting);
            }
            recentGraphs.RemoveAll(x => string.IsNullOrEmpty(x.FilePath) || !File.Exists(x.FilePath));
            recentGraphs.RemoveAll(x => x.FilePath.Equals(this.FilePath));
            recentGraphs.Insert(0, this);
            Properties.Settings.Default["RecentFiles"] = JsonConvert.SerializeObject(recentGraphs);
            Properties.Settings.Default.Save();
            await Task.Run(() =>
            {
                JsonSerializer serializer = new JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Include
                };
                using (StreamWriter sw = new StreamWriter(saveFilePath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, this);
                }
            }); 
        }

        private void UpdateModification()
        {
            LastModified = DateTime.Now;
            LastModifier = Environment.UserName;
        }
    }
}
