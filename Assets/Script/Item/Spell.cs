using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class Spell
{
    public int CurrentCD;
    public SpellModel Data;
    public Effect Effect;

    public Spell() { }

    public Spell(SpellModel data)
    {
        Data = data;
        Effect = EffectFactory.GetEffect(data.EffectID);
    }

    public void CheckCD() 
    {
        if (CurrentCD > 0) 
        {
            CurrentCD--;
        }
    }
}
