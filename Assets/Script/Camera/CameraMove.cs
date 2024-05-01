using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform Root;
    public Transform Main;

    private Vector3 _mainPosition = new Vector3(-10, 10, -10);

    public void Move(Vector3 position, Action callback)
    {
        float distance;
        distance = Vector3.Distance(Root.position, position);
        Camera.main.transform.parent.DOMove(position, 0.1f * distance).OnComplete(() =>
        {
            distance = Vector3.Distance(Main.transform.localPosition, _mainPosition);
            Camera.main.transform.DOLocalMove(_mainPosition, 0.1f * distance).OnComplete(() =>
            {
                if(callback != null)
                {
                    callback();
                }
            });
        });
    }
}
