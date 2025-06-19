using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyControllerMove : ExploreEnemyController
    {
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Treasure" || hit.collider.tag == "Door")
            {
                transform.eulerAngles += new Vector3(0, 90, 0);
            }
        }

        private void Update()
        {
            if (ExploreManager.Instance.PlayerSpeed > 0)
            {
                CharacterController.Move(transform.forward * ExploreManager.Instance.PlayerSpeed);

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