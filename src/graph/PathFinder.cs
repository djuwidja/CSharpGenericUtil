using System.Collections.Generic;
using Djuwidja.GenericUtil.Collections;

namespace Djuwidja.GenericUtil.Graph
{
    /// <summary>
    /// Class <c>PathFinder</c> contains static functions to path finding algorithms.
    /// </summary>
    public static class PathFinder
    {
        /// <summary>
        /// Performs an A* search from srcNode to destNode. Throws <c>PathNotFoundException</c> if no path can be found.
        /// </summary>
        /// <param name="srcNode"></param>
        /// <param name="destNode"></param>
        /// <returns></returns>
        public static LinkedList<Edge> AStarSearch(Node srcNode, Node destNode)
        {
            PriorityQueue<double, AStarSearchProgress> pathEvalPQ = new PriorityQueue<double, AStarSearchProgress>();
            AStarSearchProgress progress = new AStarSearchProgress(){
                CurNode = srcNode
            };
            progress.Visit(new Edge(0, srcNode));
            pathEvalPQ.Put(0, progress);

            while (pathEvalPQ.Count != 0)
            {
                AStarSearchProgress finalProgress = AStarFindPath(ref pathEvalPQ, destNode);
                if (finalProgress != null)
                {
                    return finalProgress.VisitedEdges;
                }
            }
            throw new PathNotFoundException(string.Format("Path from {0} to {1} cannot be found.", srcNode, destNode));
        }
        /// <summary>
        /// Internal function for A* search.
        /// </summary>
        /// <param name="pq"></param>
        /// <param name="destNode"></param>
        /// <returns></returns>
        private static AStarSearchProgress AStarFindPath(ref PriorityQueue<double, AStarSearchProgress> pq, Node destNode)
        {
            PriorityQueueItem<double, AStarSearchProgress> pqItem = pq.Pop();
            if (pqItem != null)
            {
                double curWeight = pqItem.Priority;
                AStarSearchProgress progress = pqItem.Item;
                Node curNode = progress.CurNode;

                if (object.ReferenceEquals(curNode, destNode))
                {
                    return progress;
                }
                else
                {
                    List<Edge> edgeList = curNode.EdgeList;
                    foreach (Edge edge in edgeList)
                    {
                        if (!progress.IsNodeVisited(edge.Node))
                        {
                            double weight = curWeight + edge.Weight;
                            AStarSearchProgress newProgress = progress.Clone();
                            newProgress.Visit(edge);
                            newProgress.CurNode = edge.Node;

                            pq.Put(weight, newProgress);
                        }
                    }
                }
            }

            return null;
        }
    }
    /// <summary>
    /// Internal class to record A* search progresses.
    /// </summary>
    internal class AStarSearchProgress
    {
        private HashSet<Node> visitedNodeSet;
        private LinkedList<Edge> visitedEdgeList;
        public Node CurNode { get; set; }

        public AStarSearchProgress()
        {
            this.visitedNodeSet = new HashSet<Node>();
            this.visitedEdgeList = new LinkedList<Edge>();
        }

        private AStarSearchProgress(Node curNode, HashSet<Node> visitedNodeSet, LinkedList<Edge> visitedEdgeList)
        {
            this.CurNode = curNode;
            this.visitedNodeSet = visitedNodeSet;
            this.visitedEdgeList = visitedEdgeList;
        }

        public bool IsNodeVisited(Node node)
        {
            return this.visitedNodeSet.Contains(node);
        }

        public void Visit(Edge edge)
        {
            if (!IsNodeVisited(edge.Node))
            {
                visitedNodeSet.Add(edge.Node);
                visitedEdgeList.AddLast(edge);
            }
        }

        public AStarSearchProgress Clone()
        {
            return new AStarSearchProgress(this.CurNode, new HashSet<Node>(this.visitedNodeSet), new LinkedList<Edge>(this.visitedEdgeList));
        }

        public LinkedList<Edge> VisitedEdges {
            get {
                return visitedEdgeList;
            }
        }
    }
}