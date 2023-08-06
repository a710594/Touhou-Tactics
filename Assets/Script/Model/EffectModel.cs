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
        Poison,
        Purify,
        Medicine,
        Sleep,
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
        AreaList = Utility.GetAreaList(Area);
    }
}
