using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class NotMoveAI : EnemyExplorerAI
    {
        public override bool GetMove(Transform transform, out Vector3 position, out Vector3 rotation)
        {
            position = transform.position;
            rotation = transform.localEulerAngles;
            return true;
        }
    }
}