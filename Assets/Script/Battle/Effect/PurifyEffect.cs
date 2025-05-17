using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifyEffect : Effect
{
    public PurifyEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            for (int i=0; i<target.Info.StatusList.Count; i++) 
            {
                if(target.Info.StatusList[i] is Poison || target.Info.StatusList[i] is Sleep) //�H���ٷ|����h���������`���A
                {
                    target.Info.StatusList.RemoveAt(i);
                    i--;
                }
            }
            logList.Add(new Log(user, target, Type, hitType, "淨化"));
        }
        else
        {
            logList.Add(new Log(user, target, Type, hitType, "Miss"));  
        }

        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, logList);
        }
    }
}
