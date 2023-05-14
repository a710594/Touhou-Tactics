using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static int ManhattanDistance(Vector3 a, Vector3 b)
    {
        checked
        {
            return (int)(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z));
        }
    }

    /*
     取得一個範圍內的座標,例如如果range=2,就會像這樣
      x
     xxx
    xxxxx
     xxx
      x
    */
    public static List<Vector3> GetRange(int range, int width, int height, Vector3 start) 
    {
        Vector3 position;
        List<Vector3> list = new List<Vector3>();
        for (int i=(int)start.x-range; i<= start.x + range; i++) 
        {
            for (int j = (int)start.z - range; j <= start.z + range; j++)
            {
                position = new Vector3(i, 0, j);
                if(position.z < height && position.z >=0 && position.x < width && position.x >=0 && ManhattanDistance(start, position) <= range) 
                {
                    list.Add(position);
                }
            }
        }
        return list;
    }
}
