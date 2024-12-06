using CustomInput.Events;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button menu;

    public void OnceBtnPause(ReturnData _)
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        if (pauseMenu.activeInHierarchy)
        {
            menu.Select();
            Time.timeScale = 0;
            Events.Pause("KeyboardMove", "GamepadMove", "GamepadFire", "MouseFire", "GamepadOpenInv", "KeyboardOpenInv");
        } else
        {
            Time.timeScale = 1;
            Events.Resume("KeyboardMove", "GamepadMove", "GamepadFire", "MouseFire", "GamepadOpenInv", "KeyboardOpenInv");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SaveFramework.Save();
        SceneManager.LoadScene("Title");
    }
}
