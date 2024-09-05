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

        public void SetAI(ExploreFileEnemy.AiEnum ai)
        {
            if(ai == ExploreFileEnemy.AiEnum.NotMove) 
            {
                AI = new NotMoveAI();
            }
            else if (ai == ExploreFileEnemy.AiEnum.Default)
            {
                AI = new DefaultAI();
            }
        }

        public void Init(ExploreFileEnemy.AiEnum ai)
        {
            if (ai == ExploreFileEnemy.AiEnum.NotMove)
            {
                AI = new NotMoveAI();
            }
            else if (ai == ExploreFileEnemy.AiEnum.Default)
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

                    ExploreManager.Instance.WaitForAllMoveComplete();
                });
                transform.DORotate(rotation, 0.5f).SetEase(Ease.Linear);
            }
        }

        public void Rotate()
        {
        }
    }
}