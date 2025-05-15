using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] TextMeshProUGUI goldUI;
    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject GameOverUI;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;   

    }

    private void OnEnable()
    {
        ActionController.UpdateGoldUI += UpdateGoldUI;
        ActionController.StartLevel += ShowGameUI;
        ActionController.GameOver += ShowGameOverUI;
    }

    private void OnDisable()
    {
        ActionController.UpdateGoldUI -= UpdateGoldUI;
        ActionController.StartLevel -= ShowGameUI;
        ActionController.GameOver -= ShowGameOverUI;
    }
    public void ShowGameUI()
    {
        GameUI.SetActive(true);
        GameOverUI.SetActive(false);
    }
    public void ShowGameOverUI()
    {
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }
    public void UpdateGoldUI()
    {
        goldUI.text = gameManager.Gold.ToString();
    }
}

public static partial class ActionController
{
    public static Action UpdateGoldUI;
}