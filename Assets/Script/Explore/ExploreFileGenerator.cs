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
        public Transform Start;
        public Transform Goal;
        public Transform Enemy;
        public Transform Trigger;
        public Transform Treasure;

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
            if (Goal != null)
            {
                file.Goal = Utility.ConvertToVector2Int(Goal.position);
            }
            else
            {
                file.Goal = new Vector2Int(int.MinValue, int.MinValue);
            }
            file.PlayerPosition = file.Start;
            file.Size = new Vector2Int(maxX - minX, maxY - minY);

            EnemyExploreFileObject enemyObj;
            foreach (Transform child in Enemy)
            {
                enemyObj = child.gameObject.GetComponent<EnemyExploreFileObject>();
                file.EnemyInfoList.Add(new NewExploreFile.EnemyInfo(enemyObj.Prefab, false, enemyObj.Map, enemyObj.Tutorial, Utility.ConvertToVector2Int(enemyObj.transform.position), (int)enemyObj.transform.eulerAngles.y));
            }

            foreach (Transform child in Trigger)
            {
                file.TriggerList.Add(new NewExploreFile.TriggerInfo(Utility.ConvertToVector2Int(child.position), child.name));
            }

            TreasureExploreFileObject treasureObj;
            foreach (Transform child in Treasure)
            {
                treasureObj = child.gameObject.GetComponent<TreasureExploreFileObject>();
                Treasure treasure = new Treasure();
                treasure.Position = Utility.ConvertToVector2Int(treasureObj.transform.position);
                treasure.Type = treasureObj.Type;
                treasure.ItemID = treasureObj.ItemID;
                treasure.Prefab = treasureObj.Prefab.name;
                treasure.Height = treasureObj.Prefab.transform.position.y;
                treasure.Rotation = Vector3Int.RoundToInt(treasureObj.Prefab.transform.localEulerAngles);
                file.TreasureList.Add(treasure);
            }

            //ExploreFile file = new ExploreFile(_info);
            DataContext.Instance.Save(file, FileName, DataContext.PrePathEnum.MapExplore);
        }
    }
}
