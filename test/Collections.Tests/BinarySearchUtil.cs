using NUnit.Framework;
using System.Collections.Generic;
using Djuwidja.GenericUtil.Collections;

namespace Djuwidja.GenericUtil.Collections.Tests
{
    public class BinarySearchUtil
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSearchExact()
        {
            List<int> searchList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.AreEqual(0, Collections.BinarySearchUtil.SearchExact(1, searchList));
            Assert.AreEqual(1, Collections.BinarySearchUtil.SearchExact(2, searchList));
            Assert.AreEqual(2, Collections.BinarySearchUtil.SearchExact(3, searchList));
            Assert.AreEqual(3, Collections.BinarySearchUtil.SearchExact(4, searchList));
            Assert.AreEqual(4, Collections.BinarySearchUtil.SearchExact(5, searchList));
            Assert.AreEqual(5, Collections.BinarySearchUtil.SearchExact(6, searchList));
            Assert.AreEqual(6, Collections.BinarySearchUtil.SearchExact(7, searchList));
            Assert.AreEqual(7, Collections.BinarySearchUtil.SearchExact(8, searchList));
            Assert.AreEqual(8, Collections.BinarySearchUtil.SearchExact(9, searchList));
            Assert.AreEqual(9, Collections.BinarySearchUtil.SearchExact(10, searchList));

            bool isExceptionThrown = false;
            try
            {
                Collections.BinarySearchUtil.SearchExact(11, searchList);
            }
            catch (ElementNotInListException)
            {
                isExceptionThrown = true;
            }

            if (!isExceptionThrown)
            {
                Assert.Fail("ElementNotInListException was never thrown.");
            }
        }

        [Test]
        public void TestSearchProximity()
        {
            List<double> searchList = new List<double>() { 0.5d, 3.5d, 9.5d, 12.5d, 18.5d, 20.6d, 36.8d, 44.7d, 58.1d, 77.4d };
            Assert.AreEqual(0, Collections.BinarySearchUtil.SearchProximity(0.1d, searchList));
            Assert.AreEqual(0, Collections.BinarySearchUtil.SearchProximity(2.2d, searchList));
            Assert.AreEqual(1, Collections.BinarySearchUtil.SearchProximity(7.8d, searchList));
            Assert.AreEqual(2, Collections.BinarySearchUtil.SearchProximity(10.7d, searchList));
            Assert.AreEqual(3, Collections.BinarySearchUtil.SearchProximity(14.2d, searchList));
            Assert.AreEqual(4, Collections.BinarySearchUtil.SearchProximity(19.4d, searchList));
            Assert.AreEqual(5, Collections.BinarySearchUtil.SearchProximity(29.1d, searchList));
            Assert.AreEqual(6, Collections.BinarySearchUtil.SearchProximity(36.8d, searchList));
            Assert.AreEqual(6, Collections.BinarySearchUtil.SearchProximity(36.9d, searchList));
            Assert.AreEqual(6, Collections.BinarySearchUtil.SearchProximity(44.6d, searchList));
            Assert.AreEqual(7, Collections.BinarySearchUtil.SearchProximity(48.3d, searchList));
            Assert.AreEqual(8, Collections.BinarySearchUtil.SearchProximity(60.4d, searchList));
            Assert.AreEqual(9, Collections.BinarySearchUtil.SearchProximity(77.7d, searchList));
            Assert.AreEqual(9, Collections.BinarySearchUtil.SearchProximity(101.5d, searchList));
        }
    }
}