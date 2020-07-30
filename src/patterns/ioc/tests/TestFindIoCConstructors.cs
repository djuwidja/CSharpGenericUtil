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

            Assert.DoesNotThrow(() => injector.GetConstructor(typeof(TestEmptyClass)));
            Assert.DoesNotThrow(() => injector.GetConstructor(typeof(TestClassWithConstructor)));
            Assert.Throws<IoCConstructorException>(() => injector.GetConstructor(typeof(TestClassWith2InjectConstructors)));
            Assert.Throws<IoCConstructorException>(() => injector.GetConstructor(typeof(TestClassWithNoInjectConstructor)));

        }
    }
}