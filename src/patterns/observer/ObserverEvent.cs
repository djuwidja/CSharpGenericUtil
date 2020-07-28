using System.Collections;
using System.Collections.Generic;

namespace Djuwidja.GenericUtil.Patterns.Observer
{
    public class ObserverEvent<T>
    {
        private T _eventType;

        public ObserverEvent(T eventType)
        {
            _eventType = eventType;
        }

        public T EventType
        {
            get => _eventType;
        }
    }
}