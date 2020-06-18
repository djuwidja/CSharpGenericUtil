using System;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Collections
{
    /// <summary>
    /// Performs binary search on a sorted list.
    /// </summary>
    public static class BinarySearchUtil
    {
        /// <summary>
        /// Search for occurence of <c>val</c> in <c>sortedValList</c>. Throws <c>ElementNotInListException</c> if it cannot be found.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="sortedValList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int SearchExact<T>(T val, List<T> sortedValList) where T : IComparable
        {
            if (sortedValList.Count == 0)
            {
                ThrowElementNotInListException(val, sortedValList);
            }

            int masterIdx = SearchProximity(val, sortedValList);
            if (sortedValList[masterIdx].CompareTo(val) != 0)
            {
                ThrowElementNotInListException(val, sortedValList);
            }

            return masterIdx;
        }
        /// <summary>
        /// Search for occurence of <c>val</c> in <c>sortedValList</c>. Returns the nearest smaller value if it cannot be found. 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="sortedValList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int SearchProximity<T>(T val, List<T> sortedValList) where T : IComparable
        {
            if (sortedValList.Count == 0)
            {
                return 0;
            }

            List<T> searchList = sortedValList;

            int masterIdx = 0;
            while (searchList != null)
            {
                int searchListLen = searchList.Count;
                T fstVal = searchList[0];
                T lstVal = searchList[searchListLen-1];
                if (val.CompareTo(fstVal) == 0)
                {
                    return masterIdx;
                }
                else if (val.CompareTo(lstVal) == 0)
                {
                    return masterIdx + searchListLen - 1;
                }

                int divIdx = searchList.Count / 2;
                if (divIdx == 0)
                {
                    break;
                }
                else
                {
                    T divIdxVal = searchList[divIdx];

                    if (val.CompareTo(divIdxVal) < 0)
                    {
                        searchList = searchList.GetRange(0, divIdx);
                    }
                    else
                    {
                        searchList = searchList.GetRange(divIdx, searchList.Count - divIdx);
                        masterIdx += divIdx;
                    }
                }
            }

            return masterIdx;
        }
        private static void ThrowElementNotInListException<T>(T val, List<T> sortedValList)
        {
            throw new ElementNotInListException(string.Format("The value {0} does not exist in list {1}", val, sortedValList));
        }
    }
}
