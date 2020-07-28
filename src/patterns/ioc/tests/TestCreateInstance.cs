using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.IoC;

namespace Djuwidja.GenericUtil.Patterns.IoC.Tests
{
    class TestCreateInstance
    {
        [Test]
        public void CanCreateInstance()
        {
            Injector injector = new Injector();
        }
    }
}
