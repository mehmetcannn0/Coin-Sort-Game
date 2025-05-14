using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public List<Coin> coins;
    public Transform coinSlot;
    public bool isMerge { get; private set; }

    public List<Coin> selectedInTube;



    public int GetLastCoinValue()
    {
        if (coins.Count > 0)
        {
            return coins[^1].coinTypeData.value;
        }
        else
        {
            return 0;
        }
    }


    public void SelectCoins(int emptySlotCount) //0 ýse bos
    {
        selectedInTube.Clear();


        if (coins.Count > 0)
        {
            for (int i = coins.Count - 1; i >= 0; i--)
            {

                if (coins[i].coinTypeData.value == coins[^1].coinTypeData.value)
                {
                    selectedInTube.Add(coins[i]);
                }
                else
                {
                    break;

                }

            }




            selectedInTube.Reverse();

            if (selectedInTube.Count > emptySlotCount)
            {
                for (int i = 0;  selectedInTube.Count > emptySlotCount; i++)
                {
                    selectedInTube.RemoveAt(0); 
                }
            }

        }
        else
        {
            Debug.Log("No coins in the tube.");
        }



    }



    public void AddCoin(List<Coin> newCoins)
    {

        foreach (Coin coin in newCoins)
        {
            coin.transform.SetParent(coinSlot);
            coin.transform.localPosition = new Vector3(0, 0, 2.5f - (coins.Count * 0.3f));
            coins.Add(coin);


        }
        if (coins.Count >= 10)
        {
            CheckMerge();

        }


    }
    public void RemoveCoin(List<Coin> newCoins)
    {
        foreach (Coin coin in newCoins)
        {
            coins.Remove(coin);

        }
    }
    public void CheckMerge()
    {
        if (coins.Count >= 10)
        {
            foreach (Coin item in coins)
            {
                if (item.coinTypeData.value != coins[0].coinTypeData.value)
                {

                    isMerge = false;
                    return;
                }



            }


            isMerge = true;

        }
        else
        {
            isMerge = false;


        }
    }

    public void DestroyCoins()
    {
        foreach (Coin item in coins)
        {
            Destroy(item.gameObject);
        }
        coins.Clear();
        isMerge = false;

    }




}
