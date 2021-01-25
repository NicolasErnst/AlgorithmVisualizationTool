using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizationTool.Model.UndoRedo
{
    abstract class UndoRedoStack<T> where T : UndoRedoAction
    {
        private readonly Stack<T> UndoStack;
        private readonly Stack<T> RedoStack;
        public int UndoCount => UndoStack.Count;
        public int RedoCount => RedoStack.Count; 


        public UndoRedoStack()
        {
            UndoStack = new Stack<T>();
            RedoStack = new Stack<T>();
        }


        public void Do(T action)
        {
            action.Do();
            UndoStack.Push(action);
            RedoStack.Clear(); 
        }

        public void Undo()
        {
            if (UndoStack.Count > 0)
            {
                T action = UndoStack.Pop();
                action.Undo();
                RedoStack.Push(action);
            }
        }

        public void Redo()
        {
            if (RedoStack.Count > 0)
            {
                T action = RedoStack.Pop();
                action.Do();
                UndoStack.Push(action);
            }
        }

        public void Reset()
        {
            UndoStack.Clear();
            RedoStack.Clear();
        }
    }
}
