using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Level> levels;
    private int currentLevelIndex;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= levels.Count)
        {
            ToMenu();
            return;
        }

        currentLevelIndex = levelIndex;
        SceneManager.LoadScene(levels[levelIndex].sceneIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levels[currentLevelIndex].sceneIndex);
    }

    public void NextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }

    public void ToMenu()
    {
        currentLevelIndex = -1;
        SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public struct Level
{
    public string name;
    public int levelIndex;
    public int sceneIndex;
    public Texture2D thumbnail;
}
