using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frontend2;
using Frontend2.Hardware;

namespace UnitTestProject1
{
    [TestClass]
    public class ComplexValidFunctionality
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
        PopCan A;
        PopCan B;
        PopCan C;




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
            A = new PopCan("A");
            B = new PopCan("B");
            C = new PopCan("C");
        }




        /*//////////////////////////////////////////////////////
        * Test_ChangingConfiguration
        *    Mimicks : T07-good-changing-configuration
        * 
        * Tests the scenario where;
        *    No coins are inserted and a button is dispensed.
        *    No change is dispensed.
        *///////////////////////////////////////////////////////
        [TestMethod]
        public void Test_ChangingConfiguration()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };

            int[] costs = new int[3] { 5, 10, 25 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[3] { "A", "B", "C" };
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

            names = new string[3] { "Coke", "water", "stuff" };
            popNames = new List<string>(names);
            costs = new int[3] { 250, 250, 205 };
            popCosts = new List<int> (costs);

            VMs[0].Configure(popNames, popCosts);

            VMs[0].SelectionButtons[0].Press();


            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());

            //Compare Actual Delivery to Exepected Delivery
            foreach (var item in expected)
            {
                Assert.IsTrue(delivered.Contains(item));
                delivered.Remove(item);
            }

            Assert.IsTrue(delivered.Count == 0);


            //Mimic Continuation of Test Script
            VMs[0].CoinSlot.AddCoin(new Coin(100));
            VMs[0].CoinSlot.AddCoin(new Coin(100));
            VMs[0].CoinSlot.AddCoin(new Coin(100));
            VMs[0].SelectionButtons[0].Press();



            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Add(two5);
            expected.Add(two5);
            expected.Add(A);


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
            var expectedCoinList = new List<Coin>() { five, ten, hund, hund, hund };
            var expectedPopList = new List<PopCan>() { B, C };
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
            expectedCoinList = new List<Coin>() { five, ten, hund, hund, hund };
            expectedPopList = new List<PopCan>() { B, C };
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



            //Mimic Continuation of test script 
            VMs[0].LoadCoins(coinsToLoad);
            VMs[0].LoadPopCans(popsToLoad);
            VMs[0].CoinSlot.AddCoin(new Coin(100));
            VMs[0].CoinSlot.AddCoin(new Coin(100));
            VMs[0].CoinSlot.AddCoin(new Coin(100));
            VMs[0].SelectionButtons[0].Press();




            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Clear();
            expected.Add(two5);
            expected.Add(two5);
            expected.Add(coke);


            //Compare Actual Delivery to Exepected Delivery
            foreach (var item in expected)
            {
                Assert.IsTrue(checker.contains(delivered, item));
                checker.remove(delivered, item);
            }

            Assert.IsTrue(delivered.Count == 0);




            //Get Actual Unload
            coinRacks = VMs[0].CoinRacks;
            foreach (var item in coinRacks)
                actualUnload.CoinsInCoinRacks.Add(item.Unload());

            popCanRacks = VMs[0].PopCanRacks;
            foreach (var item in popCanRacks)
                actualUnload.PopCansInPopCanRacks.Add(item.Unload());

            unload = VMs[0].StorageBin.Unload();
            foreach (var item in unload)
                actualUnload.PaymentCoinsInStorageBin.Add(item);


            //Set Expected Unload
            expectedCoinList = new List<Coin>() { five, ten, hund, hund, hund };
            expectedPopList = new List<PopCan>() { water, stuff };
            expectedUnload.CoinsInCoinRacks.Clear();
            expectedUnload.PopCansInPopCanRacks.Clear();
            expectedUnload.PaymentCoinsInStorageBin.Clear();
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
            expectedCoinList = new List<Coin>() { five, ten, hund, hund, hund };
            expectedPopList = new List<PopCan>() { water, stuff };
            expectedUnload.CoinsInCoinRacks.Clear();
            expectedUnload.PopCansInPopCanRacks.Clear();
            expectedUnload.PaymentCoinsInStorageBin.Clear();
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








            /*
                        foreach (var list in actualUnload.CoinsInCoinRacks)
                        {
                            foreach (var item in list)
                            {
                                Console.WriteLine(item.Value);
                            }
                        }


                        foreach (var list in actualUnload.PopCansInPopCanRacks)
                        {
                            foreach (var item in list)
                            {
                                Console.WriteLine(item.Name);
                            }
                        }


                        foreach (var item in actualUnload.PaymentCoinsInStorageBin)
                        {
                            Console.WriteLine(item.Value);
                        }


                        Console.WriteLine("------------------------");



                        foreach (var list in expectedUnload.CoinsInCoinRacks)
                        {
                            foreach (var item in list)
                            {
                                Console.WriteLine(item.Value);
                            }
                        }


                        foreach (var list in expectedUnload.PopCansInPopCanRacks)
                        {
                            foreach (var item in list)
                            {
                                Console.WriteLine(item.Name);
                            }
                        }


                        foreach (var item in expectedUnload.PaymentCoinsInStorageBin)
                        {
                            Console.WriteLine(item.Value);
                        }


                        Console.WriteLine("------------------------");

            */

        }




        /*//////////////////////////////////////////////////////
        * Test_InvalidCoins
        *    Mimics : T10-good-invalid-coin
        * 
        * Tests the scenario where;
        *    The coins inserted are invalid
        *///////////////////////////////////////////////////////
        [TestMethod]
        public void Test_InvalidCoins()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };

            int[] costs = new int[1] { 140 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[1] { "stuff" };
            List<string> popNames = new List<string>(names);

            int[] coinsToLoad = new int[4] { 1, 6, 1, 1 };
            int[] popsToLoad = new int[1] { 1 };




            //Mimic Test Script Calls
            var vm = new VendingMachine(coinKinds, 1, 10, 10, 10);
            VMs.Add(vm);
            new VendingMachineLogic(vm);

            VMs[0].Configure(popNames, popCosts);
            VMs[0].LoadCoins(coinsToLoad);
            VMs[0].LoadPopCans(popsToLoad);
            VMs[0].CoinSlot.AddCoin(new Coin(1));
            VMs[0].CoinSlot.AddCoin(new Coin(139));
            VMs[0].SelectionButtons[0].Press();


            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Add(new Coin(1));
            expected.Add(new Coin(139));


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
            var expectedCoinList = new List<Coin>() { five, ten, ten, ten, ten, ten, ten, two5, hund };
            var expectedPopList = new List<PopCan>() { stuff };
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
            expectedCoinList = new List<Coin>() { five, ten, ten, ten, ten, ten, ten, two5, hund };
            expectedPopList = new List<PopCan>() { stuff };
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
       * Test_CoinRackFull
       *    Mimics : T10-good-need-to-store-payment
       * 
       * Tests the scenario where;
       *    The coins inserted are invalid
       *///////////////////////////////////////////////////////
        [TestMethod]
        public void Test_CoinRackFull()
        {
            int[] coinKinds = new int[4] { 5, 10, 25, 100 };

            int[] costs = new int[1] { 135 };
            List<int> popCosts = new List<int>(costs);

            string[] names = new string[1] { "stuff" };
            List<string> popNames = new List<string>(names);

            int[] coinsToLoad = new int[4] { 10, 10, 10, 10 };
            int[] popsToLoad = new int[1] { 1 };




            //Mimic Test Script Calls
            var vm = new VendingMachine(coinKinds, 1, 10, 10, 10);
            VMs.Add(vm);
            new VendingMachineLogic(vm);

            VMs[0].Configure(popNames, popCosts);
            VMs[0].LoadCoins(coinsToLoad);
            VMs[0].LoadPopCans(popsToLoad);
            VMs[0].CoinSlot.AddCoin(two5);
            VMs[0].CoinSlot.AddCoin(hund);
            VMs[0].CoinSlot.AddCoin(ten);
            VMs[0].SelectionButtons[0].Press();


            //Get Actual Delivery and set Expected Delivery
            delivered = new List<IDeliverable>(VMs[0].DeliveryChute.RemoveItems());
            expected.Add(stuff);
            

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
            var expectedCoinList = new List<Coin>() { five, five, five, five, five, five, five, five, five, five,
                                                      ten,  ten,  ten,  ten,  ten,  ten,  ten,  ten,  ten,  ten,
                                                      two5, two5, two5, two5, two5, two5, two5, two5, two5, two5,
                                                      hund, hund, hund, hund, hund, hund, hund, hund, hund, hund, };
            var expectedCoinsInBin = new List<Coin>() { two5, hund, ten };
            expectedUnload.CoinsInCoinRacks.Add(expectedCoinList);
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
            expectedCoinList = new List<Coin>() { five, five, five, five, five, five, five, five, five, five,
                                                      ten,  ten,  ten,  ten,  ten,  ten,  ten,  ten,  ten,  ten,
                                                      two5, two5, two5, two5, two5, two5, two5, two5, two5, two5,
                                                      hund, hund, hund, hund, hund, hund, hund, hund, hund, hund, };
            expectedCoinsInBin = new List<Coin>() { two5, hund, ten };
            expectedUnload.CoinsInCoinRacks.Add(expectedCoinList);
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
