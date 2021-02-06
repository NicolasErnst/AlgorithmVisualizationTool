using GraphAlgorithmPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StrongConnectedComponentsPlugin
{
    public class SCCVertex : Vertex
    {
        #region SccID

        private int sccID = 0;

        /// <summary>
        /// 
        /// </summary>
        public int SccID
        {
            get
            {
                return sccID;
            }
            set
            {
                if (sccID == value)
                {
                    return;
                }

                sccID = value;

                UpdateContent();
            }
        }

        #endregion

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


        public SCCVertex()
        {
            UpdateContent();
        }


        private void UpdateContent()
        {
            string content = "";
            if (SccID > 0)
            {
                content = SccID.ToString();
            }
            else
            {
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
            }
            VertexContent = content;
        }
    }
}
