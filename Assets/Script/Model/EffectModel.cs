using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EffectModel
{
    public enum TypeEnum
    {
        None = -1,
        Idle = 0,
        PhysicalAttack,
        MagicAttack,
        Recover,
        Provocative,
        Buff,
    }

    public enum TargetEnum
    {
        Us = 1,
        Them,
        All,
        None, //只能在空地上使用
    }

    public enum TrackEnum 
    {
        None = 1,
        Straight,
        Parabola,
    }

    public TypeEnum Type;
    public int ID;
    public string Name;
    public int Value;
    public int Hit;
    public int Range;
    public string Area;
    public List<Vector2Int> AreaList = new List<Vector2Int>();
    public TargetEnum Target;
    public TrackEnum Track;
    public StatusModel.TypeEnum StatusType;
    public int StatusID;
    public TypeEnum SubType;
    public int SubID;

    public void GetAreaList() 
    {
        string str;
        string[] arr;
        Vector2Int v;
        Stack<char> stack = new Stack<char>();
        for (int i=0; i<Area.Length; i++) 
        {
            if (Area[i] != ')') 
            {
                stack.Push(Area[i]);
            }
            else
            {
                str = "";
                while(stack.Peek() != '(')
                {
                    str += stack.Pop();
                }
                while (stack.Count> 0)
                {
                    stack.Pop();
                }
                str = Utility.Reverse(str);
                arr = str.Split(',');
                v = new Vector2Int(int.Parse(arr[0]), int.Parse(arr[1]));
                AreaList.Add(v);
            }
        }
    }
}
