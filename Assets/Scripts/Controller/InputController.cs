using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] int firstCoinValue = 0;
    [SerializeField] Tube firstTube;
    [SerializeField] int secondCoinValue = 0;
    [SerializeField] Tube secondTube;

    TubeManager tubeManager;

    private void Start()
    {
        tubeManager = TubeManager.Instance;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tube tube = hit.collider.GetComponentInParent<Tube>();
                if (tube != null && tubeManager.ContainTube(tube))
                {
                    if (firstCoinValue == 0)
                    {
                        firstCoinValue = tube.GetLastCoinValue();
                        if (firstCoinValue == 0) // ilk tube bos
                        {
                            Debug.Log("First Tube is Empty");
                            return;
                        }
                        firstTube = tube;
                        firstTube.SelectCoins(firstTube.Coins.Count, true);
                    }
                    else
                    {// selected ýn value su ýle suan ký secýlenýn valuesu esýt mý esýtse tasýma ýslemý yapýlmalý

                        if (firstTube == tube)
                        {
                            Debug.Log("First Tube Selected Again");
                            ClearSelectedTube();
                            return;
                        }
                        secondCoinValue = tube.GetLastCoinValue();
                        secondTube = tube;

                        firstTube.SelectCoins(10 - secondTube.Coins.Count);

                        if (secondCoinValue == 0 && firstCoinValue != 0) // 2. tube bos hepsýný tasý
                        {
                            secondTube.AddCoin(firstTube.selectedInTube);
                            firstTube.RemoveCoin(firstTube.selectedInTube);
                            ClearSelectedTube();
                        }


                        else if (firstCoinValue == secondCoinValue)// secilenler ayný turde tasýma yapýlabýlýr
                        {
                            if (secondTube.Coins.Count <= 10)
                            {
                                if (firstTube.selectedInTube.Count + secondTube.Coins.Count <= 10)
                                {
                                    secondTube.AddCoin(firstTube.selectedInTube);
                                    firstTube.RemoveCoin(firstTube.selectedInTube);
                                }

                                ClearSelectedTube();

                            }

                        }
                        else
                        {
                            ClearSelectedTube();

                        }
                    }

                }
                else if (tube != null)
                {

                    if (tube.buyable)
                    {
                        tubeManager.BuyTube();
                    }
                    else if (tube.rentable)
                    {
                        tubeManager.RentTube();
                    }
                    else
                    {
                        Debug.Log("Tube not available for rent or buy.");
                    }
                }

            }
        }
    }
    public void ClearSelectedTube()
    {
        firstTube.ClearInTube();
        firstCoinValue = 0;
        secondCoinValue = 0;
        firstTube = null;
        secondTube = null;
    }
}
