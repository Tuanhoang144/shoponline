//using KMeansProject;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Web;

//namespace ImgSearch
//{
//    public class ImageSearchAlgorithm
//    {
//        private KMeans _kmeans;
//        private List<double[]> _dataset;

//        public ImageSearchAlgorithm()
//        {
//            _dataset = new List<double[]>();
//        }

//        public void RunAlgorithm(Bitmap searchImage, int k)
//        {
//            for (int i = 0; i < searchImage.Width; i++)
//            {
//                for (int j = 0; j < searchImage.Height; j++)
//                {


//                    Color c = searchImage.GetPixel(i, j);
//                    double[] pixelArray = new double[] { c.R, c.G, c.B };
//                    _dataset.Add(pixelArray);



//                }
//            }

//            _kmeans = new KMeans(k, new EuclideanDistance());
//            _kmeans.Run(_dataset.ToArray());
//        }

//        public Bitmap ProcessImage(Bitmap image, List<Color> cenotridColorList)
//        {
//            //sadsa
//            Bitmap resultImage = new Bitmap(image.Width, image.Height);

//            for (int i = 0; i < resultImage.Width; i++)
//            {
//                for (int j = 0; j < resultImage.Height; j++)
//                {
//                    Color c = image.GetPixel(i, j);
//                    double[] pixelArray = new double[] { c.R, c.G, c.B };
//                    int resultCentroid = _kmeans.Classify(pixelArray);
//                    Color centroidColor = cenotridColorList[resultCentroid];
//                    resultImage.SetPixel(i, j, centroidColor);
//                }
//            }

//            return resultImage;
//        }
//    }
//}