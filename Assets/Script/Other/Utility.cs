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
    public static List<Vector2Int> GetRange(int range, int width, int height, Vector2Int start)
    {
        Vector2Int position;
        List<Vector2Int> list = new List<Vector2Int>();
        for (int i = (int)start.x - range; i <= start.x + range; i++)
        {
            for (int j = (int)start.y - range; j <= start.y + range; j++)
            {
                position = new Vector2Int(i, j);
                if (position.y < height && position.y >= 0 && position.x < width && position.x >= 0 && ManhattanDistance(start, position) <= range)
                {
                    list.Add(position);
                }
            }
        }
        return list;
    }

    //public static List<Vector2> GetStepList(int step, int width, int height, Vector2 start, battlec) 
    //{
    //    int distance;
    //    List<Vector2> stepList = GetRange(step, width, height, start);
    //    for (int i = 0; i < stepList.Count; i++)
    //    {
    //        distance = AStarAlgor.Instance.GetDistance(start, stepList[i]);
    //        if (distance - 1 > step || distance == -1)
    //        {
    //            stepList.RemoveAt(i);
    //            i--;
    //        }
    //    }

    //    return stepList;
    //}

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static Vector2 ConvertToVector2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector2Int ConvertToVector2Int(Vector3 vector3)
    {
        return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.z));
    }

    public static List<Vector2> DrawLine2D(Vector2 a, Vector2 b)
    {
        int dx = (int)Math.Abs(b.x - a.x);
        int dy = (int)Math.Abs(b.y - a.y);
        List<Vector2> list = new List<Vector2>();

        if (dx > dy) //dx 最大
        {
            if (a.x > b.x)
            {
                Vector3 temp = a;
                a = b;
                b = temp;
            }

            int y;
            for (int i = (int)a.x; i <= b.x; i++)
            {
                if (a.y > b.y)
                {
                    y = (int)MathF.Round(a.y - dy * (i - a.x) / dx);
                }
                else
                {
                    y = (int)MathF.Round(a.y + dy * (i - a.x) / dx);
                }
                Console.WriteLine(i + " " + y);
                list.Add(new Vector2(i, y));
            }
        }
        else if (dy > dx) //dy 最大
        {
            if (a.y > b.y)
            {
                Vector3 temp = a;
                a = b;
                b = temp;
            }

            int x;
            for (int i = (int)a.y; i <= b.y; i++)
            {
                if (a.x > b.x)
                {
                    x = (int)MathF.Round(a.x - dx * (i - a.y) / dy);
                }
                else
                {
                    x = (int)MathF.Round(a.x + dx * (i - a.y) / dy);
                }
                Console.WriteLine(x + " " + i);
                list.Add(new Vector2(x, i));
            }
        }

        return list;
    }

    public static List<Vector3> DrawLine3D(Vector3 a, Vector3 b)
    {
        bool reverse = false;
        int dx = (int)Math.Abs(b.x - a.x);
        int dy = (int)Math.Abs(b.y - a.y);
        int dz = (int)Math.Abs(b.z - a.z);
        List<Vector3> list = new List<Vector3>();

        if (dx > dy && dx > dz) //dx 最大
        {
            if (a.x > b.x)
            {
                Vector3 temp = a;
                a = b;
                b = temp;
                reverse = true;
            }

            int y;
            int z;
            for (int i = (int)a.x; i <= b.x; i++)
            {
                if (a.y > b.y)
                {
                    y = (int)MathF.Round(a.y - dy * (i - a.x) / dx);
                }
                else
                {
                    y = (int)MathF.Round(a.y + dy * (i - a.x) / dx);
                }
                if (a.z > b.z)
                {
                    z = (int)MathF.Round(a.z - dz * (i - a.x) / dx);
                }
                else
                {
                    z = (int)MathF.Round(a.z + dz * (i - a.x) / dx);
                }
                Console.WriteLine(i + " " + y + " " + z);
                list.Add(new Vector3(i, y, z));
            }
        }
        else if (dy > dx && dy > dz) //dy 最大
        {
            if (a.y > b.y)
            {
                Vector3 temp = a;
                a = b;
                b = temp;
                reverse = true;
            }


            int x;
            int z;
            for (int i = (int)a.y; i <= b.y; i++)
            {
                if (a.x > b.x)
                {
                    x = (int)MathF.Round(a.x - dx * (i - a.y) / dy);
                }
                else
                {
                    x = (int)MathF.Round(a.x + dx * (i - a.y) / dy);
                }
                if (a.z > b.z)
                {
                    z = (int)MathF.Round(a.z - dz * (i - a.y) / dy);
                }
                else
                {
                    z = (int)MathF.Round(a.z + dz * (i - a.y) / dy);
                }
                Console.WriteLine(x + " " + i + " " + z);
                list.Add(new Vector3(x, i, z));
            }
        }
        else //dz 最大
        {
            if (a.z > b.z)
            {
                Vector3 temp = a;
                a = b;
                b = temp;
                reverse = true;
            }

            int x;
            int y;
            for (int i = (int)a.z; i <= b.z; i++)
            {
                if (a.x > b.x)
                {
                    x = (int)MathF.Round(a.x - dx * (i - a.z) / dz);
                }
                else
                {
                    x = (int)MathF.Round(a.x + dx * (i - a.z) / dz);
                }
                if (a.y > b.y)
                {
                    y = (int)MathF.Round(a.y - dy * (i - a.z) / dz);
                }
                else
                {
                    y = (int)MathF.Round(a.y + dy * (i - a.z) / dz);
                }
                list.Add(new Vector3(x, y, i));
            }
        }

        if(reverse)
        {
            list.Reverse();
        }

        return list;
    }

    public static List<Vector3> DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2, int count)
    {
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i <= count; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            list.Add(B);
            t += (1 / (float)count);
        }

        return list;
    }

    public static List<Vector3> DrawParabola(Vector3 point0, Vector3 point1, int height, bool isInt)
    {
        List<Vector3> line = DrawLine3D(point0, point1);
        Vector3 center = new Vector3((int)Math.Ceiling((point0.x + point1.x) / 2f), height * 2, (int)Math.Ceiling((point0.z + point1.z) / 2f));
        List<Vector3> bezier = new List<Vector3>();
        if (isInt)
        {
            bezier = DrawQuadraticBezierCurve(point0, center, point1, line.Count - 1);
            for (int i=0; i<bezier.Count; i++) 
            {
                bezier[i] = new Vector3(Mathf.RoundToInt(bezier[i].x), Mathf.RoundToInt(bezier[i].y), Mathf.RoundToInt(bezier[i].z));
            }
        }
        else
        {
            bezier = DrawQuadraticBezierCurve(point0, center, point1, 10);
        }

        return bezier;
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(start);
        GL.Vertex(end);
        GL.End();
    }
}