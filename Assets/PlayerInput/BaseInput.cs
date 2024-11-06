using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Input.Values;

namespace Input.Values
{
    /// <summary>
    /// The class used to store input valyes for Buttons and Axis
    /// </summary>
    public class Values
    {
        /// <summary>
        /// Vector from input (if input is axis)
        /// </summary>
        public Vector2 vector = Vector2.zero;
        /// <summary>
        /// bool from input (if input is button)
        /// </summary>
        public bool pressed = false;
    }
}

namespace Input
{
    /// <summary>
    /// class used to grab devices
    /// </summary>
    public static class Devices
    {
        private readonly static Mouse syntheticMouseFallback = new();
        private readonly static Keyboard syntheticKeyboardFallback = new();
        private readonly static Gamepad syntheticGamepadFallback = new();

        public static Mouse GetMouse()
        {
            if (Mouse.current == null)
            {
                Debug.LogWarning("[InputHandler@GetMouse] No mouse was detected, falling back to a synthetic mouse!");
                return syntheticMouseFallback;
            }
            else
            {
                return Mouse.current;
            }
        }

        public static Keyboard GetKeyboard()
        {
            if (Keyboard.current == null)
            {
                Debug.LogWarning("[InputHandler@GetKeyboard] No keyboard was detected, falling back to a synthetic keyboard!");
                return syntheticKeyboardFallback;
            }
            else
            {
                return Keyboard.current;
            }
        }

        public static Gamepad GetGamepad()
        {
            if (Gamepad.current == null)
            {
                Debug.LogWarning("[InputHandler@GetGamepad] No gamepad was detected, falling back to a synthetic gamepad!");
                return syntheticGamepadFallback;
            }
            else
            {
                return Gamepad.current;
            }
        }
    }
}

/// <summary>
/// Extension class for player input
/// </summary>
[Serializable]
public class InputEvents : MonoBehaviour
{
    /* 
     TODO
    Rename to InputEvents
    Make an input event handling script that inherits this class,
    and registers gamepad/keyboard input,
    and sends event updates to movement scripts

     */

    [SerializeField] private PlayerInput _input;

    /// <summary>
    /// Called when a new device has been added
    /// </summary>
    public virtual void DeviceConnected(InputDevice device)
    {

    }

    /// <summary>
    /// Called when a device has been removed
    /// </summary>
    public virtual void DeviceRemoved(InputDevice device)
    {

    }


    //custom events (main way of input)

    protected List<CustomInputEvent> CustomInputEvents = new();

    protected class CustomInputEvent
    {
        public string actionName;
        /// <summary>
        /// action to use for event
        /// </summary>
        public Action<Values> performed = (a) => { };
        public Action<Values> canceled = (a) => { };
        public InputAction action;
        public InputAction.CallbackContext lastCallback;

        /// <summary>
        /// true if the action is performing
        /// </summary>
        public bool isPerforming;

        /// <summary>
        /// set this if the input is a button
        /// </summary>
        public bool isButton = false;

        /// <summary>
        /// if true, performed will be ran every frame, even if not performed
        /// </summary>
        public bool continuous = false;

        /// <summary>
        /// if ture, will only run performed once per press
        /// </summary>
        public bool once = false;

        /// <summary>
        /// true if this event has ran once, and is awaitng to be cancled
        /// </summary>
        public bool hasRanOnce = false;

        public void GetPerformedCallback(InputAction.CallbackContext callback)
        {
            lastCallback = callback;
            isPerforming = true;
        }

        public void StopPerforming(InputAction.CallbackContext callback)
        {
            if (once)
                hasRanOnce = false;

            isPerforming = false;
            Values inputValues = new();

            if (isButton)
            {
                inputValues.pressed = callback.ReadValueAsButton();
            }
            else
            {
                inputValues.vector = callback.ReadValue<Vector2>();
            }

            canceled.Invoke(inputValues);
        }
    }

    ///<summary>please use AddCustomInputEvents instead</summary>
    [Obsolete]
    protected void AddCustomInputEvent(CustomInputEvent @event)
    {
        @event.action = GetAction(@event.actionName);
        @event.action.performed += @event.GetPerformedCallback;
        @event.action.canceled += @event.StopPerforming;
        //@event.action.performed += @event.performed;
        print("Registerd input event: " + @event.action.name);

        CustomInputEvents.Add(@event);
    }

    protected void AddCustomInputEvents(List<CustomInputEvent> events)
    {
        foreach (CustomInputEvent @event in events)
        {
            @event.action = GetAction(@event.actionName);
            @event.action.performed += @event.GetPerformedCallback;
            @event.action.canceled += @event.StopPerforming;
            //@event.action.performed += @event.performed;
            print("Registerd input event: " + @event.action.name);

            CustomInputEvents.Add(@event);
        }
    }

    protected void RemoveAllEvents()
    {
        foreach (CustomInputEvent @event in CustomInputEvents)
        {
            @event.action.performed -= @event.GetPerformedCallback;
            @event.action.canceled -= @event.StopPerforming;
        }
        CustomInputEvents.Clear();
    }


    private void Update()
    {
        foreach (CustomInputEvent @event in CustomInputEvents)
        {
            if (@event.isPerforming || @event.continuous)
            {
                //run only once
                if (@event.once)
                    if (@event.hasRanOnce) return;
                    else @event.hasRanOnce = true;

                Values inputValues = new();

                if (@event.isButton)
                {
                    inputValues.pressed = @event.lastCallback.ReadValueAsButton();
                }
                else
                {
                    inputValues.vector = @event.lastCallback.ReadValue<Vector2>();
                }

                @event.performed.Invoke(inputValues);

            }
        }
    }



    /// <summary>
    /// ran when the input device has been changed
    /// </summary>
    /// <param name="device">device changed</param>
    /// <param name="change">change of device</param>
    public void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        RemoveAllEvents();
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                DeviceRemoved(device);
                break;
            case InputDeviceChange.Removed:
                DeviceRemoved(device);
                break;
            case InputDeviceChange.Added:
                DeviceConnected(device);
                break;
            case InputDeviceChange.Reconnected:
                DeviceConnected(device);
                break;
        }
    }



    /// <summary>
    /// Gets an actions value from the PlayerInput
    /// </summary>
    /// <typeparam name="T">action return type struct</typeparam>
    /// <param name="action">action name</param>
    /// <returns>T read from Action</returns>
    protected T GetActionValue<T>(string action) where T : struct => _input.actions.FindAction(action).ReadValue<T>();


    /// <summary>
    /// Gets an action from the PlayerInput
    /// </summary>
    /// <typeparam name="T">action return type struct</typeparam>
    /// <param name="action">action name</param>
    /// <returns>action</returns>
    protected InputAction GetAction(string action) => _input.actions.FindAction(action);

    /// <summary>
    /// Gets the requested input device
    /// </summary>
    /// <typeparam name="T">input type to get</typeparam>
    /// <returns>T as InputDevice</returns>
    protected T GetInputDevice<T>() where T : InputDevice
    {
        if (typeof(T) == typeof(Mouse)) return (T)Mouse.current.device;
        if (typeof(T) == typeof(Keyboard)) return (T)Keyboard.current.device;
        if (typeof(T) == typeof(Gamepad)) return (T)Gamepad.current.device;
        return default;
    }

    /// <summary>
    /// gets the next available input device (keyboard or gamepad) <br></br>
    /// favours keyboard over gamepad.
    /// </summary>
    /// <returns>next available input device</returns>
    protected InputDevice GetNextAvailableInputDevice()
    {
        return Gamepad.current != null ? Gamepad.current.device : Keyboard.current.device;
    }
}

