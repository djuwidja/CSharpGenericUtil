using System;
using System.Collections.Generic;
using System.Text;

namespace Djuwidja.GenericUtil.Patterns.Observer
{
    using System.Collections.Generic;

    /// <summary>
    /// The publisher (subject) in an observer pattern. Each events are tagged by "eventID".
    /// These "eventID" will be returned to observers when Publish() is called so that observers know 
    /// what has been changed in the subject. EventIDType's type is user-defined and enforced by template.
    /// This class is NOT thread-safe.
    /// </summary>
    /// <typeparam name="EventIDType">the type of id</typeparam>
    /// <typeparam name="EventClassType">the type of event object returned to the observer.</typeparam>
    public class ObserverEventPublisher<EventIDType, EventClassType> where EventClassType : ObserverEvent<EventIDType>
    {
        /// <summary>
        /// /// Delegate for callbacks that publishers can invoke when a state changes.
        /// </summary>
        /// <param name="changeEventList"></param>
        public delegate void EventSubscriberCB(List<EventClassType> changeEventList);
        /// <summary>
        /// stores a set of unpublished changes. Consumed by calling Publish().
        /// </summary>
        private List<EventClassType> _unpublishedChangeList;
        /// <summary>
        /// stores relationship of each "eventID" with their listening callbacks.
        /// </summary>
        private Dictionary<EventIDType, HashSet<EventSubscriberCB>> _registeredSubscriberCBMap;

        public ObserverEventPublisher()
        {
            _unpublishedChangeList = new List<EventClassType>();
            _registeredSubscriberCBMap = new Dictionary<EventIDType, HashSet<EventSubscriberCB>>();
        }

        ~ObserverEventPublisher()
        {
            _unpublishedChangeList.Clear();
            _registeredSubscriberCBMap.Clear();
            _unpublishedChangeList = null;
            _registeredSubscriberCBMap = null;
        }
        /// <summary>
        /// Subscribes an observer that observes one eventID to a callback.
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="cb"></param>
        public void Subscribe(EventSubscriberCB cb, EventIDType eventID)
        {
            HashSet<EventSubscriberCB> subscriberCBSet;
            if (!_registeredSubscriberCBMap.TryGetValue(eventID, out subscriberCBSet))
            {
                subscriberCBSet = new HashSet<EventSubscriberCB>();
            }
            subscriberCBSet.Add(cb);
            _registeredSubscriberCBMap[eventID] = subscriberCBSet;
        }
        /// <summary>
        /// Subscribes an observer that observes multiple eventIDs to one callback.
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="eventIDs"></param>
        public void Subscribe(EventSubscriberCB cb, params EventIDType[] eventIDs)
        {
            foreach (EventIDType propertyAlias in eventIDs)
            {
                Subscribe(cb, propertyAlias);
            }
        }
        /// <summary>
        /// Unsubscribe a observer callback to an eventID. The pair must exist for this to be successful.
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="cb"></param>
        /// <returns>if the action succeed.</returns>
        public bool Unsubscribe(EventSubscriberCB cb, EventIDType eventID)
        {
            if (_registeredSubscriberCBMap.ContainsKey(eventID))
            {
                HashSet<EventSubscriberCB> subscriberCBSet = _registeredSubscriberCBMap[eventID];
                bool isSuccess = subscriberCBSet.Remove(cb);

                if (subscriberCBSet.Count == 0)
                {
                    _registeredSubscriberCBMap.Remove(eventID);
                }

                return isSuccess;
            }

            return false;
        }
        /// <summary>
        /// Check if a eventID is being observed.
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public bool IsObservingEventID(EventIDType eventID)
        {
            return _registeredSubscriberCBMap.ContainsKey(eventID);
        }
        /// <summary>
        /// Check if a observer's callback is subscribed with a eventID.
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        public bool IsSubscribedCB(EventSubscriberCB cb)
        {
            foreach (KeyValuePair<EventIDType, HashSet<EventSubscriberCB>> entry in _registeredSubscriberCBMap)
            {
                HashSet<EventSubscriberCB> cbSet = entry.Value;
                if (cbSet.Contains(cb))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Check if this publisher has unpublished changes.
        /// </summary>
        public bool HasUnpublishedChanges
        {
            get => _unpublishedChangeList.Count > 0;
        }
        /// <summary>
        /// Meant to be called by child class when a property change occurs.
        /// </summary>
        /// <param name="propertyAlias"></param>
        protected void RegisterChange(EventClassType changeEvent)
        {
            _unpublishedChangeList.Add(changeEvent);
        }
        /// <summary>
        /// Meant to be called once the transaction, in its entirety, has completed. It will publish events to all callbacks registered to the set of
        /// eventIDs that changes are associated with. Each callbacks will only receive state changes that are applicable to them. (If the cb is
        /// registered to the eventID propertyA, it will only received propertyA in stateChangeSet event if the transaction also changes propertyB
        /// and propertyC.)
        /// </summary>
        public void Publish()
        {
            Dictionary<EventSubscriberCB, List<EventClassType>> invokeCBMap = new Dictionary<EventSubscriberCB, List<EventClassType>>();
            foreach (EventClassType changeEvent in _unpublishedChangeList)
            {
                if (_registeredSubscriberCBMap.ContainsKey(changeEvent.EventType))
                {
                    HashSet<EventSubscriberCB> stateObserverCBSet = _registeredSubscriberCBMap[changeEvent.EventType];
                    foreach (EventSubscriberCB cb in stateObserverCBSet)
                    {
                        if (cb != null) // just to make sure the cb is not disposed by external entities.
                        {
                            List<EventClassType> stateChangeEventList;
                            if (!invokeCBMap.TryGetValue(cb, out stateChangeEventList))
                            {
                                stateChangeEventList = new List<EventClassType>();
                            }
                            stateChangeEventList.Add(changeEvent);
                            invokeCBMap[cb] = stateChangeEventList;
                        }
                    }
                }
            }

            foreach (KeyValuePair<EventSubscriberCB, List<EventClassType>> entry in invokeCBMap)
            {
                EventSubscriberCB cb = entry.Key;
                cb?.Invoke(entry.Value);
            }

            _unpublishedChangeList.Clear();
        }
    }
}
