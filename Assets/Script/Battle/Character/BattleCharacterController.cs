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
    public Action<int> RightClickHandler;

    [NonSerialized]
    public BattleAI AI = null;
    public BattleCharacterInfo Info;
    public Flash Flash; //玩家的角色會用到

    [NonSerialized]
    public int Index;
    [NonSerialized]
    public Sprite Sprite;
    [NonSerialized]
    public Vector3 LastPosition = new Vector3();

    public void Init(int lv, EnemyModel enemy)
    {
        Info = new BattleEnemyInfo(lv, enemy);
        //transform.eulerAngles = new Vector3(0, Vector3.Angle(transform.forward, Direction), 0);

        Type t = Type.GetType("Battle." + enemy.AI);
        AI = gameObject.AddComponent(t) as BattleAI;
        AI.Init(this);
    }

    public void Move(List<Vector2Int> paths, Action callback)
    {
        if (paths.Count > 0)
        {
            SetDirection(paths[0] - Utility.ConvertToVector2Int(transform.position));

            transform.DOMove(new Vector3(paths[0].x, BattleController.Instance.TileDic[paths[0]].TileData.Height * 0.5f + 0.5f, paths[0].y), 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                paths.RemoveAt(0);
                if (paths.Count > 0)
                {
                    Move(paths, callback);
                }
                else
                {
                    if (callback != null)
                    {
                        callback();
                    }
                }
            });
        }
        else
        {
            if (callback != null)
            {
                callback();
            }
        }
    }

    public void SetDirection(Vector2Int direction)
    {
        if (direction != Vector2Int.zero)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, new Vector3(direction.x, 0, direction.y), Vector3.up);
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }

    public void SetDirection(float angle) 
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - angle, 0);
    }

    public void StartFlash() 
    {
        Flash.Begin();
    }

    public void StopFlash() 
    {
        Flash.End();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && RightClickHandler != null) 
        {
            RightClickHandler(((BattlePlayerInfo)Info).Job.ID);
        }
    }
    private void Awake()
    {
    }

    private void OnDestroy()
    {
    }
}
