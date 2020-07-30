using System;
using System.Collections.Generic;
using System.Text;

namespace Djuwidja.GenericUtil.Patterns.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class InjectProperty : Attribute
    {
        public string Id { get; }

        public InjectProperty(string id = Djuwidja.GenericUtil.Patterns.IoC.Injector.DEFAULT)
        {
            this.Id = id;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class ID : Attribute
    {
        public string Id { get; }

        public ID(string id = Djuwidja.GenericUtil.Patterns.IoC.Injector.DEFAULT)
        {
            this.Id = id;
        }
    }
    
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public sealed class InjectConstructor : Attribute
    {
        public InjectConstructor()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InjectMethod : Attribute
    {
        public InjectMethod()
        {

        }
    }
}
