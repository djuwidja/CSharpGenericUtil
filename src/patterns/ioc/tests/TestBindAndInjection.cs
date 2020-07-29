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
        public void CanInject()
        {
            TestStruct testStruct;
            testStruct.idx = 8;
            testStruct.value = 99;
            const string defaultTestStr = "defaultTestStr";
            const int defaultTestInt = 6;      

            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestEmptyClass), new TestEmptyClass()));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestClassWithConstructor), new TestClassWithConstructor((TestEmptyClass) injector.Get(typeof(TestEmptyClass)))));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestClassWith2Constructors), new TestClassWith2Constructors()));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestClassWithGenericTypeParamInConstructor), new TestClassWithGenericTypeParamInConstructor(defaultTestInt, (TestEmptyClass)injector.Get(typeof(TestEmptyClass)))));

            Assert.DoesNotThrow(() => injector.Bind(typeof(string), defaultTestStr));
            Assert.DoesNotThrow(() => injector.Bind(typeof(int), defaultTestInt));
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestStruct), testStruct));

            TestClassWithConstructorInjection result = (TestClassWithConstructorInjection) injector.InjectConstructor(typeof(TestClassWithConstructorInjection));
            Assert.AreEqual(defaultTestStr, result.TestStr);
            Assert.AreEqual(defaultTestInt, result.TestInt);
            Assert.AreEqual(testStruct, result.TestStruct);
            Assert.AreSame(injector.Get(typeof(TestEmptyClass)), result.TestEmptyCls);
            Assert.AreSame(injector.Get(typeof(TestClassWithConstructor)), result.TestClsWithConstructor);
            Assert.AreSame(injector.Get(typeof(TestClassWith2Constructors)), result.TestClsWith2Constructor);
            Assert.AreSame(injector.Get(typeof(TestClassWithGenericTypeParamInConstructor)), result.TestClsWithGenericParam);
        }

        [Test]
        public void CanInjectFail()
        {
            Injector injector = new Injector();
            Assert.DoesNotThrow(() => injector.Bind(typeof(TestEmptyClass), new TestEmptyClass()));

            Assert.Throws<IoCConstructorException>(() => injector.InjectConstructor(typeof(TestClassWithConstructorInjectionFail)));
        }
    }
}
