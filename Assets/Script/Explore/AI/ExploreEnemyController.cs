using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyController : MonoBehaviour
    {
        public GameObject Arrow;
        public Vector3 MoveTo;
        public ExploreEnemyInfo Info;

        public virtual void Init(ExploreEnemyInfo info)
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
    }
}