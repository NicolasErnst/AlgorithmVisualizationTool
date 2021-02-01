using GraphAlgorithmPlugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AlgorithmVisualizationTool.Model.MVVM
{
    public sealed class GraphDirectionTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                GraphDirectionType direction = (GraphDirectionType)value;
                if (direction == GraphAlgorithmPlugin.GraphDirectionType.Directed)
                {
                    return "directed";
                }
                else if (direction == GraphAlgorithmPlugin.GraphDirectionType.Undirected)
                {
                    return "undirected";
                }
                return "not defined";
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not supported
            return null; 
        }
    }
}
