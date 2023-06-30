using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StatusModel
{
    public enum TypeEnum
    {
        None = -1,
        Provocative = 1,
        ATK,
        DEF,
        MTK,
        MEF,
        SEN,
        AGI,
        MOV, 
        WT,
    }

    public TypeEnum Type;
    public int ID;
    public string Name;
    public string Comment;
    public int Value;
    public int Time;
    public string Area;
    public List<Vector2> AreaList = new List<Vector2>();

    public void GetAreaList()
    {
        string str;
        string[] arr;
        Vector3 v;
        Stack<char> stack = new Stack<char>();
        for (int i = 0; i < Area.Length; i++)
        {
            if (Area[i] != ')')
            {
                stack.Push(Area[i]);
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
                str = Utility.Reverse(str);
                arr = str.Split(',');
                v = new Vector2(int.Parse(arr[0]), int.Parse(arr[1]));
                AreaList.Add(v);
            }
        }
    }
}
