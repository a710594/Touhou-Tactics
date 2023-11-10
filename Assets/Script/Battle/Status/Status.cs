using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public StatusModel.TypeEnum Type;
    public string Name;
    public string Comment;
    public int Value;
    public int Time;
    public int RemainTime;
    public string Icon;
    public List<Vector2Int> AreaList = new List<Vector2Int>();

    public Status() { }

    public Status(StatusModel data)
    {
        Type = data.Type;
        Name = data.Name;
        Comment = data.Comment;
        Value = data.Value;
        Time = data.Time;
        RemainTime = data.Time;
        Icon = data.Icon;
        AreaList = data.AreaList;
    }

    public Status(StatusModel.TypeEnum type, int value, int time) 
    {
        Type = type;
        Value = value;
        Time = time;
        RemainTime = time;
        AreaList = new List<Vector2Int>() { Vector2Int.zero };
        if (type == StatusModel.TypeEnum.ATK) 
        {
            Name = "力量";
            Comment = "力量";
        }
        else if(type == StatusModel.TypeEnum.DEF) 
        {
            Name = "體質";
            Comment = "體質";
        }
        else if (type == StatusModel.TypeEnum.MTK)
        {
            Name = "智力";
            Comment = "智力";
        }
        else if (type == StatusModel.TypeEnum.MEF)
        {
            Name = "精神";
            Comment = "精神";
        }
        else if(type == StatusModel.TypeEnum.SEN) 
        {
            Name = "靈巧";
            Comment = "靈巧";
        }
        else if (type == StatusModel.TypeEnum.SEN)
        {
            Name = "敏捷";
            Comment = "敏捷";
        }

        if (value > 100) 
        {
            Name += "上升";
            Comment += "上升" + (value - 100) + "%";
        }
        else 
        {
            Name += "下降";
            Comment += "下降" + (100 - value) + "%";
        }
    }

    public void ResetTurn()
    {
        RemainTime = Time;
    }
}
