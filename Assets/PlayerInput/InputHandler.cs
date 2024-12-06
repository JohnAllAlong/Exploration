using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomInput.Debug;
using CustomInput.Events;

public class InputHandler : CustomInputEventManager
{
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private GoGoGadgetGun _goGoGadgetGun;
    [SerializeField] private PlayerCollectibleController _collectibleController;
    [SerializeField] private InventoryCanvasRenderer _collectibleRenderer;
    [SerializeField] private PlayerFlip _flipper;
    [SerializeField] private UIMenuController _menu;

    public static bool isInteractingKeyboard;
    public static bool isInteractingGamepad;
    public static Action<ReturnData> OnceBtnOnInteraction = (i) => { };

    protected override void OnEnable()
    {
        base.OnEnable();
        //set onDeviceChange
        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    protected void Awake()
    {
        InitGamepadEvents();
        InitKeyboardEvents();
    }

    protected void OnDisable()
    {
        //de-set onDeviceChange
        InputSystem.onDeviceChange -= OnDeviceChanged;
        OnceBtnOnInteraction = default;
        RemoveAllEvents();
    }

    private void InteractionEventHandlerGamepad(ReturnData input)
    {
        if (currentDevice as Gamepad == null) return;

        if (input.pressed)
        {
            isInteractingGamepad = true;
        }
        else
        {
            isInteractingGamepad = false;
        }

        OnceBtnOnInteraction(input);
    }

    private void InteractionEventHandlerKeyboard(ReturnData input)
    {
        if (currentDevice as Keyboard == null) return;

        if (input.pressed)
        {
            isInteractingKeyboard = true;
        }
        else
        {
            isInteractingKeyboard = false;
        }

        OnceBtnOnInteraction(input);
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

    Recommended Input manager method naming:

    Con - means the event is continously ran even if no input is detected
    Btn - means the event value is a type of Button, and will only return a bool
    Axis - means the event value is a type of Axis, and will only return a vector
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
            eventData = new()
            {
                actionName = "GamepadMove",
            },
            performed = _playerMove.AxisGamepadMove,
            canceled = _playerMove.AxisMovementCancled
        };
        gamepadEvents.Add(gamepadMove);

        CustomInputEvent GamepadFire = new()
        {
            eventData = new()
            {
                actionName = "GamepadFire",
                modifier = new()
                {
                    isButton = true,
                    once = true,
                }
            },
            performed = _goGoGadgetGun.OnceBtnGamepadFire,
        };
        gamepadEvents.Add(GamepadFire);

        CustomInputEvent GamepadAim = new()
        {
            eventData = new()
            {
                actionName = "GamepadAim",
            },
            performed = _goGoGadgetGun.AxisGamepadAim,
        };
        gamepadEvents.Add(GamepadAim);

        CustomInputEvent GamepadLeftRight = new()
        {
            eventData = new()
            {
                actionName = "GamepadLR",
                modifier = new()
                {
                    once = true
                }
            },
            performed = _flipper.OnceAxisLeftRight,
        };
        gamepadEvents.Add(GamepadLeftRight);

        CustomInputEvent GamepadInteraction = new()
        {
            eventData = new()
            {
                actionName = "GamepadInteraction",
                modifier = new()
                {
                    isButton = true,
                    once = true
                }
            },
            performed = InteractionEventHandlerGamepad,
        };
        gamepadEvents.Add(GamepadInteraction);

        CustomInputEvent GamepadOpenInv = new()
        {
            eventData = new()
            {
                actionName = "GamepadOpenInv",
                modifier = new()
                {
                    isButton = true,
                    once = true
                }
            },
            performed = _collectibleController.OnceBtnOpenInv,
        };
        gamepadEvents.Add(GamepadOpenInv);

        CustomInputEvent GamepadDropCollectible = new()
        {
            eventData = new()
            {
                actionName = "DropCollectibleGamepad",
                modifier = new()
                {
                    isButton = true,
                    once = true
                }
            },
            performed = _collectibleRenderer.OnceBtnDropCollectible,
        };
        gamepadEvents.Add(GamepadDropCollectible);

        if (_menu != null)
        {
            CustomInputEvent GamepadPause = new()
            {
                eventData = new()
                {
                    actionName = "GamepadPause",
                    modifier = new()
                    {
                        isButton = true,
                        once = true
                    }
                },
                performed = _menu.OnceBtnPause,
            };
            gamepadEvents.Add(GamepadPause);
        }

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
            eventData = new()
            {
                actionName = "KeyboardMove",
            },
            performed = _playerMove.AxisKeyboardMove,
            canceled = _playerMove.AxisMovementCancled
        };

        keyboardEvents.Add(keyboardMove);

        CustomInputEvent MouseFire = new()
        {
            eventData = new()
            {
                actionName = "MouseFire",
                modifier = new()
                {
                    isButton = true,
                    once = true,
                }
            },
            performed = _goGoGadgetGun.OnceBtnMouseFire,

        };
        keyboardEvents.Add(MouseFire);

        CustomInputEvent KeyboardLeftRight = new()
        {
            eventData = new()
            {
                actionName = "KeyboardLR",
                modifier = new()
                {
                    once = true
                }
            },
            performed = _flipper.OnceAxisLeftRight,
        };
        keyboardEvents.Add(KeyboardLeftRight);

        CustomInputEvent KeyboardInteraction = new()
        {
            eventData = new()
            {
                actionName = "KeyboardInteraction",
                modifier = new()
                {
                    isButton = true,
                    once = true
                }
            },
            performed = InteractionEventHandlerKeyboard,

        };
        keyboardEvents.Add(KeyboardInteraction);

        CustomInputEvent KeyboardOpenInv = new()
        {
            eventData = new()
            {
                actionName = "KeyboardOpenInv",
                modifier = new()
                {
                    isButton = true,
                    once = true
                }
            },
            performed = _collectibleController.OnceBtnOpenInv,
        };
        keyboardEvents.Add(KeyboardOpenInv);

        CustomInputEvent KeyboardDropCollectible = new()
        {
            eventData = new()
            {
                actionName = "DropCollectibleKeyboard",
                modifier = new()
                {
                    isButton = true,
                    once = true
                }
            },
            performed = _collectibleRenderer.OnceBtnDropCollectible,
        };
        keyboardEvents.Add(KeyboardDropCollectible);

        if (_menu != null)
        {
            CustomInputEvent KeyboardPause = new()
            {
                eventData = new()
                {
                    actionName = "KeyboardPause",
                    modifier = new()
                    {
                        isButton = true,
                        once = true
                    }
                },
                performed = _menu.OnceBtnPause,
            };
            keyboardEvents.Add(KeyboardPause);
        }

        AddCustomInputEvents(keyboardEvents, this);
        Debugger.Print($"<color=#03d7fc>[InputHandler]</color>\n<color=#aaff00>Loaded keyboard events!</color>");

    }
}
