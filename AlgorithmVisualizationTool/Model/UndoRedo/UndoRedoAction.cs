using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizationTool.Model.UndoRedo
{
    abstract class UndoRedoAction
    {
        private readonly Action DoAction;
        private readonly Action UndoAction;


        public UndoRedoAction(Action doAction, Action undoAction)
        {
            DoAction = doAction;
            UndoAction = undoAction;
        }


        public void Do()
        {
            DoAction?.Invoke();
        }

        public void Undo()
        {
            UndoAction?.Invoke();
        }
    }
}
