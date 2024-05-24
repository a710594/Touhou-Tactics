using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform Root;
    public Transform Main;
    public CameraRotate CameraRotate;

    private Vector3 _slopePosition = new Vector3(-10, 10, -10);
    private Vector3 _verticalPosition = new Vector3(0, 10, 0);

    public void Move(Vector3 position, Action callback)
    {
        float distance;
        distance = Vector3.Distance(Root.position, position);
        Camera.main.transform.parent.DOMove(position, 0.1f * distance).OnComplete(() =>
        {
            if (callback != null)
            {
                callback();
            }
        });
    }

    private void Update()
    {
        // if (CameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
        // {
        //     Camera.main.transform.localPosition = _slopePosition;
        // }
        // else
        // {
        //     Camera.main.transform.localPosition = _verticalPosition;
        // }
    }
}
