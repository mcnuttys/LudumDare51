using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text fatalityRate;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnClickRestart);
        menuButton.onClick.AddListener(OnClickMenu);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(OnClickRestart);
        menuButton.onClick.RemoveListener(OnClickMenu);
    }

    public void SetRate(float percent)
    {
        fatalityRate.text = $"Fatality Rate: {Mathf.RoundToInt(percent)}%";
    }

    private void OnClickRestart()
    {
        GameManager.instance.RestartLevel();
    }

    private void OnClickMenu()
    {
        GameManager.instance.ToMenu();
    }
}
