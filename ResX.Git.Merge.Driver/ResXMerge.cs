
using System.Xml.Linq;

namespace ResX.Git.Merge.Driver
{
    public class ResXMerge
    {
        public IMergeResult Merge(string ancestor, string current, string other)
        {
            XDocument ancestorXml = XDocument.Parse(ancestor);
            XDocument currentXml = XDocument.Parse(current);
            XDocument otherXml = XDocument.Parse(other);

            return new SuccessfullyMerged(current);
        }
    }
}
