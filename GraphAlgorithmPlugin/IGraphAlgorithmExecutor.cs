using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public interface IGraphAlgorithmExecutor
    {
        double Progress { get; set; }
        string ProgressText { get; set; }
        void MakeAlgorithmStep(Action doAction, Action undoAction);
    }
}
