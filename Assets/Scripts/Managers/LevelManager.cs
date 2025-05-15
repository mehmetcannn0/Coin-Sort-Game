using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoSingleton<LevelManager>
{
    public int MergeScore = 2;
    public int MergeCombo = 0;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

    }


    private void OnEnable()
    {
        ActionController.CreateLevel += CreateLevel;
        ActionController.SetZeroMergeCombo += SetZeroMergeCombo;
    }

    private void OnDisable()
    {
        ActionController.CreateLevel -= CreateLevel;
        ActionController.SetZeroMergeCombo -= SetZeroMergeCombo;
    }

    public void CreateLevel()
    {
        ActionController.AddGold?.Invoke(500);
        ActionController.UpdateGoldUI?.Invoke();

        foreach (Tube tube in ActionController.GetOpenedTube?.Invoke())
        {
            for (int i = 0; i < Random.Range(3, 6); i++)
            {
                Coin coin = ActionController.GetCoin(new Vector3(0, 0, 2.5f - (i * 0.3f)), Quaternion.identity, tube.CoinSlot);
                coin.Init(Random.Range(1, MergeScore + 1));
                tube.Coins.Add(coin);
            }
        }
    }

    public void RestartLevel()
    {
        ActionController.ReGenerateTubes?.Invoke();
        ActionController.ResetOpenedTube?.Invoke();
        gameManager.RestartGame(); // degerlerý sýfýrladý
        MergeScore = 2;
        MergeCombo = 0;

        ActionController.StartLevel?.Invoke();
    }

    public void Deal()
    {
        bool anyAdded = false;
        if (ActionController.RemoveGold.Invoke(10))
        {
            MergeCombo = 0;

            List<Tube> openedTubes = ActionController.GetOpenedTube?.Invoke();

            foreach (Tube tube in openedTubes)
            {
                if (tube.Coins.Count < 10)
                {
                    for (int i = 0; i <= Random.Range(0, tube.Coins.Count < 7 ? 4 : 10 - tube.Coins.Count); i++)
                    {
                        Coin coin = ActionController.GetCoin?.Invoke(new Vector3(0, 0, 2.5f - (tube.Coins.Count * 0.3f)), Quaternion.identity, tube.CoinSlot);
                        coin.Init(Random.Range(1, openedTubes.Count > MergeScore ? MergeScore + 1 : MergeScore));
                        tube.Coins.Add(coin);
                    }
                    anyAdded = true;
                }
            }
            if (!anyAdded)
            {
                //hic ekleme yaðilmadi
                Debug.Log("No coins added");
                ActionController.CheckMerge?.Invoke();
                ActionController.MergeTube?.Invoke();
                gameManager.GameOver();
                Deal();

                return;
            }

            ActionController.CheckMerge?.Invoke();
            gameManager.ResumeGame();
        }
        else
        { // deal ýcýn para yetmedý merge varmý bakýlacak
            if (!gameManager.IsGameOver)
            {
                Debug.Log(" deal before Game Over");
                ActionController.CheckMerge?.Invoke();
                ActionController.MergeTube?.Invoke();
                gameManager.GameOver();
                Deal();



            } // merge ýle para kazanma
            else
            {
                Debug.Log(" Game Over");
                ActionController.GameOver?.Invoke();
                // open game over menu  

                //ActionController.GameOver?.Invoke();



            }

        }
    }
    public void SetZeroMergeCombo()
    {
        MergeCombo = 0;

    }

    public void Merge()
    {
        ActionController.MergeTube?.Invoke();
        Debug.Log("merge den sonra combo " + MergeCombo);
        MergeCombo = 0;
    }


}

public static partial class ActionController
{
    public static Action CreateLevel;
    public static Action SetZeroMergeCombo;

}