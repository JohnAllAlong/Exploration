using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class SceneStateManager : MonoBehaviour
{
    public float timeScale = 1.0f;//For testing only, so we can fast forward
    public static bool gameIsPaused;
    public static bool playerLost;
    public static bool isGod;
    private UIManager uiManager;

    [SerializeField]
    private CapsuleCollider2D tonyBones;

    public void Start()
    {
        uiManager = GetComponent<UIManager>();
        playerLost = false;
        isGod = false;
        ResumeGame();
        
    }

    public void Update()
    {
        if (!gameIsPaused)
        {
            Time.timeScale = timeScale;
        }

        //close app on build
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
        //reload scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            EnterMainScene();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!gameIsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GodMode();
        }
    }
    public void EnterMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
    }

    public void GodMode()
    {
        isGod = !isGod;
        if (isGod)
        {
            tonyBones.enabled = false;
        }
        else
        {
            tonyBones.enabled = true;
        }
    }
}
