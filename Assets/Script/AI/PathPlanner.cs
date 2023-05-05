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

    public List<Vector2> GetPath(Vector2 start, Vector2 goal)
    {
        if (start == goal)
        {
            return new List<Vector2>();
        }
        else
        {
            List<Vector2> path = AStarAlgor.Instance.GetPath(start, goal, _nodeDic, false);

            return path;
        }
    }
}
