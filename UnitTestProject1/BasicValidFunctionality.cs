using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frontend2;
using Frontend2.Hardware;

namespace UnitTestProject1
{
    [TestClass]
    public class BasicValidFunctionality
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





        /*//////////////////////////////////////////////////////
         * Test_PopDelivered_NoChangeDue
         *    Mimics : T01-good-insert-and-press-exact-change
         * 
         * Tests the scenario where;
         *    The amount entered is equal to the cost of the pop.
         *    No change is made or dispensed.
         *    All coins made as payment are sent to the coin racks. 
         *///////////////////////////////////////////////////////
        [TestMethod]
        public void Test_PopDelivered_NoChangeDue()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };
         
            int[] costs = new int[3] { 250, 250, 205 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[3] { "Coke", "water", "stuff" };
            List<string> popNames = new List<string>(names);

            int[] coinsToLoad = new int[4] { 1, 1, 2, 0 };
            int[] popsToLoad = new int[3] { 1, 1, 1, };




            //Mimic Test Script Calls
            var vm = new VendingMachine(coinKinds, 3, 10, 10, 10);
            VMs.Add(vm);
            new VendingMachineLogic(vm);

            VMs[0].Configure(popNames, popCosts);
            VMs[0].LoadCoins(coinsToLoad);
            VMs[0].LoadPopCans(popsToLoad);
            VMs[0].CoinSlot.AddCoin(hund);
            VMs[0].CoinSlot.AddCoin(hund);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].SelectionButtons[0].Press();


            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Add(coke);

            //Compare Actual Delivery to Exepected Delivery
            foreach (var item in expected)
            {
                Assert.IsTrue(checker.contains(delivered, item));
                checker.remove(delivered, item);
            }

            Assert.IsTrue(delivered.Count == 0);


         
            //Get Actual Unload
            var coinRacks = VMs[0].CoinRacks;
            foreach (var item in coinRacks)
                actualUnload.CoinsInCoinRacks.Add(item.Unload());
            

            var popCanRacks = VMs[0].PopCanRacks;
            foreach (var item in popCanRacks)
                actualUnload.PopCansInPopCanRacks.Add(item.Unload());

            var unload = VMs[0].StorageBin.Unload();
            foreach (var item in unload)
                actualUnload.PaymentCoinsInStorageBin.Add(item);

            //Set Expected Unload
            var expectedCoinList = new List<Coin>() { five, ten, two5, two5, two5, two5, hund, hund };
            var expectedPopList = new List<PopCan>() { water, stuff };
            expectedUnload.CoinsInCoinRacks.Add(expectedCoinList);
            expectedUnload.PopCansInPopCanRacks.Add(expectedPopList);


            //Compare Actual Unload with Expected Unload -- See Function Documentation for "checkUnload" in Checker.cs
            //--------------------------------------------------------------------------------------------------------
            //Remove all items from expected unload that are also in actual unload, return the difference (if any)
            unaccountedForUnload = checker.checkUnload(expectedUnload, actualUnload);

            //Assert that the difference is an empty set
            foreach (var coinList in unaccountedForUnload.CoinsInCoinRacks)
                Assert.IsTrue(coinList.Count == 0);

            foreach (var popList in unaccountedForUnload.PopCansInPopCanRacks)
                Assert.IsTrue(popList.Count == 0);

            Assert.IsTrue(unaccountedForUnload.PaymentCoinsInStorageBin.Count == 0);


            //Reset expected unload
            expectedUnload = new VendingMachineStoredContents(); 
            expectedCoinList = new List<Coin>() { five, ten, two5, two5, two5, two5, hund, hund };
            expectedPopList = new List<PopCan>() { water, stuff };
            expectedUnload.CoinsInCoinRacks.Add(expectedCoinList);
            expectedUnload.PopCansInPopCanRacks.Add(expectedPopList);

            //Reverse the inputs to the function call to ensure neither expected nor actual contain different elements from eachother
            //Remove all items from actual unload that are also in expected unload, return the difference (if any)
            unaccountedForUnload = checker.checkUnload(actualUnload, expectedUnload);

            //Assert that the difference is an empty set
            foreach (var coinList in unaccountedForUnload.CoinsInCoinRacks)
                Assert.IsTrue(coinList.Count == 0);

            foreach (var popList in unaccountedForUnload.PopCansInPopCanRacks)
                Assert.IsTrue(popList.Count == 0);

            Assert.IsTrue(unaccountedForUnload.PaymentCoinsInStorageBin.Count == 0);

        }










        /*//////////////////////////////////////////////////////
        * Test_PopDelivered_ChangeDueIsAvailable
        *    Mimicks : T02-good-insert-and-press-change-expected
        * 
        * Tests the scenario where;
        *    The amount entered is greater than the cost of the pop.
        *    All change is made and dispensed.
        *    All coins made as payment are sent to the coin racks. 
        *///////////////////////////////////////////////////////
        [TestMethod]
        public void Test_PopDelivered_ChangeDueIsAvailable()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };

            int[] costs = new int[3] { 250, 250, 205 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[3] { "Coke", "water", "stuff" };
            List<string> popNames = new List<string>(names);

            int[] coinsToLoad = new int[4] { 1, 1, 2, 0 };
            int[] popsToLoad = new int[3] { 1, 1, 1, };


            ////To be implemented

        }





        /*//////////////////////////////////////////////////////
        * Test_PressWithNoCredits
        *    Mimicks : T04-good-press-without-insert
        * 
        * Tests the scenario where;
        *    No coins are inserted and a button is dispensed.
        *    No change is dispensed.
        *///////////////////////////////////////////////////////
        [TestMethod]
        public void Test_PressWithNoCredits()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };

            int[] costs = new int[3] { 250, 250, 205 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[3] { "Coke", "water", "stuff" };
            List<string> popNames = new List<string>(names);

            int[] coinsToLoad = new int[4] { 1, 1, 2, 0 };
            int[] popsToLoad = new int[3] { 1, 1, 1, };


            //Mimic Test Script Calls
            var vm = new VendingMachine(coinKinds, 3, 10, 10, 10);
            VMs.Add(vm);
            new VendingMachineLogic(vm);

            VMs[0].Configure(popNames, popCosts);
            VMs[0].LoadCoins(coinsToLoad);
            VMs[0].LoadPopCans(popsToLoad);
            VMs[0].SelectionButtons[0].Press();


            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());

            //Compare Actual Delivery to Exepected Delivery
            foreach (var item in expected)
            {
                Assert.IsTrue(checker.contains(delivered, item));
                checker.remove(delivered, item);
            }

            Assert.IsTrue(delivered.Count == 0);



            //Get Actual Unload
            var coinRacks = VMs[0].CoinRacks;
            foreach (var item in coinRacks)
                actualUnload.CoinsInCoinRacks.Add(item.Unload());


            var popCanRacks = VMs[0].PopCanRacks;
            foreach (var item in popCanRacks)
                actualUnload.PopCansInPopCanRacks.Add(item.Unload());

            var unload = VMs[0].StorageBin.Unload();
            foreach (var item in unload)
                actualUnload.PaymentCoinsInStorageBin.Add(item);

            //Set Expected Unload
            var expectedCoinList = new List<Coin>() { five, ten, two5, two5 };
            var expectedPopList = new List<PopCan>() { coke, water, stuff };
            expectedUnload.CoinsInCoinRacks.Add(expectedCoinList);
            expectedUnload.PopCansInPopCanRacks.Add(expectedPopList);


            //Compare Actual Unload with Expected Unload -- See Function Documentation for "checkUnload" in Checker.cs
            //--------------------------------------------------------------------------------------------------------
            //Remove all items from expected unload that are also in actual unload, return the difference (if any)
            unaccountedForUnload = checker.checkUnload(expectedUnload, actualUnload);

            //Assert that the difference is an empty set
            foreach (var coinList in unaccountedForUnload.CoinsInCoinRacks)
                Assert.IsTrue(coinList.Count == 0);

            foreach (var popList in unaccountedForUnload.PopCansInPopCanRacks)
                Assert.IsTrue(popList.Count == 0);

            Assert.IsTrue(unaccountedForUnload.PaymentCoinsInStorageBin.Count == 0);


            //Reset expected unload
            expectedUnload = new VendingMachineStoredContents();
            expectedCoinList = new List<Coin>() { five, ten, two5, two5 };
            expectedPopList = new List<PopCan>() { coke, water, stuff };
            expectedUnload.CoinsInCoinRacks.Add(expectedCoinList);
            expectedUnload.PopCansInPopCanRacks.Add(expectedPopList);

            //Reverse the inputs to the function call to ensure neither expected nor actual contain different elements from eachother
            //Remove all items from actual unload that are also in expected unload, return the difference (if any)
            unaccountedForUnload = checker.checkUnload(actualUnload, expectedUnload);

            //Assert that the difference is an empty set
            foreach (var coinList in unaccountedForUnload.CoinsInCoinRacks)
                Assert.IsTrue(coinList.Count == 0);

            foreach (var popList in unaccountedForUnload.PopCansInPopCanRacks)
                Assert.IsTrue(popList.Count == 0);

            Assert.IsTrue(unaccountedForUnload.PaymentCoinsInStorageBin.Count == 0);

        }






    }
}
