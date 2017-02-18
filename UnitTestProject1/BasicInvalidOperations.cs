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
        //Instantiate Variables To Be Used Across All Tests In This Test Case
        //VendingMachineFactory VMF;
        List<VendingMachine> VMs;
        Checker checker;
        List<IDeliverable> delivered;
        List<IDeliverable> expected;
        List<IDeliverable> unaccountedForDelivery;
        VendingMachineStoredContents actualUnload;
        VendingMachineStoredContents expectedUnload;
        VendingMachineStoredContents unaccountedForUnload;
        Coin five;
        Coin ten;
        Coin two5;
        Coin hund;
        PopCan coke;
        PopCan water;
        PopCan stuff;



        /*//////////////////////////////////////////////////////
         * Initialize
         * 
         * Initialize variables to be used across all tests
         *    in this test case    
         *//////////////////////////////////////////////////////
        [TestInitialize]
        public void Initialize()
        {
            //VMF = new VendingMachineFactory();
            VMs = new List<VendingMachine>();
            checker = new Checker();
            //delivered = new List<IDeliverable>();
            expected = new List<IDeliverable>();
            unaccountedForDelivery = new List<IDeliverable>();
            actualUnload = new VendingMachineStoredContents();
            expectedUnload = new VendingMachineStoredContents();
            unaccountedForUnload = new VendingMachineStoredContents();
            five = new Coin(5);
            ten = new Coin(10);
            two5 = new Coin(25);
            hund = new Coin(100);
            coke = new PopCan("Coke");
            water = new PopCan("water");
            stuff = new PopCan("stuff");
        }






       // throw new Exception("The number of names and costs must be identical to the number of pop can racks in the machine");






        /*//////////////////////////////////////////////////////
        * Test_BadNamesList
        *    Mimics : U03-bad-names-list
        * 
        * Tests the scenario where;
        *    Not enough names are supplied in regards to
        *    the amount of buttons specified
        *///////////////////////////////////////////////////////
        [TestMethod]
        [ExpectedException(typeof(Exception))] 
        public void Test_BadNamesList()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };

            int[] costs = new int[2] { 250, 250 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[2] { "Coke", "water" };
            List<string> popNames = new List<string>(names);

           
            //Mimic Test Script Calls
            var vm = new VendingMachine(coinKinds, 3, 10, 10, 10);
            VMs.Add(vm);
            new VendingMachineLogic(vm);

            VMs[0].Configure(popNames, popCosts);

        }











        /*//////////////////////////////////////////////////////
        * Test_BadButtonNumber
        *    Mimics : U06-bad-button-number
        * 
        * Tests the scenario where;
        *    The button "pressed" does not exist
        *///////////////////////////////////////////////////////
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_BadButtonNumber()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };


            //Mimic Test Script Calls
            var vm = new VendingMachine(coinKinds, 3, 10, 10, 10);
            VMs.Add(vm);
            new VendingMachineLogic(vm);

            VMs[0].SelectionButtons[3].Press();

        }













    }
}
