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

        public void Init(ExploreFileEnemy enemyExplorer)
        {
            File = enemyExplorer;
            transform.position = new Vector3(enemyExplorer.Position.x, 1, enemyExplorer.Position.y);
            transform.eulerAngles = new Vector3(0, enemyExplorer.RotationY, 0);
            if(enemyExplorer.AI == ExploreFileEnemy.AiEnum.NotMove) 
            {
                AI = new NotMoveAI();
            }
            else if (enemyExplorer.AI == ExploreFileEnemy.AiEnum.Default)
            {
                AI = new DefaultAI();
            }
        }

        public void Move()
        {
            if(AI.GetMove(transform, out Vector3 position, out Vector3 rotation)) 
            {
                ExploreManager.Instance.File.WalkableList.Add(Utility.ConvertToVector2Int(transform.position));
                ExploreManager.Instance.File.WalkableList.Remove(Utility.ConvertToVector2Int(position));
                transform.DOMove(position, 0.5f).SetEase(Ease.Linear).OnComplete(()=> 
                {
                    ExploreManager.Instance.WaitForAllMoveComplete();
                });
                transform.DORotate(rotation, 0.5f).SetEase(Ease.Linear);
            }
        }

        public void Rotate()
        {
        }

        public bool CanSee(Vector2Int p1, Vector2Int p2)
        {
            List<Vector2Int> list = Utility.DrawLine2D(p1, p2);
            for (int i = 0; i < list.Count; i++)
            {
                if (ExploreManager.Instance.IsBlocked(new Vector3(list[i].x, 0, list[i].y)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}