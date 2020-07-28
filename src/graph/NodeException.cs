using System;

namespace Djuwidja.GenericUtil.Graph
{
    [Serializable]
    public class NodeIsAlreadyNeighborException : Exception
    {
        public NodeIsAlreadyNeighborException() {}
        public NodeIsAlreadyNeighborException(string message) : base(message) {}
        public NodeIsAlreadyNeighborException(string message, Exception innerException) : base(message, innerException) {}
    }
}