using System;
using System.Collections.Generic;
using System.Text;

namespace Djuwidja.GenericUtil.Patterns.IoC
{
    public class DependencyContainer
    {
        public const string DEFAULT = "default";

        private Dictionary<Type, Dictionary<string, object>> _singletonContainerMap = new Dictionary<Type, Dictionary<string, object>>();
        /// <summary>
        /// Bind an object to a type with default key. Object must have the supplied type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="obj">Target object.</param>
        public void Bind(Type type, object obj)
        {
            Bind(type, DEFAULT, obj);
        }
        /// <summary>
        /// Bind an object to a type with supplied id as key. Object must have the supplied type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="id">Custom id of the object.</param>
        /// <param name="obj">Target object.</param>
        public void Bind(Type type, string id, object obj)
        {
            Dictionary<string, object> objectMap;
            if (!_singletonContainerMap.TryGetValue(type, out objectMap))
            {
                objectMap = new Dictionary<string, object>();
            }

            if (obj.GetType() != type)
            {
                throw new InvalidIoCTypeException(string.Format("The supplied type must the the same as the object time. {0} != {1}.", type.FullName, obj.GetType().FullName));
            }

            objectMap[id] = obj;
            _singletonContainerMap[type] = objectMap;
        }
        /// <summary>
        /// Returns true if the supplied type is binded to an object in this injector.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <returns></returns>
        public bool IsManagedType(Type type)
        {
            return _singletonContainerMap.ContainsKey(type);
        }

        /// <summary>
        /// Get the default object that was binded to the type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <returns></returns>
        public object Get(Type type)
        {
            return Get(type, DEFAULT);
        }
        /// <summary>
        /// Get the object with supplied id as key that was binded to the type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="id">Custom id of the object.</param>
        /// <returns></returns>
        public object Get(Type type, string id)
        {
            if (!IsManagedType(type))
            {
                throw new IoCDefinitionNotFoundException(string.Format("{0} is not managed by this injector.", type.FullName));
            }

            if (_singletonContainerMap.ContainsKey(type))
            {
                Dictionary<string, object> objMap = _singletonContainerMap[type];
                if (objMap.ContainsKey(id))
                {
                    return objMap[id];
                }
                else
                {
                    throw new IoCDefinitionNotFoundException(string.Format("{0} does not have a resource with key {1}", type.FullName, id));
                }
            }
            else
            {
                throw new IoCDefinitionNotFoundException(string.Format("{0} is not managed by this injector.", type.FullName));
            }
        }
        public bool Contains(Type type, string id)
        {
            if (IsManagedType(type))
            {
                Dictionary<string, object> objMap = _singletonContainerMap[type];
                return objMap.ContainsKey(id);
            }

            return false;
        }
    }
}
