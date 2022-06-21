using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using WebAdmin.Models;

namespace ImgSearch
{
    public class SearchImage
    {
        //private ImageSearchAlgorithm _imageSearchAlgorithm;

        //public SearchImage(ImageSearchAlgorithm imageSearchAlgorithm)
        //{
        //    _imageSearchAlgorithm = imageSearchAlgorithm;
        //}

        //public Tuple<int, double> GetMostSimilarImage(Bitmap searchImage, string[] imageFileArray, List<Color> centroidColorList)
        //{
        //    int mostSimilarImageIndex = 0;
        //    double maxSimilarity = 0;

        //    for (int i = 0; i < imageFileArray.Length; i++)
        //    {
        //        string image = imageFileArray[i];
        //        using (Bitmap bmpImage = new Bitmap(Image.FromFile(image), searchImage.Width, searchImage.Height))
        //        {
        //            using (Bitmap bmpPorcessedImage = _imageSearchAlgorithm.ProcessImage(bmpImage, centroidColorList))
        //            {
        //                double similarityPercent = CalculateSimilarity(searchImage, bmpPorcessedImage);
        //                if (similarityPercent > maxSimilarity)
        //                {
        //                    mostSimilarImageIndex = i;
        //                    maxSimilarity = similarityPercent;
        //                }
        //            }
        //        }

        //    }

        //    return new Tuple<int, double>(mostSimilarImageIndex, maxSimilarity);
        //}


        public List<long> SortBySimilarity(Dictionary<int,string > listSearchImageProcessed, List<Color> centroidColorList)
        {
            Dictionary<long, double> similarityDic = new Dictionary<long, double>();
            WebBanHangDB db = new WebBanHangDB();
            var Product = db.Products.Where(x => x.delete != true).ToList();
            foreach (var item in Product)
            {
                double similarityPercent = CalculateSimilarity(listSearchImageProcessed, item.dataSearch.Split(','));
               
                    similarityDic.Add(item.Id, similarityPercent);
                  
            }
            return similarityDic.Where(x=>x.Value>50).OrderByDescending(x => x.Value).Select(x=>x.Key).ToList();
        }
        public List<KeyValuePair<string, double>> SortBySimilarity(Bitmap searchImage, List<KeyValuePair<string, string>> listImg, List<Color> centroidColorList)
        {
               Dictionary<string, double> similarityDic = new Dictionary<string, double>();
            foreach (var item in listImg)
            {
                double similarityPercent = CalculateSimilarity(searchImage, item.Value.Split(','));
                similarityDic.Add(item.Key, similarityPercent);
            }
            //for (int i = 0; i <listImg.Count; i++)
            //{
            //    string image = imageFileArray[i];
            //    using (Bitmap bmpImage = new Bitmap(Image.FromFile(image), searchImage.Width, searchImage.Height))
            //    {
            //        using (Bitmap bmpPorcessedImage = _imageSearchAlgorithm.ProcessImage(bmpImage, centroidColorList))
            //        {
            //            double similarityPercent = CalculateSimilarity(searchImage, bmpPorcessedImage);
            //            similarityDic.Add(image, similarityPercent);
            //        }
            //    }

            //}

            return similarityDic.OrderByDescending(x => x.Value).ToList();
        }
        //public List<KeyValuePair<string, double>> SortBySimilarity(Bitmap searchImage, string[] imageFileArray, List<Color> centroidColorList)
        //{
        //    Dictionary<string, double> similarityDic = new Dictionary<string, double>();

        //    for (int i = 0; i < imageFileArray.Length; i++)
        //    {
        //        string image = imageFileArray[i];
        //        using (Bitmap bmpImage = new Bitmap(Image.FromFile(image), searchImage.Width, searchImage.Height))
        //        {
        //            using (Bitmap bmpPorcessedImage = _imageSearchAlgorithm.ProcessImage(bmpImage, centroidColorList))
        //            {
        //                double similarityPercent = CalculateSimilarity(searchImage, bmpPorcessedImage);
        //                similarityDic.Add(image, similarityPercent);
        //            }
        //        }

        //    }

        //    return similarityDic.OrderByDescending(x => x.Value).ToList();
        //}
        //public double CalculateSimilarity(Bitmap bmpImage1, Bitmap bmpImage2)
        //{
        //    int correct = 0;
        //    for (int i = 0; i < bmpImage1.Width; i++)
        //    {
        //        for (int j = 0; j < bmpImage1.Height; j++)
        //        {
        //            Color c1 = bmpImage1.GetPixel(i, j);
        //            Color c2 = bmpImage2.GetPixel(i, j);
        //            if (c1.ToArgb() == c2.ToArgb())
        //                correct++;
        //        }
        //    }

        //    int maxPixels = bmpImage1.Width * bmpImage1.Height;

        //    double SimilarityPercent = (100.0 * correct) / maxPixels;
        //    return SimilarityPercent;
        //}
        public double CalculateSimilarity(Bitmap bmpImage1,  string [] bmpImage2)
        {
            int correct = 0;
            for (int i = 0; i < bmpImage1.Width; i++)
            {
                for (int j = 0; j < bmpImage1.Height; j++)
                {
                    Color c1 = bmpImage1.GetPixel(i, j);

                    if (c1.ToArgb() == int.Parse(bmpImage2[i + j]))
                    {
                        correct++;
                    }
                    else
                    {
                        Console.WriteLine(c1.ToArgb() + "/" + int.Parse(bmpImage2[i + j]));
                    }
                        
                }
            }

            int maxPixels = bmpImage1.Width * bmpImage1.Height;

            double SimilarityPercent = (100.0 * correct) / maxPixels;
            return SimilarityPercent;
        }


        public double CalculateSimilarity(Dictionary<int, string> bmpImage1, string[] bmpImage2)
        {
            int correct = 0;
            //for (int i = 0; i < bmpImage1.Count; i++)
            //{

            //    if (bmpImage1[i] == int.Parse(bmpImage2[i]))
            //    {
            //        correct++;
            //    }
            //    else
            //    {
            //        Console.WriteLine(c1.ToArgb() + "/" + int.Parse(bmpImage2[i + j]));
            //    }

            //}

            int i = -1;
            var Img = (from c in bmpImage2
                       select new { c,key=++i }).ToList();


            var DataSearch = (from img in
                             bmpImage1.ToList()
                             join im in Img.ToList()
                             on img.Key equals im.key
                             select new {
                             Status=(img.Value.Equals(im.c)?1:0)
                             
                             }).ToList();

            int maxPixels = bmpImage2.Count();

            double SimilarityPercent = (100.0 * DataSearch.Where(x=>x.Status==1).Count()) / maxPixels;
            return SimilarityPercent;
        }
    }
}