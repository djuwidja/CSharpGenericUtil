using System.Collections.Generic;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;
using System.Reflection;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    public class TestFindIoCConstructors
    {
        [Test]
        public void CanGetConstructorInfo()
        {
            Injector injector = new Injector();
            injector.RegisterInstance(typeof(TestEmptyClass));
            injector.RegisterInstance(typeof(TestClassWithConstructor));
            Assert.Throws<IoCConstructorException>(() => injector.RegisterInstance(typeof(TestClassWith2Constructors)));
            Assert.Throws<IoCConstructorException>(() => injector.RegisterInstance(typeof(TestClassWithGenericTypeParamInConstructor)));

            Assert.AreEqual(1, injector.FindIoCConstructors(typeof(TestEmptyClass)).Length);
            Assert.AreEqual(1, injector.FindIoCConstructors(typeof(TestClassWithConstructor)).Length);
            Assert.AreEqual(2, injector.FindIoCConstructors(typeof(TestClassWith2Constructors)).Length);
            Assert.AreEqual(0, injector.FindIoCConstructors(typeof(TestClassWithGenericTypeParamInConstructor)).Length);

        }
    }
}