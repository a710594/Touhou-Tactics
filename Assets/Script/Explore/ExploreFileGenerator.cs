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
        public Transform Tilemap;
        public Transform Ground;
        public Transform Wall;
        public Transform Start;
        public Transform Goal;
        public Transform Enemy;
        public Transform Trigger;
        public ExploreEnemyInfoObject[] Enemys;
        public GameObject[] Triggers;

        public void BuildFile()
        {
            int minX = int.MinValue;
            int maxX = int.MinValue;
            int minY = int.MinValue;
            int maxY = int.MinValue;
            Vector2Int pos;
            NewExploreFile file = new NewExploreFile();
            foreach (Transform child in Tilemap)
            {
                pos = Utility.ConvertToVector2Int(child.position);
                file.TileList.Add(new NewExploreFile.TileInfo(pos, child.name, child.tag));

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
            file.Floor = Floor;
            file.Start = Utility.ConvertToVector2Int(Start.position);
            file.Goal = Utility.ConvertToVector2Int(Goal.position);
            file.PlayerPosition = file.Start;
            file.Size = new Vector2Int(maxX - minX, maxY - minY);

            ExploreEnemyInfoObject enemyObj;
            foreach (Transform child in Enemy)
            {
                enemyObj = child.gameObject.GetComponent<ExploreEnemyInfoObject>();
                //enemyInfo = new ExploreEnemyInfo(enemyObj.Prefab, enemyObj.Map, enemyObj.Tutorial, Utility.ConvertToVector2Int(enemyObj.transform.position), (int)enemyObj.transform.eulerAngles.y);
                file.EnemyInfoList.Add(new NewExploreFile.EnemyInfo(enemyObj.Prefab, enemyObj.Map, enemyObj.Tutorial, Utility.ConvertToVector2Int(enemyObj.transform.position), (int)enemyObj.transform.eulerAngles.y));
            }

            foreach (Transform child in Trigger)
            {
                //_info.TriggerDic.Add(Utility.ConvertToVector2Int(child.position), child.name);
                file.TriggerList.Add(new NewExploreFile.TriggerInfo(Utility.ConvertToVector2Int(child.position), child.name));
            }

            //ExploreFile file = new ExploreFile(_info);
            DataContext.Instance.Save(file, FileName, DataContext.PrePathEnum.MapExplore);
        }

        /*public void LoadFile() 
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
        }*/
    }
}
