using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataContext;

public class test : MonoBehaviour
{
    public enum ModeEnum 
    {
        Rotate,
        Move
    }

    public ModeEnum CurrentMode;
    public float speed;
    public GameObject cameraObj;
    public GameObject myGameObj;


    private float _mouseX;
    private float _mouseY;

    void Update()
	{
        if (CurrentMode == ModeEnum.Rotate)
        {
            if (Input.GetMouseButton(0))
            {
                _mouseX = Input.GetAxis("Mouse X");
                cameraObj.transform.RotateAround(myGameObj.transform.position, Vector3.up, -_mouseX * speed);
                Debug.Log(_mouseX);
            }
            if (Input.GetMouseButton(1))
            {
                _mouseY = Input.GetAxis("Mouse Y");
                if (cameraObj.transform.eulerAngles.x - _mouseY * speed < 90 && cameraObj.transform.eulerAngles.x - _mouseY * speed > 0)
                {
                    cameraObj.transform.RotateAround(myGameObj.transform.position, cameraObj.transform.right, -_mouseY * speed);
                    Debug.Log(_mouseY);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                _mouseX = Input.GetAxis("Mouse X");
                cameraObj.transform.position = cameraObj.transform.position + cameraObj.transform.right * _mouseX;
            }
            if (Input.GetMouseButton(1))
            {
                _mouseY = Input.GetAxis("Mouse Y");
                cameraObj.transform.position = cameraObj.transform.position + new Vector3(cameraObj.transform.forward.x, 0, cameraObj.transform.forward.z) * _mouseY;
            }
        }

        Camera.main.orthographicSize += Input.mouseScrollDelta.y;

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if(CurrentMode == ModeEnum.Move) 
            {
                CurrentMode = ModeEnum.Rotate;
            }
            else
            {
                CurrentMode = ModeEnum.Move;
            }
        }
    }
}
