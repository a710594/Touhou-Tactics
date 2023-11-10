using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Effect
{
    //public EffectModel Data;
    public EffectModel.TypeEnum Type;
    public int Value;
    public int Hit;
    public int Range;
    public EffectModel.TargetEnum Target;
    public EffectModel.TrackEnum Track;
    public string Area;
    public List<Vector2Int> AreaList = new List<Vector2Int>();
    public Status Status;

    public Effect SubEffect;

    public Effect() { }

    public Effect(EffectModel data)
    {
        //Data = data;
        Type = data.Type;
        Value = data.Value;
        Hit = data.Hit;
        Range = data.Range;
        Target = data.Target;
        Track = data.Track;
        Area = data.Area;
        AreaList = data.AreaList;
        if (data.StatusID != -1)
        {
            Status = StatusFactory.GetStatus(data.StatusID);
        }
        if (data.SubID != -1)
        {
            SubEffect = EffectFactory.GetEffect(data.SubID);
        }
    }

    public virtual void  Use(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList) 
    {
    }
}