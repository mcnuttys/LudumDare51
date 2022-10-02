using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public TMP_Text title;
    public RawImage thumbnail;
    public Button playButton;

    private Level level;

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnClickPlay);
    }

    public void Setup(Level level)
    {
        this.level = level;

        title.text = $"Level {level.levelIndex}: {level.name}";
        thumbnail.texture = level.thumbnail;

        playButton.onClick.AddListener(OnClickPlay);
    }

    private void OnClickPlay()
    {
        GameManager.instance.LoadLevel(level.levelIndex);
    }
}
