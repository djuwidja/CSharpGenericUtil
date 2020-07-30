using System;
using System.Collections.Generic;
using System.Text;

namespace Djuwidja.GenericUtil.Patterns.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class Inject : Attribute
    {
        public string Id { get; }

        public Inject(string id = Djuwidja.GenericUtil.Patterns.IoC.Injector.DEFAULT)
        {
            this.Id = id;
        }
    }
    
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public class InjectConstructor : Attribute
    {
        public InjectConstructor()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InjectMethod : Attribute
    {
        public InjectMethod()
        {

        }
    }
}
