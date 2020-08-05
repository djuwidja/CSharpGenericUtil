using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    public class TestIoCObjectContainer
    {
        [Test]
        public void CanGetManagedType()
        {
            const string defaultKey = "default";
            const string customKey = "custom";
            const string customTestStr = "customTestStr";

            IoCObjectContainer iocCon = new IoCObjectContainer();
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(TestEmptyClass), defaultKey, new TestEmptyClass()));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(string), customKey, customTestStr));

            Assert.True(iocCon.IsManagedType(typeof(TestEmptyClass)));
            Assert.True(iocCon.IsManagedType(typeof(string)));
        }

        [Test]
        public void CanContainsId()
        {
            const string defaultKey = "default";
            const string customKey = "custom";

            IoCObjectContainer iocCon = new IoCObjectContainer();
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(TestEmptyClass), defaultKey, new TestEmptyClass()));

            Assert.IsTrue(iocCon.ContainsId(typeof(TestEmptyClass), defaultKey));
            Assert.IsFalse(iocCon.ContainsId(typeof(TestEmptyClass), customKey));
        }

        [Test]
        public void CanGetInstantiationType()
        {
            IoCObjectContainer iocCon = new IoCObjectContainer();

            MethodInfo mInfo = iocCon.GetType().GetMethod("GetInstantiationType", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.AreEqual(InstantiationType.SINGLETON, (InstantiationType) mInfo.Invoke(iocCon, new object[] { typeof(TestEmptyClass) }));
            Assert.AreEqual(InstantiationType.PROTOTYPE, (InstantiationType) mInfo.Invoke(iocCon, new object[] { typeof(TestClassWithConstructor) }));
            Assert.AreEqual(InstantiationType.SINGLETON, (InstantiationType)mInfo.Invoke(iocCon, new object[] { typeof(TestClassWithoutClassAttribute) }));
            
            try
            {
                mInfo.Invoke(iocCon, new object[] { typeof(TestClassWith2ClassAttributes) });
                Assert.Fail("Exception was not thrown.");
            }
            catch (TargetInvocationException e)
            {
                Assert.AreEqual(typeof(InvalidIoCTypeException), e.InnerException.GetType());
            }

            try
            {
                mInfo.Invoke(iocCon, new object[] { typeof(TestClassWithUnsupportedAttribute) });
                Assert.Fail("Exception was not thrown.");
            }
            catch (TargetInvocationException e)
            {
                Assert.AreEqual(typeof(InvalidIoCTypeException), e.InnerException.GetType());
            }
        }

        [Test]
        public void CanBindAndGetSingleton()
        {
            TestStruct testStruct;
            testStruct.idx = 8;
            testStruct.value = 99;

            const string defaultKey = "default";
            const string customKey = "custom";
            const string defaultTestStr = "defaultTestStr";
            const string customTestStr = "customTestStr";
            const int defaultTestInt = 6;
            const int customTestInt = 88;

            IoCObjectContainer iocCon = new IoCObjectContainer();
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(TestEmptyClass), defaultKey, new TestEmptyClass()));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(TestEmptyClass), customKey, new TestEmptyClass()));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(string), defaultKey, defaultTestStr));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(string), customKey, customTestStr));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(int), defaultKey, defaultTestInt));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(int), customKey, customTestInt));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(TestStruct), defaultKey, testStruct));

            object testEmptyClassObj1 = iocCon.Get(typeof(TestEmptyClass), defaultKey);
            object testEmptyClassObj2 = iocCon.Get(typeof(TestEmptyClass), defaultKey);
            Assert.AreSame(testEmptyClassObj1, testEmptyClassObj2);

            Assert.AreEqual(defaultTestStr, iocCon.Get(typeof(string), defaultKey));
            Assert.AreEqual(customTestStr, iocCon.Get(typeof(string), customKey));

            Assert.AreEqual(defaultTestInt, iocCon.Get(typeof(int), defaultKey));
            Assert.AreEqual(customTestInt, iocCon.Get(typeof(int), customKey));

            object testEmptyClassObj3 = iocCon.Get(typeof(TestEmptyClass), customKey);
            object testEmptyClassObj4 = iocCon.Get(typeof(TestEmptyClass), customKey);
            Assert.AreSame(testEmptyClassObj3, testEmptyClassObj4);
            Assert.AreNotSame(testEmptyClassObj2, testEmptyClassObj3);
        }

        [Test]
        public void CanBindAndGetPrototype()
        {
            const string defaultKey = "default";

            IoCObjectContainer iocCon = new IoCObjectContainer();
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(TestEmptyClass), defaultKey, new TestEmptyClass()));
            Assert.DoesNotThrow(() => iocCon.Bind(typeof(TestClassWithConstructor), defaultKey, new TestClassWithConstructor((TestEmptyClass)iocCon.Get(typeof(TestEmptyClass), defaultKey))));

            TestClassWithConstructor testObj1 = (TestClassWithConstructor) iocCon.Get(typeof(TestClassWithConstructor), defaultKey);
            TestClassWithConstructor testObj2 = (TestClassWithConstructor) iocCon.Get(typeof(TestClassWithConstructor), defaultKey);

            Assert.AreNotSame(testObj1, testObj2);
            Assert.AreSame(testObj1.TestEmptyCls, testObj2.TestEmptyCls);
        }
    }
}
