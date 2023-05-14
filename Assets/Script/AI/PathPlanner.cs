using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlanner
{
    private static PathPlanner _instance;
    public static PathPlanner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PathPlanner();
            }
            return _instance;
        }
    }

    private Dictionary<Vector2, int> _nodeDic = new Dictionary<Vector2, int>(); //position, moveCost

    public void LoadData(Dictionary<Vector3, TileInfo> tileInfoDic)
    {
        _nodeDic.Clear();

        foreach (KeyValuePair<Vector3, TileInfo> pair in tileInfoDic) 
        {
            if (pair.Value.MoveCost > 0)
            {
                _nodeDic.Add(new Vector2(pair.Key.x, pair.Key.z), pair.Value.MoveCost);
            }
        }
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 goal)
    {
        if (start == goal)
        {
            return new List<Vector3>();
        }
        else
        {
            List<Vector2> path2 = AStarAlgor.Instance.GetPath(new Vector2(start.z, start.z), new Vector2(goal.x, goal.z), _nodeDic, false);
            List<Vector3> path3 = new List<Vector3>();
            if (path2 != null)
            {
                for (int i = 0; i < path2.Count; i++)
                {
                    path3.Add(new Vector3(path2[i].x, 0, path2[i].y));
                }
            }

            return path3;
        }
    }

    public int GetDistance(Vector3 start, Vector3 goal) 
    {
        int distance = 0;
        if (start == goal)
        {
            return 0;
        }
        else
        {
            List<Vector2> path2 = AStarAlgor.Instance.GetPath(new Vector2(start.z, start.z), new Vector2(goal.x, goal.z), _nodeDic, false);
            if (path2 != null)
            {
                for (int i = 0; i < path2.Count; i++)
                {
                    distance += _nodeDic[path2[i]];
                }
            }

            return distance;
        }
    }
}
