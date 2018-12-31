
using System.Collections.Generic;

namespace ResX.Git.Merge.Driver
{
    public class TwoWayMerge
    {
        public T[] Merge<T>(T[] x, T[] y)
        {
            var lcs = new LongestCommonSubsequence<T>(x, y);

            var result = new Stack<T>();

            int i = x.Length;
            int j = y.Length;

            while (i > 0 && j > 0)
            {
                if (lcs[i - 1, j] == lcs[i, j])
                {
                    // i is not in the LCS
                    if (lcs[i, j - 1] == lcs[i, j])
                    {
                        // j is also not in the LCS, can't have both
                        throw new MergeConflictException();
                    }
                    else
                    {
                        // ith element has been removed, skip it and advance
                        --i;
                    }
                }
                else
                {
                    // i is in the LCS
                    if (lcs[i, j - 1] == lcs[i, j])
                    {
                        // j is not in the LCS, should add it and advance
                        result.Push(y[--j]);
                    }
                    else
                    {
                        // both are in the LCS, so should be equal - take either and advance both
                        result.Push(y[--j]);
                        --i;
                    }
                }
            }

            while (j > 0)
            {
                result.Push(y[--j]);
            }

            return result.ToArray();
        }
    }
}
