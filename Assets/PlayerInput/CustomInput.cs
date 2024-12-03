/* 
       Aiden C. Desjarlais
  Custom Input Events Framework V4
         "CustomInput"

Allows easy single-player setup for controllers and keyboards/mouses
 */


/* 

Recommended Input manager method naming:

Con - means the event is continously ran even if no input is detected
Btn - means the event value is a type of Button, and will only return a bool
Axis - means the event value is a type of Axis, and will only return a vector
Once - means the event is ran once at the time of the input being true

 */

using System;
using System.Collections.Generic;
using CustomInput.Debug;
using UnityEngine;
using UnityEngine.InputSystem;
using static CustomInput.Events.CustomInputEventManager;

namespace CustomInput.Debug
{
    public static class Debugger
    {
        public static bool debugInput = false;
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
        public static Mouse GetRawMouse()
        {
            return Mouse.current;
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
        public static Keyboard GetRawKeyboard()
        {
            return Keyboard.current;
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
        public static Gamepad GetRawGamepad()
        {
            return Gamepad.current;
        }

        /// <summary>
        /// Gets the current input device
        /// </summary>
        /// <returns>the current input device</returns>
        public static InputDevice GetCurrent()
        {

            if (GetRawGamepad() != null) return GetRawGamepad();
            else return GetKeyboard();
        }
    }
}


namespace CustomInput.Events
{
    /// <summary>
    /// The class used to store input values for Buttons and Axis
    /// </summary>
    public class ReturnData
    {
        /// <summary>
        /// Vector from input (if input is axis)
        /// </summary>
        public Vector2 axis = Vector2.zero;
        /// <summary>
        /// bool from input (if input is button)
        /// </summary>
        public bool pressed = false;

        public CustomInputEventData eventData;
    }

    /// <summary>
    /// Class used to interact with existing events
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Fetches an events data
        /// </summary>
        /// <param name="actionName"></param>
        public static CustomInputEventData Fetch(string actionName)
        {
            return singleton.GetCustomEventData(actionName);
        }

        /// <summary>
        /// pauses an action
        /// </summary>
        /// <param name="actionName">the action to pause</param>
        public static void Pause(string actionName)
        {
            Fetch(actionName).runEvent = false;
        }

        /// <summary>
        /// resumes an action
        /// </summary>
        /// <param name="actionName">the action to resume</param>
        public static void Resume(string actionName)
        {
            Fetch(actionName).runEvent = true;
        }

        /// <summary>
        /// pauses multiple actions
        /// </summary>
        /// <param name="actionName">actions to pause</param>
        public static void Pause(params string[] actionName)
        {
            foreach (string name in actionName)
            {
                Fetch(name).runEvent = false;
            }
        }

        /// <summary>
        /// resumes multiple actions
        /// </summary>
        /// <param name="actionName">actions to resume</param>
        public static void Resume(params string[] actionName)
        {
            foreach (string name in actionName)
            {
                Fetch(name).runEvent = true;
            }
        }

        /// <summary>
        /// checks if an event is paused
        /// </summary>
        /// <param name="actionName">actions to check</param>
        /// <returns>A list of bools in order of each provided actions, true/false if pausedk</returns>
        public static List<bool> IsPaused(params string[] actionName)
        {
            List<bool> checks = new();

            foreach (string name in actionName)
            {
                //reverse because we want true if paused
                checks.Add(!Fetch(name).runEvent);
            }

            return checks;
        }

        /// <summary>
        /// checks if an event is paused
        /// </summary>
        /// <param name="actionName">action to check</param>
        /// <returns>A bool, which is true if paused</returns>
        public static bool IsPaused(string actionName)
        {
            return Fetch(actionName).runEvent;
        }
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
    }

    /// <summary>
    /// Extension class to create a custom input event manager
    /// </summary>
    public class CustomInputEventManager : MonoBehaviour
    {
        public static CustomInputEventManager singleton;

        [SerializeField] private PlayerInput _input;

        //custom events (main way of input)
        [SerializeField]
        protected List<CustomInputEvent> CustomInputEvents = new();

        //set singleton
        protected virtual void Awake()
        {
            singleton = this;
        }

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

        public CustomInputEventData GetCustomEventData(string actionName)
        {
            return CustomInputEvents.Find(e => e.eventData.actionName == actionName).eventData;
        }

        [Serializable]
        public class CustomInputEventData
        {

            /// <summary>
            /// if true runs the event
            /// </summary>
            public bool runEvent = true;

            /// <summary>
            /// The owner of this event
            /// </summary>
            public CustomInputEventManager owner;

            /// <summary>
            /// The name of the action to listen to
            /// </summary>
            public string actionName;

            /// <summary>
            /// the input action found from actionName (set after initialization)
            /// </summary>
            public InputAction action;

            /// <summary>
            /// true if the action is performing
            /// </summary>
            public bool isPerforming;

            /// <summary>
            /// true if this event has ran once, and is awaitng to be canceled <br></br> (only used if once is true)
            /// </summary>
            public bool hasRanOnce = false;

            /// <summary>
            /// the custom modifier for this input event (all are false by default)<br></br>if no modifier is set, only a vector will be returned.
            /// </summary>
            public CustomEventModifiers modifier = new();
        }

        /// <summary>
        /// class to create custom input events with the Unity InputSystem
        /// </summary>
        [Serializable]
        protected class CustomInputEvent
        {

            /// <summary>
            /// The modifiable data for this event
            /// </summary>
            public CustomInputEventData eventData = new();

            /// <summary>
            /// action to run when input hasb ecome active.
            /// <br></br>(Called every frame the input is active if no modifiers are set)
            /// </summary>
            public Action<ReturnData> performed = (a) => { };
            /// <summary>
            /// action to run when the input has stopped
            /// </summary>
            public Action<ReturnData> canceled = (a) => { };

            /// <summary>
            /// the last input callback from the InputEvent system
            /// </summary>
            public InputAction.CallbackContext lastCallback;


            /// <summary>
            /// The base callback for every performed action<br></br>(called before the "performed" custom event)
            /// </summary>
            /// <param name="callback">callback from the input action event</param>
            public void GetPerformedCallback(InputAction.CallbackContext callback)
            {
                lastCallback = callback;
                eventData.isPerforming = true;
            }

            /// <summary>
            /// The base callback for every canceled action<br></br>(called before the "canceled" custom event)
            /// </summary>
            /// <param name="callback">callback from the input action event</param>
            public void StopPerforming(InputAction.CallbackContext callback)
            {
                if (eventData.modifier.once)
                    eventData.hasRanOnce = false;

                //send cancled input
                eventData.isPerforming = false;
                ReturnData input = new();

                if (eventData.modifier.isButton)
                {
                    input.pressed = callback.ReadValueAsButton();
                }
                else
                {
                    input.axis = callback.ReadValue<Vector2>();
                }

                input.eventData = eventData;
                canceled.Invoke(input);
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
                @event.eventData.owner = owner;
                @event.eventData.action = GetAction(@event.eventData.actionName);
                @event.eventData.action.performed += @event.GetPerformedCallback;
                @event.eventData.action.canceled += @event.StopPerforming;
                //@event.action.performed += @event.performed;
                //print("Registerd input event: " + @event.action.name);
                Debugger.Print($"<size=14><color=#03d7fc>[CustomInputEventManager]</color>\n<color=#03fc4e>Registerd input event: </color><color=#e3fc03>" + @event.eventData.actionName + "</color></size>");

                CustomInputEvents.Add(@event);
            }
        }

        protected void RemoveAllEvents()
        {
            Debugger.Print($"<color=#03d7fc>[CustomInputEventManager]</color>\n<color=#fc033d>Removed all input events</color>");
            foreach (CustomInputEvent @event in CustomInputEvents)
            {
                //@event.action.performed -= @event.GetPerformedCallback;
                @event.eventData.action.canceled -= @event.StopPerforming;
            }
            CustomInputEvents.Clear();
        }


        protected void Update()
        {
            foreach (CustomInputEvent @event in CustomInputEvents)
            {
                if (@event.eventData.runEvent)
                {
                    if (@event.eventData.isPerforming || @event.eventData.modifier.continuous)
                    {
                        //run only once
                        if (@event.eventData.modifier.once)
                            if (@event.eventData.hasRanOnce) return;
                            else @event.eventData.hasRanOnce = true;

                        ReturnData input = new();

                        if (@event.eventData.modifier.isButton)
                        {
                            input.pressed = @event.lastCallback.ReadValueAsButton();
                        }
                        else
                        {
                            input.axis = @event.lastCallback.ReadValue<Vector2>();
                        }

                        @event.eventData.owner.OnAnyCustomInput(@event);
                        @event.performed.Invoke(input);
                    }
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

