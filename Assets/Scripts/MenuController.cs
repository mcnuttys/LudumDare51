using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject levelListObject;

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;


    [Header("Levels")]
    [SerializeField] private Transform levelList;
    [SerializeField] private GameObject levelPrefab;


    void Start()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnClickStart);
        quitButton.onClick.RemoveListener(OnClickQuit);
    }

    private void AddListeners()
    {
        startButton.onClick.AddListener(OnClickStart);
        quitButton.onClick.AddListener(OnClickQuit);
    }

    void OnClickStart()
    {
        menuObject.SetActive(false);
        levelListObject.SetActive(true);
        PopulateLevels();
    }

    void OnClickQuit()
    {
        Application.Quit();
    }

    void PopulateLevels()
    {
        var levels = GameManager.instance.levels;

        foreach (var level in levels)
        {
            var l = Instantiate(levelPrefab, levelList);
            l.GetComponent<LevelUI>().Setup(level);
        }
    }
}
