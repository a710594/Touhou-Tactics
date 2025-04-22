using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public enum ModeEnum 
    {
        Rotate,
        Move
    }

    public float Speed;
    public float Distance;
    public float Height;
    public Camera Camera;

    [NonSerialized]
    public bool Enable = true;
    [NonSerialized]
    public GameObject TargetObject;

    private float _minX;
    private float _maxX;
    private float _minZ;
    private float _maxZ;
    private float _mouseX;
    private float _mouseY;
    private Vector3 _position;
    private Timer _timer = new Timer();

    public void SetMyGameObj(GameObject obj, Action callback) 
    {
        Enable = false;
        float radiansX = transform.eulerAngles.x * Mathf.PI / 180f;
        float radiansY = transform.eulerAngles.y * Mathf.PI / 180f;
        TargetObject = obj;
        Vector2Int objPosition = Utility.ConvertToVector2Int(obj.transform.position);
        int objHeight = BattleController.Instance.TileDic[objPosition].TileData.Height;
        Vector3 cameraPosition = new Vector3(-Mathf.Sin(radiansY) * MathF.Cos(radiansX) * Distance + TargetObject.transform.position.x, MathF.Sin(radiansX) * Height + objHeight, -Mathf.Cos(radiansY) * MathF.Cos(radiansX) * Distance + TargetObject.transform.position.z);

        if (callback != null)
        {
            transform.DOMove(cameraPosition, 0.5f).OnComplete(() =>
            {
                _timer.Start(0.5f, ()=> 
                {
                    Enable = true;
                    if (callback != null)
                    {
                        callback();
                    }
                });
            });
        }
        else
        {
            Enable = true;
            transform.position = cameraPosition;
        }
    }

    private void GetMinAndMax(Vector3 angle) 
    {
        float radiansX = angle.x * Mathf.PI / 180f;
        float radiansY = angle.y * Mathf.PI / 180f;
        _minX = -Mathf.Sin(radiansY) * MathF.Cos(radiansX) * Distance + BattleController.Instance.MinX;
        _maxX = -Mathf.Sin(radiansY) * MathF.Cos(radiansX) * Distance + BattleController.Instance.MaxX;
        _minZ = -Mathf.Cos(radiansY) * MathF.Cos(radiansX) * Distance + BattleController.Instance.MinY;
        _maxZ = -Mathf.Cos(radiansY) * MathF.Cos(radiansX) * Distance + BattleController.Instance.MaxY;
    }

    void Update()
    {
        if (!Enable) 
        {
            return;
        }

        GetMinAndMax(transform.eulerAngles);
        if (Input.GetMouseButton(2))
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");
            if (Math.Abs(_mouseX) > Math.Abs(_mouseY)) 
            {
                transform.RotateAround(TargetObject.transform.position, Vector3.up, _mouseX * Speed);
            }
            else
            {
                if (transform.eulerAngles.x - _mouseY * Speed < 89 && transform.eulerAngles.x - _mouseY * Speed > 0)
                {
                    transform.RotateAround(TargetObject.transform.position, transform.right, -_mouseY * Speed);
                }
            }
            _position = transform.position;
            CheckCameraPosition();
        }

        if (Input.GetKey(KeyCode.A)) 
        {
            _position = transform.position - transform.right.normalized * Time.deltaTime * Speed * 5;
            if (_position.x > _minX && _position.x < _maxX && _position.z > _minZ && _position.z < _maxZ)
            {
                transform.position = _position;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            _position = transform.position + transform.right.normalized * Time.deltaTime * Speed * 5;
            if (_position.x > _minX && _position.x < _maxX && _position.z > _minZ && _position.z < _maxZ)
            {
                transform.position = _position;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            _position = transform.position - (new Vector3(transform.forward.x, 0, transform.forward.z)).normalized * Time.deltaTime * Speed * 5;
            if (_position.x > _minX && _position.x < _maxX && _position.z > _minZ && _position.z < _maxZ)
            {
                transform.position = _position;
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            _position = transform.position + (new Vector3(transform.forward.x, 0, transform.forward.z)).normalized * Time.deltaTime * Speed * 5;
            if (_position.x > _minX && _position.x < _maxX && _position.z > _minZ && _position.z < _maxZ)
            {
                transform.position = _position;
            }
        }
        if (Input.GetKey(KeyCode.E)) 
        {
            transform.RotateAround(TargetObject.transform.position, Vector3.up, -Time.deltaTime * Speed * 20);
            _position = transform.position;
            CheckCameraPosition();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(TargetObject.transform.position, Vector3.up, Time.deltaTime * Speed * 20);
            _position = transform.position;
            CheckCameraPosition();
        }
    }

    private void CheckCameraPosition() 
    {
        if (_position.x < _minX)
        {
            _position = new Vector3(_minX, _position.y, _position.z);
        }
        if (_position.x > _maxX)
        {
            _position = new Vector3(_maxX, _position.y, _position.z);
        }
        if (_position.z < _minZ)
        {
            _position = new Vector3(_position.x, _position.y, _minZ);
        }
        if (_position.z > _maxZ)
        {
            _position = new Vector3(_position.x, _position.y, _maxZ);
        }
        transform.position = _position;
    }
}
