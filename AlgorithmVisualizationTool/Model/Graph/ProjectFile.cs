using GraphAlgorithmPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizationTool.Model.Graph
{
    public class ProjectFile
    {
        #region Graph

        private GraphFile graph = null;

        /// <summary>
        /// 
        /// </summary>
        public GraphFile Graph
        {
            get
            {
                return graph;
            }
            set
            {
                if (graph == value)
                {
                    return;
                }

                graph = value;
            }
        }

        #endregion

        #region SelectedGraphAlgorithm 

        private string selectedGraphAlgorithm = null;

        /// <summary>
        /// 
        /// </summary>
        public string SelectedGraphAlgorithm
        {
            get
            {
                return selectedGraphAlgorithm;
            }
            set
            {
                if (selectedGraphAlgorithm == value)
                {
                    return;
                }

                selectedGraphAlgorithm = value;
            }
        }

        #endregion

        #region ContainedGraphAlgorithms 

        private IEnumerable<string> containedGraphAlgorithms = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ContainedGraphAlgorithms
        {
            get
            {
                return containedGraphAlgorithms;
            }
            set
            {
                if (containedGraphAlgorithms == value)
                {
                    return;
                }

                containedGraphAlgorithms = value;
            }
        }

        #endregion

        #region MadeAlgorithmSteps

        private int madeAlgorithmSteps = 0;

        /// <summary>
        /// 
        /// </summary>
        public int MadeAlgorithmSteps
        {
            get
            {
                return madeAlgorithmSteps;
            }
            set
            {
                if (madeAlgorithmSteps == value)
                {
                    return;
                }

                madeAlgorithmSteps = value;
            }
        }

        #endregion

        #region StartVertex

        private string startVertex = "";

        /// <summary>
        /// 
        /// </summary>
        public string StartVertex
        {
            get
            {
                return startVertex;
            }
            set
            {
                if (startVertex == value)
                {
                    return;
                }

                startVertex = value;
            }
        }

        #endregion
    }
}
