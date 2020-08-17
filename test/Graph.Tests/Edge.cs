using NUnit.Framework;
using Djuwidja.GenericUtil.Graph;
using System;

namespace Djuwidja.GenericUtil.Graph.Tests
{
    public class Edge
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestInitialization()
        {
            Random rand = new Random();
            double weight = rand.NextDouble();
            Graph.Node nextNode = new Graph.Node();

            //test init
            Graph.Edge edge = new Graph.Edge(weight, nextNode);
            Assert.AreEqual(weight, edge.Weight);
            Assert.AreEqual(nextNode, edge.Node);

            //test setter
            double newWeight = rand.NextDouble();
            Graph.Node newNode = new Graph.Node();
            edge.Weight = newWeight;
            edge.Node = newNode;
            Assert.AreEqual(newWeight, edge.Weight);
            Assert.AreEqual(newNode, edge.Node);
        }

        [Test]
        public void TestComparison()
        {
            Graph.Edge edge1 = new Graph.Edge(8.0d, new Graph.Node());
            Graph.Edge edge2 = new Graph.Edge(8.0d, new Graph.Node());
            Graph.Edge edge3 = new Graph.Edge(2.0d, new Graph.Node());
            Graph.Edge edge4 = new Graph.Edge(11.0d, new Graph.Node());

            Assert.AreEqual(0, edge1.CompareTo(edge2));
            Assert.AreEqual(1, edge1.CompareTo(edge3));
            Assert.AreEqual(-1, edge1.CompareTo(edge4));
        }

    }
}