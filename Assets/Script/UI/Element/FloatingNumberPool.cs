using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class FloatingNumberPool : MonoBehaviour
{
    public FloatingNumber FloatingNumber;
    public float NextTime; //每幾秒生一個
    public float Height;
    public float ShowTime; //顯示幾秒

    private bool _lock = false;
    private Transform _anchor;
    private Timer _showTimer = new Timer();
    private Timer _recycleTimer = new Timer();
    private Queue<FloatingNumber> _objectPool = new Queue<FloatingNumber>();
    private Queue<Log> _logQueue = new Queue<Log>();

    public void SetAnchor(Transform anchor)
    {
        _anchor = anchor;
    }

    /*public void Play(Queue<Log> logQueue)
    {
        if (logQueue != null && logQueue.Count > 0)
        {
            FloatingNumber floatingNumber;
            if (_floatingNumberQueue.Count == 0)
            {
                floatingNumber = Instantiate(FloatingNumber);
                floatingNumber.transform.SetParent(transform);
                _floatingNumberQueue.Enqueue(floatingNumber);
            }
            floatingNumber = _floatingNumberQueue.Dequeue();
            floatingNumber.Play(logQueue.Dequeue(), transform.position);

            _showTimer.Start(NextTime, () => //顯示下一個數字
            {
                Play(logQueue);
            });

            _recycleTimer.Start(ShowTime, () => //當前的數字消失
            {
                _floatingNumberQueue.Enqueue(floatingNumber);
            });
        }
    }*/

    public void Play(Log log)
    {
        FloatingNumber floatingNumber;

        if (_objectPool.Count == 0)
        {
            InitObjectPool();
        }

        if(!_lock) 
        {
            _lock = true;
            floatingNumber = _objectPool.Dequeue();
            floatingNumber.Play(ShowTime, transform.position, log);

            _showTimer.Start(NextTime, () => //顯示下一個數字
            {
                _lock = false;
                if (_logQueue.Count > 0)
                {
                    Play(_logQueue.Dequeue());
                }
            });

            _recycleTimer.Start(ShowTime, () => //當前的數字消失
            {
                _objectPool.Enqueue(floatingNumber);
            });
        }
        else
        {
            _logQueue.Enqueue(log);
        }
    }

    private void InitObjectPool() 
    {
        int count = (int)Math.Ceiling(ShowTime / NextTime);
        FloatingNumber floatingNumber;
        for (int i=0; i<count; i++) 
        {
            floatingNumber = Instantiate(FloatingNumber);
            floatingNumber.transform.SetParent(transform);
            _objectPool.Enqueue(floatingNumber);
        }
    }

    void Awake()
    {

    }

    void Update()
    {
        if (_anchor != null)
        {
            this.transform.position = Camera.main.WorldToScreenPoint(_anchor.position);
        }
    }
}
