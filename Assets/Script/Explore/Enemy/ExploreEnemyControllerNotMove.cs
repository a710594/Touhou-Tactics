using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyControllerNotMove : ExploreEnemyController
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log(transform.position);
                ExploreManager.Instance.EnterBattle(File);
            }
        }
    }
}