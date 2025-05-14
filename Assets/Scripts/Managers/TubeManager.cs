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
        ActionbController.StartLevel.Invoke();
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
                ActionbController.RemoveGold(buyPrice);
                buyPrice += 20;

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
            ActionbController.RemoveGold(rentPrice);
        }
    }
}
