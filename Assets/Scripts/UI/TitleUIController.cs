using Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIController : MonoBehaviour
{
    [SerializeField] private Image _fader;
    [SerializeField] private Button _start;
    [SerializeField] private SaveDataLoader _saveLoader;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private GameObject _saveWipe;

    private void Start()
    {
        _start.Select();
        if (_saveLoader.completedTutorial)
        {
            _tutorial.SetActive(true);
            _saveWipe.SetActive(true);
        }
    }

    public void StartGame()
    {
        _fader.enabled = true;
        GUIUtilitys.FadeInSprite(_fader, 2, () =>
        {
            if (_saveLoader.completedTutorial)
            {
                SceneManager.LoadScene("Game");
            }
            else
            {
                SceneManager.LoadScene("Tutorial");
            }
        });
    }

    public void WipeSave()
    {
        SaveFramework.DestroySaveData();
        SceneManager.LoadScene("Title");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
