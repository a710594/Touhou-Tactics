using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileGenerator : MonoBehaviour
    {
        public int Floor;
        public string FileName;
        public Transform Ground;
        public Transform Wall;
        public Transform Start;
        public Transform Goal;
        public Transform Enemy;
        public Transform Trigger;
        public ExploreEnemyInfoObject[] Enemys;
        public GameObject[] Triggers;

        int minX;
        int maxX;
        int minY;
        int maxY;
        private ExploreInfo _info = new ExploreInfo();

        public void BuildFile()
        {
            minX = int.MinValue;
            maxX = int.MinValue;
            minY = int.MinValue;
            maxY = int.MinValue;
            Vector2Int pos;
            _info = new ExploreInfo();
            foreach (Transform child in Ground)
            {
                pos = Utility.ConvertToVector2Int(child.position);
                AddTile(pos, child.name);
                _info.GroundList.Add(pos);
            }
            foreach (Transform child in Wall)
            {
                pos = Utility.ConvertToVector2Int(child.position);
                AddTile(pos, child.name);
            }
            _info.Floor = Floor;
            _info.Start = Utility.ConvertToVector2Int(Start.position);
            _info.Goal = Utility.ConvertToVector2Int(Goal.position);
            _info.PlayerPosition = _info.Start;
            _info.Size = new Vector2Int(maxX - minX, maxY - minY);

            ExploreEnemyInfo enemyInfo;
            ExploreEnemyInfoObject enemyObj;
            foreach (Transform child in Enemy)
            {
                enemyObj = child.gameObject.GetComponent<ExploreEnemyInfoObject>();
                enemyInfo = new ExploreEnemyInfo(enemyObj.Prefab, enemyObj.Map, enemyObj.Tutorial, Utility.ConvertToVector2Int(enemyObj.transform.position), (int)enemyObj.transform.eulerAngles.y);
                _info.EnemyInfoList.Add(enemyInfo);
            }

            foreach (Transform child in Trigger)
            {
                _info.TriggerDic.Add(Utility.ConvertToVector2Int(child.position), child.name);
            }

            ExploreFile file = new ExploreFile(_info);
            DataContext.Instance.Save(file, "Explore/" + FileName, DataContext.PrePathEnum.Map);
        }

        private void AddTile(Vector2Int pos, string name) 
        {
            _info.TileDic.Add(pos, new TileObject(name));

            if (minX == int.MinValue || pos.x < minX)
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

        public void LoadFile() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(Application.streamingAssetsPath + "/Map/Explore/" + FileName, DataContext.PrePathEnum.Map);
            for (int i = Ground.childCount; i > 0; --i)
            {
                DestroyImmediate(Ground.GetChild(0).gameObject);
            }
            for (int i = Wall.childCount; i > 0; --i)
            {
                DestroyImmediate(Wall.GetChild(0).gameObject);
            }

            GameObject obj;

            for(int i=0; i<file.TileValues.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.TileValues[i]), Vector3.zero, Quaternion.identity);
                obj.name = file.TileValues[i];
                obj.transform.position = new Vector3(file.TileKeys[i].x, 0, file.TileKeys[i].y);
                if (obj.name == "Wall")
                {
                    obj.transform.SetParent(Wall);
                }
                else
                {
                    obj.transform.SetParent(Ground);
                }
            }

            for(int i=0; i<file.EnemyInfoList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/ExploreEnemyInfoObject"), Vector3.zero, Quaternion.identity);
                ExploreEnemyInfoObject exploreEnemyInfoObject = obj.GetComponent<ExploreEnemyInfoObject>();
                exploreEnemyInfoObject.Prefab = file.EnemyInfoList[i].Prefab;
                exploreEnemyInfoObject.Map = file.EnemyInfoList[i].Map;
                exploreEnemyInfoObject.Tutorial = file.EnemyInfoList[i].Tutorial;
            }

            for(int i=0; i<file.TriggerKeys.Count; i++)
            {
                obj = new GameObject();
                obj.transform.position = new Vector3(file.TileKeys[i].x, 0, file.TileKeys[i].y);
                obj.name = file.TriggerValues[i];
            }
        }
    }
}
