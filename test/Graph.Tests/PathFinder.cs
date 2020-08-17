using NUnit.Framework;
using Djuwidja.GenericUtil.Graph;
using System;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Graph.Tests
{
    public class PathFinder
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestPath1()
        {
            Dictionary<string, Graph.Node> graphMap = CreateGraph();
            Graph.Node srcNode = graphMap["a"];
            Graph.Node destNode = graphMap["g"];
            LinkedList<Graph.Edge> path = Graph.PathFinder.AStarSearch(srcNode, destNode);

            AssertPathResult(
                new Dictionary<double, Graph.Node>(){
                    {0, graphMap["a"]}, {2, graphMap["c"]}, {5, graphMap["h"]}, {6, graphMap["g"]}
                }, path);
        }

        [Test]
        public void TestPath2()
        {
            Dictionary<string, Graph.Node> graphMap = CreateGraph();
            Graph.Node srcNode = graphMap["c"];
            Graph.Node destNode = graphMap["f"];
            LinkedList<Graph.Edge> path = Graph.PathFinder.AStarSearch(srcNode, destNode);

            AssertPathResult(
                new Dictionary<double, Graph.Node>(){
                    {0, graphMap["c"]}, {2, graphMap["a"]}, {6, graphMap["b"]}, {1, graphMap["d"]}, {3, graphMap["f"]}
                }, path);
        }

        [Test]
        public void TestPath3()
        {
            Dictionary<string, Graph.Node> graphMap = CreateGraph();
            Graph.Node srcNode = graphMap["i"];
            Graph.Node destNode = graphMap["f"];
            LinkedList<Graph.Edge> path = Graph.PathFinder.AStarSearch(srcNode, destNode);

            AssertPathResult(
                new Dictionary<double, Graph.Node>(){
                    {0, graphMap["i"]}, {7, graphMap["h"]}, {5, graphMap["e"]}, {4, graphMap["b"]}, {1, graphMap["d"]}, {3, graphMap["f"]}
                }, path);
        }

        [Test]
        public void TestNoPath()
        {
            Dictionary<string, Graph.Node> graphMap = CreateGraph();
            Graph.Node srcNode = graphMap["a"];
            Graph.Node destNode = graphMap["k"];

            bool isExceptionThrown = false;
            try
            {
                LinkedList<Graph.Edge> path = Graph.PathFinder.AStarSearch(srcNode, destNode);
            }
            catch (PathNotFoundException)
            {
                isExceptionThrown = true;
            }

            Assert.True(isExceptionThrown);
        }

        private void AssertPathResult(Dictionary<double, Graph.Node> expectedPath, LinkedList<Graph.Edge> path)
        {
            Assert.AreEqual(expectedPath.Count, path.Count);
            LinkedListNode<Graph.Edge> edge = path.First;

            foreach (KeyValuePair<double, Graph.Node> entry in expectedPath)
            {
                double expectedWeight = entry.Key;
                Graph.Node expectedNode = entry.Value;

                Assert.AreEqual(expectedWeight, edge.Value.Weight);
                Assert.AreEqual(expectedNode, edge.Value.Node);

                edge = edge.Next;
            }
        }

        private Dictionary<string, Graph.Node> CreateGraph()
        {
            Graph.Node nodeA = new Graph.Node();
            Graph.Node nodeB = new Graph.Node();
            Graph.Node nodeC = new Graph.Node();
            Graph.Node nodeD = new Graph.Node();
            Graph.Node nodeE = new Graph.Node();
            Graph.Node nodeF = new Graph.Node();
            Graph.Node nodeG = new Graph.Node();
            Graph.Node nodeH = new Graph.Node();
            Graph.Node nodeI = new Graph.Node();
            Graph.Node nodeJ = new Graph.Node();
            Graph.Node nodeK = new Graph.Node();

            nodeA.AddNeighbor(6, nodeB);
            nodeA.AddNeighbor(2, nodeC);
            nodeA.AddNeighbor(7, nodeJ);

            nodeB.AddNeighbor(6, nodeA);
            nodeB.AddNeighbor(1, nodeD);
            nodeB.AddNeighbor(4, nodeE);

            nodeC.AddNeighbor(2, nodeA);
            nodeC.AddNeighbor(8, nodeE);
            nodeC.AddNeighbor(5, nodeH);

            nodeD.AddNeighbor(1, nodeB);
            nodeD.AddNeighbor(7, nodeE);
            nodeD.AddNeighbor(3, nodeF);

            nodeE.AddNeighbor(4, nodeB);
            nodeE.AddNeighbor(8, nodeC);
            nodeE.AddNeighbor(7, nodeD);
            nodeE.AddNeighbor(5, nodeH);
            nodeE.AddNeighbor(9, nodeG);

            nodeF.AddNeighbor(3, nodeD);

            nodeG.AddNeighbor(9, nodeE);
            nodeG.AddNeighbor(6, nodeH);

            nodeH.AddNeighbor(4, nodeJ);
            nodeH.AddNeighbor(5, nodeC);
            nodeH.AddNeighbor(6, nodeG);
            nodeH.AddNeighbor(5, nodeE);
            nodeH.AddNeighbor(7, nodeI);

            nodeI.AddNeighbor(7, nodeH);

            nodeJ.AddNeighbor(4, nodeH);
            nodeJ.AddNeighbor(7, nodeA);
            return new Dictionary<string, Graph.Node>() {
                {"a", nodeA}, {"b", nodeB}, {"c", nodeC}, {"d", nodeD}, {"e", nodeE},
                {"f", nodeF}, {"g", nodeG}, {"h", nodeH}, {"i", nodeI}, {"j", nodeJ},
                {"k", nodeK}
            };
        }
    }
}