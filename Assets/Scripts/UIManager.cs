using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseUI;
    public GameObject howUI;
    public GameObject winUI;
    public GameObject loseUI;

    private List<GameObject> uiList;

    void Start()
    {
        uiList = new List<GameObject> { gameUI, pauseUI, howUI, winUI, loseUI };
        LoadUI(gameUI);
    }

    public void LoadUI(GameObject uiToLoad)
    {
        foreach (GameObject go in uiList)
        {
            if(go != null)
            {
                go.SetActive(go == uiToLoad);
            }
        }
    }
    public void LoadGameUI()
    {
        LoadUI(gameUI);
    }
    public void LoadPauseUI()
    {
        LoadUI(pauseUI);
    }
    public void LoadHowUI()
    {
        LoadUI(howUI);
    }
    public void LoadWinUI()
    {
        LoadUI(winUI);
    }
    public void LoadLoseUI()
    {
        LoadUI(loseUI);
    }
}
