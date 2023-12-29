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

        public void LoadFile() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(Application.streamingAssetsPath + "./Map/Floor_1", DataContext.PrePathEnum.Save);
            for (int i = Tilemap.childCount; i > 0; --i)
            {
                DestroyImmediate(Tilemap.GetChild(0).gameObject);
            }

            GameObject obj;
            for(int i=0; i<file.CellValues.Count; i++)
            {
                if (file.CellValues[i].CellType == Generator2D.CellType.Wall)
                {
                    obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Wall"), Vector3.zero, Quaternion.identity);
                    obj.name = "Wall";
                }
                else
                {
                    obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Ground"), Vector3.zero, Quaternion.identity);
                    obj.name = "Ground";
                }
                obj.transform.position = new Vector3(file.CellKeys[i].x, 0, file.CellKeys[i].y);
                obj.transform.SetParent(Tilemap);
            }
        }
    }
}
