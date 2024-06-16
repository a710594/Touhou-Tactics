using Battle;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragCameraUI : MonoBehaviour
{
    public float CameraDragSpeed = 100;
    public ButtonPlus BackgroundButton;
    public Text Label;
    public Transform CameraRoot;

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
            float angle = CameraRoot.eulerAngles.y;
            Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
            Vector3 v2;
            Vector3 v3;
            if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
            {
                if (Math.Abs(angle - 0) < 1)
                {
                    v2 = new Vector3(v1.x + v1.y, 0, -v1.x + v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    //x = Camera.main.transform.position.x + v3.x;
                    //z = Camera.main.transform.position.z + v3.z;
                }
                else if (Math.Abs(angle - 90) < 1)
                {
                    v2 = new Vector3(-v1.x + v1.y, 0, -v1.x - v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    //x = Camera.main.transform.position.x + v3.x;
                    //z = Camera.main.transform.position.z + v3.z;
                }
                else if (Math.Abs(angle - 180) < 1) 
                {
                    v2 = new Vector3(-v1.x - v1.y, 0, v1.x - v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    //x = Camera.main.transform.position.x + v3.x;
                    //z = Camera.main.transform.position.z + v3.z;
                }
                else
                {
                    v2 = new Vector3(v1.x - v1.y, 0, v1.x + v1.y);
                    v3 = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                    //x = Camera.main.transform.position.x + v3.x;
                    //z = Camera.main.transform.position.z + v3.z;
                }
                x = Mathf.Clamp(CameraRoot.position.x + v3.x, 0, _width);
                z = Mathf.Clamp(CameraRoot.position.z + v3.z, 0, _height);
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
                x = Mathf.Clamp(CameraRoot.position.x + v3.x, 0, _width);
                z = Mathf.Clamp(CameraRoot.position.z + v3.z, 0, _height);

            }

            CameraRoot.DOMove(new Vector3(x, CameraRoot.position.y, z), 1f);
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
            else //?N??????S???????a??
            {
                if (BattleController.Instance.Info.IsTutorial)
                {
                    return;
                }
                BattleController.Instance.Click(new Vector2Int(int.MinValue, int.MinValue));
            }
        }
    }

    private void SetLabel(int angle)
    {
        if(_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
        {
          Label.text = "A：向左旋轉 D：向右旋轉 W：向上旋轉";  
        }
        else
        {
            Label.text = "S：向下旋轉";
        }

    }

    private void Awake()
    {
        _cameraRotate = Camera.main.GetComponent<CameraRotate>();
        _cameraRotate.RotateHandler += SetLabel;

        //IdleButton.onClick.AddListener(IdleOnClick);
        BackgroundButton.DownHandler += BackgroundDown;
        BackgroundButton.UpHandler += BackgroundUp;
    }
}
