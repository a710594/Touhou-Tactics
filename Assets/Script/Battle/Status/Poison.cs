using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Status
{
    public Poison(StatusModel data)
    {
        Data = data;
        RemainTime = data.Time;
    }

    public int GetDamage(BattleCharacterInfo target) 
    {
        return Mathf.RoundToInt(target.MaxHP * Data.Value / 100f);
    }
}
