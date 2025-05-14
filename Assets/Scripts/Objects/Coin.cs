using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] MeshRenderer coinRenderer;

    public GameObject CoinVisual;
    public CoinTypeData CoinTypeData { get; private set; }

    MaterialManager materialManager;

    private void Awake()
    {
        materialManager = MaterialManager.Instance;
    }

    public void Init(int value)
    {
        CoinTypeData = materialManager.GetCoinTypeData(value);
        coinText.text = CoinTypeData.value.ToString();
        coinRenderer.material = CoinTypeData.material;
    }
}
