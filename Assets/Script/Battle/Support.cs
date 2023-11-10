using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support
{
    public int CurrentCD;
    public SupportModel Data;
    public Effect Effect;

    public Support(SupportModel data)
    {
        Data = data;
        Effect = EffectFactory.GetEffect(data.EffectID);
    }

    public virtual void UseEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        Effect.Use(user, target, floatingList, characterList);
        user.HasUseSupport = true;
    }

    public void CheckCD()
    {
        if (CurrentCD > 0)
        {
            CurrentCD--;
        }
    }
}
