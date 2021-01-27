using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public class DOTParsingResult
    {
        public bool Success { get; private set; }

        public string ErrorMessage { get; private set; }

        public int ErrorLine { get; private set; }


        public DOTParsingResult(bool success) : this(success, "", 0)
        {
            if (!success)
            {
                throw new ArgumentException("In case of an unsuccessful parsing, the error and the line of the error must be specified as well."); 
            }
        }

        public DOTParsingResult(bool success, string errorStatement, int errorLine)
        {
            Success = success;
            ErrorMessage = errorStatement;
            ErrorLine = errorLine; 
        }
    }
}
