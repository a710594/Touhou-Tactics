using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileGenerator : MonoBehaviour
    {
        public Transform Tilemap;

        private ExploreInfo _info = new ExploreInfo();

        public void BuildFile()
        {
            Vector2Int pos;
            Generator2D.CellType type;
            foreach (Transform child in Tilemap)
            {
                if(child.name == "Wall") 
                {
                    type = Generator2D.CellType.Wall;
                }
                else
                {
                    type = Generator2D.CellType.Room;
                }
                pos = Utility.ConvertToVector2Int(child.position);
                _info.CellDic.Add(pos, new CellInfo(type, pos));
                _info.WalkableList.Add(pos);
            }

            ExploreFile file = new ExploreFile(_info);
            DataContext.Instance.Save(file, Application.streamingAssetsPath + "./Map/Floor_1", DataContext.PrePathEnum.None);
        }
    }
}
