using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class TileObject
    {
        public string Name;
        public GameObject Cube;
        public GameObject Quad;
        public GameObject Icon;
        public GameObject Treasure;

        public TileObject(string name)
        {
            Name = name;
        }
    }
}
