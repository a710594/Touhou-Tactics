using Explore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreMapBuilder : MonoBehaviour
{
    public int Floor;
    public int Seed;

    private LayerMask _mapLayer;

    public ExploreInfo GetFixed(int floor) 
    {
        FixedFloorModel data = DataContext.Instance.FixedFloorDic[floor];
        ExploreFile file = DataContext.Instance.Load<ExploreFile>(data.Name, DataContext.PrePathEnum.MapExplore);
        ExploreInfo info = CreateObject(file);

        return info;
    }

    public ExploreInfo GetRandom(int floor, int seed) 
    {
        RandomFloorModel data = DataContext.Instance.RandomFloorDic[floor];
        ExploreFile file = ExploreFileRandomGenerator.Instance.Create(data, seed);
        ExploreInfo info = CreateObject(file);

        return info;
    }

    public ExploreInfo CreateObject(ExploreFile file)
    {
        GameObject gameObj;
        TileObject tileObj;
        TreasureObject treasureObj;
        Transform parent = GameObject.Find("Generator2D").transform;

        for (int i = parent.childCount; i > 0; --i)
        {
            GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
        }

        ExploreInfo info = new ExploreInfo(file);

        for (int i = 0; i < file.TileList.Count; i++)
        {
            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + file.TileList[i].Prefab), Vector3.zero, Quaternion.identity);
            tileObj = gameObj.GetComponent<TileObject>();
            tileObj.transform.position = new Vector3(file.TileList[i].Position.x, 0, file.TileList[i].Position.y);
            tileObj.transform.SetParent(parent);
            if (file.TileList[i].IsVisited)
            {
                tileObj.Quad.layer = _mapLayer;
            }
            file.TileList[i].Object = tileObj;
            info.TileDic.Add(file.TileList[i].Position, file.TileList[i]);
        }

        for (int i = 0; i < file.TreasureList.Count; i++)
        {
            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.TreasureList[i].Prefab), Vector3.zero, Quaternion.identity);
            treasureObj = gameObj.GetComponent<TreasureObject>();
            file.TreasureList[i].Object = treasureObj;
            treasureObj.transform.position = new Vector3(file.TreasureList[i].Position.x, file.TreasureList[i].Height, file.TreasureList[i].Position.y);
            treasureObj.transform.SetParent(parent);
            info.TileDic[file.TreasureList[i].Position].Treasure = file.TreasureList[i];
            info.TileDic[file.TreasureList[i].Position].IsWalkable = false;
            if (info.TileDic[file.TreasureList[i].Position].IsVisited && treasureObj.Icon != null)
            {
                treasureObj.Icon.layer = _mapLayer;
            }

        }

        for (int i = 0; i < file.TriggerList.Count; i++)
        {
            info.TileDic[file.TriggerList[i].Position].Event = file.TriggerList[i].Name;
        }

        gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Start_Cube"), Vector3.zero, Quaternion.identity);
        gameObj.transform.position = new Vector3(file.Start.x, 1, file.Start.y);
        gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
        gameObj.transform.SetParent(parent);

        //gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/GoalParticle"), Vector3.zero, Quaternion.identity);
        gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal_Cube"), Vector3.zero, Quaternion.identity);
        gameObj.transform.position = new Vector3(file.Goal.x, 1, file.Goal.y);
        gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
        gameObj.transform.SetParent(parent);
        info.TileDic[file.Goal].Object.Icon = gameObj;
        if (info.TileDic[file.Goal].IsVisited)
        {
            info.TileDic[file.Goal].Object.Icon.layer = _mapLayer;
        }
        //info.TileDic[file.Goal].Object.Icon.GetComponent<Goal>().Red.SetActive(!file.IsArrive);
        //info.TileDic[file.Goal].Object.Icon.GetComponent<Goal>().Blue.SetActive(file.IsArrive);

        ExploreEnemyController controller;
        for (int i = 0; i < file.EnemyList.Count; i++)
        {
            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.EnemyList[i].Prefab), Vector3.zero, Quaternion.identity);
            gameObj.transform.position = new Vector3(file.EnemyList[i].Position.x, 1, file.EnemyList[i].Position.y);
            gameObj.transform.eulerAngles = new Vector3(0, file.EnemyList[i].RotationY, 0);
            gameObj.transform.SetParent(parent);
            controller = gameObj.GetComponent<ExploreEnemyController>();
            controller.SetAI(file.EnemyList[i]);
            info.TileDic[file.EnemyList[i].Position].IsWalkable = false;
            file.EnemyList[i].Controller = controller;
        }

        float x = 1;
        float y = 1;
        if (file.Size.x > 60)
        {
            x = file.Size.x / 60f;
        }
        if (file.Size.y > 60)
        {
            y = file.Size.y / 60f;
        }

        //ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
        //exploreUI.SetCameraPosition(file.Size.x / 2, file.Size.y / 2, x);

        return info;
    }

    private void Awake()
    {
        _mapLayer = LayerMask.NameToLayer("Map");
    }
}
