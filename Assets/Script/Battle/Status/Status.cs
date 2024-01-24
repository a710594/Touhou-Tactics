using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status :ICloneable
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
        if (type == StatusModel.TypeEnum.STR) 
        {
            Name = "�O�q";
            Comment = "�O�q";
            Icon = "Sword_Up";
        }
        else if(type == StatusModel.TypeEnum.CON) 
        {
            Name = "���";
            Comment = "���";
            Icon = "Shield_Up";
        }
        else if (type == StatusModel.TypeEnum.INT)
        {
            Name = "���O";
            Comment = "���O";
            Icon = "Sword_Up";
        }
        else if (type == StatusModel.TypeEnum.MEN)
        {
            Name = "�믫";
            Comment = "�믫";
            Icon = "Shield_Up";
        }
        else if(type == StatusModel.TypeEnum.SEN) 
        {
            Name = "�F��";
            Comment = "�F��";
        }
        else if (type == StatusModel.TypeEnum.SEN)
        {
            Name = "�ӱ�";
            Comment = "�ӱ�";
        }

        if (value > 100) 
        {
            Name += "�W��";
            Comment += "�W��" + (value - 100) + "%";
        }
        else 
        {
            Name += "�U��";
            Comment += "�U��" + (100 - value) + "%";
        }
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void ResetTurn()
    {
        RemainTime = Time;
    }
}
