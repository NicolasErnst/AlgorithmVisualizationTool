using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public interface IGraphAlgorithmExecutor
    {
        double Progress { get; set; }
        string ProgressText { get; set; }
        Task MakeAlgorithmStep(Action doAction, Action undoAction, CancellationToken cancellationToken);
        void FinishedAlgorithm(bool success);
    }
}
