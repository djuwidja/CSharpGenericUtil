using NUnit.Framework;
using System.Collections.Generic;
using Djuwidja.GenericUtil.Collections;
using System;

namespace Djuwidja.GenericUtil.Collections.Tests
{
    public class PriorityQueue
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestInitialization()
        {
            const int sampleSize = 10;
            Random rand = new Random();

            PriorityQueue<double, object> testQueue = new PriorityQueue<double, object>();
            List<double> expectedPriorityList = new List<double>();
            for (int i = 0; i < sampleSize; i++)
            {
                double priority = rand.NextDouble();
                testQueue.Put(priority, new object());

                expectedPriorityList.Add(priority);
            }

            Assert.AreEqual(sampleSize, testQueue.Count);

            expectedPriorityList.Sort();
            List<double> priorityList = testQueue.GetPriorityList();

            for (int i = 0; i < sampleSize; i++)
            {
                Assert.AreEqual(expectedPriorityList[i], priorityList[i]);
            }
        }
        [Test]
        public void TestRemove()
        {
            const int sampleSize = 10;
            Random rand = new Random();

            List<int> idxList = new List<int>();
            List<object> testObjList = new List<object>();
            PriorityQueue<int, object> testQueue = new PriorityQueue<int, object>();
            for (int i = 0; i < sampleSize; i++)
            {
                object newObj = new object();
                idxList.Add(i);
                testObjList.Add(newObj);
                testQueue.Put(i, newObj);
            }

            const int testTries = 2;
            for (int i = 0; i < testTries; i++)
            {
                int removeObjIdx = rand.Next(testObjList.Count);
                int priorityIdx = idxList[removeObjIdx];

                object removeObj = testObjList[removeObjIdx];
                idxList.RemoveAt(removeObjIdx);
                testObjList.RemoveAt(removeObjIdx);
                testQueue.Remove(removeObj);

                List<int> priorityList = testQueue.GetPriorityList();

                bool isExceptionThrown = false;
                try
                {
                    Collections.BinarySearchUtil.SearchExact(priorityIdx, priorityList);
                }
                catch (ElementNotInListException)
                {
                    isExceptionThrown = true;
                }

                Assert.True(isExceptionThrown);
            }

            List<int> resultPriorityList = testQueue.GetPriorityList();
            for (int i = 0; i < idxList.Count; i++)
            {
                Assert.AreEqual(idxList[i], resultPriorityList[i]);
            }
        }
    }
}