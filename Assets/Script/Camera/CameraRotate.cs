using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotate : MonoBehaviour
{
    public Action RotateHandler;

    public enum StateEnum
    {
        Slope,
        Vertical
    }

    public StateEnum CurrentState = StateEnum.Slope;

    [System.NonSerialized]
    public float Angle;

    private bool _enable = true;
    private Timer _timer = new Timer();
    private Transform _root;

    // Start is called before the first frame update
    private void Awake()
    {
        _root = GameObject.Find("BattleMapBuilder").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enable) 
        {
            if (CurrentState == StateEnum.Slope) 
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    _enable = false;
                    _timer.Start(1, () =>
                    {
                        _enable = true;
                    });

                    Angle = transform.parent.eulerAngles.y + 90;
                    transform.DOLocalMove(new Vector3(-10, 10, -10), 1f);
                    transform.parent.DORotate(new Vector3(0, Angle, 0), 1f);

                    if (RotateHandler != null)
                    {
                        RotateHandler();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _enable = false;
                    _timer.Start(1, () =>
                    {
                        _enable = true;
                    });

                    Angle = transform.parent.eulerAngles.y - 90;
                    transform.DOLocalMove(new Vector3(-10, 10, -10), 1f);
                    transform.parent.DORotate(new Vector3(0, Angle, 0), 1f);

                    if (RotateHandler != null)
                    {
                        RotateHandler();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {

                    _enable = false;
                    _timer.Start(1, () =>
                    {
                        _enable = true;
                    });

                    transform.DOLocalRotate(new Vector3(90, 0, 0), 1f);
                    Vector3 local = transform.localPosition + new Vector3(10, 0, 10);
                    transform.DOLocalMove(local, 1f);
                    CurrentState = StateEnum.Vertical;

                    if (RotateHandler != null)
                    {
                        RotateHandler();
                    }     
                }
            }
            else if (CurrentState == StateEnum.Vertical)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    _enable = false;
                    _timer.Start(1, () =>
                    {
                        _enable = true;
                    });

                    transform.DOLocalRotate(new Vector3(30, 45, 0), 1f);
                    Vector3 local = transform.localPosition + new Vector3(-10, 0, -10);
                    transform.DOLocalMove(local, 1f);
                    CurrentState = StateEnum.Slope;

                    if (RotateHandler != null)
                    {
                        RotateHandler();
                    }
                }
            }
        }
    }
}
