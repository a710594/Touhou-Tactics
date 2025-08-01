using Explore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class ExploreMapBuilder : MonoBehaviour
{
    public int Floor;
    public int Seed;
    public FileManager FileManager;

    private LayerMask _mapLayer;

    public void GetFixed(ExploreFile file, out Dictionary<Vector2Int, ExploreFileTile> tileDic)
    {
        CreateObject(file, out tileDic);
    }

    public void GetRandom(ExploreFile file, out Dictionary<Vector2Int, ExploreFileTile> tileDic) 
    {
        CreateObject(file, out tileDic);
    }

    private void CreateObject(ExploreFile file, out Dictionary<Vector2Int, ExploreFileTile> tileDic)
    {
        GameObject gameObj;
        //TileObject tileObj;
        TreasureObject treasureObj;
        Transform parent = GameObject.Find("Generator2D").transform;
        tileDic = new Dictionary<Vector2Int, ExploreFileTile>();

        for (int i = parent.childCount; i > 0; --i)
        {
            GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
        }

        for (int i = 0; i < file.TileList.Count; i++)
        {
            if (file.TileList[i].Prefab == "Wall")
            {
                file.TileList[i].Prefab = "Wall_Unlit";
            }
            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + file.TileList[i].Prefab), Vector3.zero, Quaternion.identity);
            //tileObj = gameObj.GetComponent<TileObject>();
            gameObj.transform.position = new Vector3(file.TileList[i].Position.x, 0, file.TileList[i].Position.y);
            gameObj.transform.SetParent(parent);
            //if (file.TileList[i].IsVisited)
            //{
            //    tileObj.Quad.layer = _mapLayer;
            //}
            //file.TileList[i].Object = tileObj;
            tileDic.Add(file.TileList[i].Position, file.TileList[i]);
        }

        for (int i = 0; i < file.TreasureList.Count; i++)
        {
            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + file.TreasureList[i].Prefab), Vector3.zero, Quaternion.identity);
            treasureObj = gameObj.GetComponent<TreasureObject>();
            file.TreasureList[i].Object = treasureObj;
            treasureObj.transform.position = new Vector3(file.TreasureList[i].Position.x, file.TreasureList[i].Height, file.TreasureList[i].Position.y);
            treasureObj.transform.SetParent(parent);
            tileDic[file.TreasureList[i].Position].Treasure = file.TreasureList[i];
            tileDic[file.TreasureList[i].Position].IsWalkable = false;
            if (tileDic[file.TreasureList[i].Position].IsVisited && treasureObj.Icon != null)
            {
                treasureObj.Icon.layer = _mapLayer;
            }

        }

        for (int i = 0; i < file.TriggerList.Count; i++)
        {
            tileDic[file.TriggerList[i].Position].Event = file.TriggerList[i].Name;
        }

        for (int i=0; i<file.DoorList.Count; i++) 
        {
            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Door_Cube"), Vector3.zero, Quaternion.identity);
            gameObj.transform.position = new Vector3(file.DoorList[i].Position.x, 1, file.DoorList[i].Position.y);
            gameObj.transform.SetParent(parent);
        }

        gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Start_Cube"), Vector3.zero, Quaternion.identity);
        gameObj.transform.position = new Vector3(file.Start.x, 1, file.Start.y);
        gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
        gameObj.transform.SetParent(parent);

        gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal_Cube"), Vector3.zero, Quaternion.identity);
        gameObj.transform.position = new Vector3(file.Goal.x, 1, file.Goal.y);
        gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
        gameObj.transform.SetParent(parent);

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
    }

    private void Awake()
    {
        _mapLayer = LayerMask.NameToLayer("Map");
    }
}*/
