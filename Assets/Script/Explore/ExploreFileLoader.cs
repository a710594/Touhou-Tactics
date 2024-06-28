using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileLoader : MonoBehaviour
    {
        public string FileName;
        public Transform Tilemap;
        public Transform Enemy;
        public Transform Trigger;

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

            GameObject obj;
            NewExploreFile file = DataContext.Instance.Load<NewExploreFile>("Explore/" +FileName, DataContext.PrePathEnum.Map);
            for(int i=0; i<file.TileList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.TileList[i].Prefab), Vector3.zero, Quaternion.identity);
                obj.name = file.TileList[i].Prefab;
                obj.transform.position = new Vector3(file.TileList[i].Position.x, 0, file.TileList[i].Position.y);
                obj.transform.SetParent(Tilemap);
                if(file.TileList[i].Tag == "Wall")
                {
                    obj.tag = "Wall";
                }
            }

            for(int i=0; i<file.EnemyInfoList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/ExploreEnemyInfoObject"), Vector3.zero, Quaternion.identity);
                ExploreEnemyInfoObject exploreEnemyInfoObject = obj.GetComponent<ExploreEnemyInfoObject>();
                exploreEnemyInfoObject.Prefab = file.EnemyInfoList[i].Prefab;
                exploreEnemyInfoObject.Map = file.EnemyInfoList[i].Map;
                exploreEnemyInfoObject.Tutorial = file.EnemyInfoList[i].Tutorial;
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
        }
    }
}