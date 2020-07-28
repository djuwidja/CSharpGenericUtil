using System;
using System.Collections.Generic;
using System.Reflection;

namespace Djuwidja.GenericUtil.Patterns.IoC
{
    public class Injector
    {
        private const string DEFAULT = "default";

        private Dictionary<Type, Dictionary<string, object>> _singletonContainerMap = new Dictionary<Type, Dictionary<string, object>>();
        private HashSet<Type> _supportedTypeSet = new HashSet<Type>();

        public void RegisterInstance(Type type)
        {
            if (type.IsClass)
            {
                if (!_supportedTypeSet.Contains(type))
                {
                    ConstructorInfo cInfo = GetConstructor(type);
                    if (cInfo != null)
                    {
                        _supportedTypeSet.Add(type);
                    }
                }
            }
            else
            {
                throw new InvalidIoCTypeException(string.Format("{0} must be a class.", type.FullName));
            }
        }

        public void RegisterSingleton(Type type)
        {
            RegisterSingleton(type, DEFAULT);
        }

        public void RegisterSingleton(Type type, string id)
        {
            RegisterInstance(type);

            Dictionary<string, object> objectMap;
            if (!_singletonContainerMap.TryGetValue(type, out objectMap))
            {
                objectMap  = new Dictionary<string, object>(); 
            }

            if (objectMap.ContainsKey(id))
            {
                throw new DuplicatedIoCDefinitionException(string.Format("Key {0} conflicts in Instance {1}. Key {0} already exists.", id, type.FullName));
            }
            else
            {
                objectMap[id] = CreateInstance(type);
                _singletonContainerMap[type] = objectMap;
            }
        }

        public bool IsManagedType(Type type)
        {
            return _supportedTypeSet.Contains(type);
        }

        public object NewInstance(Type type)
        {
            if (IsManagedType(type))
            {
                return CreateInstance(type);
            }
            else
            {
                throw new IoCDefinitionNotFoundException(string.Format("{0} is not managed by this injector.", type.FullName));
            }
        }

        public object Get(Type type)
        {
            try
            {
                return Get(type, DEFAULT);
            }
            catch (IoCDefinitionNotFoundException)
            {
                return CreateInstance(type);
            }
        }

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
                    throw new IoCDefinitionNotFoundException(string.Format("{0} does not have a singleton with key {1}", type.FullName, id));
                }
            }
            else
            {
                throw new IoCDefinitionNotFoundException(string.Format("{0} was not declared as a singleton.", type.FullName));
            }
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] cInfoArr = FindIoCConstructors(type);
            if (cInfoArr.Length == 0)
            {
                throw new IoCConstructorException(string.Format("No IoC compatible constructor can be found within {0}.", type.FullName));
            }
            else if (cInfoArr.Length > 1)
            {
                throw new IoCConstructorException(string.Format("More than 1 IoC compatible constructor can be found within {0}.", type.FullName));
            }

            return cInfoArr[0];
        }

        private object CreateInstance(Type type)
        {
            ConstructorInfo cInfo = GetConstructor(type);
            ParameterInfo[] cInfoParams = cInfo.GetParameters();
            object[] paramObjList = new object[cInfoParams.Length];
            for (int i = 0; i < cInfoParams.Length; i++)
            {
                ParameterInfo pInfo = cInfoParams[i];
                paramObjList[i] = CreateInstance(pInfo.ParameterType);
            }
            return cInfo.Invoke(paramObjList);
        }

        public ConstructorInfo[] FindIoCConstructors(Type type)
        {
            List<ConstructorInfo> iocConstructorList = new List<ConstructorInfo>();

            ConstructorInfo[] constructors = type.GetConstructors();
            foreach (ConstructorInfo cInfo in constructors)
            {
                if (cInfo.IsPublic && !cInfo.IsStatic)
                {
                    bool isValidIoCConstructor = true;
                    ParameterInfo[] parameters = cInfo.GetParameters();

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        ParameterInfo pInfo = parameters[i];
                        Type paramType = pInfo.ParameterType;
                        if (!paramType.IsClass)
                        {
                            isValidIoCConstructor = false;
                            break;
                        }

                        if (!_supportedTypeSet.Contains(paramType))
                        {
                            isValidIoCConstructor = false;
                            break;
                        }
                    }

                    if (isValidIoCConstructor)
                    {
                        iocConstructorList.Add(cInfo);
                    }
                }
            }

            return iocConstructorList.ToArray();
        }
    }
}
