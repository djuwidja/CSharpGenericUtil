using System;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Collections
{
    public class PriorityQueue<P, T>  where P : IComparable
    {
        public List<PriorityQueueItem<P, T>> ItemQueue { get; }
        public PriorityQueue()
        {
            this.ItemQueue = new List<PriorityQueueItem<P, T>>();
        }
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
        public List<P> GetPriorityList()
        {
            List<P> priorityList = new List<P>();
            ItemQueue.ForEach((PriorityQueueItem<P, T> item) => priorityList.Add(item.Priority));
            return priorityList;
        }
    }

    public class PriorityQueueItem<P, T> : IComparable where P : IComparable
    {
        public P Priority { get; }
        public T Item { get ;}
        public PriorityQueueItem(P priority, T item)
        {
            this.Priority = priority;
            this.Item = item;
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            PriorityQueueItem<P, T> otherObj = obj as PriorityQueueItem<P, T>;
            return Priority.CompareTo(otherObj.Priority);
        }
    }
}