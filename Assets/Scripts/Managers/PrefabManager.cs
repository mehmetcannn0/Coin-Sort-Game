using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoSingleton<PrefabManager>
{
    [SerializeField] private GameObject coinPrefab;  

    public Coin InstantiateCoin(Vector3 objectPosition, Quaternion rotation, Transform parent)
    { 
        GameObject coin = Instantiate(coinPrefab, objectPosition, rotation, parent);
        coin.transform.localPosition = objectPosition;  
        return coin.GetComponent<Coin>() ;
    }
}
