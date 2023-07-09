using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager
{
    private static PathManager _instance;
    public static PathManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PathManager();
            }
            return _instance;
        }
    }

    public Dictionary<Vector2Int, TileInfo> _tileInfoDic;

    public void LoadData(Dictionary<Vector2Int, TileInfo> tileInfoDic)
    {
        _tileInfoDic = tileInfoDic;
    }

    //public List<Vector2> GetPath(Vector2 start, Vector2 goal)
    //{
    //    if (start == goal)
    //    {
    //        return new List<Vector2>();
    //    }
    //    else
    //    {
    //        List<Vector2> path2 = AStarAlgor.Instance.GetPath(start, goal, _tileInfoDic, false);
    //        return path2;
    //    }
    //}

    //public int GetDistance(Vector2 start, Vector2 goal)
    //{
    //    int distance = 0;
    //    if(_tileInfoDic[goal].MoveCost == -1) 
    //    {
    //        return -1;
    //    }
    //    else if (start == goal)
    //    {
    //        return 0;
    //    }
    //    else
    //    {
    //        List<Vector2> path2 = AStarAlgor.Instance.GetPath(start, goal, _tileInfoDic, false);
    //        if (path2 != null)
    //        {
    //            for (int i = 0; i < path2.Count; i++)
    //            {
    //                distance += _tileInfoDic[path2[i]].MoveCost;
    //            }
    //        }
    //        else
    //        {
    //            distance = -1;
    //        }

    //        return distance;
    //    }
    //}
}
