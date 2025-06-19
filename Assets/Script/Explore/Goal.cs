using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class Goal : MonoBehaviour
    {
        public GameObject Red;
        public GameObject Blue;
        public GameObject Quad;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                ExploreManager.Instance.ArriveGoal();
            }
        }
    }
}
