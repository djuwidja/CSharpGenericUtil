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
        private TestClassWithConstructor()
        {

        }
        public TestClassWithConstructor(TestEmptyClass cls)
        {

        }
    }

    class TestClassWith2Constructors
    {
        public TestClassWith2Constructors()
        {

        }

        public TestClassWith2Constructors(TestEmptyClass cls)
        {

        }
    }
    struct TestStruct
    {
        int idx;
        int value;
    }
    class TestClassWithGenericTypeParamInConstructor
    {
        public TestClassWithGenericTypeParamInConstructor(int idx, TestEmptyClass cls)
        {

        }

        public TestClassWithGenericTypeParamInConstructor(TestStruct structObj, TestEmptyClass cls)
        {

        }
    }
}
