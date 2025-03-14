using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyControllerMove : ExploreEnemyController
    {
        public CharacterController CharacterController;

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Treasure")
            {
                transform.eulerAngles += new Vector3(0, 90, 0);
            }
            else if (hit.collider.tag == "Player")
            {
                CharacterController.Move(Vector3.zero);
                ExploreManager.Instance.EnterBattle(File);
            }
        }

        private void Update()
        {
            if (ExploreManager.Instance.PlayerSpeed > 0)
            {
                CharacterController.Move(transform.forward * ExploreManager.Instance.PlayerSpeed);
            }
        }
    }
}