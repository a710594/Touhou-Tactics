using Battle;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCameraUI : MonoBehaviour
{
    public float CameraDragSpeed = 100;
    public ButtonPlus BackgroundButton;

    [NonSerialized]
    public bool DontDrag = false;

    private int _width;
    private int _height;
    private Vector3 _dragOrigin;
    private CameraRotate _cameraRotate;
    private float _distance = 20;

    public void Init(int width, int height)
    {
        _width = width;
        _height = height;
    }

    private void BackgroundDown(ButtonPlus button)
    {
        _dragOrigin = Input.mousePosition;
    }

    private void BackgroundUp(ButtonPlus button)
    {
        if (DontDrag) 
        {
            DontDrag = false;
            return;
        }

        if (Vector2.Distance(_dragOrigin, Input.mousePosition) > _distance)
        {
            float x;
            float z;
            Vector3 move;
            Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
            if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
            {
                Vector3 v2 = new Vector3(v1.x * Mathf.Sin(45) + v1.y * Mathf.Cos(45), 0, v1.x * Mathf.Sin(-45) + v1.y * Mathf.Cos(-45));
                move = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                x = Mathf.Clamp(Camera.main.transform.position.x + move.x, -10, _width - 10);
                z = Mathf.Clamp(Camera.main.transform.position.z + move.z, -10, _height - 10);
            }
            else
            {
                move = new Vector3(v1.x * CameraDragSpeed, 0, v1.y * CameraDragSpeed);
                x = Mathf.Clamp(Camera.main.transform.position.x + move.x, 0, _width);
                z = Mathf.Clamp(Camera.main.transform.position.z + move.z, 0, _height);
            }

            Camera.main.transform.DOMove(new Vector3(x, Camera.main.transform.position.y, z), 1f);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                BattleController.Instance.Click(Utility.ConvertToVector2Int(hit.transform.position));
            }
            else //代表按到沒有按鍵的地方
            {
                BattleController.Instance.Click(new Vector2Int(int.MinValue, int.MinValue));
            }
        }
    }

    private void Awake()
    {
        _cameraRotate = Camera.main.GetComponent<CameraRotate>();

        //IdleButton.onClick.AddListener(IdleOnClick);
        BackgroundButton.DownHandler += BackgroundDown;
        BackgroundButton.UpHandler += BackgroundUp;
    }
}
