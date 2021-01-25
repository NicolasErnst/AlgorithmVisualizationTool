using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaktionslogik für NumberInputDialog.xaml
    /// </summary>
    public partial class NumberInputDialog : Window
    {
        public MessageBoxResult Result { get; private set; }

        public int Value { get; private set; }


        public NumberInputDialog(string title, int defaultValue)
        {
            InitializeComponent();
            Title = title;
            ScrollBar.Minimum = 0;
            ScrollBar.Maximum = int.MaxValue;
            ScrollBar.Value = defaultValue;
            NumberInput.Focus();

            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void NumberInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Value = int.Parse(NumberInput.Text);
            Result = MessageBoxResult.OK;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            DialogResult = true;
            Close();
        }
    }
}
