using System;
using System.Collections.Generic;
using System.Text;
using Djuwidja.GenericUtil.Patterns.IoC.Attributes;
using NUnit.Framework;
using System.Reflection;

namespace Djuwidja.GenericUtil.Patterns.IoC.Attributes.Tests
{
    public class TestInjectMethod
    {
        [Test]
        public void CanFindInjectConstructor()
        {
            int numInjectConstructor = 0;
            ConstructorInfo[] cInfoArr = typeof(TestClass).GetConstructors();
            foreach (ConstructorInfo cInfo in cInfoArr)
            {
                InjectConstructor cAttr = cInfo.GetCustomAttribute<InjectConstructor>();
                if (cAttr != null)
                {
                    numInjectConstructor++;
                }
            }

            Assert.AreEqual(1, numInjectConstructor);
        }

        [Test]
        public void CanFindInjectorConstructorParam()
        {
            int numInjectAttribute = 0;

            ConstructorInfo[] cInfoArr = typeof(TestClass).GetConstructors();
            foreach (ConstructorInfo cInfo in cInfoArr)
            {
                IEnumerator<InjectConstructor> cAttributeIter = cInfo.GetCustomAttributes<InjectConstructor>().GetEnumerator();
                while (cAttributeIter.MoveNext())
                {
                    ParameterInfo[] pInfoArr = cInfo.GetParameters();
                    foreach (ParameterInfo pInfo in pInfoArr)
                    {
                        IEnumerator<InjectProperty> pAttributeIter = pInfo.GetCustomAttributes<InjectProperty>().GetEnumerator();
                        while (pAttributeIter.MoveNext())
                        {
                            numInjectAttribute++;
                        }      
                    }  
                }
            }

            Assert.AreEqual(2, numInjectAttribute);
        }

        [Test]
        public void CanFindInjectField()
        {
            int numFieldInjectAttribute = 0;

            FieldInfo[] fInfoArr = typeof(TestClass).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo fInfo in fInfoArr)
            {
                IEnumerator<InjectProperty> fInfoIter = fInfo.GetCustomAttributes<InjectProperty>().GetEnumerator();
                while (fInfoIter.MoveNext())
                {
                    numFieldInjectAttribute++;
                }
            }

            Assert.AreEqual(1, numFieldInjectAttribute);
        }

        [Test]
        public void CanFindInjectMethod()
        {
            int numMethodInjection = 0;
            int numParamInjection = 0;

            MethodInfo[] mInfoArr = typeof(TestClass).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (MethodInfo mInfo in mInfoArr)
            {
                InjectMethod mInfoAttribute = mInfo.GetCustomAttribute<InjectMethod>();
                if (mInfoAttribute != null)
                {
                    numMethodInjection++;

                    ParameterInfo[] pInfoArr = mInfo.GetParameters();
                    foreach (ParameterInfo pInfo in pInfoArr)
                    {
                        IEnumerator<InjectProperty> customAttrIter = pInfo.GetCustomAttributes<InjectProperty>().GetEnumerator();
                        while (customAttrIter.MoveNext())
                        {
                            numParamInjection++;
                        }
                    }
                }
            }

            Assert.AreEqual(2, numMethodInjection);
            Assert.AreEqual(3, numParamInjection);
        }
    }

    class TestClass
    {
        [InjectProperty("custom")]
        private int _intValue;
        private float _floatValue;

        public int IntValue { get { return _intValue;  } }

        public float FloatValue { get { return _floatValue; } }
        public TestClass()
        {

        }

        [InjectConstructor]
        public TestClass(
            int idx, 
            [InjectProperty("custom")] string str,
            [InjectProperty("custom")] float flt)
        {

        }

        [InjectMethod]
        public void SetValues(
            [InjectProperty] float fvalue,
            [InjectProperty] int ivalue)
        {
            this._floatValue = fvalue;
            this._intValue = ivalue;
        }

        [InjectMethod]
        private void SetInt(
            [InjectProperty] int value)
        {
            this._intValue = value;
        }
    }

}
