using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizationTool.Model.MVVM
{
    abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Title

        private static string title = "Algorithm Visualization Tool";

        /// <summary>
        /// 
        /// </summary>
        public static string Title
        {
            get
            {
                return title;
            }
            private set
            {
                if (title == value)
                {
                    return;
                }

                title = value;

                RaiseStaticPropertyChanged();
            }
        }

        #endregion

        #region ShowingView

        private static DisplayableViewModel showingView = null;

        /// <summary>
        /// 
        /// </summary>
        public static DisplayableViewModel ShowingView
        {
            get
            {
                return showingView;
            }
            set
            {
                if (showingView == value)
                {
                    return;
                }

                showingView = value;

                RaiseStaticPropertyChanged();
            }
        }

        #endregion


        protected void SetWindowTitle(string titleAddition)
        {
            if (string.IsNullOrWhiteSpace(titleAddition))
            {
                Title = "Algorithm Visualization Tool";
            }
            else
            {
                Title = "Algorithm Visualization Tool - " + titleAddition;
            }
        }


        public static event PropertyChangedEventHandler StaticPropertyChanged;

        protected static void RaiseStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler propertyChanged = StaticPropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
