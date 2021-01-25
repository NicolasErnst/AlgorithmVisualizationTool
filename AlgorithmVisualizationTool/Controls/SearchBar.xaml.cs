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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlgorithmVisualizationTool.Controls
{
    /// <summary>
    /// Interaktionslogik für SearchBar.xaml
    /// </summary>
    public partial class SearchBar : TextBox
    {
        #region Placeholder

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(SearchBar), new PropertyMetadata(""));

        #endregion

        private TextBox searchText;
        private TextBox placeholderText;


        public SearchBar()
        {
            InitializeComponent();
        }


        public override void OnApplyTemplate()
        {
            searchText = GetTemplateChild("SearchText") as TextBox;
            placeholderText = GetTemplateChild("PlaceholderText") as TextBox;
        }

        private void PlaceholderText_GotFocus(object sender, RoutedEventArgs e)
        {
            placeholderText.Visibility = Visibility.Hidden;
            searchText.Focus();
            searchText.CaretIndex = int.MaxValue;
        }

        private void SearchText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchText.Text) || string.IsNullOrWhiteSpace(searchText.Text))
                placeholderText.Visibility = Visibility.Visible;
        }
    }
}
