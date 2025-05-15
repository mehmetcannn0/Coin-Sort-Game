using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    [SerializeField] private GameObject coinPrefab;

    private List<Coin> coinPool = new List<Coin>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject newCoin = Instantiate(coinPrefab, Vector3.zero, Quaternion.identity, transform);
            newCoin.gameObject.SetActive(false);
            coinPool.Add(newCoin.GetComponent<Coin>());
        }
    }
    private void OnEnable()
    {
        ActionController.GetCoin += GetCoin;
        ActionController.ReturnCoin += ReturnCoin;
    }
    private void OnDisable()
    {
        ActionController.GetCoin -= GetCoin;
        ActionController.ReturnCoin -= ReturnCoin;
    }
    public Coin GetCoin(Vector3 objectPosition, Quaternion rotation, Transform parent)
    {
        if (coinPool.Count > 0)
        {
            Coin coin = coinPool[0];
            coin.transform.SetParent(parent);
            coinPool.RemoveAt(0);
            coin.transform.localPosition = objectPosition;
            coin.transform.rotation = rotation;
            coin.gameObject.SetActive(true);
            return coin;


        }

        GameObject newCoin = Instantiate(coinPrefab, objectPosition, rotation, parent);
        newCoin.transform.localPosition = objectPosition;
        return newCoin.GetComponent<Coin>();
    }
    public void ReturnCoin(Coin coin)
    {
        coin.gameObject.SetActive(false);
        coin.transform.SetParent(transform);
        coin.transform.localPosition = Vector3.zero;
        coin.transform.localRotation = Quaternion.identity;
        coinPool.Add(coin);
    }


}
public static partial class ActionController
{
    public static Func<Vector3, Quaternion, Transform, Coin> GetCoin;
    public static Action<Coin> ReturnCoin;
}
