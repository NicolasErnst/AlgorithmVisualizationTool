using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AlgorithmVisualizationTool.Model
{
    class GraphTemplate
    {
        #region ImagePath

        private string imagePath = "";

        /// <summary>
        /// 
        /// </summary>
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                if (imagePath == value)
                {
                    return;
                }

                imagePath = value;
            }
        }

        #endregion

        #region Image

        [JsonIgnore]
        public ImageSource Image
        {
            get
            {
                BitmapImage biImg = new BitmapImage();
                byte[] imageData = new byte[0];
                if (File.Exists(ImagePath))
                {
                    imageData = File.ReadAllBytes(ImagePath);
                    MemoryStream ms = new MemoryStream(imageData);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                }
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

        #region DOTDescription

        private string dotDescription = "";

        /// <summary>
        /// 
        /// </summary>
        public string DOTDescription
        {
            get
            {
                return dotDescription;
            }
            set
            {
                if (dotDescription == value)
                {
                    return;
                }

                dotDescription = value;
            }
        }

        #endregion


        public GraphTemplate()
        {
            ImagePath = @"Resources\nodes.png";
            ImageDescription = "";
            DOTDescription = "";
        }

        public GraphTemplate(string imageDescription, string dotDescription)
        {
            ImagePath = @"Resources\nodes.png";
            ImageDescription = imageDescription;
            DOTDescription = dotDescription; 
        }

        public GraphTemplate(string imagePath, string imageDescription, string dotDescription)
        {
            if (File.Exists(imagePath))
            {
                ImagePath = imagePath;
            }
             else
            {
                ImagePath = @"Resources\nodes.png";
            }
            ImageDescription = imageDescription;
            DOTDescription = dotDescription;
        }


        public void SetTemplateFilePath(string templateFilePath)
        {
            string templateDirectory = Path.GetDirectoryName(templateFilePath);
            ImagePath = Path.Combine(templateDirectory, ImagePath); 
        }
    }
}
