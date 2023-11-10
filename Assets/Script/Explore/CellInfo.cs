using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class CellInfo
    {
        public Generator2D.CellType CellType;
        public Vector2Int Position;
        [NonSerialized]
        public GameObject Cube;
        [NonSerialized]
        public GameObject Quad;
        [NonSerialized]
        public GameObject Icon;
        [NonSerialized]
        public GameObject Treasure;

        private bool _isVisited = false;
        public CellInfo(Generator2D.CellType type, Vector2Int pos) 
        {
            CellType = type;
            Position = pos;
        }


        public void CheckVidsited(Vector2Int v2)
        {
            if (v2 == Position)
            {
                _isVisited = true;
                Quad.layer = ExploreManager.Instance.MapLayer;
                if (Icon != null)
                {
                    Icon.layer = ExploreManager.Instance.MapLayer;
                }
            }
        }
    }
}