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
    {// coinler býraz yukarýya cýkacak

    }
    public void OnDeselectCoin()
    {// ayný yere týklandýysa gerý eský konumuna donecek


    }

    public void OnDropNewTube()
    {// yený yere týklandýgýnda konumlarý degýsecek


    }



}
