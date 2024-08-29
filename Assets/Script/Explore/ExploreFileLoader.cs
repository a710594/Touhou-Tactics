using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileLoader : MonoBehaviour
    {
        public string FileName;
        public Transform Tilemap;
        public Transform Start;
        public Transform Goal;
        public Transform Enemy;
        public Transform Trigger;
        public Transform Treasure;

        public void Load()
        {
            for (int i = Tilemap.childCount; i > 0; --i)
            {
                DestroyImmediate(Tilemap.GetChild(0).gameObject);
            }

            for (int i = Enemy.childCount; i > 0; --i)
            {
                DestroyImmediate(Enemy.GetChild(0).gameObject);
            }

            for (int i = Trigger.childCount; i > 0; --i)
            {
                DestroyImmediate(Trigger.GetChild(0).gameObject);
            }

            for (int i = Treasure.childCount; i > 0; --i)
            {
                DestroyImmediate(Treasure.GetChild(0).gameObject);
            }

            GameObject obj;
            GameObject child;
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(FileName, DataContext.PrePathEnum.MapExplore);
            for(int i=0; i<file.TileList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + file.TileList[i].Prefab), Vector3.zero, Quaternion.identity);
                obj.name = file.TileList[i].Prefab;
                obj.transform.position = new Vector3(file.TileList[i].Position.x, 0, file.TileList[i].Position.y);
                obj.transform.SetParent(Tilemap);
                if(file.TileList[i].Tag == "Wall")
                {
                    obj.tag = "Wall";
                }
            }

            Start.transform.position = new Vector3(file.Start.x, 0, file.Start.y);

            if (Goal != null)
            {
                Goal.transform.position = new Vector3(file.Goal.x, 0, file.Goal.y);
            }

            for(int i=0; i<file.EnemyInfoList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/EnemyExploreFileObject"), Vector3.zero, Quaternion.identity);
                ExploreFileEnemyObject exploreEnemyObject = obj.GetComponent<ExploreFileEnemyObject>();
                exploreEnemyObject.Prefab = file.EnemyInfoList[i].Prefab;
                exploreEnemyObject.Map = file.EnemyInfoList[i].Map;
                exploreEnemyObject.Tutorial = file.EnemyInfoList[i].Tutorial;
                exploreEnemyObject.EnemyGroup = file.EnemyInfoList[i].EnemyGroupId;
                obj.transform.SetParent(Enemy);
                obj.transform.position = new Vector3(file.EnemyInfoList[i].Position.x, 1, file.EnemyInfoList[i].Position.y);
            }

            for(int i=0; i<file.TriggerList.Count; i++)
            {
                obj = new GameObject();
                obj.transform.position = new Vector3(file.TriggerList[i].Position.x, 0, file.TriggerList[i].Position.y);
                obj.name = file.TriggerList[i].Name;
                obj.transform.SetParent(Trigger);
            }

            for(int i=0; i<file.TreasureList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/TreasureExploreFileObject"), Vector3.zero, Quaternion.identity);
                TreasureObject treasureExploreFileObject = obj.GetComponent<TreasureObject>();
                treasureExploreFileObject.Type = file.TreasureList[i].Type;
                treasureExploreFileObject.ItemID = file.TreasureList[i].ItemID;
                obj.transform.SetParent(Treasure);
                obj.transform.position = new Vector3(file.TreasureList[i].Position.x, 1, file.TreasureList[i].Position.y);
                string prefab = file.TreasureList[i].Prefab;
                child = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + prefab), Vector3.zero, Quaternion.identity);
                child.name = prefab;
                child.transform.position = new Vector3(obj.transform.position.x, file.TreasureList[i].Height, obj.transform.position.z);
                child.transform.localEulerAngles = file.TreasureList[i].Rotation;
                child.transform.SetParent(obj.transform);
                treasureExploreFileObject.Prefab = child;
            }
        }
    }
}