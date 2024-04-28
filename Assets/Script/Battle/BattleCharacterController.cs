using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Battle;

public class BattleCharacterController : MonoBehaviour
{
    public Action MoveEndHandler;
    public Action<Vector2Int> SetDirectionHandler;

    public Transform HpAnchor;
    public SpriteRenderer SpriteRenderer;

    private Sprite _front;
    private Sprite _back;
    private CameraRotate _cameraRotate;
    private Vector2 _direction = Vector2Int.left;

    public void SetSprite(string sprite) 
    {
        _front = Resources.Load<Sprite>("Image/" + sprite + "_F");
        _back = Resources.Load<Sprite>("Image/" + sprite + "_B");
        SpriteRenderer.sprite = _front;
        SpriteRenderer.flipX = false;
        //_direction = Vector2Int.left;
    }

    public void Move(List<Vector2Int> paths)
    {
        if (paths.Count > 0)
        {
            SetDirection(_cameraRotate.CurrentState, paths[0] - Utility.ConvertToVector2Int(transform.position), Camera.main.transform.eulerAngles.y);

            transform.DOMove(new Vector3(paths[0].x, BattleController.Instance.Info.TileAttachInfoDic[paths[0]].Height, paths[0].y), 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                paths.RemoveAt(0);
                if (paths.Count > 0)
                {
                    Move(paths);
                }
                else
                {
                    if (MoveEndHandler != null)
                    {
                        MoveEndHandler();
                    }
                }
            });
        }
        else 
        {
            if (MoveEndHandler != null)
            {
                MoveEndHandler();
            }
        }
    }

    //public void SetDirection(Vector2 position) 
    //{
    //    Vector2Int direction = new Vector2Int();
    //    if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
    //    {
    //        if (position.x > transform.position.x)
    //        {
    //            SpriteRenderer.sprite = _back;
    //            SpriteRenderer.flipX = false;
    //            direction = Vector2Int.right;
    //        }
    //        else if (position.x < transform.position.x)
    //        {
    //            SpriteRenderer.sprite = _front;
    //            SpriteRenderer.flipX = false;
    //            direction = Vector2Int.left;
    //        }
    //        else if (position.y > transform.position.z)
    //        {
    //            SpriteRenderer.sprite = _back;
    //            SpriteRenderer.flipX = true;
    //            direction = Vector2Int.up;
    //        }
    //        else if (position.y < transform.position.z)
    //        {
    //            SpriteRenderer.sprite = _front;
    //            SpriteRenderer.flipX = true;
    //            direction = Vector2Int.down;
    //        }
    //    }
    //    else 
    //    {
    //        if (position.x > transform.position.x)
    //        {
    //            SpriteRenderer.sprite = _front;
    //            SpriteRenderer.flipX = true;
    //            direction = Vector2Int.right;
    //        }
    //        else if (position.x < transform.position.x)
    //        {
    //            SpriteRenderer.sprite = _front;
    //            SpriteRenderer.flipX = false;
    //            direction = Vector2Int.left;
    //        }
    //        else if (position.y > transform.position.z)
    //        {
    //            SpriteRenderer.sprite = _back;
    //            SpriteRenderer.flipX = true;
    //            direction = Vector2Int.up;
    //        }
    //        else if (position.y < transform.position.z)
    //        {
    //            SpriteRenderer.sprite = _front;
    //            SpriteRenderer.flipX = true;
    //            direction = Vector2Int.down;
    //        }
    //    }

    //    if (SetDirectionHandler != null) 
    //    {
    //        SetDirectionHandler(direction);
    //    }
    //}

    public void SetDirection(CameraRotate.StateEnum state, Vector2Int direction, float angle)
    {
        _direction = direction;
        Vector2Int localDirection = Vector2Int.RoundToInt(Quaternion.AngleAxis(angle, Vector3.forward) * _direction);
        if (state == CameraRotate.StateEnum.Slope)
        {
            if (localDirection == Vector2Int.right)
            {
                SpriteRenderer.sprite = _back;
                SpriteRenderer.flipX = false;
            }
            else if (localDirection == Vector2Int.left)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = false;
            }
            else if (localDirection == Vector2Int.up)
            {
                SpriteRenderer.sprite = _back;
                SpriteRenderer.flipX = true;
            }
            else if (localDirection == Vector2Int.down)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = true;
            }
        }
        else
        {
            if (localDirection == Vector2Int.right)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = true;
            }
            else if (localDirection == Vector2Int.left)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = false;
            }
            else if (localDirection == Vector2Int.up)
            {
                SpriteRenderer.sprite = _back;
                SpriteRenderer.flipX = true;
            }
            else if (localDirection == Vector2Int.down)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = true;
            }
        }

        if (SetDirectionHandler != null)
        {
            SetDirectionHandler(direction);
        }
    }

    public void SetGray(bool isGray)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        SpriteRenderer.GetPropertyBlock(mpb);
        mpb.SetInteger("IsGray", isGray ? 1 : 0);
        SpriteRenderer.SetPropertyBlock(mpb);
    }

    private void Rotate(CameraRotate.StateEnum state, float angle) 
    {
        if (state == CameraRotate.StateEnum.Slope) 
        {
            transform.eulerAngles = new Vector3(30, 45 + angle, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(90, angle, 0);
        }

        SetDirection(_cameraRotate.CurrentState, Vector2Int.RoundToInt(_direction), angle);
    }

    private void Awake()
    {
        transform.eulerAngles = new Vector3(30, 45, 0);

        _cameraRotate = Camera.main.GetComponent<CameraRotate>();
        _cameraRotate.RotateHandler += Rotate;
    }

    private void OnDestroy()
    {
        _cameraRotate.RotateHandler -= Rotate;
    }
}
