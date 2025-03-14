using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreEnemyController : MonoBehaviour
    {
        public GameObject Arrow;
        public ExploreFileEnemy File;

        public void Init(ExploreFileEnemy file)
        {
            File = file;
        }
    }
}