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
        PopCan pepsi;
        PopCan fourloco;



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
            pepsi = new PopCan("Pepsi");
            fourloco = new PopCan("FourLoco");
        }





        /*//////////////////////////////////////////////////////
       * Test_CreditsLeftInSystemThenUsed
       *    Mimics : MyCustomBonusScript
       * 
       * Tests the scenario where;
       *    The (25c) coin rack is full with a capactity of 4
       *    User enters 8 quarters, expects 7 as change.
       *    Only 4 quarters returned as change, 75c left in
       *      system as credit, then multiple vends follow
       *///////////////////////////////////////////////////////
        [TestMethod]
        public void Test_CreditsLeftInSystemThenUsed()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };

            int[] costs = new int[3] { 25, 10, 25 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[3] { "Coke", "Pepsi", "FourLoco" };
            List<string> popNames = new List<string>(names);

            int[] coinsToLoad = new int[4] { 0, 0, 4, 0 };
            int[] popsToLoad = new int[3] { 4, 1, 1, };




            //Mimic Test Script Calls
            var vm = new VendingMachine(coinKinds, 3, 4, 10, 10);
            VMs.Add(vm);
            new VendingMachineLogic(vm);

            VMs[0].Configure(popNames, popCosts);
            VMs[0].LoadCoins(coinsToLoad);
            VMs[0].LoadPopCans(popsToLoad);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(two5);

            //First Press
            VMs[0].SelectionButtons[0].Press();


            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Add(coke);
            expected.Add(two5);
            expected.Add(two5);
            expected.Add(two5);
            expected.Add(two5);


            //Compare Actual Delivery to Exepected Delivery
            foreach (var item in expected)
            {
                Assert.IsTrue(checker.contains(delivered, item));
                checker.remove(delivered, item);
            }

            Assert.IsTrue(delivered.Count == 0);


            

            //Second Press
            VMs[0].SelectionButtons[0].Press();

            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Clear();
            expected.Add(coke);
            
            //Compare Actual Delivery to Exepected Delivery
            foreach (var item in expected)
            {
                Assert.IsTrue(checker.contains(delivered, item));
                checker.remove(delivered, item);
            }

            Assert.IsTrue(delivered.Count == 0);



            //Third Press
            VMs[0].SelectionButtons[0].Press();

            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Clear();
            expected.Add(coke);

            //Compare Actual Delivery to Exepected Delivery
            foreach (var item in expected)
            {
                Assert.IsTrue(checker.contains(delivered, item));
                checker.remove(delivered, item);
            }

            Assert.IsTrue(delivered.Count == 0);



            //Fourth Press
            VMs[0].SelectionButtons[0].Press();

            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Clear();
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
            var expectedPopList = new List<PopCan>() { pepsi, fourloco };
            var expectedCoinsInBin = new List<Coin>() { two5, two5, two5, two5, two5, two5, two5, two5 };
            expectedUnload.PopCansInPopCanRacks.Add(expectedPopList);
            expectedUnload.PaymentCoinsInStorageBin.AddRange(expectedCoinsInBin);

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
            expectedPopList = new List<PopCan>() { pepsi, fourloco };
            expectedCoinsInBin = new List<Coin>() { two5, two5, two5, two5, two5, two5, two5, two5 };
            expectedUnload.PopCansInPopCanRacks.Add(expectedPopList);
            expectedUnload.PaymentCoinsInStorageBin.AddRange(expectedCoinsInBin);


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


