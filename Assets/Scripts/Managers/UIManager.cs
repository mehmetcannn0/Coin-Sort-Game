using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] TextMeshProUGUI goldUI;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        ActionbController.AddGold.Invoke(50);
        UpdateGoldUI();

    }

    private void OnEnable()
    {
        ActionbController.UpdateGoldUI += UpdateGoldUI;
    }

    private void OnDisable()
    {
        ActionbController.UpdateGoldUI -= UpdateGoldUI;
    }

    public void UpdateGoldUI()
    {
        goldUI.text = gameManager.Gold.ToString();
    }
}

public static partial class ActionbController
{
    public static Action UpdateGoldUI;
}