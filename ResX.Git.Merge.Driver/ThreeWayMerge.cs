
using System;
using System.Collections.Generic;

namespace ResX.Git.Merge.Driver
{
    public class ThreeWayMerge
    {
        public T[] Merge<T>(T[] a, T[] x, T[] y) where T : IEquatable<T>
        {
            var result = new List<T>();

            var lcsAx = new LongestCommonSubsequence<T>(a, x).Result;
            var lcs = new LongestCommonSubsequence<T>(lcsAx, y).Result;

            var aPos = new ArrayWithPosition<T>(a);
            var xPos = new ArrayWithPosition<T>(x);
            var yPos = new ArrayWithPosition<T>(y);

            foreach (T element in lcs)
            {
                aPos.FindNext(element);
                xPos.FindNext(element);
                yPos.FindNext(element);

                Advance();

                result.Add(element);
            }

            aPos.MoveToEnd();
            xPos.MoveToEnd();
            yPos.MoveToEnd();

            Advance();

            void Advance()
            {
                if (aPos.Excess == 0)
                {
                    if (xPos.Excess == 0)
                    {
                        if (yPos.Excess == 0)
                        {
                            // all set
                        }
                        else
                        {
                            yPos.CopyExcessTo(result);
                        }
                    }
                    else
                    {
                        if (yPos.Excess == 0)
                        {
                            xPos.CopyExcessTo(result);
                        }
                        else
                        {
                            if (xPos.HasEqualExcessTo(yPos))
                            {
                                xPos.CopyExcessTo(result);
                            }
                            else
                            {
                                throw new MergeConflictException();
                            }
                        }
                    }
                }
                else
                {
                    if (xPos.Excess == 0)
                    {
                        if (yPos.Excess == 0)
                        {
                            // all set
                        }
                        else
                        {
                            if (!aPos.HasEqualExcessTo(yPos))
                            {
                                throw new MergeConflictException();
                            }
                        }
                    }
                    else
                    {
                        if (yPos.Excess == 0)
                        {
                            if (!aPos.HasEqualExcessTo(xPos))
                            {
                                throw new MergeConflictException();
                            }
                        }
                        else
                        {
                            if (xPos.HasEqualExcessTo(yPos))
                            {
                                xPos.CopyExcessTo(result);
                            }
                            else if (aPos.HasEqualExcessTo(xPos))
                            {
                                yPos.CopyExcessTo(result);
                            }
                            else if (aPos.HasEqualExcessTo(yPos))
                            {
                                xPos.CopyExcessTo(result);
                            }
                            else
                            {
                                throw new MergeConflictException();
                            }
                        }
                    }
                }
            }

            return result.ToArray();
        }

        private class ArrayWithPosition<T> where T : IEquatable<T>
        {
            private readonly T[] _a;

            private int _lastPosition;

            private int _currentPosition;

            public int Excess => _currentPosition - _lastPosition - 1;

            public ArrayWithPosition(T[] a)
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

            public bool HasEqualExcessTo(ArrayWithPosition<T> another)
            {
                if (Excess != another.Excess)
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
