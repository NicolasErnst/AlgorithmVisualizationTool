using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AlgorithmVisualizationTool.Model.Validations
{
    class NameValidationRule : ValidationRule
    {
        public int MinLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string nameValue = "";

            try
            {
                nameValue = ((string)value).Trim();
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            nameValue = nameValue.Trim();

            if (string.IsNullOrWhiteSpace(nameValue) || nameValue.Length < MinLength)
            {
                return new ValidationResult(false, $"A name must consist of at least {MinLength} character.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
