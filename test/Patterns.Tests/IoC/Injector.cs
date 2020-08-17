﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    public class Injector
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

            IoC.Injector injector = new IoC.Injector();
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
            Assert.AreEqual(injector.Get<int>(customId), obj.IntValue);
            Assert.AreEqual(injector.Get<float>(customId), obj.FloatValue);
            Assert.AreEqual(injector.Get<TestStruct>(), obj.TestStruct);
            Assert.AreEqual(injector.Get<long>(), obj.LongValue);
            Assert.AreEqual(injector.Get<short>(customId), obj.ShortValue);
        }

        [Test]
        public void CanNewInstanceFail()
        {
            IoC.Injector injector = new IoC.Injector();
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

            IoC.Injector injector = new IoC.Injector();
            injector.Bind(defaultSingletonEmptyObj);
            injector.Bind(customSingletonEmptyObj, customSingletonId);
            injector.Bind(customPrototypeEmptyObj, InstantiationType.PROTOTYPE, customPrototypeId);

            // Default singleton
            TestEmptyClass resultDefaultSingletonObj = injector.Get<TestEmptyClass>();
            Assert.AreSame(defaultSingletonEmptyObj, resultDefaultSingletonObj);

            // custom singleton
            TestEmptyClass resultCustomSingletonObj = injector.Get<TestEmptyClass>(customSingletonId);
            Assert.AreSame(customSingletonEmptyObj, resultCustomSingletonObj);
            Assert.AreNotSame(defaultSingletonEmptyObj, resultCustomSingletonObj);

            // custom prototype
            TestEmptyClass resultCustomPrototypeObj = injector.Get<TestEmptyClass>(customPrototypeId);
            Assert.AreNotSame(customPrototypeEmptyObj, resultCustomPrototypeObj);
            Assert.AreEqual(typeof(TestEmptyClass), resultCustomPrototypeObj.GetType());
        }

        [Test]
        public void CanGetIsManagedType()
        {
            IoC.Injector injector = new IoC.Injector();
            injector.Bind(new TestEmptyClass());
            injector.Bind(568);

            Assert.IsTrue(injector.IsManagedType<TestEmptyClass>());
            Assert.IsTrue(injector.IsManagedType<int>());
            Assert.IsFalse(injector.IsManagedType<float>());
            Assert.IsFalse(injector.IsManagedType<TestClassWithConstructor>());
        }

        [Test]
        public void CanGetContainsCustomId()
        {
            string defaultId = "default";
            string customId1 = "custom1";
            string customId2 = "custom2";

            IoC.Injector injector = new IoC.Injector();
            injector.Bind(new TestEmptyClass(), customId1);
            injector.Bind(new TestEmptyClass(), customId2);
            injector.Bind(662, customId1);
            injector.Bind(477);

            Assert.IsTrue(injector.ContainsCustomId<TestEmptyClass>(customId1));
            Assert.IsTrue(injector.ContainsCustomId<TestEmptyClass>(customId2));
            Assert.IsFalse(injector.ContainsCustomId<TestEmptyClass>(defaultId));
            Assert.IsTrue(injector.ContainsCustomId<int>(customId1));
            Assert.IsTrue(injector.ContainsCustomId<int>(defaultId));
            Assert.IsFalse(injector.ContainsCustomId<int>(customId2));
        }
    }
}