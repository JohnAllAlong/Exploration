using CustomInput.Debug;
using CustomInput.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class InputHandler : CustomInputEventManager
{
    private InputDevice currentDevice;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private GoGoGadgetGun _goGoGadgetGun;
    [SerializeField] private PlayerCollectibleController _collectibleController;
    [SerializeField] private PlayerFlip _flipper;
    public static System.Action<Values> OnceBtnOnInteractionUse;

    protected void OnEnable()
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

    protected void OnDisable()
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
    Once - means the event is ran once at the time of the input being true

     */

    //all input events for gamepad go here
    public void InitGamepadEvents()
    {
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#0dff00>Loading gamepad events</color>");
        List<CustomInputEvent> gamepadEvents = new();
        //
        CustomInputEvent gamepadMove = new()
        {
            actionName = "GamepadMove",
            performed = _playerMove.VecGamepadMove
        };
        gamepadEvents.Add(gamepadMove);

        CustomInputEvent GamepadFire = new()
        {
            actionName = "GamepadFire",
            performed = _goGoGadgetGun.OnceBtnGamepadFire,
            modifier = new()
            {
                isButton = true,
                once = true,
            }
        };
        gamepadEvents.Add(GamepadFire);

        CustomInputEvent GamepadAim = new()
        {
            actionName = "GamepadAim",
            performed = _goGoGadgetGun.VecGamepadAim,
        };
        gamepadEvents.Add(GamepadAim);

        CustomInputEvent GamepadPickupItem = new()
        {
            actionName = "GamepadPickupItem",
            performed = _collectibleController.OnceBtnPickupItem,
            modifier = new()
            {
                isButton = true,
                once = true,
            }
        };
        gamepadEvents.Add(GamepadPickupItem);

        CustomInputEvent GamepadScrollInventory = new()
        {
            actionName = "GamepadScrollInventory",
            performed = _collectibleController.OnceVecScrollInventory,
            modifier = new()
            {
                once = true
            }
        };
        gamepadEvents.Add(GamepadScrollInventory);

        CustomInputEvent GamepadDropItem = new()
        {
            actionName = "GamepadDropItem",
            performed = _collectibleController.OnceBtnDropItem,
            modifier = new()
            {
                isButton = true,
                once = true
            }
        };
        gamepadEvents.Add(GamepadDropItem);

        CustomInputEvent GamepadUseItem = new()
        {
            actionName = "GamepadUseItem",
            performed = OnceBtnOnInteractionUse,
            modifier = new()
            {
                isButton = true,
                once = true,
            }
        };
        gamepadEvents.Add(GamepadUseItem);

        CustomInputEvent GamepadLeftRight = new()
        {
            actionName = "GamepadLR",
            performed = _flipper.OnceVecLeftRight,
            modifier = new()
            {
                once = true
            }
        };
        gamepadEvents.Add(GamepadLeftRight);

        AddCustomInputEvents(gamepadEvents, this);
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#aaff00>Loaded gamepad events!</color>");
    }

    //all input events for keyboard/mouse go here
    public void InitKeyboardEvents()
    {
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#0dff00>Loading keyboard/mouse events</color>");
        List<CustomInputEvent> keyboardEvents = new();
        //
        CustomInputEvent keyboardMove = new()
        {
            actionName = "KeyboardMove",
            performed = _playerMove.VecKeyboardMove
        };
        keyboardEvents.Add(keyboardMove);

        CustomInputEvent MouseFire = new()
        {
            actionName = "MouseFire",
            performed = _goGoGadgetGun.OnceBtnMouseFire,
            modifier = new()
            {
                isButton = true,
                once = true,
            }
        };
        keyboardEvents.Add(MouseFire);

        CustomInputEvent KeyboardPickupItem = new()
        {
            actionName = "KeyboardPickupItem",
            performed = _collectibleController.OnceBtnPickupItem,
            modifier = new()
            {
                isButton = true,
                once = true,
            }
        };
        keyboardEvents.Add(KeyboardPickupItem);

        CustomInputEvent KeyboardScrollInventory = new()
        {
            actionName = "KeyboardScrollInventory",
            performed = _collectibleController.OnceVecScrollInventory,
            modifier = new()
            {
                once = true
            }
        };
        keyboardEvents.Add(KeyboardScrollInventory);

        CustomInputEvent KeyboardDropItem = new()
        {
            actionName = "KeyboardDropItem",
            performed = _collectibleController.OnceBtnDropItem,
            modifier = new()
            {
                isButton = true,
                once = true
            }
        };
        keyboardEvents.Add(KeyboardDropItem);

        CustomInputEvent KeyboardUseItem = new()
        {
            actionName = "KeyboardUseItem",
            performed = OnceBtnOnInteractionUse,
            modifier = new()
            {
                isButton = true,
                once = true
            }
        };
        keyboardEvents.Add(KeyboardUseItem);

        CustomInputEvent KeyboardLeftRight = new()
        {
            actionName = "KeyboardLR",
            performed = _flipper.OnceVecLeftRight,
            modifier = new()
            {
                once = true
            }
        };
        keyboardEvents.Add(KeyboardLeftRight);

        AddCustomInputEvents(keyboardEvents, this);
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#aaff00>Loaded keyboard events!</color>");

    }
}
