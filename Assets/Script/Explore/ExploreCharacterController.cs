using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Explore
{
    public class ExploreCharacterController : MonoBehaviour
    {
        public Action MoveHandler;
        public Action RotateHandler;

        public Vector3 MoveTo;
        public Vector3 RotateTo;

        private Vector3 position;
        private bool _isMoving = false;

        private void Up()
        {
            if (!_isMoving) 
            {
                position = transform.position + transform.forward;
                if (ExploreManager.Instance.IsWalkable(position))
                {
                    _isMoving = true;
                    transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        ExploreManager.Instance.CheckVidsit(transform);
                        _isMoving = false;
                    });
                    MoveTo = position;
                    if (MoveHandler != null)
                    {
                        MoveHandler();
                    }
                }
            }
        }

        private void Down()
        {
            if (!_isMoving) 
            {
                position = transform.position - transform.forward;
                if (ExploreManager.Instance.IsWalkable(position))
                {
                    _isMoving = true;
                    transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        _isMoving = false;
                        ExploreManager.Instance.CheckVidsit(transform);
                    });
                    MoveTo = position;
                    if (MoveHandler != null)
                    {
                        MoveHandler();
                    }
                }
            }
        }

        private void Left()
        {
            if (!_isMoving)
            {
                position = transform.position - transform.right;
                if (ExploreManager.Instance.IsWalkable(position))
                {
                    _isMoving = true;
                    transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        _isMoving = false;
                        ExploreManager.Instance.CheckVidsit(transform);
                    });
                    MoveTo = position;
                    if (MoveHandler != null)
                    {
                        MoveHandler();
                    }
                }
            }
        }

        private void Right()
        {
            if (!_isMoving)
            {
                position = transform.position + transform.right;
                if (ExploreManager.Instance.IsWalkable(position))
                {
                    _isMoving = true;
                    transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        _isMoving = false;
                        ExploreManager.Instance.CheckVidsit(transform);
                    });
                    MoveTo = position;
                    if (MoveHandler != null)
                    {
                        MoveHandler();
                    }
                }
            }
        }

        private void TurnLeft()
        {
            if (!_isMoving)
            {
                _isMoving = true;
                RotateTo = transform.localEulerAngles + Vector3.down * 90;
                transform.DORotate(RotateTo, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _isMoving = false;
                    ExploreManager.Instance.CheckVidsit(transform);
                });
                if (RotateHandler != null)
                {
                    RotateHandler();
                }
            }
        }

        private void TurnRight()
        {
            if (!_isMoving)
            {
                _isMoving = true;
                RotateTo = transform.localEulerAngles + Vector3.up * 90;
                transform.DORotate(RotateTo, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _isMoving = false;
                    ExploreManager.Instance.CheckVidsit(transform);
                });
                if (RotateHandler != null)
                {
                    RotateHandler();
                }
            }
        }

        private void Awake()
        {
            //InputMamager.Instance.UpHandler += Up;
            //InputMamager.Instance.DownHandler += Down;
            //InputMamager.Instance.LeftHandler += TurnLeft;
            //InputMamager.Instance.RightHandler += TurnRight;
        }

        private void OnDestroy()
        {
            //InputMamager.Instance.UpHandler -= Up;
            //InputMamager.Instance.DownHandler -= Down;
            //InputMamager.Instance.LeftHandler -= TurnLeft;
            //InputMamager.Instance.RightHandler -= TurnRight;
        }

        private void Update()
        {
            if (!InputMamager.Instance.IsLock) 
            {
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                {
                    Up();
                }
                else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    Down();
                }
                else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    TurnLeft();
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    TurnRight();
                }
                else if (Input.GetKey(KeyCode.Q)) 
                {
                    Left();
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    Right();
                }
            }
        }
    }
}