using NUnit.Framework;
using Djuwidja.GenericUtil.Graph;
using System;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Graph.Tests
{
    public class Node
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestInitialization()
        {
            Random rand = new Random();
            Graph.Node newNode = new Graph.Node();

            HashSet<Graph.Node> addedNodeSet = new HashSet<Graph.Node>();
            List<double> weightList = new List<double>();

            //success case
            const int nodeCount = 10;
            for (int i = 0; i < nodeCount; i++)
            {
                double weight = rand.NextDouble();
                Graph.Node neighborNode = new Graph.Node();
                newNode.AddNeighbor(weight, neighborNode);

                addedNodeSet.Add(neighborNode);
                weightList.Add(weight);
            }

            // test edge list
            weightList.Sort();
            List<Graph.Edge> edgeList = newNode.EdgeList;
            Assert.AreEqual(weightList.Count, edgeList.Count);
            for (int i = 0; i < edgeList.Count; i++)
            {
                double weight = weightList[i];
                Assert.AreEqual(weight, edgeList[i].Weight);
            }

            // test neighbor list
            List<Graph.Node> neighborList = newNode.NeighborList;
            Assert.AreEqual(addedNodeSet.Count, neighborList.Count);

            foreach (Graph.Node node in neighborList)
            {
                Assert.True(newNode.IsNeighbor(node));
                Assert.True(addedNodeSet.Contains(node));
            }

            // test add neighbor that is already a neighbor.
            int addedIdx = rand.Next(neighborList.Count);
            Graph.Node addedNode = neighborList[addedIdx];
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