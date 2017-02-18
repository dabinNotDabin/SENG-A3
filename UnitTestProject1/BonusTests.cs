using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frontend2;
using Frontend2.Hardware;

namespace UnitTestProject1
{
    [TestClass]
    public class BonusTests
    {
        [TestMethod]
        public void Test_CreditsLeftInSystemThenUsed()
        {

            ///script I wrote below to be translated for this bonus test
        }
    }
}


/*
CREATE(5, 10, 25, 100; 3; 4; 10; 10)
CONFIGURE([0] "Coke", 25; "Pepsi", 10; "FourLoco", 25)
COIN_LOAD([0] 2; 25, 4)
POP_LOAD([0] 0; "Coke", 4)
POP_LOAD([0] 1; "Pepsi", 1)
POP_LOAD([0] 2; "FourLoco", 1)
INSERT([0] 25)
INSERT([0] 25)
INSERT([0] 25)
INSERT([0] 25)
INSERT([0] 25)
INSERT([0] 25)
INSERT([0] 25)
INSERT([0] 25)

PRESS([0] 0)
EXTRACT([0])
CHECK_DELIVERY(100, "Coke")

PRESS([0] 0)
EXTRACT([0])
CHECK_DELIVERY(0, "Coke")

PRESS([0] 0)
EXTRACT([0])
CHECK_DELIVERY(0, "Coke")

PRESS([0] 0)
EXTRACT([0])
CHECK_DELIVERY(0, "Coke")

UNLOAD([0])
CHECK_TEARDOWN(0; 200; "Pepsi", "FourLoco")
*/