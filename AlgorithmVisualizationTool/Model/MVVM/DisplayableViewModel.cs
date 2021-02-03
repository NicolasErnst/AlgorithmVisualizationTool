using AlgorithmVisualizationTool.Controls;
using AlgorithmVisualizationTool.Model.Graph;
using AlgorithmVisualizationTool.ViewModel;
using GraphAlgorithmPlugin;
using Ionic.Zip;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgorithmVisualizationTool.Model.MVVM
{
    public abstract class DisplayableViewModel : ViewModelBase
    {
        #region KeyExitCommand 

        public virtual Action KeyExitCommand
        {
            get
            {
                return new Action(() => Application.Current.Shutdown());
            }
        }

        #endregion 

        #region KeyOpenCommand 

        public virtual Action KeyOpenCommand
        {
            get
            {
                return new Action(() => ShowOpenGraphDialog());
            }
        }

        #endregion 

        #region KeySaveCommand 

        public virtual Action KeySaveCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyNewCommand 

        public virtual Action KeyNewCommand
        {
            get
            {
                return new Action(() => ShowCreateGraphDialog());
            }
        }

        #endregion 

        #region KeyCopyCommand 

        public virtual Action KeyCopyCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyPasteCommand 

        public virtual Action KeyPasteCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyUndoCommand 

        public virtual Action KeyUndoCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyRedoCommand 

        public virtual Action KeyRedoCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion

        #region KeyInfoCommand

        public virtual Action KeyInfoCommand
        {
            get
            {
                return new Action(() => { new InfoDialog().ShowDialog(); });
            }
        }

        #endregion 


        protected void ShowCreateGraphDialog(string dotContent = "")
        {
            TextInputDialog nameDialog = new TextInputDialog();
            nameDialog.Title = "Enter a graph name...";
            if (nameDialog.ShowDialog() == true)
            {
                GraphFile graph = new GraphFile(nameDialog.Input, dotContent);
                OpenGraph(graph);
            }
        }

        protected void ShowOpenGraphDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select a graph to open...",
                Filter = "Graph Files (*.graph)|*.graph",
                Multiselect = false
            };

            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.FileName;
                OpenGraph(fileName);
            }
        }

        protected void ShowImportProjectDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select a graph project to import...",
                Filter = "Graph Projects (*.gpr)|*.gpr",
                Multiselect = false
            };

            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.FileName;
                ImportProjectFile(fileName);
            }
        }

        protected void ShowOpenGraphOrImportProjectDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select a graph to open or a graph project to import...",
                Filter = "Graph Files (*.graph)|*.graph|Graph Projects (*.gpr)|*.gpr",
                Multiselect = false
            };

            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.FileName;
                if (Path.GetExtension(fileName).Equals(".gpr"))
                {
                    ImportProjectFile(fileName);
                }
                else if (Path.GetExtension(fileName).Equals(".graph"))
                {
                    OpenGraph(fileName); 
                }
            }
        }

        public void OpenGraph(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    GraphFile graph = JsonConvert.DeserializeObject<GraphFile>(File.ReadAllText(filePath));
                    graph.FilePath = filePath; 
                    OpenGraph(graph);
                }
                catch (Exception)
                {
                    ShowOpenOrImportErrorMessage();
                }
            }
        }

        public void OpenGraphFromContent(string content, string selectedAlgorithm, int madeAlgorithmSteps, string startVertex)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    GraphFile graph = JsonConvert.DeserializeObject<GraphFile>(content);
                    ShowingView = new GraphViewVM(graph, selectedAlgorithm, madeAlgorithmSteps, startVertex);
                }
                catch (Exception) { }
            }
        }

        public void OpenGraph(GraphFile file)
        {
            ShowingView = new GraphViewVM(file);
        }

        public void ImportProjectFile(string filePath)
        {
            try
            {
                string graphFileContent = "";
                dynamic projectFile = null; 
                using (var zip = ZipFile.Read(filePath))
                {
                    int totalEntries = zip.Entries.Count;
                    foreach (ZipEntry e in zip.Entries)
                    {
                        if (Path.GetExtension(e.FileName).Equals(".graph"))
                        {
                            using (MemoryStream reader = new MemoryStream())
                            {
                                e.Extract(reader);
                                reader.Position = 0;
                                using (var streamReader = new StreamReader(reader))
                                {
                                    graphFileContent = streamReader.ReadToEnd();
                                }
                            }
                        }
                        else if (Path.GetExtension(e.FileName).Equals(".project"))
                        {
                            using (MemoryStream reader = new MemoryStream())
                            {
                                e.Extract(reader);
                                reader.Position = 0;
                                using (var streamReader = new StreamReader(reader))
                                {
                                    projectFile = JsonConvert.DeserializeObject<ProjectDefinitionFile>(streamReader.ReadToEnd());
                                }
                            }
                        }
                        else if (Path.GetExtension(e.FileName).Equals(".dll"))
                        {
                            if (!File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms", e.FileName))) {
                                e.Extract(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Algorithms", e.FileName));
                            }
                        }
                    }

                    if (projectFile != null && projectFile.SelectedAlgorithm != null)
                    {
                        if (projectFile.MadeAlgorithmSteps != null)
                        {
                            OpenGraphFromContent(graphFileContent, projectFile.SelectedAlgorithm, projectFile.MadeAlgorithmSteps, projectFile.StartVertex);
                        }
                        else
                        {
                            OpenGraphFromContent(graphFileContent, projectFile.SelectedAlgorithm, 0, null);
                        }
                    }
                }
            }
            catch (Exception)
            {
                ShowOpenOrImportErrorMessage();
            }
        }

        public void ExportProjectFile(ProjectFile project, string filePath)
        {
            try
            {
                if (!Directory.Exists("export"))
                {
                    Directory.CreateDirectory("export");
                }
                using (ZipFile zip = new ZipFile())
                using (var tempFiles = new TempFileCollection("export", false))
                {
                    JsonSerializer serializer = new JsonSerializer()
                    {
                        NullValueHandling = NullValueHandling.Include
                    };

                    string graphFileName = tempFiles.AddExtension("graph");
                    using (FileStream fs = File.OpenWrite(graphFileName))
                    using (StreamWriter sw = new StreamWriter(fs))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, project.Graph);
                    }
                    zip.AddFile(graphFileName, "");

                    foreach (string algorithmPluginPath in project.ContainedGraphAlgorithms)
                    {
                        zip.AddFile(algorithmPluginPath, "");
                    }

                    string projectFileName = tempFiles.AddExtension("project");
                    using (FileStream fs = File.OpenWrite(projectFileName))
                    using (StreamWriter sw = new StreamWriter(fs))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        ProjectDefinitionFile pdf = new ProjectDefinitionFile()
                        {
                            SelectedAlgorithm = Path.GetFileName(project.SelectedGraphAlgorithm),
                            MadeAlgorithmSteps = project.MadeAlgorithmSteps,
                            StartVertex = project.StartVertex
                        };
                        serializer.Serialize(writer, pdf);
                    }
                    zip.AddFile(projectFileName, "");
                    zip.Save(filePath);
                }
                MessageBox.Show("The project was exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } 
            catch (Exception)
            {
                MessageBox.Show("The project could not be exported.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowOpenOrImportErrorMessage()
        {
            MessageBox.Show("The graph file could not be opened. Please check the existence and the validity of the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
