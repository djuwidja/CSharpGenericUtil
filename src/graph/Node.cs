using System.Collections.Generic;
using Djuwidja.GenericUtil.Collections;

namespace Djuwidja.GenericUtil.Graph
{
    public class Node
    {
        private HashSet<Node> NeighborSet;
        private PriorityQueue<double, Edge> EdgePQueue;
        
        public Node()
        {
            this.NeighborSet = new HashSet<Node>();
            this.EdgePQueue = new PriorityQueue<double, Edge>();
        }

        public void AddNeighbor(double weight, Node node)
        {
            if (NeighborSet.Contains(node))
            {
                throw new NodeIsAlreadyNeighborException(string.Format("Node {0} is already a neighbor of {1}", node, this));
            }
            Edge edge = new Edge(weight, node);
            this.EdgePQueue.Put(weight, edge);
            this.NeighborSet.Add(node);
        }

        public bool IsNeighbor(Node node)
        {
            return this.NeighborSet.Contains(node);
        }

        public List<Node> NeighborList {
            get {
                List<Node> neighborList = new List<Node>();
                EdgePQueue.ItemQueue.ForEach(o => neighborList.Add(o.Item.Node));
                return neighborList;
            }
        }

        public List<Edge> EdgeList {
            get {
                List<Edge> edgeList = new List<Edge>();
                EdgePQueue.ItemQueue.ForEach(o => edgeList.Add(o.Item));
                return edgeList;
            }
        }
    }
}