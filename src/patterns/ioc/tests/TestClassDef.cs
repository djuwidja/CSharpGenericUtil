﻿using System;
using System.Collections.Generic;
using System.Text;
using Djuwidja.GenericUtil.Patterns.IoC.Attributes;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    class TestEmptyClass
    {
        [InjectConstructor]
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

        [InjectConstructor]
        public TestClassWithConstructor(TestEmptyClass cls)
        {
            this.TestEmptyCls = cls;
        }
    }
    struct TestStruct
    {
        public int idx;
        public int value;
    }

    class TestClassWith2InjectConstructors
    {
        [InjectConstructor]
        public TestClassWith2InjectConstructors()
        {

        }

        [InjectConstructor]
        public TestClassWith2InjectConstructors(int idx)
        {

        }
    }

    class TestClassWithNoInjectConstructor
    {
        public TestClassWithNoInjectConstructor()
        {

        }
    }

    class TestClassWithConstructorInjection
    {
        private int _intValue;
        [InjectProperty("custom")] private float _floatValue;
        [InjectProperty] public TestStruct TestStruct { get; set; }

        private long _longValue;
        private short _shortValue;

        public int IntValue { get { return _intValue; } }
        public long LongValue { get { return _longValue; } }
        public short ShortValue { get { return _shortValue;  } }
        public float FloatValue { get { return _floatValue; } }
        
        public TestEmptyClass EmptyClass { get; }

        [InjectConstructor]
        public TestClassWithConstructorInjection(TestEmptyClass emptyClass,
                                                 [ID("custom")] int intValue)
        {
            _intValue = intValue;
            EmptyClass = emptyClass;
        }

        [InjectMethod]
        public void TestPublicInjection(long longValue)
        {
            _longValue = longValue;
        }

        [InjectMethod]
        private void TestPrivateMethodInjection([ID("custom")] short shortValue)
        {
            _shortValue = shortValue;
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
