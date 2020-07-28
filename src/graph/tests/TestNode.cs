using NUnit.Framework;
using Djuwidja.GenericUtil.Graph;
using System;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Test.Graph
{
    public class TestNode
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestInitialization()
        {
            Random rand = new Random();
            Node newNode = new Node();

            HashSet<Node> addedNodeSet = new HashSet<Node>();
            List<double> weightList = new List<double>();

            //success case
            const int nodeCount = 10000;
            for (int i = 0; i < nodeCount; i++)
            {
                double weight = rand.NextDouble();
                Node neighborNode = new Node();
                newNode.AddNeighbor(weight, neighborNode);

                addedNodeSet.Add(neighborNode);
                weightList.Add(weight);
            }

            // test edge list
            weightList.Sort();
            List<Edge> edgeList = newNode.EdgeList;
            Assert.AreEqual(weightList.Count, edgeList.Count);
            for (int i = 0; i < edgeList.Count; i++)
            {
                double weight = weightList[i];
                Assert.AreEqual(weight, edgeList[i].Weight);
            }

            // test neighbor list
            List<Node> neighborList = newNode.NeighborList;
            Assert.AreEqual(addedNodeSet.Count, neighborList.Count);

            foreach (Node node in neighborList)
            {
                Assert.True(newNode.IsNeighbor(node));
                Assert.True(addedNodeSet.Contains(node));
            }

            // test add neighbor that is already a neighbor.
            int addedIdx = rand.Next(neighborList.Count);
            Node addedNode = neighborList[addedIdx];
            bool isExceptionThrown = false;
            try
            {
                newNode.AddNeighbor(rand.NextDouble(), addedNode);
            }
            catch (NodeIsAlreadyNeighborException)
            {
                isExceptionThrown = true;
            }

            Assert.True(isExceptionThrown);
        }
    }
}