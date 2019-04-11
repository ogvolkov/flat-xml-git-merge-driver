
using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ResX.Git.Merge.Driver
{
    public class ResXMerge
    {
        public IMergeResult Merge(string @base, string ours, string theirs)
        {
            var baseXml = XDocument.Parse(@base);
            var oursXml = XDocument.Parse(ours);
            var theirsXml = XDocument.Parse(theirs);

            // right now don't bother with comparison of roots

            var baseEntries = GetEntries(baseXml);
            var oursEntries = GetEntries(oursXml);
            var theirsEntries = GetEntries(theirsXml);

            var mergedEntries = ThreeWayMerge.Merge(baseEntries, oursEntries, theirsEntries,
                (part1, part2) => part2.Concat(part1));

            var mergedXml = new XDocument(baseXml);
            mergedXml.Root.RemoveAll();

            foreach (Entry mergedEntry in mergedEntries)
            {
                mergedXml.Root.Add(mergedEntry.Element);
            }

            var mergedText = mergedXml.Declaration + Environment.NewLine + mergedXml;

            return new SuccessfullyMerged(mergedText);
        }

        private Entry[] GetEntries(XDocument document)
        {
            return document.Root.Elements()
                .Where(xElement => xElement.Name == "data")
                .Select(xElement => new Entry(xElement)).ToArray();
        }

        private class Entry : IEquatable<Entry>
        {
            public XElement Element { get; }

            private string Key => Element.Attribute("name").Value;

            private string Value => Element.Element("value").Value;

            public Entry(XElement element)
            {
                Element = element;
            }

            public bool Equals(Entry other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                return Key == other.Key && Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Entry)obj);
            }

            public override int GetHashCode()
            {
                return (Element != null ? Element.GetHashCode() : 0);
            }
        }
    }
}
