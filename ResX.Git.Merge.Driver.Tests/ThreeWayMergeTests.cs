
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ResX.Git.Merge.Driver.Tests
{
    public class ThreeWayMergeTests
    {
        [Fact]
        public void SecondTheSameAsAncestor()
        {
            string mergeResult = Merge("ab", "abc", "ab");

            Assert.Equal("abc", mergeResult);
        }

        [Fact]
        public void FirstTheSameAsAncestor()
        {
            string mergeResult = Merge("ab", "ab", "abc");

            Assert.Equal("abc", mergeResult);
        }

        [Fact]
        public void SameInserts()
        {
            string mergeResult = Merge("ab", "abc", "abc");

            Assert.Equal("abc", mergeResult);
        }

        [Fact]
        public void NonOverlappingInserts()
        {
            string mergeResult = Merge("abc", "aXbc", "abYc");

            Assert.Equal("aXbYc", mergeResult);
        }

        [Fact]
        public void NonOverlappingInsertsAtStartAndEnd()
        {
            string mergeResult = Merge("abc", "XYabc", "abcUW");

            Assert.Equal("XYabcUW", mergeResult);
        }

        [Fact]
        public void NonOverlappingReplaces()
        {
            string mergeResult = Merge("abcde", "aXcde", "abcYe");

            Assert.Equal("aXcYe", mergeResult);
        }

        [Fact]
        public void NonOverlappingDeletes()
        {
            string mergeResult = Merge("abcde", "acde", "abce");

            Assert.Equal("ace", mergeResult);
        }

        [Fact]
        public void InsertsAndDeletes()
        {
            string mergeResult = Merge("abcde", "aXbcde", "abe");

            Assert.Equal("aXbe", mergeResult);
        }

        [Fact]
        public void MultipleOperations()
        {
            string mergeResult = Merge("abcdefghi", "acWefghiX", "abcWefghiX");

            Assert.Equal("acWefghiX", mergeResult);
        }

        [Fact]
        public void BothDeletes()
        {
            string mergeResult = Merge("abcde", "aXbcd", "abcd");

            Assert.Equal("aXbcd", mergeResult);
        }

        [Fact]
        public void DifferentInsertsAtTheSamePlace()
        {
            Assert.Throws<MergeConflictException>(() => Merge("abcd", "Xabcd", "Yabcd"));
        }

        [Fact]
        public void DifferentReplacesAtTheSamePlace()
        {
            Assert.Throws<MergeConflictException>(() => Merge("abcd", "Xbcd", "Ybcd"));
        }

        [Fact]
        public void CallsMergeResolutionStrategy()
        {
            string mergePart1 = null;
            string mergePart2 = null;

            Merge("abcd", "abXd", "abYd",
                (p1, p2) =>
                {
                    mergePart1 = new string(p1.ToArray());
                    mergePart2 = new string(p2.ToArray());
                    return "";
                });

            Assert.Equal("X", mergePart1);
            Assert.Equal("Y", mergePart2);
        }

        [Fact]
        public void UsesConflictResolutionStrategyResult()
        {
            string mergeResult = Merge("abcd", "abXd", "abYd",
                (_, __) => "OMG");

            Assert.Equal("abOMGd", mergeResult);
        }

        private string Merge(string ancestor, string one, string another)
        {
            return Merge(ancestor, one, another, (_, __) => throw new MergeConflictException());
        }

        private string Merge(string ancestor, string one, string another,
                Func<IEnumerable<char>, IEnumerable<char>, IEnumerable<char>> conflictResolutionStrategy)
        {
            var result = ThreeWayMerge.Merge(ancestor.ToCharArray(), one.ToCharArray(), another.ToCharArray(),
                conflictResolutionStrategy);
            return new string(result);
        }
    }
}
