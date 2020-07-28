using System;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Graph
{
    public class Edge : IComparable<Edge>
    {
        public double Weight { get; set; }
        public Node Node { get; set; }

        public Edge(double weight, Node node)
        {
            this.Weight = weight;
            this.Node = node;
        }

        public int CompareTo(Edge obj)
        {
            return this.Weight.CompareTo(obj.Weight);
        }
    }
}