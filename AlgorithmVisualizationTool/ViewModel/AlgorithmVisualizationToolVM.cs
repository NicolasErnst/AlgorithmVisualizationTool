using AlgorithmVisualizationTool.Model.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgorithmVisualizationTool.ViewModel
{
    class AlgorithmVisualizationToolVM : ViewModelBase
    {
        #region KeyExitCommand

        private RelayCommand keyExitCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeyExitCommand
        {
            get
            {
                return keyExitCommand ?? (keyExitCommand = new RelayCommand(KeyExitExe, KeyExitCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeyExitCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeyExitExe(object param)
        {
            ShowingView?.KeyExitCommand?.Invoke();
        }

        #endregion 

        #region KeyOpenCommand

        private RelayCommand keyOpenCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeyOpenCommand
        {
            get
            {
                return keyOpenCommand ?? (keyOpenCommand = new RelayCommand(KeyOpenExe, KeyOpenCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeyOpenCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeyOpenExe(object param)
        {
            ShowingView?.KeyOpenCommand?.Invoke();
        }

        #endregion 

        #region KeySaveCommand

        private RelayCommand keySaveCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeySaveCommand
        {
            get
            {
                return keySaveCommand ?? (keySaveCommand = new RelayCommand(KeySaveExe, KeySaveCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeySaveCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeySaveExe(object param)
        {
            ShowingView?.KeySaveCommand?.Invoke();
        }

        #endregion 

        #region KeyNewCommand

        private RelayCommand keyNewCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeyNewCommand
        {
            get
            {
                return keyNewCommand ?? (keyNewCommand = new RelayCommand(KeyNewExe, KeyNewCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeyNewCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeyNewExe(object param)
        {
            ShowingView?.KeyNewCommand?.Invoke();
        }

        #endregion 

        #region KeyCopyCommand

        private RelayCommand keyCopyCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeyCopyCommand
        {
            get
            {
                return keyCopyCommand ?? (keyCopyCommand = new RelayCommand(KeyCopyExe, KeyCopyCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeyCopyCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeyCopyExe(object param)
        {
            ShowingView?.KeyCopyCommand?.Invoke();
        }

        #endregion 

        #region KeyPasteCommand

        private RelayCommand keyPasteCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeyPasteCommand
        {
            get
            {
                return keyPasteCommand ?? (keyPasteCommand = new RelayCommand(KeyPasteExe, KeyPasteCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeyPasteCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeyPasteExe(object param)
        {
            ShowingView?.KeyPasteCommand?.Invoke();
        }

        #endregion 

        #region KeyUndoCommand

        private RelayCommand keyUndoCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeyUndoCommand
        {
            get
            {
                return keyUndoCommand ?? (keyUndoCommand = new RelayCommand(KeyUndoExe, KeyUndoCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeyUndoCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeyUndoExe(object param)
        {
            ShowingView?.KeyUndoCommand?.Invoke();
        }

        #endregion 

        #region KeyRedoCommand

        private RelayCommand keyRedoCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeyRedoCommand
        {
            get
            {
                return keyRedoCommand ?? (keyRedoCommand = new RelayCommand(KeyRedoExe, KeyRedoCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeyRedoCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeyRedoExe(object param)
        {
            ShowingView?.KeyRedoCommand?.Invoke();
        }

        #endregion 

        #region KeySpaceCommand

        private RelayCommand keySpaceCommand;

        /// <summary>
        /// Eigenschaft, die das Kommando liefert
        /// </summary>
        public ICommand KeySpaceCommand
        {
            get
            {
                return keySpaceCommand ?? (keySpaceCommand = new RelayCommand(KeySpaceExe, KeySpaceCanExe));
            }
        }

        /// <summary>
        /// Gibt an, ob das Kommando ausgeführt werden kann
        /// <param name="param">Parameter</param>
        /// <returns>Gibt an, ob das Kommando ausgeführt werden kann</returns>
        /// </summary>
        protected virtual bool KeySpaceCanExe(object param)
        {
            return true;
        }

        /// <summary>
        /// Führt das Kommando aus
        /// <param name="param">Parameter</param>
        /// </summary>
        protected virtual void KeySpaceExe(object param)
        {
            ShowingView?.KeySpaceCommand?.Invoke();
        }

        #endregion 


        public AlgorithmVisualizationToolVM()
        {
            ShowingView = new WelcomeViewVM();
        }
    }
}
