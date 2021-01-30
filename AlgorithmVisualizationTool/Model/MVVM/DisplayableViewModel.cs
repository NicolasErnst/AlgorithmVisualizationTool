using AlgorithmVisualizationTool.Controls;
using AlgorithmVisualizationTool.Model.Graph;
using AlgorithmVisualizationTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgorithmVisualizationTool.Model.MVVM
{
    abstract class DisplayableViewModel : ViewModelBase
    {
        #region KeyExitCommand 

        public virtual Action KeyExitCommand
        {
            get
            {
                return new Action(() => Application.Current.Shutdown());
            }
        }

        #endregion 

        #region KeyOpenCommand 

        public virtual Action KeyOpenCommand
        {
            get
            {
                return new Action(() => ShowOpenGraphDialog());
            }
        }

        #endregion 

        #region KeySaveCommand 

        public virtual Action KeySaveCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyNewCommand 

        public virtual Action KeyNewCommand
        {
            get
            {
                return new Action(() => ShowCreateGraphDialog());
            }
        }

        #endregion 

        #region KeyCopyCommand 

        public virtual Action KeyCopyCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyPasteCommand 

        public virtual Action KeyPasteCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyUndoCommand 

        public virtual Action KeyUndoCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion 

        #region KeyRedoCommand 

        public virtual Action KeyRedoCommand
        {
            get
            {
                return new Action(() => { });
            }
        }

        #endregion

        #region KeyInfoCommand

        public virtual Action KeyInfoCommand
        {
            get
            {
                return new Action(() => { new InfoDialog().ShowDialog(); });
            }
        }

        #endregion 


        protected void ShowCreateGraphDialog()
        {
            // TODO: SFD and Name
            // TODO: Add DOT if template selected
            OpenGraph(null);
        }

        protected void ShowOpenGraphDialog()
        {
            // TODO: ODF 
            OpenGraph(null);
        }

        protected void OpenGraph(GraphFile fileToOpen)
        {
            if (fileToOpen != null)
            {
                ShowingView = new GraphViewVM(fileToOpen);
            }
            else
            {
                MessageBox.Show("The graph file could not be opened. Please check the existence and the validity of the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
