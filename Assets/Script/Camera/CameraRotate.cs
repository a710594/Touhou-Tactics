using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotate : MonoBehaviour
{
    public Action<Vector3> RotateHandler;

    public enum StateEnum
    {
        Slope,
        Vertical
    }

    public StateEnum CurrentState = StateEnum.Slope;

    private bool _enable = true;
    private Timer _timer = new Timer();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_enable && Input.GetKeyDown(KeyCode.R)) 
        {
            _enable = false;
            _timer.Start(1, ()=> 
            {
                _enable = true;
            });

            if (CurrentState == StateEnum.Slope)
            {
                transform.DORotate(new Vector3(90, 0, 0), 1f);
                transform.DOMove(transform.position + new Vector3(10, 0, 10), 1f);
                CurrentState = StateEnum.Vertical;

                if(RotateHandler != null) 
                {
                    RotateHandler(new Vector3(90, 0, 0));
                }
            }
            else
            {
                transform.DORotate(new Vector3(30, 45, 0), 1f);
                transform.DOMove(transform.position + new Vector3(-10, 0, -10), 1f);
                CurrentState = StateEnum.Slope;

                if (RotateHandler != null)
                {
                    RotateHandler(new Vector3(30, 45, 0));
                }
            }
        }
    }
}
