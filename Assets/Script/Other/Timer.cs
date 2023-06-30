using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private int _count;
    private float _time;
    private float _endTime = -1;
    private bool _isLoop = false;
    private Action _onTimeOutHandler = null; //每次時間到時執行
    private Action _onLastHandler = null; //最後一次時間到時執行

    public Timer() { }

    public Timer(float time, Action callback, bool isLoop = false)
    {
        _time = time;
        _count = 0;
        _endTime = Time.time + time;
        _isLoop = isLoop;
        _onTimeOutHandler = callback;

        TimerUpdater.UpdateHandler += OnUpdate;
    }

    public void Start(float time, Action callback, bool isLoop = false)
    {
        _time = time;
        _endTime = Time.time + time;
        _isLoop = isLoop;
        _onTimeOutHandler = callback;

        TimerUpdater.UpdateHandler += OnUpdate;
    }

    public void Start(float time, int count, Action timeOutCallback, Action lastCallback)
    {
        _time = time;
        _count = count;
        _endTime = Time.time + time;
        _onTimeOutHandler = timeOutCallback;
        _onLastHandler = lastCallback;

        TimerUpdater.UpdateHandler += OnUpdate;
    }

    public void Stop()
    {
        _endTime = -1;
        _isLoop = false;
        TimerUpdater.UpdateHandler -= OnUpdate;
    }

    public void StopLoop()
    {
        _endTime = -1;
        TimerUpdater.UpdateHandler -= OnUpdate;
    }

    private void OnUpdate()
    {
        if (_endTime != -1 && Time.time > _endTime)
        {
            TimerUpdater.UpdateHandler -= OnUpdate;
            _endTime = -1;
            _onTimeOutHandler();
            if (_onTimeOutHandler != null)
            {
                if (_isLoop)
                {
                    TimerUpdater.UpdateHandler += OnUpdate;
                    _endTime = Time.time + _time;
                }
                else if (_count > 1)
                {
                    TimerUpdater.UpdateHandler += OnUpdate;
                    _endTime = Time.time + _time;
                    _count--;
                }
                else
                {
                    if (_onLastHandler != null)
                    {
                        _onLastHandler();
                        _onLastHandler = null;
                    }
                }
            }
        }
    }
}