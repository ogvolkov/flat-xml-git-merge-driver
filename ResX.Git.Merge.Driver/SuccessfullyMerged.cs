namespace ResX.Git.Merge.Driver
{
    public class SuccessfullyMerged: IMergeResult
    {
        public string MergeResult { get; }

        public SuccessfullyMerged(string mergeResult)
        {
            MergeResult = mergeResult;
        }
    }
}
