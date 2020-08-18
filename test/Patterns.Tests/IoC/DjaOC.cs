using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;
using System.Reflection;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    public class DjaOC
    {
        private const string DEFAULT_ID = "default";
        private const string CUSTOM_ID = "custom";
        private const string PROTOTYPE_ID = "prototype";
        private const string SUBCLASS_ID = "subclass";
        private TestEmptyClass DEFAULT_EMPTY_CLASS = new TestEmptyClass();
        private TestEmptyClass CUSTOM_EMPTY_CLASS = new TestEmptyClass();
        private TestEmptyClass PROTOTYPE_EMPTY_CLASS = new TestEmptyClass();
        private TestEmptyChildClass EMPTY_CHILD_CLASS = new TestEmptyChildClass();

        [Test, Order(1)]
        public void CanInstantiate()
        {
            IoC.DjaOC.Instantiate();
            Assert.IsTrue(IoC.DjaOC.IsInstantiated);
        }

        [Test, Order(2)]
        public void CanBind()
        {
            TestStruct testStruct;
            testStruct.idx = 668;
            testStruct.value = 4672;

            IoC.DjaOC.Bind<TestEmptyClass>(DEFAULT_EMPTY_CLASS);
            IoC.DjaOC.Bind<TestEmptyClass>(CUSTOM_EMPTY_CLASS, CUSTOM_ID);
            IoC.DjaOC.Bind<TestEmptyClass>(PROTOTYPE_EMPTY_CLASS, InstantiationType.PROTOTYPE, PROTOTYPE_ID);
            IoC.DjaOC.Bind<TestStruct>(testStruct, CUSTOM_ID);
            IoC.DjaOC.Bind<TestEmptyClass>(EMPTY_CHILD_CLASS, SUBCLASS_ID);
        }

        [Test, Order(3)]
        public void CanGetIsManagedType()
        {
            Assert.IsTrue(IoC.DjaOC.IsManagedType<TestEmptyClass>());
            Assert.IsTrue(IoC.DjaOC.IsManagedType<TestStruct>());
            Assert.IsFalse(IoC.DjaOC.IsManagedType<int>());
        }

        [Test, Order(4)]
        public void CanGetContainsCustomId()
        {
            Assert.IsTrue(IoC.DjaOC.ContainsCustomId<TestEmptyClass>(DEFAULT_ID));
            Assert.IsTrue(IoC.DjaOC.ContainsCustomId<TestEmptyClass>(CUSTOM_ID));
            Assert.IsTrue(IoC.DjaOC.ContainsCustomId<TestEmptyClass>(PROTOTYPE_ID));
            Assert.IsFalse(IoC.DjaOC.ContainsCustomId<TestStruct>(DEFAULT_ID));
            Assert.IsTrue(IoC.DjaOC.ContainsCustomId<TestStruct>(CUSTOM_ID));
        }

        [Test, Order(5)]
        public void CanGet()
        {
            TestEmptyClass defaultSingleton = IoC.DjaOC.Get<TestEmptyClass>();
            Assert.AreSame(DEFAULT_EMPTY_CLASS, defaultSingleton);

            TestEmptyClass customSingleton = IoC.DjaOC.Get<TestEmptyClass>(CUSTOM_ID);
            Assert.AreSame(CUSTOM_EMPTY_CLASS, customSingleton);

            TestEmptyClass prototype = IoC.DjaOC.Get<TestEmptyClass>(PROTOTYPE_ID);
            Assert.AreNotSame(PROTOTYPE_EMPTY_CLASS, prototype);
            Assert.AreEqual(typeof(TestEmptyClass), prototype.GetType());

            TestEmptyChildClass childClass = (TestEmptyChildClass) IoC.DjaOC.Get<TestEmptyClass>(SUBCLASS_ID);
            Assert.AreSame(EMPTY_CHILD_CLASS, childClass);
        }

        [Test, Order(6)]
        public void CanNewInstance()
        {
            TestClassWithConstructor objWithDefaultSingleton = IoC.DjaOC.NewInstance<TestClassWithConstructor>();
            Assert.IsNotNull(objWithDefaultSingleton);
            Assert.AreSame(DEFAULT_EMPTY_CLASS, objWithDefaultSingleton.TestEmptyCls);

            TestClassWithCustomConstructor objWithCustomSingleton = IoC.DjaOC.NewInstance<TestClassWithCustomConstructor>();
            Assert.IsNotNull(objWithCustomSingleton);
            Assert.AreSame(CUSTOM_EMPTY_CLASS, objWithCustomSingleton.TestEmptyCls);
        }

        [Test, Order(7)]
        public void CanDispose()
        {
            IoC.DjaOC.Dispose();
            Assert.IsFalse(IoC.DjaOC.IsInstantiated);
        }

        [Test]
        public void CountWrapperMethods()
        {
            Type djaOCType = typeof(IoC.DjaOC);
            MethodInfo[] djaOCMethodArr = djaOCType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            Type injectorType = typeof(IoC.Injector);
            MethodInfo[] injectorMethodArr = injectorType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            //DjaOC must wrap all public non-static methods from Injector. So it has 3 more methods [Instantiate(), IsInstantiated Getter, Dispose()] than Injector.
            Assert.AreEqual(djaOCMethodArr.Length - 3, injectorMethodArr.Length);
        }
    }
}
