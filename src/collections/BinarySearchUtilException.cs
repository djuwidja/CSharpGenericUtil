using System;

namespace Djuwidja.GenericUtil.Collections
{
    [Serializable]
    public class ElementNotInListException : Exception
    {
        public ElementNotInListException() {}
        public ElementNotInListException(string message) : base(message) {}
        public ElementNotInListException(string message, Exception innerException) : base(message, innerException) {}
    }
}