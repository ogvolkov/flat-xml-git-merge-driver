
using System;

namespace ResX.Git.Merge.Driver
{
    public class LongestCommonSubsequence<T>
    {
        private readonly int[,] _c;

        public LongestCommonSubsequence(T[] x, T[] y)
        {
            _c = new int[x.Length + 1, y.Length + 1];

            for (int i = 1; i <= x.Length; i++)
            {
                for (int j = 1; j <= y.Length; j++)
                {
                    if (x[i - 1].Equals(y[j - 1]))
                    {
                        _c[i, j] = _c[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        _c[i, j] = Math.Max(_c[i, j-1], _c[i - 1, j]);
                    }
                }
            }
        }

        public int this[int i, int j] => _c[i, j];
    }
}
