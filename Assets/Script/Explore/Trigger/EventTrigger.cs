using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class EventTrigger : MonoBehaviour
    {
        public ExploreFileEvent Event;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                ExploreManager.Instance.CheckEvent(this);
            }
        }
    }
}
