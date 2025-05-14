using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoSingleton<LevelManager>
{
    private int mergeScore = 2;

    TubeManager tubeManager;
    PrefabManager prefabManager;


    void Start()
    {
        prefabManager = PrefabManager.Instance;
        tubeManager = TubeManager.Instance;
    }

    private void OnEnable()
    {
        ActionbController.StartLevel += CreateLevel;
    }

    private void OnDisable()
    {
        ActionbController.StartLevel -= CreateLevel;
    }

    public void CreateLevel()
    {

        foreach (Tube tube in tubeManager.GetOpenedTubes())
        {
            for (int i = 0; i < Random.Range(3, 6); i++)
            {
                Coin coin = prefabManager.InstantiateCoin(new Vector3(0, 0, 2.5f - (i * 0.3f)), Quaternion.identity, tube.CoinSlot);
                coin.Init(Random.Range(1, mergeScore + 1));
                tube.Coins.Add(coin);
            }
        }
    }

    public void Deal()
    {

        foreach (Tube tube in tubeManager.GetOpenedTubes())
        {
            if (tube.Coins.Count < 10)
            {
                for (int i = 0; i <= Random.Range(0, tube.Coins.Count < 7 ? 4 : 10 - tube.Coins.Count); i++)
                {
                    Coin coin = prefabManager.InstantiateCoin(new Vector3(0, 0, 2.5f - (tube.Coins.Count * 0.3f)), Quaternion.identity, tube.CoinSlot);
                    coin.Init(Random.Range(1, tubeManager.GetOpenedTubes().Count > mergeScore ? mergeScore + 1 : mergeScore));
                    tube.Coins.Add(coin);
                }
                tube.CheckMerge();
            }
        }
        ActionbController.RemoveGold(10);
    }

    public void Merge()
    {
        foreach (Tube tube in tubeManager.GetOpenedTubes())
        {//combo //action
            if (tube.isMerge)
            {
                int coinValue = tube.GetLastCoinValue();
                tube.DestroyCoins();

                Coin coin = prefabManager.InstantiateCoin(new Vector3(0, 0, 2.5f), Quaternion.identity, tube.CoinSlot);
                coin.Init(coinValue + 1);
                //mergeScore = coinValue + 1;
                if (coinValue + 1 > mergeScore)
                {
                    mergeScore = coinValue + 1;
                    ActionbController.AddGold(coinValue * 20);
                }

                tube.Coins.Add(coin);

                ActionbController.AddGold(coinValue * 10);
            }
        }
    }
}

public static partial class ActionbController
{
    public static Action StartLevel;
}