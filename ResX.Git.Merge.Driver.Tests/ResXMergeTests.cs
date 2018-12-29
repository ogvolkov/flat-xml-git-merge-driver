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
            string ancestor =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <root>
                    <data name=""_Base"" xml:space=""preserve"">
                        <value>Base value</value>
                    </data>
                </root>";

            string current =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <root>
                    <data name=""_Base"" xml:space=""preserve"">
                        <value>Base value</value>
                    </data>
                    <data name=""_Current"" xml:space=""preserve"">
                        <value>Current value</value>
                    </data>
                </root>";

            string other =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <root>
                    <data name=""_Base"" xml:space=""preserve"">
                        <value>Base value</value>
                    </data>
                    <data name=""_Other"" xml:space=""preserve"">
                        <value>Other value</value>
                    </data>
                </root>";

            // act
            IMergeResult result = _merge.Merge(ancestor, current, other);

            // assert
            Assert.IsType<SuccessfullyMerged>(result);

            string expectedMerge =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <root>
                    <data name=""_Base"" xml:space=""preserve"">
                        <value>Base value</value>
                    </data>
                    <data name=""_Other"" xml:space=""preserve"">
                        <value>Other value</value>
                    </ data>
                    <data name=""_Current"" xml:space=""preserve"">
                        <value>Current value</value>
                    </data>
                </root>";

            Assert.Equal(expectedMerge, ((SuccessfullyMerged)result).MergeResult);
        }
    }
}
