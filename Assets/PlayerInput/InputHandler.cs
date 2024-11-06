using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Input;
using Input.Values;

public class InputHandler : InputEvents
{
    //register events
    private void OnEnable()
    {
        //set onDeviceChange
        InputSystem.onDeviceChange += OnDeviceChanged;

        //set the launch input device
        InputDevice nextAvailable = GetNextAvailableInputDevice();

        if (nextAvailable as Gamepad != null)
        {
            nextAvailable.MakeCurrent();
            InitializeGamepadEvents();
        }
        else if (nextAvailable as Keyboard != null)
        {
            nextAvailable.MakeCurrent();
            InitializeKeyboardEvents();
        }
    }

    //de-register events
    private void OnDisable()
    {
        //de-set onDeviceChange
        InputSystem.onDeviceChange -= OnDeviceChanged;
        RemoveAllEvents();
    }

    //handle input switching
    public override void DeviceConnected(InputDevice device)
    {
        if (device as Gamepad != null)
        {
            device.MakeCurrent();
            InitializeGamepadEvents();
        }
        else if (device as Keyboard != null)
        {
            device.MakeCurrent();
            InitializeKeyboardEvents();
        }
    }

    //handle input switching
    public override void DeviceRemoved(InputDevice device)
    {
        //attempt to switch to the next available method of input
        //(Gamepad or Keyboard)
        InputDevice nextAvailable = GetNextAvailableInputDevice();


        //re-assign input events
        if (nextAvailable as Gamepad != null)
        {
            nextAvailable.MakeCurrent();
            InitializeGamepadEvents();
        }
        else if (nextAvailable as Keyboard != null)
        {
            nextAvailable.MakeCurrent();
            InitializeKeyboardEvents();
        }
    }


    /* 
     
    Input handler naming:

    Con - means the event is continously ran even if no input is detected
    Btn - means the event value is a type of Button, and will only return a bool
    Vec - means the event value is a type of Axis, and will only return a vector
    Once - means the event is ran once at the time of the input being true

     */

    //all input events for gamepad go here
    public void InitializeGamepadEvents()
    {
        List<CustomInputEvent> gamepadEvents = new();

        //movement
        CustomInputEvent GamepadMove = new()
        {
            actionName = "GamepadMove",
            performed = (values) => print("[GamepadMove] Received Input: " + values)
        };
        gamepadEvents.Add(GamepadMove);


        AddCustomInputEvents(gamepadEvents);
    }

    //all input events for keyboard/mouse go here
    public void InitializeKeyboardEvents()
    {
        List<CustomInputEvent> keyboardEvents = new();

        //movement
        CustomInputEvent KeyboardMove = new()
        {
            actionName = "KeyboardMove",
            performed = (values) => print("[KeybaordMove] Received Input: " + values)
        };
        keyboardEvents.Add(KeyboardMove);


        AddCustomInputEvents(keyboardEvents);
    }

}
