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
    public GameObject MyGameObj;

    [NonReorderable]
    public bool Enable = true;

    private float _minX;
    private float _maxX;
    private float _minZ;
    private float _maxZ;
    private float _mouseX;
    private float _mouseY;
    private Vector3 _position;
    private Vector3 _angle;

    public void SetMyGameObj(GameObject obj)
    {
        float radiansX = transform.eulerAngles.x * Mathf.PI / 180f;
        float radiansY = transform.eulerAngles.y * Mathf.PI / 180f;
        MyGameObj = obj;
        Vector3 position = new Vector3(-Mathf.Sin(radiansY) * MathF.Cos(radiansX) * Distance + MyGameObj.transform.position.x, MathF.Sin(radiansX) * Height, -Mathf.Cos(radiansY) * MathF.Cos(radiansX) * Distance + MyGameObj.transform.position.z);
        transform.position = position;
    }

    public void SetMyGameObj(GameObject obj, Action callback) 
    {
        Enable = false;
        float radiansX = transform.eulerAngles.x * Mathf.PI / 180f;
        float radiansY = transform.eulerAngles.y * Mathf.PI / 180f;
        MyGameObj = obj;
        Vector3 position = new Vector3(-Mathf.Sin(radiansY) * MathF.Cos(radiansX) * Distance + MyGameObj.transform.position.x, MathF.Sin(radiansX) * Height, -Mathf.Cos(radiansY) * MathF.Cos(radiansX) * Distance + MyGameObj.transform.position.z);
        transform.DOMove(position, 0.5f).OnComplete(()=> 
        {
            Enable = true;
            if (callback != null) 
            {
                callback();
            }
        });
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

        /*if (Input.GetMouseButton(0))
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");
            GetMinAndMax(transform.eulerAngles);

            if(Mathf.Abs(_mouseX) > 0.1f) 
            {
                _position = transform.position - transform.right.normalized * _mouseX;
                if (_position.x > _minX && _position.x < _maxX && _position.z > _minZ && _position.z < _maxZ)
                {
                    transform.position = _position;
                }
            }

            if (Mathf.Abs(_mouseY) > 0.1f)
            {
                _position = transform.position - (new Vector3(transform.forward.x, 0, transform.forward.z)).normalized * _mouseY;
                if (_position.x > _minX && _position.x < _maxX && _position.z > _minZ && _position.z < _maxZ)
                {
                    transform.position = _position;
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            _mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(MyGameObj.transform.position, Vector3.up, -_mouseX * Speed);
        }*/
        GetMinAndMax(transform.eulerAngles);
        if (Input.GetMouseButton(2))
        {
            _mouseY = Input.GetAxis("Mouse Y");
            if (transform.eulerAngles.x - _mouseY * Speed < 89 && transform.eulerAngles.x - _mouseY * Speed > 0)
            {
                transform.RotateAround(MyGameObj.transform.position, transform.right, -_mouseY * Speed);
                _position = transform.position;
            }
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
            transform.RotateAround(MyGameObj.transform.position, Vector3.up, - Time.deltaTime * Speed * 20);
            _position = transform.position;
            CheckCameraPosition();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(MyGameObj.transform.position, Vector3.up, +Time.deltaTime * Speed * 20);
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
