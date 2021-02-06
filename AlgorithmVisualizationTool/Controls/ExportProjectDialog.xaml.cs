using AlgorithmVisualizationTool.Model.Graph;
using GraphAlgorithmPlugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AlgorithmVisualizationTool.Controls
{
    /// <summary>
    /// Interaktionslogik für ExportProjectDialog.xaml
    /// </summary>
    public partial class ExportProjectDialog : Window
    {
        public string FileName { get; set; }
        public ProjectFile FileToExport { get; set; }

        private readonly int MadeAlgorithmSteps;
        private readonly IGraphAlgorithmPlugin SelectedGraphAlgorithm;
        private readonly List<IGraphAlgorithmPlugin> AvailablePlugins;
        private readonly GraphFile Graph;
        private readonly string StartVertex; 


        public ExportProjectDialog()
        {
            InitializeComponent();
        }

        public ExportProjectDialog(GraphFile graph, string startVertex, int madeAlgorithmSteps, IGraphAlgorithmPlugin selectedGraphAlgorithm, IEnumerable<IGraphAlgorithmPlugin> availablePlugins)
        {
            InitializeComponent();
            Graph = graph;
            StartVertex = startVertex; 
            MadeAlgorithmSteps = madeAlgorithmSteps;
            SelectedGraphAlgorithm = selectedGraphAlgorithm;
            AvailablePlugins = availablePlugins.ToList();
            AvailablePlugins.Remove(selectedGraphAlgorithm);

            for (int i = 0; i < AvailablePlugins.Count(); i++)
            {
                AlgorithmSelection.Items.Add(AvailablePlugins.ElementAt(i).AlgorithmName);
            }

            if (SelectedGraphAlgorithm == null)
            {
                ExportCurrentState.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Export project...",
                Filter = "Graph Project (*.gpr)|*.gpr",
            };

            if (sfd.ShowDialog() == true)
            {
                FileName = sfd.FileName;
                FileNameTextBlock.Text = FileName;
                if (!string.IsNullOrWhiteSpace(FileName))
                {
                    Export.IsEnabled = true;
                }
                else
                {
                    Export.IsEnabled = false; 
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AlgorithmSelection.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AlgorithmSelection.IsEnabled = false;
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            FileToExport = new ProjectFile
            {
                Graph = Graph
            };
            if (ExportCurrentState.IsChecked == true)
            {
                FileToExport.StartVertex = StartVertex;
                FileToExport.SelectedGraphAlgorithm = SelectedGraphAlgorithm.FileName;
                FileToExport.MadeAlgorithmSteps = MadeAlgorithmSteps; 
            }

            List<string> plugins = new List<string>();
            int counter = 0;
            foreach(var item in AlgorithmSelection.Items)
            {
                plugins.Add(AvailablePlugins.ElementAt(counter).FileName);
                counter++; 
            }
            DialogResult = true; 
        }
    }
}
