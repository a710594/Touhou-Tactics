using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Battle;

public static class Utility
{
    public static int ManhattanDistance(Vector2Int a, Vector2Int b)
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

        //range == -1 代表射程無限
        if (range == -1)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    list.Add(new Vector2Int(i, j));
                }
            }
        }
        else
        {
            //BFS
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                position = queue.Dequeue();
                if (!list.Contains(position))
                {
                    list.Add(position);
                }

                if (!list.Contains(position + Vector2Int.up) && (position + Vector2Int.up).y < height && ManhattanDistance(position + Vector2Int.up, start) <= range)
                {
                    queue.Enqueue(position + Vector2Int.up);
                }
                if (!list.Contains(position + Vector2Int.down) && (position + Vector2Int.down).y >= 0 && ManhattanDistance(position + Vector2Int.down, start) <= range)
                {
                    queue.Enqueue(position + Vector2Int.down);
                }
                if (!list.Contains(position + Vector2Int.left) && (position + Vector2Int.left).x >= 0 && ManhattanDistance(position + Vector2Int.left, start) <= range)
                {
                    queue.Enqueue(position + Vector2Int.left);
                }
                if (!list.Contains(position + Vector2Int.right) && (position + Vector2Int.right).x < width && ManhattanDistance(position + Vector2Int.right, start) <= range)
                {
                    queue.Enqueue(position + Vector2Int.right);
                }
            }
        }

        return list;
    }


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

    public static List<Vector2Int> GetAreaList(string area)
    {
        string str;
        string[] arr;
        Vector2Int v;
        Stack<char> stack = new Stack<char>();
        List<Vector2Int> areaList = new List<Vector2Int>();
        for (int i = 0; i < area.Length; i++)
        {
            if (area[i] != ')')
            {
                stack.Push(area[i]);
            }
            else
            {
                str = "";
                while (stack.Peek() != '(')
                {
                    str += stack.Pop();
                }
                while (stack.Count > 0)
                {
                    stack.Pop();
                }
                str = Reverse(str);
                arr = str.Split(',');
                v = new Vector2Int(int.Parse(arr[0]), int.Parse(arr[1]));
                areaList.Add(v);
            }
        }
        return areaList;
    }

    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
        float u, v, S;

        do
        {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }

    public static bool ComparePosition(Vector3 v1, Vector3 v2) 
    {
        if(Mathf.RoundToInt(v1.x) == Mathf.RoundToInt(v2.x) && Mathf.RoundToInt(v1.z) == Mathf.RoundToInt(v2.z)) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public static Effect GetEffect(object obj) 
    {
        if (obj is Skill)
        {
            return ((Skill)obj).Effect;
        }
        else if (obj is Support)
        {
            return ((Support)obj).Effect;
        }
        else if (obj is Card)
        {
            return ((Card)obj).Effect;
        }
        else if (obj is Consumables)
        {
            return ((Consumables)obj).Effect;
        }
        else if (obj is Food)
        {
            return ((Food)obj).Effect;
        }
        else
        {
            return null;
        }
    }

    public static void CheckLine(Vector3 from, Vector3 to, List<BattleCharacterInfo> characterList, Dictionary<Vector2Int, TileAttachInfo> tileDic, out bool isBlock, out Vector3 result)
    {
        isBlock = false;
        int height;
        Vector2Int position;
        List<Vector3> list = DrawLine3D(from, to);
        for (int i = 0; i < list.Count; i++)
        {
            position = ConvertToVector2Int(list[i]);
            if (tileDic.ContainsKey(position))
            {
                height = tileDic[position].Height;
                if (tileDic[position].AttachID != null)
                {
                    height += DataContext.Instance.AttachSettingDic[tileDic[position].AttachID].Height;
                }

                for (int j = 0; j < characterList.Count; j++)
                {
                    if (ConvertToVector2Int(from) != ConvertToVector2Int(characterList[j].Position) && ConvertToVector2Int(to) != ConvertToVector2Int(characterList[j].Position) && position == ConvertToVector2Int(characterList[j].Position))
                    {
                        height++;
                    }
                }

                if (height > list[i].y)
                {
                    isBlock = true;
                    result = list[i];
                    return;
                }
            }
        }

        result = to;
    }

    //和 CheckLine 相似,但是無視 attach 和 character
    public static void CheckThrough(Vector3 from, Vector3 to, Dictionary<Vector2Int, TileAttachInfo> tileDic, out bool isBlock, out Vector3 result)
    {
        isBlock = false;
        int height;
        Vector2Int position;
        List<Vector3> list = DrawLine3D(from, to);
        for (int i = 0; i < list.Count; i++)
        {
            position = ConvertToVector2Int(list[i]);
            if (tileDic.ContainsKey(position))
            {
                height = tileDic[position].Height;

                if (height > list[i].y)
                {
                    isBlock = true;
                    result = list[i];
                    return;
                }
            }
        }

        result = to;
    }

    public static void CheckParabola(Vector3 from, Vector3 to, int parabolaHeight, List<BattleCharacterInfo> characterList, Dictionary<Vector2Int, TileAttachInfo> tileDic, out bool isBlock, out List<Vector3> result)
    {
        isBlock = false;
        int height;
        Vector2Int position;
        List<Vector3> list = DrawParabola(from, to, parabolaHeight, true);
        for (int i = 0; i < list.Count; i++)
        {
            position = ConvertToVector2Int(list[i]);
            if (tileDic.ContainsKey(position))
            {
                height = tileDic[position].Height;
                if (tileDic[position].AttachID != null)
                {
                    height += DataContext.Instance.AttachSettingDic[tileDic[position].AttachID].Height;
                }
                for (int j = 0; j < characterList.Count; j++)
                {
                    if (ConvertToVector2Int(from) != ConvertToVector2Int(characterList[j].Position) && ConvertToVector2Int(to) != ConvertToVector2Int(characterList[j].Position) && position == ConvertToVector2Int(characterList[j].Position))
                    {
                        height++;
                    }
                }

                if (height > list[i].y)
                {
                    isBlock = true;
                    to = list[i];
                    break;
                }
            }
        }

        result = DrawParabola(from, to, parabolaHeight, false);
    }
}