using NUnit.Framework;
using System.Collections.Generic;
using Djuwidja.GenericUtil.Collections;

namespace Djuwidja.GenericUtil.Test.Collections
{
    public class TestBinarySearchUtil
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSearchExact()
        {
            List<int> searchList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.AreEqual(0, BinarySearchUtil.SearchExact(1, searchList));
            Assert.AreEqual(1, BinarySearchUtil.SearchExact(2, searchList));
            Assert.AreEqual(2, BinarySearchUtil.SearchExact(3, searchList));
            Assert.AreEqual(3, BinarySearchUtil.SearchExact(4, searchList));
            Assert.AreEqual(4, BinarySearchUtil.SearchExact(5, searchList));
            Assert.AreEqual(5, BinarySearchUtil.SearchExact(6, searchList));
            Assert.AreEqual(6, BinarySearchUtil.SearchExact(7, searchList));
            Assert.AreEqual(7, BinarySearchUtil.SearchExact(8, searchList));
            Assert.AreEqual(8, BinarySearchUtil.SearchExact(9, searchList));
            Assert.AreEqual(9, BinarySearchUtil.SearchExact(10, searchList));

            bool isExceptionThrown = false;
            try
            {
                BinarySearchUtil.SearchExact(11, searchList);
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
            Assert.AreEqual(0, BinarySearchUtil.SearchProximity(0.1d, searchList));
            Assert.AreEqual(0, BinarySearchUtil.SearchProximity(2.2d, searchList));
            Assert.AreEqual(1, BinarySearchUtil.SearchProximity(7.8d, searchList));
            Assert.AreEqual(2, BinarySearchUtil.SearchProximity(10.7d, searchList));
            Assert.AreEqual(3, BinarySearchUtil.SearchProximity(14.2d, searchList));
            Assert.AreEqual(4, BinarySearchUtil.SearchProximity(19.4d, searchList));
            Assert.AreEqual(5, BinarySearchUtil.SearchProximity(29.1d, searchList));
            Assert.AreEqual(6, BinarySearchUtil.SearchProximity(36.8d, searchList));
            Assert.AreEqual(6, BinarySearchUtil.SearchProximity(36.9d, searchList));
            Assert.AreEqual(6, BinarySearchUtil.SearchProximity(44.6d, searchList));
            Assert.AreEqual(7, BinarySearchUtil.SearchProximity(48.3d, searchList));
            Assert.AreEqual(8, BinarySearchUtil.SearchProximity(60.4d, searchList));
            Assert.AreEqual(9, BinarySearchUtil.SearchProximity(77.7d, searchList));
            Assert.AreEqual(9, BinarySearchUtil.SearchProximity(101.5d, searchList));
        }
    }
}