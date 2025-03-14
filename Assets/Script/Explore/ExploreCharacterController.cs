using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*namespace Explore
{
    public class ExploreCharacterController : MonoBehaviour
    {
        //public Action MoveHandler;
        //public Action RotateHandler;

        //public Vector3 MoveTo;
        //public Vector3 RotateTo;

        //private Vector3 position;
        private bool _isMoving = false;

        private void Up()
        {
            if (!_isMoving) 
            {
                Move(transform.position + transform.forward);
            }
        }

        private void Down()
        {
            if (!_isMoving) 
            {
                Move(transform.position - transform.forward);
            }
        }

        private void Left()
        {
            if (!_isMoving)
            {
                Move(transform.position - transform.right);
            }
        }

        private void Right()
        {
            if (!_isMoving)
            {
                Move(transform.position + transform.right);
            }
        }

        private void Move(Vector3 position)
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(position);
            ExploreManager.Instance.File.PlayerPosition = v2;
            ExploreManager.Instance.CheckEnemyCollision();

            if (ExploreManager.Instance.TileDic[v2].IsWalkable) 
            {
                _isMoving = true;
                ExploreManager.Instance.EnemyMove();
                transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    ExploreManager.Instance.WaitForAllMoveComplete();
                    _isMoving = false;
                });
            }
        }

        private void TurnLeft()
        {
            if (!_isMoving)
            {
                Rotate(transform.localEulerAngles + Vector3.down * 90);
            }
        }

        private void TurnRight()
        {
            if (!_isMoving)
            {
                Rotate(transform.localEulerAngles + Vector3.up * 90);
            }
        }

        private void Rotate(Vector3 rotation) 
        {
            _isMoving = true;
            InputMamager.Instance.Lock();
            ExploreManager.Instance.File.PlayerRotationY = Mathf.RoundToInt(rotation.y);
            transform.DORotate(rotation, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _isMoving = false;
                InputMamager.Instance.Unlock();
                ExploreManager.Instance.CheckVidsit(transform);
            });
        }

        private void Awake()
        {
        }

        private void OnDestroy()
        {
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
}*/