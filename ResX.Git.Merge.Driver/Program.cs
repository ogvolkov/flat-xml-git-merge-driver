using System;
using System.IO;

namespace ResX.Git.Merge.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: <merge driver> <original> <current> <other>");
                Environment.Exit(1);
                return;
            }

            string original = args[0];
            string current = args[1];
            string other = args[2];

            string originalContents = File.ReadAllText(original);
            string currentContents = File.ReadAllText(current);
            string otherContents = File.ReadAllText(other);

            var merge = new ResXMerge();
            var mergeResult = merge.Merge(originalContents, currentContents, otherContents);

            switch (mergeResult)
            {
                case MergeConflicts _:
                    Environment.Exit(2);
                    return;
                case SuccessfullyMerged successfulMerge:
                    File.WriteAllText(current, successfulMerge.MergeResult);
                    Environment.Exit(0);
                    return;
                default:
                    throw new InvalidOperationException($"Unexpected merge result type {mergeResult.GetType()}");
            }
        }
    }
}
