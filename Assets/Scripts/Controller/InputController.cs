using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] int firstCoinValue = 0;
    [SerializeField] Tube firstTube;
    [SerializeField] int secondCoinValue = 0;
    [SerializeField] Tube secondTube;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tube tube = hit.collider.GetComponentInParent<Tube>();
                if (tube != null)
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

                        firstTube.SelectCoins(10 - secondTube.coins.Count);


                        if (secondCoinValue == 0 && firstCoinValue != 0) // 2. tube bos heps�n� tas�
                        {

                            secondTube.AddCoin(firstTube.selectedInTube);
                            firstTube.RemoveCoin(firstTube.selectedInTube);

                            ClearSelectedTube();

                            Debug.Log("2. sec�m slot bos");

                        }


                        else if (firstCoinValue == secondCoinValue)// secilenler ayn� turde tas�ma yap�lab�l�r
                        {
                            if (secondTube.coins.Count <= 10)
                            {
                                //eklenecek olana yer var m� 

                                if (firstTube.selectedInTube.Count + secondTube.coins.Count <= 10)
                                {
                                    secondTube.AddCoin(firstTube.selectedInTube);
                                    firstTube.RemoveCoin(firstTube.selectedInTube);
                                }
                                else
                                {

                                    Debug.Log("turler ayn� ama yeter� kadar yer yok");
                                }

                                ClearSelectedTube();


                            }
                            else
                            {
                                Debug.Log("2. sec�m slot dolu ve ayn� turde ama 10 uzer� coin var merge yap�lacak");
                            }




                        }
                        else // deselect islemi yap�lacak
                        {
                            Debug.Log("2. sec�m slot dolu ve farkl� turde");

                            ClearSelectedTube();

                        }
                    }

                }
            }
        }
    }
    public void ClearSelectedTube()
    {
        firstTube.selectedInTube.Clear();
        firstCoinValue = 0;
        secondCoinValue = 0;
        firstTube = null;
        secondTube = null;
    }
}
