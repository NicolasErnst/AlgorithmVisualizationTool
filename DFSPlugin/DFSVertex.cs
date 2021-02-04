using GraphAlgorithmPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFSPlugin
{
    public class DFSVertex : Vertex
    {
        #region PushTime

        private int pushTime = 0;

        /// <summary>
        /// 
        /// </summary>
        public int PushTime
        {
            get
            {
                return pushTime;
            }
            set
            {
                if (pushTime == value)
                {
                    return;
                }

                pushTime = value;

                UpdateContent();
            }
        }

        #endregion

        #region PopTime

        private int popTime = 0;

        /// <summary>
        /// 
        /// </summary>
        public int PopTime
        {
            get
            {
                return popTime;
            }
            set
            {
                if (popTime == value)
                {
                    return;
                }

                popTime = value;

                UpdateContent();
            }
        }

        #endregion


        public DFSVertex() : base()
        {
            // Nothing to do here
        }

        public DFSVertex(string vertexName) : base(vertexName)
        {
            // Nothing to do here
        }


        private void UpdateContent()
        {
            string content = "-";

            if (PushTime > 0)
            {
                if (PopTime > 0)
                {
                    content = PushTime + " / " + PopTime;
                }
                else
                {
                    content = PushTime + " / -";
                }
            }

            VertexContent = content;
        }
    }
}
