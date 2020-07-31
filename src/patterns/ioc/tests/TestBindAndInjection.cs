using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    public class TestBindAndInjection
    {
        [Test]
        public void CanBind()
        {
            TestStruct testStruct;
            testStruct.idx = 8;
            testStruct.value = 99;
            const string customKey = "custom";
            const string defaultTestStr = "defaultTestStr";
            const string customTestStr = "customTestStr";
            const int defaultTestInt = 6;
            const int customTestInt = 88;

            DependencyContainer container = new DependencyContainer();
            Assert.DoesNotThrow(() => container.Bind(typeof(TestEmptyClass), new TestEmptyClass()));
            Assert.DoesNotThrow(() => container.Bind(typeof(string), defaultTestStr));
            Assert.DoesNotThrow(() => container.Bind(typeof(string), customKey, customTestStr));
            Assert.DoesNotThrow(() => container.Bind(typeof(int), defaultTestInt));
            Assert.DoesNotThrow(() => container.Bind(typeof(int), customKey, customTestInt));
            Assert.DoesNotThrow(() => container.Bind(typeof(TestStruct), testStruct));

            Assert.True(container.IsManagedType(typeof(TestEmptyClass)));
            Assert.True(container.IsManagedType(typeof(string)));
            Assert.True(container.IsManagedType(typeof(int)));
            Assert.True(container.IsManagedType(typeof(TestStruct)));

            object testEmptyClassObj1 = container.Get(typeof(TestEmptyClass));
            object testEmptyClassObj2 = container.Get(typeof(TestEmptyClass));
            Assert.AreSame(testEmptyClassObj1, testEmptyClassObj2);

            Assert.AreEqual(defaultTestStr, container.Get(typeof(string)));
            Assert.AreEqual(customTestStr, container.Get(typeof(string), customKey));

            Assert.AreEqual(defaultTestInt, container.Get(typeof(int)));
            Assert.AreEqual(customTestInt, container.Get(typeof(int), customKey));

            Assert.DoesNotThrow(() => container.Bind(typeof(TestEmptyClass), customKey, new TestEmptyClass()));
            object testEmptyClassObj3 = container.Get(typeof(TestEmptyClass), customKey);
            object testEmptyClassObj4 = container.Get(typeof(TestEmptyClass), customKey);
            Assert.AreSame(testEmptyClassObj3, testEmptyClassObj4);
            Assert.AreNotSame(testEmptyClassObj2, testEmptyClassObj3);
        }

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

            Injector injector = new Injector(new DependencyContainer());
            injector.Container.Bind(typeof(float), defaultTestFloat);
            injector.Container.Bind(typeof(float), customId, customTestFloat);
            injector.Container.Bind(typeof(TestStruct), testStruct);
            injector.Container.Bind(typeof(int), defaultTestInt);
            injector.Container.Bind(typeof(int), customId, customTestInt);
            injector.Container.Bind(typeof(TestEmptyClass), injector.NewInstance(typeof(TestEmptyClass)));
            injector.Container.Bind(typeof(long), defaultTestLong);
            injector.Container.Bind(typeof(long), customId, customTestLong);
            injector.Container.Bind(typeof(short), defaultTestShort);
            injector.Container.Bind(typeof(short), customId, customTestShort);

            TestClassWithConstructorInjection result = (TestClassWithConstructorInjection) injector.NewInstance(typeof(TestClassWithConstructorInjection));
            Assert.NotNull(result);
            Assert.AreEqual(injector.Container.Get(typeof(int), customId), result.IntValue);
            Assert.AreEqual(injector.Container.Get(typeof(float), customId), result.FloatValue);
            Assert.AreEqual(injector.Container.Get(typeof(TestStruct)), result.TestStruct);
            Assert.AreEqual(injector.Container.Get(typeof(long)), result.LongValue);
            Assert.AreEqual(injector.Container.Get(typeof(short), customId), result.ShortValue);
        }

        [Test]
        public void CanNewInstanceFail()
        {
            Injector injector = new Injector(new DependencyContainer());
            Assert.DoesNotThrow(() => injector.Container.Bind(typeof(TestEmptyClass), new TestEmptyClass()));

            Assert.Throws<IoCConstructorException>(() => injector.NewInstance(typeof(TestClassWithConstructorInjectionFail)));
        }
    }
}
