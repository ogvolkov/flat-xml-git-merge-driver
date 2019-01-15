
using System;
using System.Collections.Generic;

namespace ResX.Git.Merge.Driver
{
    public static class ThreeWayMerge
    {
        public static T[] Merge<T>(T[] @base, T[] ours, T[] theirs) where T : IEquatable<T>
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
                    theirsPosition.CopyExcessTo(result);
                }
                else if (basePosition.HasEqualExcessTo(theirsPosition))
                {
                    oursPosition.CopyExcessTo(result);
                }
                else if (oursPosition.HasEqualExcessTo(theirsPosition))
                {
                    oursPosition.CopyExcessTo(result);
                }
                else
                {
                    throw new MergeConflictException();
                }
            }

            return result.ToArray();
        }

        private class Position<T> where T : IEquatable<T>
        {
            private readonly T[] _a;

            private int _lastPosition;

            private int _currentPosition;

            private int ExcessSize => _currentPosition - _lastPosition - 1;

            public Position(T[] a)
            {
                _a = a;
                _lastPosition = 0;
                _currentPosition = -1;
            }

            public void FindNext(T element)
            {
                _lastPosition = _currentPosition;
                ++_currentPosition;

                while (_currentPosition < _a.Length && !_a[_currentPosition].Equals(element))
                {
                    ++_currentPosition;
                }
            }

            public void MoveToEnd()
            {
                _lastPosition = _currentPosition;
                _currentPosition = _a.Length;
            }

            public void CopyExcessTo(ICollection<T> destination)
            {
                for (int i = _lastPosition + 1; i < _currentPosition; i++)
                {
                    destination.Add(_a[i]);
                }
            }

            public bool HasEqualExcessTo(Position<T> another)
            {
                if (ExcessSize != another.ExcessSize)
                {
                    return false;
                }

                for (int i = 1; i < _currentPosition - _lastPosition; i++)
                {
                    if (!_a[_lastPosition + i].Equals(another._a[another._lastPosition + i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
