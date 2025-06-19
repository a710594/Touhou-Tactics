using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyControllerNotMove : ExploreEnemyController
    {
        private void Update()
        {
            if (ExploreManager.Instance.PlayerSpeed > 0)
            {
                if (ExploreManager.Instance.TileDic[Utility.ConvertToVector2Int(transform.position)].IsVisited)
                {
                    ExploreManager.Instance.ShowEnemy(transform.position, this);
                }
                else
                {
                    ExploreManager.Instance.HideEnemy(this);
                }
            }
        }
    }
}