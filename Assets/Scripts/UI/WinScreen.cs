using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text survivalRate;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnClickRestart);
        nextButton.onClick.AddListener(OnClickNext);
        menuButton.onClick.AddListener(OnClickMenu);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(OnClickRestart);
        nextButton.onClick.RemoveListener(OnClickNext);
        menuButton.onClick.RemoveListener(OnClickMenu);
    }

    public void SetRate(float percent)
    {
        survivalRate.text = $"Survival Rate: {Mathf.RoundToInt(percent)}%";
    }

    private void OnClickRestart()
    {
        GameManager.instance.RestartLevel();
    }

    private void OnClickNext()
    {
        GameManager.instance.NextLevel();
    }

    private void OnClickMenu()
    {
        GameManager.instance.ToMenu();
    }
}
