using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{

    TubeManager tubeManager;
    PrefabManager prefabManager;


    void Start()
    {
        prefabManager = PrefabManager.Instance;
        tubeManager = TubeManager.Instance;
        CreateLEvel();

    }

    public void CreateLEvel()
    {

        foreach (Tube tube in tubeManager.tubes)
        {
            for (int i = 0; i < Random.Range(5, 9); i++)
            {
                Coin coin = prefabManager.InstantiateCoin(new Vector3(0, 0, 2.5f - (i * 0.3f)), Quaternion.identity, tube.coinSlot);
                coin.Init(Random.Range(1, 5));
                tube.coins.Add(coin);
            }
        }


    }
    public void Deal()
    {
        Debug.Log("Deal");
        foreach (Tube tube in tubeManager.tubes)
        {
            for (int i = 0; i < Random.Range(0, tube.coins.Count<10? 4: 10-tube.coins.Count); i++)
            {
                Coin coin = prefabManager.InstantiateCoin(new Vector3(0, 0, 2.5f - (tube.coins.Count * 0.3f)), Quaternion.identity, tube.coinSlot);
                coin.Init(Random.Range(1, 5));
                tube.coins.Add(coin);
            }
        }


    }
    public void Merge()
    {
        foreach (Tube tube in tubeManager.tubes)
        {
            if (tube.isMerge)
            {
                int coinValue = tube.GetLastCoinValue();
                tube.DestroyCoins();

                Coin coin = prefabManager.InstantiateCoin(new Vector3(0, 0, 2.5f), Quaternion.identity, tube.coinSlot);
                coin.Init(coinValue + 1);
                tube.coins.Add(coin);

            }
        }
    }

}
