using System;
using UnityEngine;

public class PrefabManager : MonoSingleton<PrefabManager>
{
    [SerializeField] private GameObject coinPrefab;
    private void OnEnable()
    {
        ActionController.InstantiateCoin += InstantiateCoin;

    }
    private void OnDisable()
    {
        ActionController.InstantiateCoin -= InstantiateCoin;
    }

    public Coin InstantiateCoin(Vector3 objectPosition, Quaternion rotation, Transform parent)
    {
        GameObject coin = Instantiate(coinPrefab, objectPosition, rotation, parent);
        coin.transform.localPosition = objectPosition;
        return coin.GetComponent<Coin>();
    }


}
public static partial class ActionController
{
    public static Func<Vector3, Quaternion, Transform, Coin> InstantiateCoin;
}
