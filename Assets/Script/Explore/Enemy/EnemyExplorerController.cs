using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class EnemyExplorerController : MonoBehaviour
    {
        public GameObject Arrow;
        public EnemyExplorer EnemyExplorer;
        public EnemyExplorerAI AI;

        public void Init(EnemyExplorer enemyExplorer)
        {
            EnemyExplorer = enemyExplorer;
            transform.position = new Vector3(enemyExplorer.Position.x, 1, enemyExplorer.Position.y);
            transform.eulerAngles = new Vector3(0, enemyExplorer.RotationY, 0);
            if(enemyExplorer.AiType == EnemyExplorer.AiEnum.NotMove) 
            {
                AI = new NotMoveAI();
            }
            else if (enemyExplorer.AiType == EnemyExplorer.AiEnum.Default)
            {
                AI = new DefaultAI();
            }
        }

        public void Move()
        {
            if(AI.GetMove(transform, out Vector3 position, out Vector3 rotation)) 
            {
                ExploreManager.Instance.File.WalkableList.Add(Utility.ConvertToVector2Int(transform.position));
                transform.DOMove(position, 0.5f).SetEase(Ease.Linear);
                transform.DORotate(rotation, 0.5f).SetEase(Ease.Linear);
                ExploreManager.Instance.File.WalkableList.Remove(Utility.ConvertToVector2Int(transform.position));
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