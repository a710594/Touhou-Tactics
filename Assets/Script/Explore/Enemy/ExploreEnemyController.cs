using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyController : MonoBehaviour
    {
        public GameObject Arrow;
        public ExploreFileEnemy File;
        public EnemyExplorerAI AI;

        public ExploreFileEnemy Enemy;

        public void SetAI(ExploreFileEnemy enemy)
        {
            Enemy = enemy;
            if(enemy.AI == EnemyGroupModel.AiEnum.NotMove) 
            {
                AI = new NotMoveAI();
            }
            else if (enemy.AI == EnemyGroupModel.AiEnum.Default)
            {
                AI = new DefaultAI();
            }
        }

        public void Init(EnemyGroupModel.AiEnum ai)
        {
            if (ai == EnemyGroupModel.AiEnum.NotMove)
            {
                AI = new NotMoveAI();
            }
            else if (ai == EnemyGroupModel.AiEnum.Default)
            {
                AI = new DefaultAI();
            }
        }

        public void Move()
        {
            if(AI.GetMove(transform, out Vector3 to, out Vector3 rotation)) 
            {
                ExploreManager.Instance.TileDic[Utility.ConvertToVector2Int(transform.position)].IsWalkable = true;
                ExploreManager.Instance.TileDic[Utility.ConvertToVector2Int(to)].IsWalkable = false;
                transform.DOMove(to, 0.5f).SetEase(Ease.Linear).OnComplete(()=> 
                {
                    Enemy.Position = Utility.ConvertToVector2Int(transform.position);
                    Enemy.RotationY = Mathf.RoundToInt(transform.eulerAngles.y);
                    ExploreManager.Instance.WaitForAllMoveComplete();
                });
                transform.DORotate(rotation, 0.5f).SetEase(Ease.Linear);
            }
            else 
            {
                ExploreManager.Instance.WaitForAllMoveComplete();
            }
        }

        public void Rotate()
        {
        }
    }
}