using NUnit.Framework;
using System.Collections.Generic;
using Djuwidja.GenericUtil.Collections;
using System;

namespace Djuwidja.GenericUtil.Test.Collections
{
    public class TestPriorityQueue
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestInitialization()
        {
            const int sampleSize = 10000;
            Random rand = new Random();

            PriorityQueue<double, object> testQueue = new PriorityQueue<double, object>();
            List<double> expectedPriorityList = new List<double>();
            for (int i = 0; i < sampleSize; i++)
            {
                double priority = rand.NextDouble();
                testQueue.Put(priority, new object());

                expectedPriorityList.Add(priority);
            }

            expectedPriorityList.Sort();
            List<double> priorityList = testQueue.GetPriorityList();

            for (int i = 0; i < sampleSize; i++)
            {
                Assert.AreEqual(expectedPriorityList[i], priorityList[i]);
            }
        }
    }
}