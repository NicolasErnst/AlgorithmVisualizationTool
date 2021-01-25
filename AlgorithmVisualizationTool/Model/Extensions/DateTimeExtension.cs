using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizationTool.Model.Extensions
{
    static class DateTimeExtension
    {
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy - HH:mm:ss");
        }
    }
}
