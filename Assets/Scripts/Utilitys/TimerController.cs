using System;
using System.Collections.Generic;
using UnityEngine;


//mono so we can update timers 
[ExecuteAlways]
public class TimerController : MonoBehaviour
{
    protected void Awake()
    {
        Timers.Wipe();
    }

    protected void Update()
    {
        //update timers
        Timers.UpdateTimers();
    }
}


//timers class used to manage timers as an extends.
//also for general timer management outside of this script.

/// <summary>
/// calss that manages all active timers
/// </summary>
public class Timers
{
    protected static List<Timer> _timers = new();

    /** 
    <summary>gets a timer</summary>
    */
    public static Timer GetTimer(string name)
    {
        return _timers.Find(t => t.timerName == name);
    }

    /** 
    <summary>returns all timers</summary>
     */
    public static List<Timer> GetTimers()
    {
        return _timers;
    }

    /** 
     <summary>adds a timer</summary>
     */
    public static void AddTimer(Timer timer)
    {
        _timers.Add(timer);
    }

    /** 
    <summary>wipes all timers</summary>
    */
    public static void Wipe()
    {
        _timers.Clear();
    }

    /** 
     <summary>updates timers</summary>
     */
    public static void UpdateTimers()
    {
        //update timers
        for (int i = 0; i != _timers.Count; i++)
        {

            if (_timers.Count < i)
                return;

            if (_timers[i].WillBeDestroy()) //destroy timers if they are slated for deletion
            {
                _timers.Remove(_timers[i]);
                continue;
            }
            _timers[i].UpdateTimer();
        }
    }
}


/// <summary>
/// Creates a timer when constructed. Must be started with StartTimer()
/// You can choose to identify a timer if needed, otherwise it will be anonymous
/// </summary>
public class Timer : Timers
{
    public string timerName; //name
    [SerializeField] private readonly float _time = 1f; //timer time
    [SerializeField] private float _timeElapsed = 0f; //timer elapsed
    [SerializeField] private bool _running = false; //timer is on
    [SerializeField] private bool _loop = false; //loop at end
    [SerializeField] private bool _countdown = false; //count down instead
    [SerializeField] private bool _destroyOnEnd = true; //if true will be destoryed on end (does not apply if loop is true)
    [SerializeField] private bool _destroy = false; //if true will be destoryed next frame

    private Action _runAtEnd = null; //end action
    private Action<float> _update = null; //update action

    public Timer(float time, string timerName = null)
    {
        this._time = time; //set time

        //check if timer with name already exists
        if (timerName != null)
            if (GetTimers().Find(t => t.timerName == timerName) != null)
            {
                Debug.LogWarning("[TimerController] Timer with id of " + timerName + " already exists!");
                return;
            }
        //set name and add timer
        this.timerName = timerName;
        AddTimer(this);
    }

    /// <summary>
    /// gets the time this timer is set to
    /// </summary>
    /// <returns>the time</returns>
    public float GetTime()
    {
        return _time;
    }

    /// <summary>
    /// gets the elapsed time
    /// </summary>
    /// <returns>elapsed time</returns>
    public float GetElapsedTime()
    {
        return _timeElapsed;
    }

    /// <summary>
    /// ends the timer on end by "destroy"
    /// </summary>
    /// <param name="destroy">end on end?</param>
    public Timer DestroyOnEnd(bool destroy)
    {
        _destroyOnEnd = destroy;
        return this;
    }

    /// <summary>
    /// deletes the timer within the next frame
    /// </summary>
    public void Destroy()
    {
        this._destroy = true;
    }

    /// <summary>
    /// hard-resets the timer back to defaults
    /// </summary>
    public void Reset()
    {
        _running = false;

        _timeElapsed = 0f;
        _loop = false;
        _countdown = false;
        _destroyOnEnd = true; //not processed if loop is true
        _destroy = false;
        _runAtEnd = null;
        _update = null;
    }

    /// <summary>
    /// is the timer runninng?
    /// </summary>
    /// <returns>bool</returns>
    public bool IsRunning()
    {
        return this._running;
    }

    /// <summary>
    /// true if this timer is slated to be destroyed by the next frame
    /// </summary>
    /// <returns>bool</returns>
    public bool WillBeDestroy()
    {
        return this._destroy;
    }

    /// <summary>
    /// idk man but this MAY start the timer (not started by default)
    /// </summary>
    public Timer StartTimer() //starts the timer
    {
        _running = true;
        if (_countdown) _timeElapsed = _time;
        return this;
    }

    /// <summary>
    /// updates this timers time
    /// </summary>
    public void UpdateTimer()
    {
        if (!_running) return; //timer is off


        if (_countdown)
        {

            _timeElapsed -= Time.deltaTime;
            if (_update != null) _update(_timeElapsed); //run update action with elapsed param

            if (_timeElapsed < 0)
            {
                if (!_loop)
                    _running = false; //stop

                _timeElapsed = _time; //reset

                if (_runAtEnd != null) _runAtEnd(); //run the end action
            }

        }
        else
        {

            _timeElapsed += Time.deltaTime;

            if (_update != null) _update(_timeElapsed); //run update action with elapsed param

            if (_timeElapsed > _time)
            {
                if (!_loop)
                {
                    _running = false; //stop
                    if (_destroyOnEnd)
                        _timers.Remove(this);
                }

                _timeElapsed = 0f; //reset

                if (_runAtEnd != null) _runAtEnd(); //run the end action
            }

        }
    }


    /**
     <summary>action to run every time timer has ended</summary>
     */
    public Timer OnEnd(Action action)
    {
        _runAtEnd = action;
        return this;
    }

    /**
     <summary>action to run every time timer is updated. returns elapsed time</summary>
     */
    public Timer OnUpdate(Action<float> action)
    {
        _update = action;
        return this;
    }

    /**
    <summary>if true it loops the timer when it ends</summary>
    */
    public Timer Loop(bool loop)
    {
        this._loop = loop;
        return this;
    }

    /**
    <summary>if true timer will count down, not up</summary>
    */
    public void Countdown(bool countdown)
    {
        this._countdown = countdown;
    }
}
