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
            TestStruct testStruct;
            testStruct.idx = 10;
            testStruct.value = 88;

            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.Bind(typeof(int), 78));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestStruct), testStruct));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestEmptyClass), new TestEmptyClass()));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestClassWithConstructor), new TestClassWithConstructor(new TestEmptyClass())));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestClassWith2Constructors), new TestClassWith2Constructors()));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestClassWithGenericTypeParamInConstructor), new TestClassWithGenericTypeParamInConstructor(5, new TestEmptyClass())));

            Assert.AreEqual(1, injector.FindIoCConstructors(typeof(TestEmptyClass)).Length);
            Assert.AreEqual(1, injector.FindIoCConstructors(typeof(TestClassWithConstructor)).Length);
            Assert.AreEqual(2, injector.FindIoCConstructors(typeof(TestClassWith2Constructors)).Length);
            Assert.AreEqual(2, injector.FindIoCConstructors(typeof(TestClassWithGenericTypeParamInConstructor)).Length);
        }
    }
}