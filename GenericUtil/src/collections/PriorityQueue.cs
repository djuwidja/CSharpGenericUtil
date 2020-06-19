using System;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Collections
{
    /// <summary>
    /// Generic Class PriorityQueue. (Not thread-safe)
    /// </summary>
    /// <typeparam name="P">Priority type, must extends IComparable.</typeparam>
    /// <typeparam name="T">Object type that a priority is assigned to.</typeparam>
    public class PriorityQueue<P, T>  where P : IComparable
    {
        public List<PriorityQueueItem<P, T>> ItemQueue { get; }
        public PriorityQueue()
        {
            this.ItemQueue = new List<PriorityQueueItem<P, T>>();
        }
        /// <summary>
        /// Put an element into this priority queue. The element is inserted into the index found by BinarySearchUtil.SearchProximity. (Ave. O(log(n))).
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="item"></param>
        public void Put(P priority, T item)
        {
            PriorityQueueItem<P, T> newItem = new PriorityQueueItem<P, T>(priority, item);
            int idx = BinarySearchUtil.SearchProximity(newItem, ItemQueue);
            if (ItemQueue.Count == 0)
            {
                ItemQueue.Add(newItem);
            }
            else
            {
                if (idx == 0 && newItem.CompareTo(ItemQueue[0]) < 0)
                {
                    ItemQueue.Insert(0, newItem);
                }
                else if (idx + 1 < ItemQueue.Count)
                {
                    ItemQueue.Insert(idx + 1, newItem);
                }
                else
                {
                    ItemQueue.Add(newItem);
                }
            }
        }
        /// <summary>
        /// Pop the first element in this priority queue.
        /// </summary>
        /// <returns></returns>
        public PriorityQueueItem<P, T> Pop()
        {
            if (ItemQueue.Count > 0)
            {
                PriorityQueueItem<P, T> popItem = ItemQueue[0];
                ItemQueue.RemoveAt(0);
                return popItem;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Remove the first element that has its item reference equals to the param. (Worst O(n)).
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            int idx = -1;
            for (int i = 0; i < ItemQueue.Count; i++)
            {
                PriorityQueueItem<P, T> queueItem = ItemQueue[i];
                if (Object.ReferenceEquals(queueItem.Item, item))
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                ItemQueue.RemoveAt(idx);
            }
        }
        /// <summary>
        /// Get the ordered priority value list from this priority queue. (O(n)).
        /// </summary>
        /// <returns></returns>
        public List<P> GetPriorityList()
        {
            List<P> priorityList = new List<P>();
            ItemQueue.ForEach((PriorityQueueItem<P, T> item) => priorityList.Add(item.Priority));
            return priorityList;
        }
        /// <summary>
        /// Return Count of this priority queue.
        /// </summary>
        /// <value></value>
        public int Count
        {
            get {
                return ItemQueue.Count;
            }
        }
    }
    /// <summary>
    /// Class <c>PriorityQueueItem</c> represents an item in a <c>PriorityQueue</c>. 
    /// </summary>
    /// <typeparam name="P">Priority type, must extends IComparable.</typeparam>
    /// <typeparam name="T">Object type that a priority is assigned to.</typeparam>
    public class PriorityQueueItem<P, T> : IComparable where P : IComparable
    {
        public P Priority { get; }
        public T Item { get ;}
        public PriorityQueueItem(P priority, T item)
        {
            this.Priority = priority;
            this.Item = item;
        }
        /// <summary>
        /// Override fuction from <c>IComparable</c>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            PriorityQueueItem<P, T> otherObj = obj as PriorityQueueItem<P, T>;
            return Priority.CompareTo(otherObj.Priority);
        }
    }
}