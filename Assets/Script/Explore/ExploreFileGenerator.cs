using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileGenerator : MonoBehaviour
    {
        public int Floor;
        public Transform Tilemap;
        public Transform Start;
        public Transform Goal;
        public ExploreEnemyInfoObject[] Enemys;

        private ExploreInfo _info = new ExploreInfo();

        public void BuildFile()
        {
            int minX = int.MinValue;
            int maxX = int.MinValue;
            int minY = int.MinValue;
            int maxY = int.MinValue;
            Vector2Int pos;
            _info = new ExploreInfo();
            foreach (Transform child in Tilemap)
            {
                pos = Utility.ConvertToVector2Int(child.position);
                _info.TileDic.Add(pos, new TileObject(child.name));
                if(child.name != "Wall")
                    _info.WalkableList.Add(pos);

                if(minX == int.MinValue || pos.x < minX)
                {
                    minX = pos.x;
                }
                if (maxX == int.MinValue || pos.x > maxX)
                {
                    maxX = pos.x;
                }
                if (minY == int.MinValue || pos.y < minY)
                {
                    minY = pos.y;
                }
                if (maxY == int.MinValue || pos.y > maxY)
                {
                    maxY = pos.y;
                }
            }
            _info.Floor = Floor;
            _info.Start = Utility.ConvertToVector2Int(Start.position);
            _info.Goal = Utility.ConvertToVector2Int(Goal.position);
            _info.PlayerPosition = _info.Start;
            _info.Size = new Vector2Int(maxX - minX, maxY - minY);

            ExploreEnemyInfo enemyInfo;
            for(int i=0; i<Enemys.Length; i++)
            {
                enemyInfo = new ExploreEnemyInfo(Enemys[i].Prefab, Enemys[i].Map, Utility.ConvertToVector2Int(Enemys[i].transform.position), (int)Enemys[i].transform.eulerAngles.y);
                _info.EnemyInfoList.Add(enemyInfo);
            }

            ExploreFile file = new ExploreFile(_info);
            DataContext.Instance.Save(file, "Explore/Floor_1", DataContext.PrePathEnum.Map);
        }

        public void LoadFile() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(Application.streamingAssetsPath + "/Map/Explore/Floor_1", DataContext.PrePathEnum.Map);
            for (int i = Tilemap.childCount; i > 0; --i)
            {
                DestroyImmediate(Tilemap.GetChild(0).gameObject);
            }

            GameObject obj;
            for(int i=0; i<file.TileValues.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.TileValues[i]), Vector3.zero, Quaternion.identity);
                obj.name = file.TileValues[i];
                obj.transform.position = new Vector3(file.TileKeys[i].x, 0, file.TileKeys[i].y);
                obj.transform.SetParent(Tilemap);
            }
        }
    }
}
