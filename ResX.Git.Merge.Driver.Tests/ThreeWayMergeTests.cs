﻿
using Xunit;

namespace ResX.Git.Merge.Driver.Tests
{
    public class ThreeWayMergeTests
    {
        private readonly ThreeWayMerge _merge = new ThreeWayMerge();

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
            string mergeResult = Merge("Mary had a little lamb", "Susy had a little lamb.", "Mary once had a pig.");

            Assert.Equal("Susy once had a pig.", mergeResult);
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

        private string Merge(string ancestor, string one, string another)
        {
            var result = _merge.Merge(ancestor.ToCharArray(), one.ToCharArray(), another.ToCharArray());
            return new string(result);
        }
    }
}