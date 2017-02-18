using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frontend2;
using Frontend2.Hardware;

namespace UnitTestProject1
{
    [TestClass]
    public class BasicInvalidOperations
    {
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))] //CHANGE "AssertFailedException" TO MATCH EXCEPTION TYPES THROWN BY TONY'S CODE (if necessary)
        public void Test_ConfigureBeforeCreate()
        {
            Boolean b = false;
            Assert.IsTrue(b);
        }
    }
}
