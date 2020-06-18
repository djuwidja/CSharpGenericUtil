using NUnit.Framework;
using Djuwidja.GenericUtil.Graph;
using System;

namespace Djuwidja.GenericUtil.Test.Graph
{
    public class TestEdge
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
            Node nextNode = new Node();

            //test init
            Edge edge = new Edge(weight, nextNode);
            Assert.AreEqual(weight, edge.Weight);
            Assert.AreEqual(nextNode, edge.Node);

            //test setter
            double newWeight = rand.NextDouble();
            Node newNode = new Node();
            edge.Weight = newWeight;
            edge.Node = newNode;
            Assert.AreEqual(newWeight, edge.Weight);
            Assert.AreEqual(newNode, edge.Node);
        }

        [Test]
        public void TestComparison()
        {
            Edge edge1 = new Edge(8.0d, new Node());
            Edge edge2 = new Edge(8.0d, new Node());
            Edge edge3 = new Edge(2.0d, new Node());
            Edge edge4 = new Edge(11.0d, new Node());

            Assert.AreEqual(0, edge1.CompareTo(edge2));
            Assert.AreEqual(1, edge1.CompareTo(edge3));
            Assert.AreEqual(-1, edge1.CompareTo(edge4));
        }

    }
}