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
    private Vector2Int _direction = Vector2Int.left;
    private CameraRotate _cameraRotate;

    public void SetSprite(string sprite) 
    {
        _front = Resources.Load<Sprite>("Image/" + sprite + "_F");
        _back = Resources.Load<Sprite>("Image/" + sprite + "_B");
        SpriteRenderer.sprite = _front;
        SpriteRenderer.flipX = false;
        _direction = Vector2Int.left;
    }

    public void Move(List<Vector2Int> paths)
    {
        if (paths.Count > 0)
        {
            SetDirection(paths[0]);

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

    public void SetDirection(Vector2 position) 
    {
        if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
        {
            if (position.x > transform.position.x)
            {
                SpriteRenderer.sprite = _back;
                SpriteRenderer.flipX = false;
                _direction = Vector2Int.right;
            }
            else if (position.x < transform.position.x)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = false;
                _direction = Vector2Int.left;
            }
            else if (position.y > transform.position.z)
            {
                SpriteRenderer.sprite = _back;
                SpriteRenderer.flipX = true;
                _direction = Vector2Int.up;
            }
            else if (position.y < transform.position.z)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = true;
                _direction = Vector2Int.down;
            }
        }
        else 
        {
            if (position.x > transform.position.x)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = true;
            }
            else if (position.x < transform.position.x)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = false;
            }
            else if (position.y > transform.position.z)
            {
                SpriteRenderer.sprite = _back;
                SpriteRenderer.flipX = true;
            }
            else if (position.y < transform.position.z)
            {
                SpriteRenderer.sprite = _front;
                SpriteRenderer.flipX = true;
            }
        }

        if (SetDirectionHandler != null) 
        {
            SetDirectionHandler(_direction);
        }
    }

    public void SetGray(bool isGray)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        SpriteRenderer.GetPropertyBlock(mpb);
        mpb.SetInteger("IsGray", isGray ? 1 : 0);
        SpriteRenderer.SetPropertyBlock(mpb);
    }

    private void Rotate(CameraRotate.StateEnum state) 
    {
        if (state == CameraRotate.StateEnum.Slope) 
        {
            transform.eulerAngles = new Vector3(30, 45, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(90, 0, 0);
        }
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
