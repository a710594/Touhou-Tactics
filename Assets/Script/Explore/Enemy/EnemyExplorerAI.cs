using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class EnemyExplorerAI
    {
        public virtual bool GetMove(Transform transform, out Vector3 position, out Vector3 rotation)
        {
            position = new Vector3();
            rotation = new Vector3();
            return false;
        }
    }
}
