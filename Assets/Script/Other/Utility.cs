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

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static Vector2Int ConvertToVector2Int(Vector3 v3)
    {
        return new Vector2Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.z));
    }

    public static Vector3 ConvertToVector3(Vector2Int v2)
    {
        return new Vector3(v2.x, 0, v2.y);
    }

    public static List<Vector2Int> DrawLine2D(Vector2Int a, Vector2Int b)
    {
        int dx = (int)Math.Abs(b.x - a.x);
        int dy = (int)Math.Abs(b.y - a.y);
        List<Vector2Int> list = new List<Vector2Int>();

        if (dx > dy) //dx 最大
        {
            if (a.x > b.x)
            {
                Vector2Int temp = a;
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
                list.Add(new Vector2Int(i, y));
            }
        }
        else if (dy > dx) //dy 最大
        {
            if (a.y > b.y)
            {
                Vector2Int temp = a;
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
                list.Add(new Vector2Int(x, i));
            }
        }

        return list;
    }

    public static List<Vector3> DrawLine3D(Vector3 a, Vector3 b)
    {
        bool reverse = false;
        float dx = Math.Abs(b.x - a.x);
        float dy = Math.Abs(b.y - a.y);
        float dz = Math.Abs(b.z - a.z);
        List<Vector3> list = new List<Vector3>();

        if (dx > dz) //dx 最大
        {
            if (a.x > b.x)
            {
                Vector3 temp = a;
                a = b;
                b = temp;
                reverse = true;
            }

            float y;
            float z;
            for (int i = (int)a.x; i <= b.x; i++)
            {
                if (a.y > b.y)
                {
                    y = a.y - dy * (i - a.x) / dx;
                }
                else
                {
                    y = a.y + dy * (i - a.x) / dx;
                }
                if (a.z > b.z)
                {
                    z = a.z - dz * (i - a.x) / dx;
                }
                else
                {
                    z = a.z + dz * (i - a.x) / dx;
                }
                list.Add(new Vector3(i, y, z));
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

            float x;
            float y;
            for (int i = (int)a.z; i <= b.z; i++)
            {
                if (a.x > b.x)
                {
                    x = a.x - dx * (i - a.z) / dz;
                }
                else
                {
                    x = a.x + dx * (i - a.z) / dz;
                }
                if (a.y > b.y)
                {
                    y = a.y - dy * (i - a.z) / dz;
                }
                else
                {
                    y = a.y + dy * (i - a.z) / dz;
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

    public static List<Vector3> DrawParabola(Vector3 point0, Vector3 point1, int height, int count)
    {
        Vector3 center = new Vector3((point0.x + point1.x) / 2f, height, (point0.z + point1.z) / 2f);
        List<Vector3> bezier = DrawQuadraticBezierCurve(point0, center, point1, count);

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
        if(area!=null)
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
        else
        {
            return null;
        }
    }

    public static Vector3Int GetVector3Int(string str)
    {
        Vector3Int position = new Vector3Int();
        string[] arr;
        Stack<char> stack = new Stack<char>();

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != ')')
            {
                stack.Push(str[i]);
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
                position = new Vector3Int(int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]));
            }
        }
        return position;
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
        else if (obj is Sub)
        {
            return ((Sub)obj).Effect;
        }
        else if (obj is Spell)
        {
            return ((Spell)obj).Effect;
        }
        else if (obj is Battle.ItemCommand)
        {
            return ((Battle.ItemCommand)obj).Effect;
        }
        else
        {
            return null;
        }
    }

    //和 CheckLine 相似,但是無視地型
    public static void CheckThrough(Vector3 from, Vector3 to, Dictionary<Vector2Int, BattleInfoTile> tileDic, out bool isBlock, out Vector3 result)
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
                height = tileDic[position].TileData.Height;

                if (height - list[i].y > 1)
                {
                    isBlock = true;
                    result = list[i];
                    return;
                }
            }
        }

        result = to;
    }

    public static bool GetRandomPosition(int minX, int maxX, int minY, int maxY, List<Vector2Int> invalidList, out Vector2Int result)
    {
        int count = 0;
        Vector2Int v2;
        while (true)
        {
            v2 = new Vector2Int(Mathf.RoundToInt(Utility.RandomGaussian(minX, maxX)), Mathf.RoundToInt(Utility.RandomGaussian(minY, maxY)));
            if(!invalidList.Contains(v2))
            {
                result = v2;
                return true;
            }
            else
            {
                count++;
                if (count > 100) 
                {
                    result = new Vector2Int();
                    return false;
                }
            }
        }
    }

    public static Vector3 GetRotateAroundPosition(Vector3 point, Vector3 pivot, Vector3 axis, float angle)
    {
        // 計算旋轉後的位置
        return Quaternion.AngleAxis(angle, axis) * (point - pivot) + pivot;
    }

    public static Quaternion GetRotateAroundRotation(Quaternion currentRotation, Vector3 axis, float angle)
    {
        // 計算旋轉後的 Quaternion
        return Quaternion.AngleAxis(angle, axis) * currentRotation;
    }

    private static float doubleClickTime = 0.2f;
    private static float lastClickTime = 0;
    public static bool GetMouseButtonDoubleClick(int n)
    {
        if (Input.GetMouseButtonDown(n))
        {
            bool result = false;
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickTime)
                result = true;

            lastClickTime = Time.time;
            return result;
        }
        return false;
    }

    public static bool LookPlayer(Transform transform, float angle, int amount) 
    {
        float subAngle = angle / amount;
        Vector3 direction;
        for (int i=-amount; i<=amount; i++) 
        {
            direction = Quaternion.AngleAxis(subAngle * i, Vector3.up) * transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 5) && hit.collider.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    public static Vector2Int GenerateNormalPoints(Vector2Int center, float stdDev, int minX, int maxX, int minY, int maxY, List<Vector2Int> invalidList) 
    {
        Vector2Int result = new Vector2Int();

        while(true)
        {
            float u1 = 1f - UnityEngine.Random.Range(0, 1f); // Uniform(0,1] random doubles
            float u2 = 1f - UnityEngine.Random.Range(0, 1f);
            float randStdNormal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2)); // Standard normal
            float randStdNormal2 = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2)); // Second value

            float x = center.x + stdDev * randStdNormal;
            float y = center.y + stdDev * randStdNormal2;

            result = new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));

            if (!invalidList.Contains(result) && result.x >= minX && result.x <= maxX && result.y >= minY && result.y <=maxY) 
            {
                break;
            }
        }

        return result;
    }

    public static float GetRandomAngle() 
    {
        int random = UnityEngine.Random.Range(0, 4);
        if (random == 0) 
        {
            return 0;
        }
        else if (random == 1) 
        {
            return 90;
        }
        else if(random == 2) 
        {
            return 180;
        }
        else 
        {
            return 270;
        }
    }
}