using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileLoader : MonoBehaviour
    {
        public Transform Tilemap;

        public ExploreFile LoadFile()
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>("Explore/Floor_1", DataContext.PrePathEnum.Map);

            return file;
        }
    }
}