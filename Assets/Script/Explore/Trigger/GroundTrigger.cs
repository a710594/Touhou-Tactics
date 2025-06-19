using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class GroundTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                ExploreManager.Instance.CheckVisit(Utility.ConvertToVector2Int(transform.position), Color.green);
            }
        }
    }
}