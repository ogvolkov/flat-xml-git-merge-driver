using Xunit;

namespace ResX.Git.Merge.Driver.Tests
{
    public class TwoWayMergeTests
    {
        private readonly TwoWayMerge _merge = new TwoWayMerge();

        [Fact]
        public void SecondHasAddsAtTheEnd()
        {
            string mergeResult = Merge("ab", "abc");
            Assert.Equal("abc", mergeResult);
        }

        [Fact]
        public void SecondHasAddsInBetween()
        {
            string mergeResult = Merge("ab", "acb");
            Assert.Equal("acb", mergeResult);
        }

        [Fact]
        public void SecondHasAddsAtStart()
        {
            string mergeResult = Merge("ab", "cab");
            Assert.Equal("cab", mergeResult);
        }

        [Fact]
        public void SecondHasRemovesAtEnd()
        {
            string mergeResult = Merge("abc", "ab");
            Assert.Equal("ab", mergeResult);
        }

        [Fact]
        public void SecondHasRemovesInBetween()
        {
            string mergeResult = Merge("abc", "ac");
            Assert.Equal("ac", mergeResult);
        }

        [Fact]
        public void SecondHasRemovesAtStart()
        {
            string mergeResult = Merge("abc", "bc");
            Assert.Equal("bc", mergeResult);
        }


        [Fact]
        public void ComplexInsertsAndRemoves()
        {
            string mergeResult = Merge("abcd", "XacYdUW");
            Assert.Equal("XacYdUW", mergeResult);
        }

        [Fact]
        public void ThrowsAtMergeConflict()
        {
            Assert.Throws<MergeConflictException>(() => Merge("abcd", "abce"));
        }

        private string Merge(string one, string another)
        {
            var result = _merge.Merge(one.ToCharArray(), another.ToCharArray());
            return new string(result);
        }
    }
}
