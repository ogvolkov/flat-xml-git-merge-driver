
using System;

namespace ResX.Git.Merge.Driver
{
    public class LongestCommonSubsequence<T>
    {
        private readonly int[,] _partialLengths;

        public LongestCommonSubsequence(T[] first, T[] second)
        {
            _partialLengths = new int[first.Length + 1, second.Length + 1];

            for (int i = 1; i <= first.Length; i++)
            {
                for (int j = 1; j <= second.Length; j++)
                {
                    if (first[i - 1].Equals(second[j - 1]))
                    {
                        _partialLengths[i, j] = _partialLengths[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        _partialLengths[i, j] = Math.Max(_partialLengths[i, j-1], _partialLengths[i - 1, j]);
                    }
                }
            }
        }

        public int this[int i, int j] => _partialLengths[i, j];
    }
}
