using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    public class TestInjector
    {
         [Test]
        public void CanNewInstance()
        {
            const string customId = "custom";
            const float defaultTestFloat = 0.6f;
            const float customTestFloat = 1.14f;

            TestStruct testStruct;
            testStruct.idx = 8;
            testStruct.value = 99;

            const int defaultTestInt = 6;
            const int customTestInt = 88;

            const long defaultTestLong = 8864;
            const long customTestLong = 7143;

            const short defaultTestShort = 3;
            const short customTestShort = 16;

            Injector injector = new Injector();
            // Dependency cannot be found
            Assert.Throws<IoCConstructorException>(() =>  injector.NewInstance<TestClassWithConstructor>());

            injector.Bind(defaultTestFloat);
            injector.Bind(customTestFloat, customId);
            injector.Bind(testStruct);
            injector.Bind(defaultTestInt);
            injector.Bind(customTestInt, customId);
            injector.Bind(injector.NewInstance<TestEmptyClass>());
            injector.Bind(defaultTestLong);
            injector.Bind(customTestLong, customId);
            injector.Bind(defaultTestShort);
            injector.Bind(customTestShort, customId);

            // Dependecy can be found.
            // Prototype
            Assert.DoesNotThrow(() => injector.NewInstance<TestClassWithConstructor>());


            //Singleton
            TestClassWithConstructorInjection obj = injector.NewInstance<TestClassWithConstructorInjection>();
            Assert.NotNull(obj);
            Assert.AreEqual(injector.Get(typeof(int), customId), obj.IntValue);
            Assert.AreEqual(injector.Get(typeof(float), customId), obj.FloatValue);
            Assert.AreEqual(injector.Get(typeof(TestStruct)), obj.TestStruct);
            Assert.AreEqual(injector.Get(typeof(long)), obj.LongValue);
            Assert.AreEqual(injector.Get(typeof(short), customId), obj.ShortValue);
        }

        [Test]
        public void CanNewInstanceFail()
        {
            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.Bind(new TestEmptyClass()));

            Assert.Throws<IoCConstructorException>(() => injector.NewInstance<TestClassWithConstructorInjectionFail>());
        }

        [Test]
        public void CanBindAndGet()
        {
            const string customSingletonId = "customSingleton";
            const string customPrototypeId = "customPrototype";

            TestEmptyClass defaultSingletonEmptyObj = new TestEmptyClass();
            TestEmptyClass customSingletonEmptyObj = new TestEmptyClass();
            TestEmptyClass customPrototypeEmptyObj = new TestEmptyClass();

            Injector injector = new Injector();
            injector.Bind(defaultSingletonEmptyObj);
            injector.Bind(customSingletonEmptyObj, customSingletonId);
            injector.Bind(customPrototypeEmptyObj, InstantiationType.PROTOTYPE, customPrototypeId);

            // Default singleton
            TestEmptyClass resultDefaultSingletonObj = (TestEmptyClass) injector.Get(typeof(TestEmptyClass));
            Assert.AreSame(defaultSingletonEmptyObj, resultDefaultSingletonObj);

            // custom singleton
            TestEmptyClass resultCustomSingletonObj = (TestEmptyClass)injector.Get(typeof(TestEmptyClass), customSingletonId);
            Assert.AreSame(customSingletonEmptyObj, resultCustomSingletonObj);
            Assert.AreNotSame(defaultSingletonEmptyObj, resultCustomSingletonObj);

            // custom prototype
            TestEmptyClass resultCustomPrototypeObj = (TestEmptyClass)injector.Get(typeof(TestEmptyClass), customPrototypeId);
            Assert.AreNotSame(customPrototypeEmptyObj, resultCustomPrototypeObj);
            Assert.AreEqual(typeof(TestEmptyClass), resultCustomPrototypeObj.GetType());
        }
    }
}
