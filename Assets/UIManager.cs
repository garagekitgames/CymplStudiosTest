using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using garagekitgames;
public class UIManager : MonoBehaviour
{
    public IntVariable level;

    public TextMeshProUGUI levelText;

    public RectTransform levelClearMenu, levelFailMenu;
    private void Start()
    {
        levelText.text = level.value.ToString();
    }
    public void ClickNextLevel()
    {
        level.value += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PersistableSO.Instance.Save();
    }

    public void ClickRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowLevelClearMenu()
    {
        levelClearMenu.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void ShowLevelFailMenu()
    {
        levelFailMenu.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void HideLevelClearMenu()
    {
        levelClearMenu.DOAnchorPos(new Vector2(800f, -800f), 0.25f);
    }

    public void HideLevelFailMenu()
    {
        levelFailMenu.DOAnchorPos(new Vector2(-800f, 800f), 0.25f);
    }
}
