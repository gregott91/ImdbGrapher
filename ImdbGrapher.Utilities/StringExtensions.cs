using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbGrapher.Utilities
{
    public static class StringExtensions
    {
        /// <summary>
        /// Compares similarity of 2 strings based on Levenshtein distance
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="compareString">String to compare</param>
        /// <returns>Factor of similarity. Less is more similar</returns>
        public static int CompareSimilarity(this string source, string compareString)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(compareString))
                {
                    return 0;
                }

                return compareString.Length;
            }

            if (string.IsNullOrEmpty(compareString))
            {
                return source.Length;
            }

            int sourceLength = source.Length;
            int compareLength = compareString.Length;
            int[,] d = new int[sourceLength + 1, compareLength + 1];

            for (int i = 0; i <= sourceLength; d[i, 0] = i++) ;
            for (int j = 1; j <= compareLength; d[0, j] = j++) ;

            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= compareLength; j++)
                {
                    int cost = (compareString[j - 1] == source[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[sourceLength, compareLength];
        }
    }
}

