

using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int Gold { get; private set; }
    public bool IsGameOver { get; private set; }

    private void OnEnable()
    {
        ActionController.AddGold += AddGold;
        ActionController.RemoveGold += RemoveGold;
    }

    private void OnDisable()
    {
        ActionController.AddGold -= AddGold;
        ActionController.RemoveGold -= RemoveGold;
    }

    private void AddGold(int amount)
    {
        Gold += amount;

        ActionController.UpdateGoldUI?.Invoke();
    }

    private bool RemoveGold(int amount)
    {
        if (Gold < amount)
        {
            Debug.Log("Not enough gold");
            return false;
        }
        Gold -= amount;
        ActionController.UpdateGoldUI?.Invoke();

        return true;
    }
    public void GameOver()
    {
        IsGameOver = true;
    }
    public void ResumeGame()
    {
        IsGameOver = false;
    }
    public void RestartGame()
    {
        Gold = 0;        
        IsGameOver = false;
        ActionController.UpdateGoldUI?.Invoke();
    }
}

public static partial class ActionController
{
    public static Action<int> AddGold;
    public static Func<int, bool> RemoveGold;
    public static Action GameOver;

}
