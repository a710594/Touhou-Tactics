using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleCharacterController : MonoBehaviour
{
    public Action MoveEndHandler;

    public Transform HpAnchor;
    public Sprite Front;
    public Sprite Back;
    public SpriteRenderer SpriteRenderer;

    private CameraRotate _cameraRotate;

    //public void Move(Vector2 position)
    //{
    //    List<Vector2> paths = PathManager.Instance.GetPath(new Vector2(transform.position.x, transform.position.z), position);
    //    if (paths.Count > 0)
    //    {
    //        Move(paths);
    //    }
    //    else 
    //    {
    //        if (MoveEndHandler != null)
    //        {
    //            MoveEndHandler();
    //        }
    //    }
    //}

    public void Move(List<Vector2> paths)
    {
        if (paths.Count > 0)
        {
            SetDirection(paths[0]);

            transform.DOMove(new Vector3(paths[0].x, BattleController.Instance.BattleInfo.TileInfoDic[paths[0]].Height, paths[0].y), 0.25f).SetEase(Ease.Linear).OnComplete(() =>
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
                SpriteRenderer.sprite = Back;
                SpriteRenderer.flipX = false;
            }
            else if (position.x < transform.position.x)
            {
                SpriteRenderer.sprite = Front;
                SpriteRenderer.flipX = false;
            }
            else if (position.y > transform.position.z)
            {
                SpriteRenderer.sprite = Back;
                SpriteRenderer.flipX = true;
            }
            else if (position.y < transform.position.z)
            {
                SpriteRenderer.sprite = Front;
                SpriteRenderer.flipX = true;
            }
        }
        else 
        {
            if (position.x > transform.position.x)
            {
                SpriteRenderer.sprite = Front;
                SpriteRenderer.flipX = true;
            }
            else if (position.x < transform.position.x)
            {
                SpriteRenderer.sprite = Front;
                SpriteRenderer.flipX = false;
            }
            else if (position.y > transform.position.z)
            {
                SpriteRenderer.sprite = Back;
                SpriteRenderer.flipX = true;
            }
            else if (position.y < transform.position.z)
            {
                SpriteRenderer.sprite = Front;
                SpriteRenderer.flipX = true;
            }
        }
    }

    private void Rotate(Vector3 angle) 
    {
        transform.eulerAngles = angle;
    }

    private void Awake()
    {
        transform.eulerAngles = new Vector3(0, 45, 0);

        _cameraRotate = Camera.main.GetComponent<CameraRotate>();
        _cameraRotate.RotateHandler += Rotate;
    }
}
