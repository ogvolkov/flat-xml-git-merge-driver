
using System.ComponentModel;
using Xunit;

namespace ResX.Git.Merge.Driver.Tests
{
    public class LongestCommonSubsequenceTests
    {
        [Fact]
        public void OneElementSame()
        {
            var lcm = Lcm("a", "a");

            AssertMatrix(lcm, new[,]
            {
                { 0, 0 },
                { 0, 1 }
            });

            Assert.Equal("a", lcm.Result);
        }

        [Fact]
        public void OneElementDifferent()
        {
            var lcm = Lcm("a", "b");

            AssertMatrix(lcm, new[,]
            {
                { 0, 0 },
                { 0, 0 }
            });

            Assert.Equal("", lcm.Result);
        }

        [Fact]
        public void SecondHasAddedElements()
        {
            var lcm = Lcm("a", "ab");

            AssertMatrix(lcm, new[,]
            {
                { 0, 0, 0 },
                { 0, 1, 1 }
            });

            Assert.Equal("a", lcm.Result);
        }

        [Fact]
        public void SecondHasRemovedElements()
        {
            var lcm = Lcm("ab", "a");

            AssertMatrix(lcm, new[,]
            {
                { 0, 0 },
                { 0, 1 },
                { 0, 1 }
            });

            Assert.Equal("a", lcm.Result);
        }

        [Fact]
        public void TwoSameElements()
        {
            var lcm = Lcm("ab", "ab");

            AssertMatrix(lcm, new[,]
            {
                { 0, 0, 0 },
                { 0, 1, 1 },
                { 0, 1, 2 }
            });

            Assert.Equal("ab", lcm.Result);
        }

        [Fact]
        public void ComplexAddsAndRemoves()
        {
            var lcm = Lcm("xAyBC", "ABzCu");

            AssertMatrix(lcm, new[,]
            {
                { 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 1, 1 },
                { 0, 1, 1, 1, 1, 1 },
                { 0, 1, 2, 2, 2, 2 },
                { 0, 1, 2, 2, 3, 3 }
            });

            Assert.Equal("ABC", lcm.Result);
        }

        private LongestCommonSubsequence<char> Lcm(string first, string second)
        {
            return new LongestCommonSubsequence<char>(first.ToCharArray(), second.ToCharArray());
        }

        private void AssertMatrix<T>(LongestCommonSubsequence<T> lcm, int[,] expected)
        {
            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    Assert.True(
                        expected[i, j] == lcm[i, j],
                        $"[{i}, {j}] expected to be {expected[i, j]} but was {lcm[i, j]}"
                    );
                }
            }
        }
    }
}
