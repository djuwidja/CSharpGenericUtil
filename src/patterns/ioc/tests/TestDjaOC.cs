using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    public class TestDjaOC
    {
        private const string DEFAULT_ID = "default";
        private const string CUSTOM_ID = "custom";
        private const string PROTOTYPE_ID = "prototype";
        private TestEmptyClass DEFAULT_EMPTY_CLASS = new TestEmptyClass();
        private TestEmptyClass CUSTOM_EMPTY_CLASS = new TestEmptyClass();
        private TestEmptyClass PROTOTYPE_EMPTY_CLASS = new TestEmptyClass();

        [Test, Order(1)]
        public void CanInstantiate()
        {
            DjaOC.Instantiate();
            Assert.IsTrue(DjaOC.IsInstantiated);
        }

        [Test, Order(2)]
        public void CanBind()
        {
            TestStruct testStruct;
            testStruct.idx = 668;
            testStruct.value = 4672;

            DjaOC.Bind(DEFAULT_EMPTY_CLASS);
            DjaOC.Bind(CUSTOM_EMPTY_CLASS, CUSTOM_ID);
            DjaOC.Bind(PROTOTYPE_EMPTY_CLASS, InstantiationType.PROTOTYPE, PROTOTYPE_ID);
            DjaOC.Bind(testStruct, CUSTOM_ID);
        }

        [Test, Order(3)]
        public void CanGetIsManagedType()
        {
            Assert.IsTrue(DjaOC.IsManagedType<TestEmptyClass>());
            Assert.IsTrue(DjaOC.IsManagedType<TestStruct>());
            Assert.IsFalse(DjaOC.IsManagedType<int>());
        }

        [Test, Order(4)]
        public void CanGetContainsCustomId()
        {
            Assert.IsTrue(DjaOC.ContainsCustomId<TestEmptyClass>(DEFAULT_ID));
            Assert.IsTrue(DjaOC.ContainsCustomId<TestEmptyClass>(CUSTOM_ID));
            Assert.IsTrue(DjaOC.ContainsCustomId<TestEmptyClass>(PROTOTYPE_ID));
            Assert.IsFalse(DjaOC.ContainsCustomId<TestStruct>(DEFAULT_ID));
            Assert.IsTrue(DjaOC.ContainsCustomId<TestStruct>(CUSTOM_ID));
        }

        [Test, Order(5)]
        public void CanGet()
        {
            TestEmptyClass defaultSingleton = DjaOC.Get<TestEmptyClass>();
            Assert.AreSame(DEFAULT_EMPTY_CLASS, defaultSingleton);

            TestEmptyClass customSingleton = DjaOC.Get<TestEmptyClass>(CUSTOM_ID);
            Assert.AreSame(CUSTOM_EMPTY_CLASS, customSingleton);

            TestEmptyClass prototype = DjaOC.Get<TestEmptyClass>(PROTOTYPE_ID);
            Assert.AreNotSame(PROTOTYPE_EMPTY_CLASS, prototype);
            Assert.AreEqual(typeof(TestEmptyClass), prototype.GetType());
        }

        [Test, Order(6)]
        public void CanNewInstance()
        {
            TestClassWithConstructor objWithDefaultSingleton = DjaOC.NewInstance<TestClassWithConstructor>();
            Assert.IsNotNull(objWithDefaultSingleton);
            Assert.AreSame(DEFAULT_EMPTY_CLASS, objWithDefaultSingleton.TestEmptyCls);

            TestClassWithCustomConstructor objWithCustomSingleton = DjaOC.NewInstance<TestClassWithCustomConstructor>();
            Assert.IsNotNull(objWithCustomSingleton);
            Assert.AreSame(CUSTOM_EMPTY_CLASS, objWithCustomSingleton.TestEmptyCls);
        }

        [Test, Order(7)]
        public void CanDispose()
        {
            DjaOC.Dispose();
            Assert.IsFalse(DjaOC.IsInstantiated);
        }
    }
}
