using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class AI : MonoBehaviour
    {
        public GameObject Back;
        public Vector3 MoveTo;

        public virtual void Init(List<Room> rooms)
        {
        }

        public virtual void Move()
        {
        }

        public virtual void Rotate()
        {
        }
    }
}