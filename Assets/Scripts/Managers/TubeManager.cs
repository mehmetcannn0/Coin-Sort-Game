using System;
using System.Collections.Generic;
using UnityEngine;

public class TubeManager : MonoSingleton<TubeManager>
{
    [SerializeField] GameObject tubesParents;

    private List<Tube> openedTubes = new List<Tube>();
    private List<Tube> tubes = new List<Tube>();

    private int buyPrice = 20;
    private int rentPrice = 10;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        foreach (var item in tubesParents.GetComponentsInChildren<Tube>())
        {
            tubes.Add(item);
        }

        StartLevel();
    }

    private void StartLevel()
    {

        for (int i = 0; i < 3; i++)
        {
            tubes[0].Buy();
            openedTubes.Add(tubes[0]);
            tubes.RemoveAt(0);


        }
        tubes[0].ShowBuyUI(buyPrice);
        tubes[1].ShowRentUI(rentPrice);
        ActionController.CreateLevel?.Invoke();
    }

    public void ClearOpenedTubes()
    {
        buyPrice = 20;
        rentPrice = 10;
        openedTubes.Reverse();
        foreach (Tube tube in openedTubes)
        {
            tubes.Insert(0, tube);
        }
        openedTubes.Clear();

    }

    private void OnEnable()
    {
        ActionController.StartLevel += StartLevel;
        ActionController.GetOpenedTube += GetOpenedTubes;
        ActionController.ContainThisTube += ContainTube;
        ActionController.RemoveTube += RemoveTube;
        ActionController.BuyTube += BuyTube;
        ActionController.RentTube += RentTube;
        ActionController.ResetOpenedTube += ClearOpenedTubes;
    }
    private void OnDisable()
    {
        ActionController.StartLevel -= StartLevel;
        ActionController.GetOpenedTube -= GetOpenedTubes;
        ActionController.ContainThisTube -= ContainTube;
        ActionController.RemoveTube -= RemoveTube;
        ActionController.BuyTube -= BuyTube;
        ActionController.RentTube -= RentTube;
        ActionController.ResetOpenedTube -= ClearOpenedTubes;
    }

    public bool ContainTube(Tube tube)
    {
        return openedTubes.Contains(tube);
    }

    public List<Tube> GetOpenedTubes()
    {
        return openedTubes;
    }

    public void AddTube(Tube tube)
    {
        openedTubes.Add(tube);

    }
    public void RemoveTube(Tube tube)
    {
        tube.rentable = true;
        tube.ShowRentUI(rentPrice);

        if (tubes.Count >= 1)
        {
            tubes.Insert(1, tube);
        }
        openedTubes.Remove(tube);
    }

    public void BuyTube()
    {
        if (tubes.Count > 0 && gameManager.Gold >= buyPrice)
        {
            if (!openedTubes[^1].isRented)
            {
                tubes[0].Buy();
                openedTubes.Add(tubes[0]);
                tubes.RemoveAt(0);
                ActionController.RemoveGold?.Invoke(buyPrice);
                buyPrice *= 2;

                if (tubes.Count >= 1)
                {
                    tubes[0].ShowBuyUI(buyPrice);
                }

                if (tubes.Count >= 2)
                {
                    tubes[1].ShowRentUI(rentPrice);
                }
            }

        }
        else
        {
            Debug.Log("No more tubes to buy or not enough gold");
        }

    }

    public void RentTube()
    {
        if (gameManager.Gold >= rentPrice)
        {
            Tube rentedTube = tubes[1];
            rentedTube.Rent();
            openedTubes.Add(rentedTube);
            tubes.Remove(rentedTube);
            ActionController.RemoveGold?.Invoke(rentPrice);
        }
    }
}
public static partial class ActionController
{
    public static Func<List<Tube>> GetOpenedTube;
    public static Func<Tube, bool> ContainThisTube;
    public static Action<Tube> RemoveTube;
    public static Action BuyTube;
    public static Action RentTube;
    public static Action StartLevel;
    public static Action ResetOpenedTube;


}
