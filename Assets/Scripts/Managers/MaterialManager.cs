 
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MaterialManager : MonoSingleton<MaterialManager>
{
   
    [SerializeField] private List<CoinTypeData> materials;

    public CoinTypeData GetCoinTypeData(int requestedType) => materials.FirstOrDefault(x => x.value == requestedType);
}

[Serializable]
public class CoinTypeData
{
    public int value;
    public Material material;
}