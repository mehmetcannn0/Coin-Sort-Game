using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] MeshRenderer coinRenderer;
    public CoinTypeData coinTypeData { get; private set; } 
    private MaterialManager materialManager;

    private void Awake()
    {
        materialManager = MaterialManager.Instance;
        
         


    }
    public void Init(int value)
    {
        coinTypeData = materialManager.GetCoinTypeData(value);
        coinText.text = coinTypeData.value.ToString();
        coinRenderer.material = coinTypeData.material;
    }
    public void OnSelectCoin()
    {// coinler b�raz yukar�ya c�kacak

    }
    public void OnDeselectCoin()
    {// ayn� yere t�kland�ysa ger� esk� konumuna donecek


    }

    public void OnDropNewTube()
    {// yen� yere t�kland�g�nda konumlar� deg�secek


    }



}
