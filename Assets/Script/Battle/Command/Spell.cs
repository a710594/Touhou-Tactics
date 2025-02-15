using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Spell : Command
{
    public Spell() { }

    public Spell(SpellModel data)
    {
        ID = data.ID;
        Name = data.Name;
        Comment = data.Comment;
        Hit = data.Hit;
        Range = data.Range;
        RangeTarget = data.RangeTarget;
        AreaTarget = data.AreaTarget;
        AreaType = data.AreaType;
        Track = data.Track;
        ArrayList = Utility.GetAreaList(data.Array);

        Effect = EffectFactory.GetEffect(data.EffectID);

        if(data.SubSpellID != -1)
        {
            SubCommand = new Spell(DataContext.Instance.SpellDic[data.SubSpellID]);
        }
    }
}
