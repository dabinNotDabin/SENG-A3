using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend2;
using Frontend2.Hardware;

namespace UnitTestProject1
{
    public class Checker
    {
        public Checker() { }


        /*///////////////////////////////
       // Name    : checkUnload
       // Purpose : Return a VendingMachineContents containing any items in expected delivery that were not found in actual delivery.
       // 
       // 
       // Inputs  :
       //     expected  -- VendingMachineStoredContents representing the expected return from a vending machine unload
       //     delivered -- VendingMachineStoredContents representing the actual contents returned by a vending machine unload
       // 
       // Return  :
       //     remaining -- VendingMachineStoredContents
       //                  - initially is set equal to expected delivery, items that match in actual delivery are removed one by one
       //                  - returns an empty list if expected is identical to actual
       //      
       *////////////////////////////////

        public VendingMachineStoredContents checkUnload(VendingMachineStoredContents expected, VendingMachineStoredContents delivered)
        {
            VendingMachineStoredContents remaining = expected;
            List<List<Coin>> coinsInRacks = delivered.CoinsInCoinRacks;
            List<List<PopCan>> popsInRacks = delivered.PopCansInPopCanRacks;
            List<Coin> coinsInStorage = delivered.PaymentCoinsInStorageBin;

            foreach (var list in coinsInRacks)
                foreach (var item in list)
                {
                    for (int i = 0; i < remaining.CoinsInCoinRacks.Count; i++)
                    {
                        if (containsCoin(remaining.CoinsInCoinRacks[i], item))
                        {
                            remaining.CoinsInCoinRacks[i] = removeCoin(remaining.CoinsInCoinRacks[i], item);
                            break;
                        }
                    }
                }


            foreach (var list in popsInRacks)
                foreach (var item in list)
                {
                    for (int i = 0; i < remaining.PopCansInPopCanRacks.Count; i++)
                    {
                        if (containsPop(remaining.PopCansInPopCanRacks[i], item))
                        {
                            remaining.PopCansInPopCanRacks[i] = removePop(remaining.PopCansInPopCanRacks[i], item);
                            break;
                        }
                    }
                }


            foreach (var item in coinsInStorage)
            {
                if (containsCoin(remaining.PaymentCoinsInStorageBin, item))
                {
                    remaining.PaymentCoinsInStorageBin.Clear();
                    remaining.PaymentCoinsInStorageBin.AddRange(removeCoin(remaining.PaymentCoinsInStorageBin, item));
                }
            }

            return remaining;
        }






        public bool containsCoin(List<Coin> searchIn, Coin searchFor)
        {
            Boolean contains = false;

            foreach (var item in searchIn)
            {
                if (((Coin)item).Value == ((Coin)searchFor).Value)
                {
                    contains = true;
                    break;
                }
            }

            return contains;
        }



        public bool containsPop(List<PopCan> searchIn, PopCan searchFor)
        {
            Boolean contains = false;

            foreach (var item in searchIn)
            {
                if (((PopCan)item).Name == ((PopCan)searchFor).Name)
                {
                    contains = true;
                    break;
                }
            }

            return contains;
        }


        public List<Coin> removeCoin(List<Coin> searchIn, Coin searchFor)
        {

            foreach (var item in searchIn)
                if (((Coin)item).Value == ((Coin)searchFor).Value)
                {
                    searchIn.Remove(item);
                    break;
                }

            return searchIn;
        }


        public List<PopCan> removePop(List<PopCan> searchIn, PopCan searchFor)
        {

            foreach (var item in searchIn)
                if (((PopCan)item).Name == ((PopCan)searchFor).Name)
                {
                    searchIn.Remove(item);
                    break;
                }

            return searchIn;
        }



        public bool contains(List<IDeliverable> searchIn, IDeliverable searchFor)
        {
            Boolean contained = false;

            foreach (var item in searchIn)
            {
                if (item is Coin && searchFor is Coin)
                {
                    if (((Coin)item).Value == ((Coin)searchFor).Value)
                    {
                        contained = true;
                        break;
                    }
                }
                else if (item is PopCan && searchFor is PopCan)
                {
                    if (((PopCan)item).Name == ((PopCan)searchFor).Name)
                    {
                        contained = true;
                        break;
                    }
                }
            }

            return contained;
        }





        public List<IDeliverable> remove(List<IDeliverable> removeFrom, IDeliverable removeThis)
        {
            foreach (var item in removeFrom)
            {
                if (item is Coin && removeThis is Coin)
                {
                    if (((Coin)item).Value == ((Coin)removeThis).Value)
                    {
                        removeFrom.Remove(item);
                        break;
                    }
                }
                else if (item is PopCan && removeThis is PopCan)
                {
                    if (((PopCan)item).Name == ((PopCan)removeThis).Name)
                    {
                        removeFrom.Remove(item);
                        break;
                    }
                }
            }




            return removeFrom;
        }


    }
}
