using AlgorithmVisualizationTool.Model.UndoRedo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizationTool.Model.Graph
{
    class AlgorithmStep : UndoRedoAction 
    {
        public AlgorithmStep(Action doAction, Action undoAction) : base(doAction, undoAction)
        {
            // Nothing to do here
        }
    }
}
