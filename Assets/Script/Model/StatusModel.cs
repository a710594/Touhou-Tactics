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
        Poison,
        Sleep,
    }

    public TypeEnum Type;
    public int ID;
    public string Name;
    public string Comment;
    public int Value;
    public int Time;
    public string Area;
    public string Icon;
    public List<Vector2Int> AreaList = new List<Vector2Int>();

    public void GetAreaList()
    {
        AreaList = Utility.GetAreaList(Area);
    }
}
