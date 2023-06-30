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

    private bool _isLock = false;
    private Transform _anchor;
    private Timer _unlockTimer = new Timer();
    private Timer _recycleTimer = new Timer();
    private Queue<FloatingNumber> _poolQueue = new Queue<FloatingNumber>();
    private Queue<FloatingNumberData> _dataQueue = new Queue<FloatingNumberData>();

    public void SetAnchor(Transform anchor)
    {
        _anchor = anchor;
    }

    public void Play(string text, FloatingNumberData.TypeEnum type)
    {
        FloatingNumber floatingNumber;

        if (!_isLock)
        {
            _isLock = true;
            if (_poolQueue.Count == 0) 
            {
                floatingNumber = Instantiate(FloatingNumber);
                floatingNumber.transform.SetParent(transform);
                _poolQueue.Enqueue(floatingNumber);
            }
            floatingNumber = _poolQueue.Dequeue();
            floatingNumber.Play(text, type, transform.position);

            _unlockTimer.Start(NextTime, () => //顯示下一個數字
            {
                _isLock = false;
                if (_dataQueue.Count > 0)
                {
                    FloatingNumberData data = _dataQueue.Dequeue();
                    Play(data.Text, data.Type);
                }
            });

            _recycleTimer.Start(ShowTime, () => //當前的數字消失
            {
                _poolQueue.Enqueue(floatingNumber);
            });
        }
        else
        {
            FloatingNumberData data = new FloatingNumberData(type, text);
            _dataQueue.Enqueue(data);
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
