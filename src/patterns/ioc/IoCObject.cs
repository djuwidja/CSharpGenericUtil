using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Djuwidja.GenericUtil.Patterns.IoC.Attributes;

namespace Djuwidja.GenericUtil.Patterns.IoC
{
    internal class IoCObject
    {
        private InstantiationType _instantiationType;
        private object _obj;

        public InstantiationType InstantiationType { get { return _instantiationType; } }
        public object Object { get { return _obj; } }

        public IoCObject(InstantiationType instType, object obj)
        {
            this._instantiationType = instType;
            this._obj = obj;
        }
    }
}
