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

            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestEmptyClass), new TestEmptyClass()));
            Assert.DoesNotThrow(() => injector.Bind(typeof(string), defaultTestStr));
            Assert.DoesNotThrow(() => injector.Bind(typeof(string), customKey, customTestStr));
            Assert.DoesNotThrow(() => injector.Bind(typeof(int), defaultTestInt));
            Assert.DoesNotThrow(() => injector.Bind(typeof(int), customKey, customTestInt));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestStruct), testStruct));

            Assert.True(injector.IsManagedType(typeof(TestEmptyClass)));
            Assert.True(injector.IsManagedType(typeof(string)));
            Assert.True(injector.IsManagedType(typeof(int)));
            Assert.True(injector.IsManagedType(typeof(TestStruct)));

            object testEmptyClassObj1 = injector.Get(typeof(TestEmptyClass));
            object testEmptyClassObj2 = injector.Get(typeof(TestEmptyClass));
            Assert.AreSame(testEmptyClassObj1, testEmptyClassObj2);

            Assert.AreEqual(defaultTestStr, injector.Get(typeof(string)));
            Assert.AreEqual(customTestStr, injector.Get(typeof(string), customKey));

            Assert.AreEqual(defaultTestInt, injector.Get(typeof(int)));
            Assert.AreEqual(customTestInt, injector.Get(typeof(int), customKey));

            Assert.DoesNotThrow(() => injector.Bind(typeof(TestEmptyClass), customKey, new TestEmptyClass()));
            object testEmptyClassObj3 = injector.Get(typeof(TestEmptyClass), customKey);
            object testEmptyClassObj4 = injector.Get(typeof(TestEmptyClass), customKey);
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

            Injector injector = new Injector();
            injector.Bind(typeof(float), defaultTestFloat);
            injector.Bind(typeof(float), customId, customTestFloat);
            injector.Bind(typeof(TestStruct), testStruct);
            injector.Bind(typeof(int), defaultTestInt);
            injector.Bind(typeof(int), customId, customTestInt);
            injector.Bind(typeof(TestEmptyClass), injector.NewInstance(typeof(TestEmptyClass)));
            injector.Bind(typeof(long), defaultTestLong);
            injector.Bind(typeof(long), customId, customTestLong);
            injector.Bind(typeof(short), defaultTestShort);
            injector.Bind(typeof(short), customId, customTestShort);

            TestClassWithConstructorInjection result = (TestClassWithConstructorInjection) injector.NewInstance(typeof(TestClassWithConstructorInjection));
            Assert.NotNull(result);
            Assert.AreEqual(injector.Get(typeof(int), customId), result.IntValue);
            Assert.AreEqual(injector.Get(typeof(float), customId), result.FloatValue);
            Assert.AreEqual(injector.Get(typeof(TestStruct)), result.TestStruct);
            Assert.AreEqual(injector.Get(typeof(long)), result.LongValue);
            Assert.AreEqual(injector.Get(typeof(short), customId), result.ShortValue);
        }

        [Test]
        public void CanNewInstanceFail()
        {
            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestEmptyClass), new TestEmptyClass()));

            Assert.Throws<IoCConstructorException>(() => injector.NewInstance(typeof(TestClassWithConstructorInjectionFail)));
        }
    }
}
