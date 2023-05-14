using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController
{
    private static BattleController _instance;
    public static BattleController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BattleController();
            }
            return _instance;
        }
    }

    public void Init(int width, int height, Dictionary<Vector3, TileComponent> tileComponentDic, Dictionary<Vector3, TileInfo> tileInfoDic, Dictionary<Vector3, GameObject> attachDic) 
    {
        int step = 5;
        int range = 3;
        Vector3 start = new Vector3(5, 0, 5);
        Vector3 target = new Vector3(3, 0, 2);
        List<Vector3> stepList = new List<Vector3>();

        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Character/" + "Reimu_S"), Vector3.zero, Quaternion.identity);
        obj.transform.position = start;
        Camera.main.transform.parent = obj.transform;
        Camera.main.transform.localPosition = obj.transform.position + new Vector3(0, 8, -13);
        Camera.main.transform.localEulerAngles = new Vector3(30, 0, 0);

        //DFS(width, height, step, start, stepList, tileInfoDic);
        stepList = Utility.GetRange(step, width, height, start);
        PathPlanner.Instance.LoadData(tileInfoDic);
        for (int i = 0; i < stepList.Count; i++)
        {
            if (PathPlanner.Instance.GetDistance(start, stepList[i]) - 1 <= step)
            {
                tileComponentDic[stepList[i]].Quad.gameObject.SetActive(true);
            }
        }
    }

    private void DFS(int width, int height, int step, Vector3 position, List<Vector3> visited, Dictionary<Vector3, TileInfo> tileInfoDic)
    {
        visited.Add(position); // 標記頂點v為已訪問

        // 遍歷所有鄰接頂點，如果未訪問則遞迴呼叫DFSUtil函數
        List<Vector3> adjList = GetAdjacentPosition(width, height, position);
        for (int i = 0; i < adjList.Count; i++)
        {
            if (!visited.Contains(adjList[i]) && step - tileInfoDic[adjList[i]].MoveCost >= 0)
            {
                if (tileInfoDic[adjList[i]].MoveCost != -1)
                {
                    DFS(width, height, step - tileInfoDic[adjList[i]].MoveCost, adjList[i], visited, tileInfoDic);
                }
            }
        }
    }

    private static List<Vector3> GetAdjacentPosition(int width, int height, Vector3 position)
    {
        List<Vector3> list = new List<Vector3>();
        if ((position + Vector3.forward).z < height)
        {
            list.Add(position + Vector3.forward);
        }
        if ((position + Vector3.back).z >= 0)
        {
            list.Add(position + Vector3.back);
        }
        if ((position + Vector3.left).x >= 0)
        {
            list.Add(position + Vector3.left);
        }
        if ((position + Vector3.right).x < width)
        {
            list.Add(position + Vector3.right);
        }

        return list;
    }
}
