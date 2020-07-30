using System;
using System.Collections.Generic;
using System.Text;

namespace Djuwidja.GenericUtil.Patterns.IoC.Attributes
{
    /// <summary>
    /// Attribute to tag a Property Injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class InjectProperty : Attribute
    {
        public string Id { get; }

        public InjectProperty(string id = Djuwidja.GenericUtil.Patterns.IoC.Injector.DEFAULT)
        {
            this.Id = id;
        }
    }
    /// <summary>
    /// Attribute to be used in a method or constructor parameters to specify the custom id of the object to be injected.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class ID : Attribute
    {
        public string Id { get; }

        public ID(string id = Djuwidja.GenericUtil.Patterns.IoC.Injector.DEFAULT)
        {
            this.Id = id;
        }
    }
    /// <summary>
    /// Attribute to tag a Constructor Injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public sealed class InjectConstructor : Attribute
    {
        public InjectConstructor()
        {

        }
    }
    /// <summary>
    /// Attribute to tag a Method Injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InjectMethod : Attribute
    {
        public InjectMethod()
        {

        }
    }
}
