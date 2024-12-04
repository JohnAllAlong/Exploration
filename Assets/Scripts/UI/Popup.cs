using UnityEngine;
using UnityEngine.InputSystem;
using CustomInput;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private GameObject _gamepadPopup;
    [SerializeField]
    private GameObject _keyboardPopup;

    protected void Update()
    {
        if (Devices.GetCurrent() as Gamepad != null)
        {//gamepad
            _gamepadPopup.SetActive(true);
            _keyboardPopup.SetActive(false);
        }
        else
        {//keyboard
            _keyboardPopup.SetActive(true);
            _gamepadPopup.SetActive(false);
        }
    }
}
