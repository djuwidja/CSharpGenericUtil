using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    class TestRegistrationAndCreation
    {
        [Test]
        public void CanRegisterAndCreateInstance()
        {
            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.RegisterInstance(typeof(TestEmptyClass)));
            Assert.Throws<IoCConstructorException>(() => injector.RegisterInstance(typeof(string)));
            Assert.Throws<InvalidIoCTypeException>(() => injector.RegisterInstance(typeof(int)));            
            Assert.Throws<InvalidIoCTypeException>(() => injector.RegisterInstance(typeof(TestStruct)));

            Assert.True(injector.IsManagedType(typeof(TestEmptyClass)));
            Assert.False(injector.IsManagedType(typeof(string)));
            Assert.False(injector.IsManagedType(typeof(int)));
            Assert.False(injector.IsManagedType(typeof(TestStruct)));

            object testEmptyClassObj1 = injector.NewInstance(typeof(TestEmptyClass));
            object testEmptyClassObj2 = injector.NewInstance(typeof(TestEmptyClass));
            Assert.AreNotEqual(testEmptyClassObj1, testEmptyClassObj2);

            Assert.Throws<IoCDefinitionNotFoundException>(() => injector.NewInstance(typeof(string)));
            Assert.Throws<IoCDefinitionNotFoundException>(() => injector.NewInstance(typeof(TestStruct)));
        }

        [Test]
        public void CanRegisterSingleton()
        {
            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.RegisterSingleton(typeof(TestEmptyClass)));
            Assert.Throws<IoCConstructorException>(() => injector.RegisterSingleton(typeof(string)));
            Assert.Throws<InvalidIoCTypeException>(() => injector.RegisterInstance(typeof(int)));
            Assert.Throws<InvalidIoCTypeException>(() => injector.RegisterInstance(typeof(TestStruct)));

            Assert.True(injector.IsManagedType(typeof(TestEmptyClass)));
            Assert.False(injector.IsManagedType(typeof(string)));
            Assert.False(injector.IsManagedType(typeof(int)));
            Assert.False(injector.IsManagedType(typeof(TestStruct)));

            object testEmptyClassObj1 = injector.Get(typeof(TestEmptyClass));
            object testEmptyClassObj2 = injector.Get(typeof(TestEmptyClass));
            Assert.AreSame(testEmptyClassObj1, testEmptyClassObj2);

            Assert.Throws<IoCDefinitionNotFoundException>(() => injector.NewInstance(typeof(string)));
            Assert.Throws<IoCDefinitionNotFoundException>(() => injector.NewInstance(typeof(TestStruct)));

            Assert.DoesNotThrow(() => injector.RegisterSingleton(typeof(TestEmptyClass), "test1"));
            object testEmptyClassObj3 = injector.Get(typeof(TestEmptyClass), "test1");
            object testEmptyClassObj4 = injector.Get(typeof(TestEmptyClass), "test1");
            Assert.AreSame(testEmptyClassObj3, testEmptyClassObj4);
            Assert.AreNotSame(testEmptyClassObj2, testEmptyClassObj3);
        }
    }
}
