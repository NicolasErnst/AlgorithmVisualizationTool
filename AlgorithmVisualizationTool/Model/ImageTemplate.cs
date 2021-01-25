using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AlgorithmVisualizationTool.Model
{
    class ImageTemplate
    {
        // TODO: Image as Bitmap, Created by Graph Framework during initialization of Templates

        #region Image

        public ImageSource Image
        {
            get
            {
                byte[] imageData = File.ReadAllBytes(@"Resources\nodes.png");
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                ImageSource imgSrc = biImg;
                return imgSrc;
            }
        }

        #endregion

        #region ImageDescription

        private string imageDescription = "";

        /// <summary>
        /// 
        /// </summary>
        public string ImageDescription
        {
            get
            {
                return imageDescription;
            }
            set
            {
                if (imageDescription == value)
                {
                    return;
                }

                imageDescription = value;
            }
        }

        #endregion


        public ImageTemplate(string imageDescription)
        {
            ImageDescription = imageDescription;
        }
    }
}
