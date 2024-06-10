using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNumberPool : MonoBehaviour
{
    public FloatingNumber FloatingNumber;
    public float NextTime; //�C�X��ͤ@��
    public float Height;
    public float ShowTime; //��ܴX��

    private bool _isLock = false;
    private Transform _anchor;
    private Timer _unlockTimer = new Timer();
    private Timer _recycleTimer = new Timer();
    private Queue<FloatingNumber> _poolQueue = new Queue<FloatingNumber>();
    //private Queue<FloatingNumberData> _dataQueue = new Queue<FloatingNumberData>();
    private Queue<Battle.Log> _logQueue = new Queue<Battle.Log>();

    public void SetAnchor(Transform anchor)
    {
        _anchor = anchor;
    }

    /*public void Play(string text, FloatingNumberData.TypeEnum type)
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

            _unlockTimer.Start(NextTime, () => //��ܤU�@�ӼƦr
            {
                _isLock = false;
                if (_dataQueue.Count > 0)
                {
                    FloatingNumberData data = _dataQueue.Dequeue();
                    Play(data.Text, data.Type);
                }
            });

            _recycleTimer.Start(ShowTime, () => //��e���Ʀr����
            {
                _poolQueue.Enqueue(floatingNumber);
            });
        }
        else
        {
            FloatingNumberData data = new FloatingNumberData(type, text);
            _dataQueue.Enqueue(data);
        }
    }*/

    public void Play(Queue<Battle.Log> logQueue)
    {
        if (logQueue != null && logQueue.Count > 0)
        {
            FloatingNumber floatingNumber;
            if (_poolQueue.Count == 0)
            {
                floatingNumber = Instantiate(FloatingNumber);
                floatingNumber.transform.SetParent(transform);
                _poolQueue.Enqueue(floatingNumber);
            }
            floatingNumber = _poolQueue.Dequeue();
            floatingNumber.Play(logQueue.Dequeue(), transform.position);

            _unlockTimer.Start(NextTime, () => //��ܤU�@�ӼƦr
            {
                Play(logQueue);
            });

            _recycleTimer.Start(ShowTime, () => //��e���Ʀr����
            {
                _poolQueue.Enqueue(floatingNumber);
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
