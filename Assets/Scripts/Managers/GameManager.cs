using System;

public class GameManager : MonoSingleton<GameManager>
{

    public int Gold { get; private set; }  

     
    private void OnEnable()
    {
        ActionbController.AddGold += AddGold;
        ActionbController.RemoveGold += RemoveGold;
    }
    private void OnDisable()
    {
        ActionbController.AddGold -= AddGold;
        ActionbController.RemoveGold -= RemoveGold;
    }


    private void AddGold(int amount)
    {
        Gold += amount;

        ActionbController.UpdateGoldUI?.Invoke();
    }
    private void RemoveGold(int amount)
    {
        Gold -= amount;
        ActionbController.UpdateGoldUI?.Invoke();
    }
}
public static partial class ActionbController
{
    public static Action<int> AddGold;
    public static Action<int> RemoveGold;
}
