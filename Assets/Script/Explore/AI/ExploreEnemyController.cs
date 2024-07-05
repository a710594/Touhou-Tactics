using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyController : MonoBehaviour
    {
        public GameObject Arrow;
        public Vector3 MoveTo;
        public NewExploreFile.EnemyInfo Info;

        public virtual void Init(NewExploreFile.EnemyInfo info)
        {
            Info = info;
            transform.position = new Vector3(info.Position.x, 1, info.Position.y);
            transform.eulerAngles = new Vector3(0, info.Rotation, 0);
            MoveTo = transform.position;
        }

        public virtual void Move()
        {
        }

        public virtual void Rotate()
        {
        }

        protected bool CanSee(Vector2Int p1, Vector2Int p2) 
        {
            List<Vector2Int> list = Utility.DrawLine2D(p1, p2);
            for (int i=0; i<list.Count; i++) 
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