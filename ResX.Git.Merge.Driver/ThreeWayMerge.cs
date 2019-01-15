
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResX.Git.Merge.Driver
{
    public static class ThreeWayMerge
    {
        public static T[] Merge<T>(
            T[] @base,
            T[] ours,
            T[] theirs,
            Func<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>> conflictResolutionStrategy
        ) where T : IEquatable<T>
        {
            var result = new List<T>();

            var lcsBaseOurs = new LongestCommonSubsequence<T>(@base, ours).Result;
            var lcsAllThree = new LongestCommonSubsequence<T>(lcsBaseOurs, theirs).Result;

            var basePosition = new Position<T>(@base);
            var oursPosition = new Position<T>(ours);
            var theirsPosition = new Position<T>(theirs);

            foreach (T element in lcsAllThree)
            {
                basePosition.FindNext(element);
                oursPosition.FindNext(element);
                theirsPosition.FindNext(element);

                Advance();

                result.Add(element);
            }

            basePosition.MoveToEnd();
            oursPosition.MoveToEnd();
            theirsPosition.MoveToEnd();

            Advance();

            void Advance()
            {
                if (basePosition.HasEqualExcessTo(oursPosition))
                {
                    result.AddRange(theirsPosition.Excess);
                }
                else if (basePosition.HasEqualExcessTo(theirsPosition))
                {
                    result.AddRange(oursPosition.Excess);
                }
                else if (oursPosition.HasEqualExcessTo(theirsPosition))
                {
                    result.AddRange(oursPosition.Excess);
                }
                else
                {
                    var resolvedConflicts = conflictResolutionStrategy(oursPosition.Excess, theirsPosition.Excess);
                    result.AddRange(resolvedConflicts);
                }
            }

            return result.ToArray();
        }

        private class Position<T> where T : IEquatable<T>
        {
            private readonly T[] _a;

            private int _lastPosition;

            private int _currentPosition;

            public IEnumerable<T> Excess => new ArraySegment<T>(_a, _lastPosition + 1, _currentPosition - _lastPosition - 1);

            public Position(T[] a)
            {
                _a = a;
                _lastPosition = 0;
                _currentPosition = -1;
            }

            public void FindNext(T element)
            {
                _lastPosition = _currentPosition;
                _currentPosition = Array.IndexOf(_a, element, _currentPosition + 1);
            }

            public void MoveToEnd()
            {
                _lastPosition = _currentPosition;
                _currentPosition = _a.Length;
            }

            public bool HasEqualExcessTo(Position<T> other)
            {
                return Excess.SequenceEqual(other.Excess);
            }
        }
    }
}
