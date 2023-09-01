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

        public Vector3 MoveTo;

        private Vector3 position;
        private bool _isMoving = false;
        private Grid2D<Generator2D.CellType> _grid;

        private void Update()
        {
            if (!_isMoving)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    position = transform.position + transform.forward;
                    if (ExploreManager.Instance.IsWalkable(position))
                    {
                        _isMoving = true;
                        transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() => { _isMoving = false; });
                        MoveTo = position;
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
                        transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(() => { _isMoving = false; });
                        MoveTo = position;
                        if (MoveHandler != null)
                        {
                            MoveHandler();
                        }
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    _isMoving = true;
                    transform.DORotate(transform.localEulerAngles + Vector3.down * 90, 0.5f).SetEase(Ease.Linear).OnComplete(() => { _isMoving = false; });
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    _isMoving = true;
                    transform.DORotate(transform.localEulerAngles + Vector3.up * 90, 0.5f).SetEase(Ease.Linear).OnComplete(() => { _isMoving = false; });
                }
            }
        }
    }
}