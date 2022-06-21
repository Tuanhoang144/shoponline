using System;
using System.Collections.Generic;

namespace VMLLib.ExtensionMethods
{
    public static class VectorExtensionMethods
    {
        public static double[] Remap(this double[] valueArray, double from1, double to1, double from2, double to2)
        {
            List<double> result = new List<double>();

            foreach (double value in valueArray)
                result.Add((value - from1) / (to1 - from1) * (to2 - from2) + from2);

            return result.ToArray();
        }

        public static void PrintArray(this double[] valueArray)
        {
            Console.WriteLine(String.Format("Array: {0}", string.Join(", ", valueArray)));
        }
    }
}
