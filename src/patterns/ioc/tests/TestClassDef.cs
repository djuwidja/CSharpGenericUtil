using System;
using System.Collections.Generic;
using System.Text;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    class TestEmptyClass
    {
        public TestEmptyClass()
        {

        }
    }

    class TestClassWithConstructor
    {
        public TestEmptyClass TestEmptyCls { get; }

        private TestClassWithConstructor()
        {
            this.TestEmptyCls = null;
        }
        public TestClassWithConstructor(TestEmptyClass cls)
        {
            this.TestEmptyCls = cls;
        }
    }

    class TestClassWith2Constructors
    {
        public TestEmptyClass TestEmptyCls { get; }

        public TestClassWith2Constructors()
        {
            this.TestEmptyCls = null;
        }

        public TestClassWith2Constructors(TestEmptyClass cls)
        {
            this.TestEmptyCls = cls;
        }
    }
    struct TestStruct
    {
        public int idx;
        public int value;
    }

    class TestClassWithGenericTypeParamInConstructor
    {
        public int Idx { get; }
        public string Str { get; }
        public TestStruct TestStruct { get; }
        public TestEmptyClass TestEmptyCls { get; }

        public TestClassWithGenericTypeParamInConstructor(int idx, TestEmptyClass cls)
        {
            this.Idx = idx;
            this.TestEmptyCls = cls;
        }

        public TestClassWithGenericTypeParamInConstructor(TestStruct structObj, TestEmptyClass cls)
        {
            this.TestStruct = structObj;
            this.TestEmptyCls = cls;
        }

        public TestClassWithGenericTypeParamInConstructor(string str, TestEmptyClass cls)
        {
            this.Str = str;
            this.TestEmptyCls = cls;
        }
    }

    class TestClassWithConstructorInjection
    {
        public string TestStr { get; }
        public int TestInt { get; }
        public TestStruct TestStruct { get; }
        public TestEmptyClass TestEmptyCls { get; }
        public TestClassWithConstructor TestClsWithConstructor { get; }
        public TestClassWith2Constructors TestClsWith2Constructor { get; }
        public TestClassWithGenericTypeParamInConstructor TestClsWithGenericParam { get; }

        public TestClassWithConstructorInjection(string str, int num, TestStruct structure, TestEmptyClass emptyCls, TestClassWithConstructor conCls, TestClassWith2Constructors con2Cls, TestClassWithGenericTypeParamInConstructor genCls)
        {
            this.TestStr = str;
            this.TestInt = num;
            this.TestStruct = structure;
            this.TestEmptyCls = emptyCls;
            this.TestClsWithConstructor = conCls;
            this.TestClsWith2Constructor = con2Cls;
            this.TestClsWithGenericParam = genCls;
        }
    }

    class TestClassWithConstructorInjectionFail
    {
        public float TestFloat { get; }
        public TestEmptyClass TestEmptyCls { get; }

        public TestClassWithConstructorInjectionFail(float testFloat, TestEmptyClass emptyCls)
        {
            this.TestFloat = testFloat;
            this.TestEmptyCls = emptyCls;
        }
    }
}
