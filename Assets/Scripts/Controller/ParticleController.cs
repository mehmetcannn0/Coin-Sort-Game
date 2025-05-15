using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private GameObject confettiEffectPrefab;

    private void OnEnable()
    {
        ActionController.PlayMergeEffect += PlayMergeEffect;
    }
    private void OnDisable()
    {
        ActionController.PlayMergeEffect -= PlayMergeEffect;
    }

    public void PlayMergeEffect(Vector3 position)
    {
        GameObject effect = Instantiate(confettiEffectPrefab, position, confettiEffectPrefab.transform.rotation);
        Destroy(effect, 2f);
    }

}
public static partial class ActionController
{
    public static Action<Vector3> PlayMergeEffect;
}
