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
            ExploreFile file = new ExploreFile();
            foreach (Transform child in Tilemap)
            {
                pos = Utility.ConvertToVector2Int(child.position);
                file.TileList.Add(new ExploreFileTile(true, false, child.name, pos));

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

            ExploreFileEnemy enemy;
            ExploreFileEnemyObject enemyObj;
            foreach (Transform child in Enemy)
            {
                enemy = new ExploreFileEnemy();
                enemyObj = child.gameObject.GetComponent<ExploreFileEnemyObject>();
                enemy.AI = enemyObj.AiType;
                enemy.Prefab = enemyObj.Prefab;
                enemy.Position = Utility.ConvertToVector2Int(enemyObj.transform.position);
                enemy.RotationY = (int)enemyObj.transform.eulerAngles.y;
                enemy.Map = enemyObj.Map;
                enemy.Tutorial = enemyObj.Tutorial;
                enemy.EnemyGroupId = enemyObj.EnemyGroup;
                file.EnemyList.Add(enemy);
            }

            foreach (Transform child in Trigger)
            {
                file.TriggerList.Add(new ExploreFileTrigger(Utility.ConvertToVector2Int(child.position), child.name));
            }

            TreasureObject treasureObj;
            foreach (Transform child in Treasure)
            {
                treasureObj = child.gameObject.GetComponent<TreasureObject>();
                ExploreFileTreasure treasure = new ExploreFileTreasure();
                treasure.Height = treasureObj.transform.position.y;
                treasure.Position = Utility.ConvertToVector2Int(treasureObj.transform.position);
                treasure.Rotation = Vector3Int.RoundToInt(treasureObj.transform.localEulerAngles);
                treasure.ItemID = treasureObj.ItemID;
                treasure.Prefab = treasureObj.Prefab;
                file.TreasureList.Add(treasure);
            }

            //ExploreFile file = new ExploreFile(_info);
            DataContext.Instance.Save(file, FileName, DataContext.PrePathEnum.MapExplore);
        }
    }
}