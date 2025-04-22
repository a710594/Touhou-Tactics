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
        Target = data.Target;
        AreaType = data.AreaType;
        Track = data.Track;
        Particle = data.Particle;
        Shake = data.Shake;
        ArrayList = Utility.GetAreaList(data.AreaArray);
        Effect = EffectFactory.GetEffect(data.EffectID);
    }
}
