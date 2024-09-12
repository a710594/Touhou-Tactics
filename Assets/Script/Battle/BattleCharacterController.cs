using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Battle;
using UnityEngine.UIElements;

public class BattleCharacterController : MonoBehaviour
{
    public Action MoveEndHandler;

    public Transform HpAnchor;
    public SpriteRenderer SpriteRenderer;
    public SpriteFlash SpriteFlash;

    private Sprite _front;
    private Sprite _back;
    private CameraRotate _cameraRotate;
    public Vector2 Direction = Vector2Int.left;

    public void Init(string sprite) 
    {
        _front = Resources.Load<Sprite>("Image/" + sprite + "_F");
        _back = Resources.Load<Sprite>("Image/" + sprite + "_B");
        SpriteRenderer.sprite = _front;
        SpriteRenderer.flipX = false;
    }

    public void Move(List<Vector2Int> paths)
    {
        if (paths.Count > 0)
        {
            SetDirection(paths[0] - Utility.ConvertToVector2Int(transform.position));
            SetSprite();

            transform.DOMove(new Vector3(paths[0].x, BattleController.Instance.Info.TileDic[paths[0]].TileData.Height, paths[0].y), 0.25f).SetEase(Ease.Linear).OnComplete(() =>
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

    public void SetDirection(Vector2Int direction)
    {
        Direction = direction;
    }

    public void SetSprite()
    {
        Vector2Int localDirection = Vector2Int.RoundToInt(Quaternion.AngleAxis(_cameraRotate.Angle, Vector3.forward) * Direction);
        if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
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
    }

    public void SetFlash(bool enable)
    {
        SpriteFlash.SetFlash(enable);
    }

    public void Rotate(int angle) 
    {
        if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope) 
        {
            transform.DORotate(new Vector3(30, 45 + angle, 0), 1f);
        }
        else
        {
            transform.DORotate(new Vector3(90, angle, 0), 1f);
        }

        SetSprite();
    }

    public void SetAngle() 
    {
        if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope) 
        {
            transform.eulerAngles = new Vector3(30, 45 + _cameraRotate.Angle, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(90, _cameraRotate.Angle, 0);
        }

        SetSprite();
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
