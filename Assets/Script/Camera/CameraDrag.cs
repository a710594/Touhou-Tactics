using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraDrag : MonoBehaviour
{
    public float DragSpeed = 100;

    private int _width;
    private int _height;
    private float _distance = 10;
    private Vector3 _dragOrigin;

    public void SetMapInfo(int width, int height) 
    {
        _width = width;
        _height = height;
    }

    private void Update()
    {
        //if(transform.position.x > -10 && transform.position.x < _width - 10 && transform.position.z > -10 && transform.position.z < _height - 10)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        _dragOrigin = Input.mousePosition;
        //    }
        //    else if (Input.GetMouseButton(0) && Vector2.Distance(_dragOrigin, Input.mousePosition) > _distance)
        //    {
        //        Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
        //        Vector3 v2 = new Vector3(v1.x * Mathf.Sin(45) + v1.y * Mathf.Cos(45), 0, v1.x * Mathf.Sin(-45) + v1.y * Mathf.Cos(-45));
        //        Vector3 move = new Vector3(v2.x * DragSpeed, 0, v2.z * DragSpeed);

        //        transform.Translate(move, Space.World);
        //    }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && Vector2.Distance(_dragOrigin, Input.mousePosition) > _distance)
        {
            Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
            Vector3 v2 = new Vector3(v1.x * Mathf.Sin(45) + v1.y * Mathf.Cos(45), 0, v1.x * Mathf.Sin(-45) + v1.y * Mathf.Cos(-45));
            Vector3 move = new Vector3(v2.x * DragSpeed, 0, v2.z * DragSpeed);

            if (transform.position.x + move.x > -10 && transform.position.x + move.x < _width - 10 && transform.position.z + move.z > -10 && transform.position.z + move.z < _height - 10)
            {
                transform.DOMove(transform.position + move, 1f);
            }
        }
    }
}
