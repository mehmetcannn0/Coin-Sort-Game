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
                    {// selected ýn value su ýle suan ký secýlenýn valuesu esýt mý esýtse tasýma ýslemý yapýlmalý

                        if (firstTube == tube)
                        {
                            Debug.Log("First Tube Selected Again");
                            ClearSelectedTube();
                            return;
                        }
                        secondCoinValue = tube.GetLastCoinValue();
                        secondTube = tube;

                        firstTube.SelectCoins(10 - secondTube.coins.Count);


                        if (secondCoinValue == 0 && firstCoinValue != 0) // 2. tube bos hepsýný tasý
                        {

                            secondTube.AddCoin(firstTube.selectedInTube);
                            firstTube.RemoveCoin(firstTube.selectedInTube);

                            ClearSelectedTube();

                            Debug.Log("2. secým slot bos");

                        }


                        else if (firstCoinValue == secondCoinValue)// secilenler ayný turde tasýma yapýlabýlýr
                        {
                            if (secondTube.coins.Count <= 10)
                            {
                                //eklenecek olana yer var mý 

                                if (firstTube.selectedInTube.Count + secondTube.coins.Count <= 10)
                                {
                                    secondTube.AddCoin(firstTube.selectedInTube);
                                    firstTube.RemoveCoin(firstTube.selectedInTube);
                                }
                                else
                                {

                                    Debug.Log("turler ayný ama yeterý kadar yer yok");
                                }

                                ClearSelectedTube();


                            }
                            else
                            {
                                Debug.Log("2. secým slot dolu ve ayný turde ama 10 uzerý coin var merge yapýlacak");
                            }




                        }
                        else // deselect islemi yapýlacak
                        {
                            Debug.Log("2. secým slot dolu ve farklý turde");

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
