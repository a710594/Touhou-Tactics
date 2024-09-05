using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class DefaultAI : EnemyExplorerAI
    {
        public override bool GetMove(Transform transform, out Vector3 position, out Vector3 rotation)
        {
            if (ExploreManager.Instance.TileDic[Utility.ConvertToVector2Int(transform.position + transform.forward)].IsWalkable)
            {
                position = transform.position + transform.forward;
                rotation = transform.localEulerAngles;
                return true;
            }
            else if (ExploreManager.Instance.TileDic[Utility.ConvertToVector2Int(transform.position + transform.right)].IsWalkable)
            {
                position = transform.position + transform.right;
                rotation = transform.localEulerAngles + Vector3.up * 90;
                return true;
            }
            else if (ExploreManager.Instance.TileDic[Utility.ConvertToVector2Int(transform.position - transform.right)].IsWalkable)
            {
                position = transform.position - transform.right;
                rotation = transform.localEulerAngles - Vector3.up * 90;
                return true;
            }
            else if (ExploreManager.Instance.TileDic[Utility.ConvertToVector2Int(transform.position - transform.forward)].IsWalkable)
            {
                position = transform.position - transform.forward;
                rotation = transform.localEulerAngles + Vector3.up * 180;
                return true;
            }

            position = new Vector3();
            rotation = new Vector3();

            return false;
        }
    }
}