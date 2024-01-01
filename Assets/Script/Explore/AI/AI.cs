using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class AI : MonoBehaviour
    {
        public string Prefab;
        public GameObject Arrow;
        public Vector3 MoveTo;
        public string Map = null;

        public virtual void Init(string name, string map, Vector2Int position, int rotation)
        {
            Prefab = name;
            if (Map != null)
            {
                Map = map;
            }
            transform.position = new Vector3(position.x, 1, position.y);
            transform.eulerAngles = new Vector3(0, rotation, 0);
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