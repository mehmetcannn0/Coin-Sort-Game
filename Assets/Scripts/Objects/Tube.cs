

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public List<Coin> Coins;
    public Transform CoinSlot;

    [SerializeField] MeshRenderer tubeRenderer;
    [SerializeField] GameObject buyUI;
    [SerializeField] GameObject rentUI;
    [SerializeField] TextMeshProUGUI buyPriceText;
    [SerializeField] TextMeshProUGUI rentPriceText;
    public bool isMerge { get; private set; }
    public bool isRented { get; private set; }

    public bool buyable;
    public bool rentable;

    public TextMeshProUGUI rentTimeText;
    public int rentDuration;

    public List<Coin> selectedInTube;

    MaterialManager materialManager;
    LevelManager levelManager;

    private void Awake()
    {
        levelManager = LevelManager.Instance;
        materialManager = MaterialManager.Instance;
    }
    private void OnEnable()
    {
        ActionController.CheckMerge += CheckMerge;
        ActionController.MergeTube += MergeTube;
        ActionController.ReGenerateTubes += ReGenerateTube;
    }
    private void OnDisable()
    {
        ActionController.CheckMerge -= CheckMerge;
        ActionController.MergeTube -= MergeTube;
        ActionController.ReGenerateTubes -= ReGenerateTube;
    }

    public int GetLastCoinValue()
    {
        if (Coins.Count > 0)
        {
            return Coins[^1].CoinTypeData.value;
        }
        else
        {
            return 0;
        }
    }

    public void SelectCoins(int emptySlotCount, bool firstSelection = false)
    {
        selectedInTube.Clear();

        if (Coins.Count > 0)
        {
            for (int i = Coins.Count - 1; i >= 0; i--)
            {
                if (Coins[i].CoinTypeData.value == Coins[^1].CoinTypeData.value)
                {
                    selectedInTube.Add(Coins[i]);

                    if (firstSelection)
                    {
                        Coins[i].CoinVisual.transform.DOLocalMove(new Vector3(0, 1, 0), 0.3f);
                    }
                }
                else
                {
                    break;
                }
            }

            selectedInTube.Reverse();

            if (selectedInTube.Count > emptySlotCount)
            {
                for (int i = 0; selectedInTube.Count > emptySlotCount; i++)
                {
                    selectedInTube[0].CoinVisual.transform.DOLocalMove(Vector3.zero, 0.1f);
                    selectedInTube.RemoveAt(0);
                }
            }
        }
    }

    public void AddCoin(List<Coin> newCoins)
    {
        foreach (Coin coin in newCoins)
        {
            coin.transform.SetParent(CoinSlot);
            coin.transform.DOLocalMove(new Vector3(0, 0, 2.5f - (Coins.Count * 0.3f)), 0.5f);
            coin.CoinVisual.transform.DOLocalMove(Vector3.zero, 0.1f);

            Coins.Add(coin);
        }

    }

    public void RemoveCoin(List<Coin> newCoins)
    {
        foreach (Coin coin in newCoins)
        {
            Coins.Remove(coin);
        }

    }

    public void CheckMerge()
    {

        if (Coins.Count >= 10)
        {
            foreach (Coin item in Coins)
            {
                if (item.CoinTypeData.value != Coins[0].CoinTypeData.value)
                {
                    if (isMerge)
                    {
                        UpdateTubeMaterial(materialManager.tubeMaterial);
                    }
                    isMerge = false;
                    return;
                }

            }
            if (!isMerge)
            {
                UpdateTubeMaterial(materialManager.mergeableTubeMaterial);
            }
            isMerge = true;
            levelManager.MergeCombo++;

        }
        else
        {
            if (isMerge)
            {
                UpdateTubeMaterial(isRented ? materialManager.rentedTubeMaterial : materialManager.tubeMaterial);
            }
            isMerge = false;
        }
    }

    private void MergeTube()
    {

        if (isMerge)
        {
            ActionController.PlayMergeEffect?.Invoke(CoinSlot.position);
            Debug.Log("in merge  " + levelManager.MergeCombo);
            int coinValue = GetLastCoinValue();
            DestroyCoins();

            Coin coin = ActionController.InstantiateCoin?.Invoke(new Vector3(0, 0, 2.5f), Quaternion.identity, CoinSlot);
            coin.Init(coinValue + 1);
            if (coinValue == 9)
            {
                Coin secondCoin = ActionController.InstantiateCoin?.Invoke(new Vector3(0, 0, 2.2f), Quaternion.identity, CoinSlot);
                secondCoin.Init(coinValue + 1);
                Coins.Add(secondCoin);

            }
            //mergeScore = coinValue + 1;
            if (coinValue + 1 > levelManager.MergeScore)
            {
                levelManager.MergeScore = coinValue + 1;
                ActionController.AddGold?.Invoke(coinValue * 20);
            }

            Coins.Add(coin);

            ActionController.AddGold?.Invoke(coinValue * 10 * levelManager.MergeCombo);
        }
    }
    public void ClearInTube()
    {
        foreach (Coin item in selectedInTube)
        {
            item.CoinVisual.transform.DOLocalMove(Vector3.zero, 0.1f);
        }

        selectedInTube.Clear();
    }

    public void DestroyCoins()
    {
        foreach (Coin item in Coins)
        {
            Destroy(item.gameObject);
        }
        Coins.Clear();
        isMerge = false;
        UpdateTubeMaterial(isRented ? materialManager.rentedTubeMaterial : materialManager.tubeMaterial);
    }

    public void UpdateTubeMaterial(Material newMaterial)
    {
        tubeRenderer.material = newMaterial;
    }

    public void Rent()
    {
        isRented = true;
        rentable = false;
        UpdateTubeMaterial(materialManager.rentedTubeMaterial);
        rentUI.SetActive(false);
        buyUI.SetActive(false);

        StartCoroutine(RentCountdown());
    }

    private IEnumerator RentCountdown()
    {
        int timeLeft = rentDuration;
        rentTimeText.gameObject.SetActive(true);

        while (timeLeft > 0)
        {
            rentTimeText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        //rentTimeText.text = "Süre doldu!";
        rentTimeText.gameObject.SetActive(false);
        isRented = false;

        //tubeManager.RemoveTube(this);
        ActionController.RemoveTube?.Invoke(this);

        foreach (var item in Coins)
        {
            Destroy(item.gameObject);

        }

        Coins.Clear();
    }

   

    public void Buy()
    {
        isRented = false;
        rentable = false;
        buyable = false;

        UpdateTubeMaterial(materialManager.tubeMaterial);
        rentUI.SetActive(false);
        buyUI.SetActive(false);
    
    }

    public void ShowBuyUI(int price)
    {
        buyable = true;
        rentable = false;
        buyUI.SetActive(true);
        rentUI.SetActive(false);
        buyPriceText.text = price.ToString();
        UpdateTubeMaterial(materialManager.greyMaterial);
    }

    public void ShowRentUI(int price)
    {
        buyable = false;
        rentable = true;
        rentUI.SetActive(true);
        buyUI.SetActive(false);
        rentPriceText.text = price.ToString();
        UpdateTubeMaterial(materialManager.greyMaterial);
    }
    public void ReGenerateTube()
    {
        if (isRented)
        {
            CancleRent();

        }
        isRented = false;
        isMerge = false;
        rentable = false;
        buyable = false;

        foreach (Coin item in selectedInTube)
        {
            Destroy(item.gameObject);
        }

        selectedInTube.Clear();

        UpdateTubeMaterial(materialManager.greyMaterial);

        rentUI.SetActive(false);
        buyUI.SetActive(false);

        foreach (var item in Coins)
        {
            Destroy(item.gameObject);

        }
        Coins.Clear();
    }
    public void CancleRent()
    {
        StopAllCoroutines();
        rentTimeText.gameObject.SetActive(false);
        isRented = false;

        //tubeManager.RemoveTube(this);
        ActionController.RemoveTube?.Invoke(this);

        foreach (var item in Coins)
        {
            Destroy(item.gameObject);

        }

        Coins.Clear();
    }

}

public static partial class ActionController
{
    public static Action CheckMerge;
    public static Action MergeTube;
    public static Action ReGenerateTubes;

}