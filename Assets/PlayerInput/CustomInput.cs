/* 
       Aiden C. Desjarlais
  Custom Input Events Framework V3
         "CustomInput"

Allows easy single-player setup for controllers and keyboards/mouses
 */


/* 

Recommended Input manager method naming:

Con - means the event is continously ran even if no input is detected
Btn - means the event value is a type of Button, and will only return a bool
Vec - means the event value is a type of Axis, and will only return a vector
Once - means the event is ran once at the time of the input being true

 */

using System;
using System.Collections.Generic;
using CustomInput.Debug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CustomInput.Debug
{
    public static class Debugger
    {
        public static bool debugInput = true;
        public static void Print(object msg)
        {
            if (!debugInput) return;
            MonoBehaviour.print(msg);
        }
    }
}

namespace CustomInput
{

    /// <summary>
    /// class used to fetch devices
    /// </summary>
    public static class Devices
    {
        private readonly static Mouse syntheticMouseFallback = new();
        private readonly static Keyboard syntheticKeyboardFallback = new();
        private readonly static Gamepad syntheticGamepadFallback = new();

        /// <summary>
        /// gets the current mouse if available
        /// </summary>
        /// <returns>Mouse</returns>
        public static Mouse GetMouse()
        {
            if (Mouse.current == null)
            {
                Debugger.Print($"<color=#03d7fc>[Devices]</color>\n<color=#bf0060>No mouse was detected, falling back to a <color=#e3fc03>synthetic</color> mouse!</color>");
                return syntheticMouseFallback;
            }
            else
            {
                return Mouse.current;
            }
        }

        /// <summary>
        /// gets the current keyboard if available
        /// </summary>
        /// <returns>Keyboard</returns>
        public static Keyboard GetKeyboard()
        {
            if (Keyboard.current == null)
            {
                Debugger.Print($"<color=#03d7fc>[Devices]</color>\n<color=#bf0060>No keyboard was detected, falling back to a <color=#e3fc03>synthetic</color> keyboard!</color>");
                return syntheticKeyboardFallback;
            }
            else
            {
                return Keyboard.current;
            }
        }

        /// <summary>
        /// gets the current gamepad if available
        /// </summary>
        /// <returns>Gamepad</returns>
        public static Gamepad GetGamepad()
        {
            if (Gamepad.current == null)
            {
                Debugger.Print($"<color=#03d7fc>[Devices]</color>\n<color=#bf0060>No gamepad was detected, falling back to a <color=#e3fc03>synthetic</color> gamepad!</color>");
                return syntheticGamepadFallback;
            }
            else
            {
                return Gamepad.current;
            }
        }
    }
}


namespace CustomInput.Events
{
    /// <summary>
    /// The class used to store input values for Buttons and Axis
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


    /// <summary>
    /// modifiers for custom input events
    /// </summary>
    public class CustomEventModifiers
    {
        /// <summary>
        /// set this to true if the input is a button
        /// </summary>
        public bool isButton = false;

        /// <summary>
        /// if true, the event is continously ran even if no input is detected
        /// </summary>
        public bool continuous = false;

        /// <summary>
        /// if true, the event is ran once at the time of the input being true
        /// </summary>
        public bool once = false;

        /// <summary>
        /// true if this event has ran once, and is awaitng to be canceled <br></br> (only used if once is true)
        /// </summary>
        public bool hasRanOnce = false;
    }

    /// <summary>
    /// Extension class to create a custom input event manager
    /// </summary>
    [Serializable]
    public class CustomInputEventManager : MonoBehaviour
    {

        [SerializeField] private PlayerInput _input;

        /// <summary>
        /// called when any cutom registered input is performed
        /// </summary>
        protected Action<CustomInputEvent> OnAnyCustomInput = (e) => { };

        /// <summary>
        /// Called when a new device has been added
        /// </summary>
        protected virtual void DeviceConnected(InputDevice device)
        {
            Debugger.Print($"<color=#03d7fc>[CustomInputEventManager]</color>\n<color=#03fc4e>New device added: </color><color=#e3fc03>" + device.displayName + "</color>");
        }

        /// <summary>
        /// Called when a device has been removed
        /// </summary>
        protected virtual void DeviceRemoved(InputDevice device)
        {
            Debugger.Print($"<color=#03d7fc>[CustomInputEventManager]</color>\n<color=#03fc4e>Device removed: </color><color=#e3fc03>" + device.displayName + "</color>");
        }


        //custom events (main way of input)

        protected List<CustomInputEvent> CustomInputEvents = new();

        /// <summary>
        /// class to create cusotm input events with the Unity InputSystem
        /// </summary>
        protected class CustomInputEvent
        {
            /// <summary>
            /// The owner of this event
            /// </summary>
            public CustomInputEventManager owner;
            /// <summary>
            /// The name of the action to listen to
            /// </summary>
            public string actionName;
            /// <summary>
            /// action to run when input hasb ecome active.
            /// <br></br>(Called every frame the input is active if no modifiers are set)
            /// </summary>
            public Action<Values> performed = (a) => { };
            /// <summary>
            /// action to run when the input has stopped
            /// </summary>
            public Action<Values> canceled = (a) => { };
            /// <summary>
            /// the input action found from actionName (set after initialization)
            /// </summary>
            public InputAction action;
            /// <summary>
            /// the last input callback from the InputEvent system
            /// </summary>
            public InputAction.CallbackContext lastCallback;

            /// <summary>
            /// true if the action is performing
            /// </summary>
            public bool isPerforming;

            /// <summary>
            /// the custom modifier for this input event (all are false by default)<br></br>if no modifier is set, only a vector will be returned.
            /// </summary>
            public CustomEventModifiers modifier = new();

            /// <summary>
            /// The base callback for every performed action<br></br>(called before the "performed" custom event)
            /// </summary>
            /// <param name="callback">callback from the input action event</param>
            public void GetPerformedCallback(InputAction.CallbackContext callback)
            {
                lastCallback = callback;
                isPerforming = true;
            }

            /// <summary>
            /// The base callback for every canceled action<br></br>(called before the "canceled" custom event)
            /// </summary>
            /// <param name="callback">callback from the input action event</param>
            public void StopPerforming(InputAction.CallbackContext callback)
            {
                if (modifier.once)
                    modifier.hasRanOnce = false;

                isPerforming = false;
                Values inputValues = new();

                if (modifier.isButton)
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

        /// <summary>
        /// Adds a list of custom input events to the InputEvent system
        /// </summary>
        /// <param name="events">list of custom input events</param>
        protected void AddCustomInputEvents(List<CustomInputEvent> events, CustomInputEventManager owner)
        {
            foreach (CustomInputEvent @event in events)
            {
                @event.owner = owner;
                @event.action = GetAction(@event.actionName);
                @event.action.performed += @event.GetPerformedCallback;
                @event.action.canceled += @event.StopPerforming;
                //@event.action.performed += @event.performed;
                //print("Registerd input event: " + @event.action.name);
                Debugger.Print($"<size=14><color=#03d7fc>[CustomInputEventManager]</color>\n<color=#03fc4e>Registerd input event: </color><color=#e3fc03>" + @event.actionName + "</color></size>");

                CustomInputEvents.Add(@event);
            }
        }

        protected void RemoveAllEvents()
        {
            Debugger.Print($"<color=#03d7fc>[CustomInputEventManager]</color>\n<color=#fc033d>Removed all input events</color>");
            foreach (CustomInputEvent @event in CustomInputEvents)
            {
                //@event.action.performed -= @event.GetPerformedCallback;
                @event.action.canceled -= @event.StopPerforming;
            }
            CustomInputEvents.Clear();
        }


        protected void Update()
        {
            foreach (CustomInputEvent @event in CustomInputEvents)
            {
                if (@event.isPerforming || @event.modifier.continuous)
                {
                    //run only once
                    if (@event.modifier.once)
                        if (@event.modifier.hasRanOnce) return;
                        else @event.modifier.hasRanOnce = true;

                    Values inputValues = new();

                    if (@event.modifier.isButton)
                    {
                        inputValues.pressed = @event.lastCallback.ReadValueAsButton();
                    }
                    else
                    {
                        inputValues.vector = @event.lastCallback.ReadValue<Vector2>();
                    }

                    @event.owner.OnAnyCustomInput(@event);
                    @event.performed.Invoke(inputValues);

                }
            }
        }



        /// <summary>
        /// ran when the input device has been changed. You'll likley want to call base if overriding.
        /// </summary>
        /// <param name="device">device that has changed</param>
        /// <param name="change">change type of device</param>
        protected virtual void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            Debugger.Print($"<color=#03d7fc>[CustomInputEventManager]</color>\n<color=#03fc4e>Device changed: </color><color=#e3fc03>" + device.displayName + "</color>\nChange: <color=#fc9d03>" + change + "</color>");
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
        /// Gets an action from the PlayerInput
        /// </summary>
        /// <typeparam name="T">action return type struct</typeparam>
        /// <param name="action">action name</param>
        /// <returns>action</returns>
        protected InputAction GetAction(string action) => _input.actions.FindAction(action);

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
}

