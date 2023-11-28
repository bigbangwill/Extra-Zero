using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public struct GameTime
{
    public int minute, hour, day;
    public GameTime(int _Minute, int _Hour, int _Day)
    {
        minute = _Minute;
        hour = _Hour;
        day = _Day;
    }

    public GameTime CurrentTime()
    {
        float second = EventManager.Instance._Seconds;
        int minute = EventManager.Instance._Minutes;
        int hour = EventManager.Instance._Hours;
        int day = EventManager.Instance._Days;
        return new GameTime(minute,hour,day);
    }

    public GameTime CurrentTimeMinusSavedTime(GameTime savedTime)
    {
        int difMinutes, difHours, difDays;
        difDays = CurrentTime().day - savedTime.day;
        difHours = CurrentTime().hour - savedTime.hour;
        difMinutes = CurrentTime().minute - savedTime.minute;
        return new GameTime(difMinutes, difHours, difDays);
    }

    public float RawTimeCurrentMinusSaved(GameTime savedTime)
    {
        int difMinutes, difHours, difDays;
        difDays = CurrentTime().day - savedTime.day;
        difHours = CurrentTime().hour - savedTime.hour;
        difMinutes = CurrentTime().minute - savedTime.minute;
        Debug.Log(difMinutes + " min " + difHours + " hour " + difDays + " day");

        difHours += difDays * 24;
        difMinutes += difHours * 60;
        return difMinutes;
    }
}

public class EventManager : SingletonComponent<EventManager>
{
    #region Singleton
    public static EventManager Instance
    {
        get { return ((EventManager)_Instance); }
        set { _Instance = value; }
    }
    #endregion

    public UnityEvent _PauseEvent;
    public UnityEvent _ResumeEvent;
    public static event Action _RefreshInventoryEvent;

    // For the time system event handler
    private int secondElapsedEventCurrentListeners;

    public static event Action _SecondElapsed;

    // For time system.
    private float seconds;
    private int minutes;
    private int hour;
    private int day;

    public float _Seconds { get { return seconds; } }
    public int _Minutes { get { return minutes; } }
    public int _Hours { get { return hour; } }
    public int _Days { get { return day; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _PauseEvent = new();
        _ResumeEvent = new();
    }

    private void Update()
    {
        seconds += Time.deltaTime;
        TimeHandler();
    }


    public void RefreshUIAddListener(Action action)
    {
        if (_RefreshInventoryEvent != null)
        {
            foreach (Delegate d in _RefreshInventoryEvent.GetInvocationList())
            {
                if ((Delegate)action == d)
                {
                    Debug.Log("AlreadyExisted");
                    return;
                }
            }
        }
        _RefreshInventoryEvent += action;
    }

    public void RefreshUIRemoveListener(Action action)
    {
        _RefreshInventoryEvent -= action;
    }



    // MAKE SURE TO ADD PAUSE TO THE TIMER.
    /// <summary>
    /// to add a method to put as a listner for the tick rate of the time system.
    /// </summary>
    /// <param name="action"></param>
    public void SecondsElapsedAddListener(Action action)
    {
        if(_SecondElapsed != null)
        {
            foreach (Delegate d in _SecondElapsed.GetInvocationList())
            {
                if ((Delegate)action == d)
                {
                    Debug.Log("Already existed");
                    return;
                }
            }
        }
        _SecondElapsed += action;
        Debug.Log("added listener Current listener : " + secondElapsedEventCurrentListeners);
    }

    /// <summary>
    /// To remove the added method to put as a listener.
    /// </summary>
    /// <param name="action"></param>
    public void SecondsElapsedRemoveListener(Action action)
    {
        _SecondElapsed -= action;
        Debug.Log("Remove listener Current listener : " + secondElapsedEventCurrentListeners);
    }

    public void SecondsElapsedRemoveAll()
    {
        _SecondElapsed = null;
        secondElapsedEventCurrentListeners = 0;
    }

    public void RefreshInventory()
    {
        if(_RefreshInventoryEvent != null)
            _RefreshInventoryEvent.Invoke();
        Debug.Log("Inventory Refreshed Event");
    }

    public void Pause()
    {
        _PauseEvent.Invoke();
        Debug.Log("Paused event");
    }

    public void Resume()
    {
        _ResumeEvent.Invoke();
        Debug.Log("Resume event");
    }

    private void SecondElapsed()
    {
        _SecondElapsed.Invoke();
        Debug.Log("Second Elapsed");
    }

    /// <summary>
    /// The normal tick rate of the time system
    /// </summary>
    private void TimeHandler()
    {
        if (seconds >= 1)
        {

            if (_SecondElapsed != null)
            {
                SecondElapsed();
            }
            float leftover = seconds - 1;
            seconds = 0 + leftover;
            minutes++;
            if (minutes >= 60)
            {
                hour++;
                minutes = 0;
                if(hour >= 24)
                {
                    hour = 0;
                    day++;
                }
            }
        }

    }
}