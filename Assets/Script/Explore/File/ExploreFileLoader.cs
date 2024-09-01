using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileLoader
    {
        private static ExploreFileLoader _instance;
        public static ExploreFileLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExploreFileLoader();
                }
                return _instance;
            }
        }

        private LayerMask _mapLayer = LayerMask.NameToLayer("Map");

        public ExploreInfo LoadFile(string name, DataContext.PrePathEnum pathEnum)
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(name, pathEnum);
            SystemManager.Instance.SystemInfo.CurrentFloor = file.Floor;
            ExploreInfo Info = new ExploreInfo(file);

            GameObject gameObj;
            TileObject tileObject;
            TreasureObject treasureObj;
            Transform parent = GameObject.Find("Generator2D").transform;

            for (int i = parent.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
            }

            ExploreInfoTile tile;
            for (int i = 0; i < file.TileList.Count; i++)
            {
                tile = new ExploreInfoTile(file.TileList[i]);
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + tile.Prefab), Vector3.zero, Quaternion.identity);
                tileObject = gameObj.GetComponent<TileObject>();
                tileObject.transform.position = new Vector3(file.TileList[i].Position.x, 0, file.TileList[i].Position.y);
                tileObject.transform.SetParent(parent);
                if (tile.IsVisited)
                {
                    tileObject.Quad.layer = _mapLayer;
                }
                tile.Object = tileObject;
                Info.TileDic.Add(file.TileList[i].Position, tile);
            }

            for (int i = 0; i < file.TreasureList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.TreasureList[i].Prefab), Vector3.zero, Quaternion.identity);
                treasureObj = gameObj.GetComponent<TreasureObject>();
                treasureObj.transform.position = new Vector3(file.TreasureList[i].Position.x, file.TreasureList[i].Height, file.TreasureList[i].Position.y);
                treasureObj.transform.SetParent(parent);
                treasureObj.ItemID = file.TreasureList[i].ItemID;
                treasureObj.Type = file.TreasureList[i].Type;
                tile = Info.TileDic[file.TreasureList[i].Position];
                tile.Treasure = treasureObj;
                tile.IsWalkable = false;
                if (tile.IsVisited)
                {
                    treasureObj.Icon.layer = _mapLayer;
                }
            }

            for (int i = 0; i < file.TriggerList.Count; i++)
            {
                Info.TileDic[file.TriggerList[i].Position].Event = file.TriggerList[i].Name;
            }           

            if (Info.Goal.x != int.MinValue && Info.Goal.y != int.MinValue)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(Info.Goal.x, 0, Info.Goal.y);
                gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
                gameObj.transform.SetParent(parent);
                Info.TileDic[Info.Goal].Object.Icon = gameObj;
                if (Info.TileDic[Info.Goal].IsVisited)
                {
                    Info.TileDic[Info.Goal].Object.Icon.layer = _mapLayer;
                }
                Info.TileDic[Info.Goal].Object.Icon.GetComponent<Goal>().Red.SetActive(!Info.IsArrive);
                Info.TileDic[Info.Goal].Object.Icon.GetComponent<Goal>().Blue.SetActive(Info.IsArrive);
            }

            ExploreInfoEnemy enemy;
            ExploreEnemyController controller;
            for (int i = 0; i < file.EnemyList.Count; i++)
            {
                enemy = new ExploreInfoEnemy(file.EnemyList[i]);
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.EnemyList[i].Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(file.EnemyList[i].Position.x, 1, file.EnemyList[i].Position.y);
                gameObj.transform.SetParent(parent);
                controller = gameObj.GetComponent<ExploreEnemyController>();
                controller.Init(file.EnemyList[i]);
                enemy.Controller = controller;
                Info.EnemyInfoList.Add(enemy);
            }

            return Info;
        }
    }
}