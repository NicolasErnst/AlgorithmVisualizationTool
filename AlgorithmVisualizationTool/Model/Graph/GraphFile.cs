using AlgorithmVisualizationTool.Model.Extensions;
using AlgorithmVisualizationTool.Model.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizationTool.Model.Graph
{
    class GraphFile : ViewModelBase
    {
        #region Name

        private string name = "";

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region FilePath

        private string filePath = "";

        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                if (filePath == value)
                {
                    return;
                }

                filePath = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region LastOpened

        private DateTime lastOpened = DateTime.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastOpened
        {
            get
            {
                return lastOpened;
            }
            set
            {
                if (lastOpened == value)
                {
                    return;
                }

                lastOpened = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region LastOpenedToString

        public string LastOpenedToString
        {
            get
            {
                return lastOpened.ToDateTimeString();
            }
        }

        #endregion

        #region LastModified

        private DateTime lastModified = DateTime.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return lastModified;
            }
            set
            {
                if (lastModified == value)
                {
                    return;
                }

                lastModified = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        #region LastModifiedToString

        public string LastModifiedToString
        {
            get
            {
                return LastModified.ToDateTimeString();
            }
        }

        #endregion

        #region LastModifier

        private string lastModifier = "";

        /// <summary>
        /// 
        /// </summary>
        public string LastModifier
        {
            get
            {
                return lastModifier;
            }
            set
            {
                if (lastModifier == value)
                {
                    return;
                }

                lastModifier = value;

                RaisePropertyChanged();
            }
        }

        #endregion


        public GraphFile(string name, string filePath, DateTime lastOpened, DateTime lastModified, string lastModifier)
        {
            Name = name;
            FilePath = filePath;
            LastOpened = lastOpened;
            LastModified = lastModified;
            LastModifier = lastModifier;
        }


        public async Task Save()
        {
            // TODO
            Console.WriteLine("SAVED FILE!");
        }

        public void UpdateModification()
        {
            LastModified = DateTime.Now;
            LastModifier = Environment.UserName;
        }
    }
}
