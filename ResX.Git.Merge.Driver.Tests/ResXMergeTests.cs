using System.IO;
using System.Reflection;
using Xunit;

namespace ResX.Git.Merge.Driver.Tests
{
    public class ResXMergeTests
    {
        private readonly ResXMerge _merge = new ResXMerge();

        [Fact]
        public void CombinesKeyAdditionsAtTheEndOfTheFile()
        {
            // arrange
            string ancestor = GetEmbeddedResourceContent("TestCases.AdditionsAtTheEnd.base.xml");

            string current = GetEmbeddedResourceContent("TestCases.AdditionsAtTheEnd.current.xml");

            string other = GetEmbeddedResourceContent("TestCases.AdditionsAtTheEnd.other.xml");

            // act
            IMergeResult result = _merge.Merge(ancestor, current, other);

            // assert
            Assert.IsType<SuccessfullyMerged>(result);

            string expectedMerge = GetEmbeddedResourceContent("TestCases.AdditionsAtTheEnd.result.xml");

            Assert.Equal(expectedMerge, ((SuccessfullyMerged)result).MergeResult);
        }

        [Fact]
        public void KeepsResxHeader()
        {
            // arrange
            string ancestor = GetEmbeddedResourceContent("TestCases.ResXHeader.base.xml");

            string current = GetEmbeddedResourceContent("TestCases.ResXHeader.current.xml");

            string other = GetEmbeddedResourceContent("TestCases.ResXHeader.other.xml");

            // act
            IMergeResult result = _merge.Merge(ancestor, current, other);

            // assert
            Assert.IsType<SuccessfullyMerged>(result);

            string expectedMerge = GetEmbeddedResourceContent("TestCases.ResXHeader.result.xml");

            Assert.Equal(expectedMerge, ((SuccessfullyMerged)result).MergeResult);
        }

        private string GetEmbeddedResourceContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{GetType().Namespace}.{fileName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
