
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialManager : MonoSingleton<MaterialManager>
{
    [SerializeField] private List<CoinTypeData> coinMaterials;

    public Material greyMaterial;
    public Material tubeMaterial;
    public Material rentedTubeMaterial;
    public Material mergeableTubeMaterial;

    public CoinTypeData GetCoinTypeData(int requestedType) => coinMaterials.FirstOrDefault(x => x.value == requestedType);
}

[Serializable]
public class CoinTypeData
{
    public int value;
    public Material material;
}