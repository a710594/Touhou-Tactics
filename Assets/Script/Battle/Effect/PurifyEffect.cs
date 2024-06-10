using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifyEffect : Effect
{
    public PurifyEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            for (int i=0; i<target.StatusList.Count; i++) 
            {
                if(target.StatusList[i] is Poison || target.StatusList[i] is Sleep) //�H���ٷ|����h���������`���A
                {
                    target.StatusList.RemoveAt(i);
                    i--;
                }
            }
            logList.Add(new Log(user, target, this, hitType, "淨化"));
        }
        else
        {
            logList.Add(new Log(user, target, this, hitType, "Miss"));  
        }

        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, logList);
        }
    }
}
