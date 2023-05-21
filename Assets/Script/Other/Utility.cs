using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Utility
{
    public static int ManhattanDistance(Vector2 a, Vector2 b)
    {
        checked
        {
            return (int)(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
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
    public static List<Vector2> GetRange(int range, int width, int height, Vector2 start)
    {
        Vector2 position;
        List<Vector2> list = new List<Vector2>();
        for (int i = (int)start.x - range; i <= start.x + range; i++)
        {
            for (int j = (int)start.y - range; j <= start.y + range; j++)
            {
                position = new Vector3(i, j);
                if (position.y < height && position.y >= 0 && position.x < width && position.x >= 0 && ManhattanDistance(start, position) <= range)
                {
                    list.Add(position);
                }
            }
        }
        return list;
    }

    public static List<Vector2> GetStepList(int step, int width, int height, Vector2 start) 
    {
        int distance;
        List<Vector2> stepList = GetRange(step, width, height, start);
        for (int i = 0; i < stepList.Count; i++)
        {
            distance = PathManager.Instance.GetDistance(start, stepList[i]);
            if (distance - 1 > step || distance == -1)
            {
                stepList.RemoveAt(i);
                i--;
            }
        }

        return stepList;
    }

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}