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

        public bool CanMove = true;
        public Vector3 MoveTo;
        public Vector3 RotateTo;

        private Vector3 position;
        private bool _isMoving = false;

        private void Update()
        {
            if (CanMove && !_isMoving)
            {
                if (Input.GetKey(KeyCode.UpArrow))
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
                        ExploreManager.Instance.CheckCollision();
                        if (MoveHandler != null)
                        {
                            MoveHandler();
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.DownArrow))
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
                        ExploreManager.Instance.CheckCollision();
                        if (MoveHandler != null)
                        {
                            MoveHandler();
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
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
                else if (Input.GetKey(KeyCode.RightArrow))
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
        }
    }
}