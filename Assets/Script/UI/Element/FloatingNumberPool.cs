using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNumberPool : MonoBehaviour
{
    public FloatingNumber FloatingNumber;
    public float NextTime; //每幾秒生一個
    public float Height;
    public float ShowTime; //顯示幾秒

    private Transform _anchor;
    private Timer _showTimer = new Timer();
    private Timer _recycleTimer = new Timer();
    private Queue<FloatingNumber> _floatingNumberQueue = new Queue<FloatingNumber>();

    public void SetAnchor(Transform anchor)
    {
        _anchor = anchor;
    }

    public void Play(Queue<Battle.Log> logQueue)
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
