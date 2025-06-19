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
        public Transform Door;
        public FileManager FileManager;


        public void BuildFile()
        {
            int minX = int.MinValue;
            int maxX = int.MinValue;
            int minY = int.MinValue;
            int maxY = int.MinValue;
            Vector2Int pos;
            ExploreFile file = new ExploreFile();
            ExploreFileTile tile;
            foreach (Transform child in Tilemap)
            {
                pos = Utility.ConvertToVector2Int(child.position);
                tile = new ExploreFileTile(child.name, pos);
                file.TileList.Add(tile);

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
            file.Size = new Vector2Int(maxX - minX, maxY - minY);

            ExploreFileEnemy enemy;
            ExploreFileEnemyObject enemyObj;
            foreach (Transform child in Enemy)
            {
                enemy = new ExploreFileEnemy();
                enemyObj = child.gameObject.GetComponent<ExploreFileEnemyObject>();
                enemy.AI = enemyObj.AiType;
                enemy.Prefab = enemyObj.Prefab;
                enemy.PositionX = enemyObj.transform.position.x;
                enemy.PositionZ = enemyObj.transform.position.z;
                enemy.RotationY = enemyObj.transform.eulerAngles.y;
                enemy.Map = enemyObj.Map;
                enemy.Tutorial = enemyObj.Tutorial;
                enemy.EnemyGroupId = enemyObj.EnemyGroup;
                file.EnemyList.Add(enemy);
            }

            TriggerObject triggerObject;
            foreach (Transform child in Trigger)
            {
                triggerObject = child.GetComponent<TriggerObject>();
                file.EventList.Add(new ExploreFileEvent(Utility.ConvertToVector2Int(child.position), triggerObject.Name));
            }

            TreasureEditor treasureObj;
            foreach (Transform child in Treasure)
            {
                treasureObj = child.gameObject.GetComponent<TreasureEditor>();
                ExploreFileTreasure treasureFile = new ExploreFileTreasure(treasureObj.ItemID, treasureObj.Prefab, Utility.ConvertToVector2Int(treasureObj.transform.position));
                file.TreasureList.Add(treasureFile);
            }

            foreach (Transform child in Door) 
            {
                ExploreFileDoor door = new ExploreFileDoor(Utility.ConvertToVector2Int(child.position));
                file.DoorList.Add(door);
            }

            file.PlayerPositionX = file.Start.x;
            file.PlayerPositionZ = file.Start.y;
            file.PlayerRotationY = 0;

            FileManager.Save(file, FileName, FileManager.PathEnum.MapExplore);
        }
    }
}
