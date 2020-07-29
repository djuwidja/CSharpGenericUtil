using System;
using System.Collections.Generic;
using System.Reflection;

namespace Djuwidja.GenericUtil.Patterns.IoC
{
    public class Injector
    {
        private const string DEFAULT = "default";

        private Dictionary<Type, Dictionary<string, object>> _singletonContainerMap = new Dictionary<Type, Dictionary<string, object>>();

        public void Bind(Type type, object obj)
        {
            Bind(type, DEFAULT, obj);
        }

        public void Bind(Type type, string id, object obj)
        {
            Dictionary<string, object> objectMap;
            if (!_singletonContainerMap.TryGetValue(type, out objectMap))
            {
                objectMap  = new Dictionary<string, object>(); 
            }

            if (obj.GetType() != type)
            {
                throw new InvalidIoCTypeException(string.Format("The supplied type must the the same as the object time. {0} != {1}.", type.FullName, obj.GetType().FullName));
            }

            objectMap[id] = obj;
            _singletonContainerMap[type] = objectMap;
            
        }

        public bool IsManagedType(Type type)
        {
            return _singletonContainerMap.ContainsKey(type);
        }

        public object Get(Type type)
        {
            return Get(type, DEFAULT);
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

        public object InjectConstructor(Type type)
        {
            return CreateInstance(type);
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] cInfoArr = FindIoCConstructors(type);
            if (cInfoArr.Length == 0)
            {
                throw new IoCConstructorException(string.Format("No IoC compatible constructor can be found within {0}.", type.FullName));
            }

            return cInfoArr[0];
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
                        if (!_singletonContainerMap.ContainsKey(paramType))
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

        private object CreateInstance(Type type)
        {
            ConstructorInfo cInfo = GetConstructor(type);
            ParameterInfo[] cInfoParams = cInfo.GetParameters();
            object[] paramObjList = new object[cInfoParams.Length];
            for (int i = 0; i < cInfoParams.Length; i++)
            {
                ParameterInfo pInfo = cInfoParams[i];
                if (_singletonContainerMap.ContainsKey(pInfo.ParameterType))
                {
                    paramObjList[i] = _singletonContainerMap[pInfo.ParameterType][DEFAULT];
                }
                else
                {
                    paramObjList[i] = CreateInstance(pInfo.ParameterType);
                }
            }
            return cInfo.Invoke(paramObjList);
        }
    }
}
