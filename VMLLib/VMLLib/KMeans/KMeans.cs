using System;
using System.Collections.Generic;
using System.Threading;

namespace KMeansProject
{
    public delegate void OnUpdateProgress(object sender, KMeansEventArgs eventArgs);
    public class KMeans
    {
        private IDistance _distance;
        private int _k;
        private List<Centroid> centroidList;

        public event OnUpdateProgress UpdateProgress;
        protected virtual void OnUpdateProgress(KMeansEventArgs eventArgs)
        {
            if (UpdateProgress != null)
                UpdateProgress(this, eventArgs);
            Thread.Sleep(1500);
        }

        public KMeans(int k, IDistance distance)
        {
            _k = k;
            _distance = distance;
        }

        public int Classify(double[] input)
        {
            int closestIndex = -1;
            double minDistance = Double.MaxValue;
            for (int k = 0; k < centroidList.Count; k++)
            {
                double distance = _distance.Run(centroidList[k].Array, input);
                if (distance < minDistance)
                {
                    closestIndex = k;
                    minDistance = distance;
                }
            }
            return closestIndex;
        }

        public Centroid[] Run(double[][] dataSet)
        {
             centroidList = new List<Centroid>();

            for (int i = 0; i < _k; i++)
            {
                Centroid centroid = new Centroid(dataSet);
                centroidList.Add(centroid);
            }

            OnUpdateProgress(new KMeansEventArgs(centroidList, dataSet));

            while (true)
            {
                foreach (Centroid centroid in centroidList)
                    centroid.Reset();

                for (int i = 0; i < dataSet.GetLength(0); i++)
                {
                    double[] point = dataSet[i];
                    int closestIndex = -1;
                    double minDistance = Double.MaxValue;
                    for (int k = 0; k < centroidList.Count; k++)
                    {
                        double distance = _distance.Run(centroidList[k].Array, point);
                        if (distance < minDistance)
                        {
                            closestIndex = k;
                            minDistance = distance;
                        }
                    }
                    centroidList[closestIndex].addPoint(point);
                }

                foreach (Centroid centroid in centroidList)
                    centroid.MoveCentroid();

                OnUpdateProgress(new KMeansEventArgs(centroidList, null));

                bool hasChanged = false;
                foreach (Centroid centroid in centroidList)
                    if (centroid.HasChanged())
                    {
                        hasChanged = true;
                        break;
                    }
                if (!hasChanged)
                    break;
            }

            return centroidList.ToArray();
        }
    }
}
