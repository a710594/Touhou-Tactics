using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianPassive : Passive
{

    //玻璃大砲,滿血時傷害更高
    public MagicianPassive(PassiveModel data)
    {
        Data = data;
    }

    public static float GetValue(BattleCharacterInfo character) 
    {
        return (1 + (character.CurrentHP / (float)character.MaxHP));
    }
}
