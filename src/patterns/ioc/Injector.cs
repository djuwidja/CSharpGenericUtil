using System;
using System.Collections.Generic;
using System.Reflection;
using Djuwidja.GenericUtil.Patterns.IoC.Attributes;

namespace Djuwidja.GenericUtil.Patterns.IoC
{
    public sealed class Injector
    {
        public const string DEFAULT = "default";

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
                objectMap = new Dictionary<string, object>();
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
                    throw new IoCDefinitionNotFoundException(string.Format("{0} does not have a resource with key {1}", type.FullName, id));
                }
            }
            else
            {
                throw new IoCDefinitionNotFoundException(string.Format("{0} is not managed by this injector.", type.FullName));
            }
        }
        public object NewInstance(Type type)
        {
            //Constructor Injection
            ConstructorInfo cInfo = GetConstructor(type);
            ParameterInfo[] cInfoParams = cInfo.GetParameters();
            object[] constructorParamList = ComputeParamInjection(cInfoParams);
            object result = cInfo.Invoke(constructorParamList);

            //Method Injection
            MethodInfo[] mInfoArr = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (MethodInfo mInfo in mInfoArr)
            {
                InjectMethod customInjectMethod = mInfo.GetCustomAttribute<InjectMethod>();
                if (customInjectMethod != null)
                {
                    ParameterInfo[] mInfoParams = mInfo.GetParameters();
                    object[] methodParamList = ComputeParamInjection(mInfoParams);
                    mInfo.Invoke(result, methodParamList);
                }
            }

            //Field Injection
            FieldInfo[] fInfoArr = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo fInfo in fInfoArr)
            {
                string resId;
                if (FindInjectProperties(fInfo, fInfo.FieldType, out resId))
                {
                    fInfo.SetValue(result, _singletonContainerMap[fInfo.FieldType][resId]);
                }
            }

            //Property Injection
            PropertyInfo[] pInfoArr = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (PropertyInfo pInfo in pInfoArr)
            {
                string resId;
                if (FindInjectProperties(pInfo, pInfo.PropertyType, out resId))
                {
                    pInfo.SetValue(result, _singletonContainerMap[pInfo.PropertyType][resId]);
                }
            }

            return result;
        }
        private bool FindInjectProperties(MemberInfo info, Type infoType, out string resId)
        {
            InjectProperty customInject = info.GetCustomAttribute<InjectProperty>();
            if (customInject != null)
            {
                if (VerifyManagedType(infoType, customInject.Id))
                {
                    resId = customInject.Id;
                    return true;
                }
            }

            resId = "";
            return false;
        }
        private object[] ComputeParamInjection(ParameterInfo[] paramArr)
        {
            object[] paramObjList = new object[paramArr.Length];
            for (int i = 0; i < paramArr.Length; i++)
            {
                ParameterInfo pInfo = paramArr[i];
                ID injectAttr = pInfo.GetCustomAttribute<ID>();
                string resId = DEFAULT;
                if (injectAttr != null)
                {
                    resId = injectAttr.Id;
                }

                if (VerifyManagedType(pInfo.ParameterType, resId))
                {
                    paramObjList[i] = _singletonContainerMap[pInfo.ParameterType][resId];
                }
            }
            return paramObjList;
        }
        private bool VerifyManagedType(Type type, string id)
        {
            if (IsManagedType(type))
            {
                Dictionary<string, object> resMap = _singletonContainerMap[type];
                if (resMap.ContainsKey(id))
                {
                    return true;
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
        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] cInfoArr = FindIoCConstructors(type);
            if (cInfoArr.Length == 0)
            {
                throw new IoCConstructorException(string.Format("No IoC compatible constructor can be found within {0}.", type.FullName));
            }
            else if (cInfoArr.Length > 1)
            {
                throw new IoCConstructorException(string.Format("Only 1 InjectConstructor is allowed within {0}.", type.FullName));
            }

            return cInfoArr[0];
        }
        private ConstructorInfo[] FindIoCConstructors(Type type)
        {
            List<ConstructorInfo> iocConstructorList = new List<ConstructorInfo>();

            ConstructorInfo[] constructors = type.GetConstructors();
            foreach (ConstructorInfo cInfo in constructors)
            {
                if (cInfo.IsPublic && !cInfo.IsStatic)
                {
                    bool isValidIoCConstructor = true;
                    InjectConstructor cInject = cInfo.GetCustomAttribute<InjectConstructor>();
                    if (cInject == null)
                    {
                        isValidIoCConstructor = false;
                    }
                    else
                    {
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
