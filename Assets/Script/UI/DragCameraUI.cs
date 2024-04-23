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
        Debug.Log(_width + " " + _height);
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
            float angle = Camera.main.transform.parent.eulerAngles.y;
            Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
            Vector3 v2;
            Vector3 v3;
            if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
            {
                if (Math.Abs(angle - 0) < 1)
                {
                    v2 = new Vector3(v1.x + v1.y, 0, -v1.x + v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    x = Camera.main.transform.position.x + v3.x;
                    z = Camera.main.transform.position.z + v3.z;
                    x = Mathf.Clamp(Camera.main.transform.position.x + v3.x, -10, _width - 10);
                    z = Mathf.Clamp(Camera.main.transform.position.z + v3.z, -10, _height - 10);
                }
                else if (Math.Abs(angle - 90) < 1)
                {
                    v2 = new Vector3(v1.x - v1.y, 0, v1.x + v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    x = Camera.main.transform.position.x + v3.x;
                    z = Camera.main.transform.position.z + v3.z;
                    x = Mathf.Clamp(Camera.main.transform.position.x + v3.x, -10, _width - 10);
                    z = Mathf.Clamp(Camera.main.transform.position.z + v3.z, 10, _height + 10);
                }
                else if (Math.Abs(angle - 180) < 1) 
                {
                    v2 = new Vector3(v1.x + v1.y, 0, -v1.x + v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    x = Camera.main.transform.position.x + v3.x;
                    z = Camera.main.transform.position.z + v3.z;
                    x = Mathf.Clamp(Camera.main.transform.position.x + v3.x, 10, _width + 10);
                    z = Mathf.Clamp(Camera.main.transform.position.z + v3.z, 10, _height + 10);
                }
                else
                {
                    v2 = new Vector3(v1.x - v1.y, 0, v1.x + v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    x = Camera.main.transform.position.x + v3.x;
                    z = Camera.main.transform.position.z + v3.z;
                    x = Mathf.Clamp(Camera.main.transform.position.x + v3.x, 10, _width + 10);
                    z = Mathf.Clamp(Camera.main.transform.position.z + v3.z, -10, _height - 10);
                }
            }
            else
            {
                if (Math.Abs(angle - 0) < 1)
                {
                    v3 = new Vector3(v1.x * CameraDragSpeed, 0, v1.y * CameraDragSpeed);
                }
                else if (Math.Abs(angle - 90) < 1)
                {
                    v3 = new Vector3(v1.y * CameraDragSpeed, 0, -v1.x * CameraDragSpeed);
                }
                else if (Math.Abs(angle - 180) < 1)
                {
                    v3 = new Vector3(-v1.x * CameraDragSpeed, 0, -v1.y * CameraDragSpeed);
                }
                else
                {
                    v3 = new Vector3(-v1.y * CameraDragSpeed, 0, v1.x * CameraDragSpeed);
                }
                x = Mathf.Clamp(Camera.main.transform.position.x + v3.x, 0, _width);
                z = Mathf.Clamp(Camera.main.transform.position.z + v3.z, 0, _height);

            }

            Camera.main.transform.DOMove(new Vector3(x, Camera.main.transform.position.y, z), 1f);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector2Int v2 = Utility.ConvertToVector2Int(hit.point);
                if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CheckClick(v2))
                {
                    return;
                }
                BattleController.Instance.Click(v2);
            }
            else //代表按到沒有按鍵的地方
            {
                if (BattleController.Instance.Info.IsTutorial)
                {
                    return;
                }
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
