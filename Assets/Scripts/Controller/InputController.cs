using Unity.VisualScripting;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] int firstCoinValue = 0;
    [SerializeField] Tube firstTube;
    [SerializeField] int secondCoinValue = 0;
    [SerializeField] Tube secondTube;
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !gameManager.IsGameOver)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tube tube = hit.collider.GetComponentInParent<Tube>();
                if (tube != null && ActionController.ContainThisTube.Invoke(tube))
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
                    {// selected �n value su �le suan k� sec�len�n valuesu es�t m� es�tse tas�ma �slem� yap�lmal�

                        if (firstTube == tube)
                        {
                            Debug.Log("First Tube Selected Again");
                            ClearSelectedTube();
                            return;
                        }
                        secondCoinValue = tube.GetLastCoinValue();
                        secondTube = tube;

                        firstTube.SelectCoins(10 - secondTube.Coins.Count);

                        if (secondCoinValue == 0 && firstCoinValue != 0) // 2. tube bos heps�n� tas�
                        {
                            secondTube.AddCoin(firstTube.selectedInTube);
                            firstTube.RemoveCoin(firstTube.selectedInTube);
                            ClearSelectedTube();
                        }
                        else if (firstCoinValue == secondCoinValue)// secilenler ayn� turde tas�ma yap�lab�l�r
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
                          
                        ActionController.SetZeroMergeCombo?.Invoke();   
                        ActionController.CheckMerge?.Invoke();
                    }
                }
                else if (tube != null)
                {

                    if (tube.buyable)
                    {
                        ActionController.BuyTube?.Invoke();
                    }
                    else if (tube.rentable)
                    {
                        ActionController.RentTube?.Invoke();
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
