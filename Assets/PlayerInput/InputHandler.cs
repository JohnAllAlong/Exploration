using CustomInput.Debug;
using CustomInput.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : CustomInputEventManager
{

    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private PlayerGrapple _playerGrapple;
    private InputDevice currentDevice;

    private void OnEnable()
    {
        //set onDeviceChange
        InputSystem.onDeviceChange += OnDeviceChanged;

        //set OnAnyCustomInput
        OnAnyCustomInput += SwitchedInput;

        //set the launch input device
        currentDevice = GetNextAvailableInputDevice();

        InitGamepadEvents();
        InitKeyboardEvents();
    }

    private void OnDisable()
    {
        //de-set onDeviceChange
        InputSystem.onDeviceChange -= OnDeviceChanged;

        //de-set OnAnyCustomInput
        OnAnyCustomInput -= SwitchedInput;

        RemoveAllEvents();
    }

    private void SwitchedInput(CustomInputEvent @event)
    {
        if (@event.action.activeControl.device == currentDevice) return;

        currentDevice = @event.action.activeControl.device;
    }


    protected override void DeviceConnected(InputDevice device)
    {
        base.DeviceConnected(device);
        if (device as Gamepad != null)
        {
            device.MakeCurrent();
        }
        else if (device as Keyboard != null)
        {
            device.MakeCurrent();
        }
    }

    protected override void DeviceRemoved(InputDevice device)
    {
        base.DeviceRemoved(device);
        //attempt to switch to the next available method of input
        //(Gamepad or Keyboard)
        InputDevice nextAvailable = GetNextAvailableInputDevice();


        //re-assign input events
        if (nextAvailable as Gamepad != null)
        {
            nextAvailable.MakeCurrent();
        }
        else if (nextAvailable as Keyboard != null)
        {
            nextAvailable.MakeCurrent();
        }
    }


    /* 
     
    Input method naming:

    Con - means the event is continously ran even if no input is detected
    Btn - means the event value is a type of Button, and will only return a bool
    Vec - means the event value is a type of Axis, and will only return a vector
    Once - means the event is ran once at the timev of the input being true

     */

    //all input events for gamepad go here
    public void InitGamepadEvents()
    {
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#0dff00>Loading gamepad events</color>");
        List<CustomInputEvent> gamepadEvents = new();

        //movement (no mod)
        CustomInputEvent GamepadMove = new()
        {
            actionName = "GamepadMove",
            performed = _playerMove.VecHandleGamepadMovement,
            canceled = _playerMove.VecHandleGamepadMovement,
        };
        gamepadEvents.Add(GamepadMove);

        //jumping
        CustomInputEvent GamepadJump = new()
        {
            actionName = "GamepadJump",
            modifier = new()
            {
                isButton = true,
                once = true
            },
            performed = _playerMove.OnceBtnHandleGamepadJump
        };
        gamepadEvents.Add(GamepadJump);

        //grapple
        CustomInputEvent GamepadGrapple = new()
        {
            actionName = "GamepadGrapple",
            modifier = new()
            {
                once = true
            },
            performed = _playerGrapple.VecOnceHandleGamepadGrapple
        };
        gamepadEvents.Add(GamepadGrapple);

        //grapple shrink
        CustomInputEvent GamepadGrappleShrink = new()
        {
            actionName = "GamepadGrappleShrink",
            modifier = new()
            {
                isButton = true
            },
            performed = _playerGrapple.BtnHandleGamepadGrappleShrink
        };
        gamepadEvents.Add(GamepadGrappleShrink);

        //grapple stop (only gamepad)
        CustomInputEvent GamepadGrappleStop = new()
        {
            actionName = "GamepadGrappleStop",
            modifier = new()
            {
                isButton = true
            },
            performed = _playerGrapple.BtnHandleGamepadGrappleStop
        };
        gamepadEvents.Add(GamepadGrappleStop);

        AddCustomInputEvents(gamepadEvents, this);
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#aaff00>Loaded gamepad events!</color>");
    }

    //all input events for keyboard/mouse go here
    public void InitKeyboardEvents()
    {
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#0dff00>Loading keyboard/mouse events</color>");
        List<CustomInputEvent> keyboardEvents = new();

        //movement (no mod)
        CustomInputEvent KeyboardMove = new()
        {
            actionName = "KeyboardMove",
            performed = _playerMove.VecHandleKeyboardMovement,
            canceled = _playerMove.VecHandleKeyboardMovement
        };
        keyboardEvents.Add(KeyboardMove);

        //jumping
        CustomInputEvent KeyboardJump = new()
        {
            actionName = "KeyboardJump",
            modifier = new()
            {
                isButton = true,
                once = true
            },
            performed = _playerMove.OnceBtnHandleKeyboardJump
        };
        keyboardEvents.Add(KeyboardJump);

        //grapple
        CustomInputEvent MouseGrapple = new()
        {
            actionName = "MouseGrapple",
            modifier = new()
            {
                isButton = true,
                once = true
            },
            performed = _playerGrapple.OnceBtnHandleMouseGrapple
        };
        keyboardEvents.Add(MouseGrapple);

        //grapple shrink
        CustomInputEvent MouseGrappleShrink = new()
        {
            actionName = "MouseGrappleShrink",
            modifier = new()
            {
                isButton = true,
            },
            performed = _playerGrapple.BtnHandleMouseGrappleShrink
        };
        keyboardEvents.Add(MouseGrappleShrink);

        AddCustomInputEvents(keyboardEvents, this);
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#aaff00>Loaded keyboard events!</color>");

    }
}
