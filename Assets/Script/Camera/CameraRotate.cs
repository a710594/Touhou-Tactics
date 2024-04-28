using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotate : MonoBehaviour
{
    public Action<StateEnum, float> RotateHandler;

    public enum StateEnum
    {
        Slope,
        Vertical
    }

    public StateEnum CurrentState = StateEnum.Slope;

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
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    _enable = false;
            //    _timer.Start(1, () =>
            //    {
            //        _enable = true;
            //    });

            //    if (CurrentState == StateEnum.Slope)
            //    {
            //        transform.DOLocalRotate(new Vector3(90, 0, 0), 1f);
            //        transform.DOLocalMove(transform.localPosition + new Vector3(10, 0, 10), 1f);
            //        CurrentState = StateEnum.Vertical;
            //    }
            //    else
            //    {
            //        transform.DOLocalRotate(new Vector3(30, 45, 0), 1f);
            //        transform.DOLocalMove(transform.localPosition + new Vector3(-10, 0, -10), 1f);
            //        CurrentState = StateEnum.Slope;
            //    }
            //    if (RotateHandler != null)
            //    {
            //        RotateHandler(CurrentState);
            //    }
            //}

            if (CurrentState == StateEnum.Slope) 
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    _enable = false;
                    _timer.Start(1, () =>
                    {
                        _enable = true;
                    });

                    float y = transform.parent.eulerAngles.y;
                    transform.DOLocalMove(new Vector3(-10, 10, -10), 1f);
                    transform.parent.DORotate(new Vector3(0, y + 90, 0), 1f);

                    if (RotateHandler != null)
                    {
                        RotateHandler(CurrentState, y + 90);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _enable = false;
                    _timer.Start(1, () =>
                    {
                        _enable = true;
                    });

                    float y = transform.parent.eulerAngles.y;
                    transform.DOLocalMove(new Vector3(-10, 10, -10), 1f);
                    transform.parent.DORotate(new Vector3(0, y - 90, 0), 1f);

                    if (RotateHandler != null)
                    {
                        RotateHandler(CurrentState, y - 90);
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
                    transform.DOLocalMove(transform.localPosition + new Vector3(10, 0, 10), 1f);
                    CurrentState = StateEnum.Vertical;

                    if (RotateHandler != null)
                    {
                        RotateHandler(CurrentState, transform.parent.eulerAngles.y);
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
                    transform.DOLocalMove(transform.localPosition + new Vector3(-10, 0, -10), 1f);
                    CurrentState = StateEnum.Slope;

                    if (RotateHandler != null)
                    {
                        RotateHandler(CurrentState, transform.parent.eulerAngles.y);
                    }
                }
            }
        }
    }
}
