using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverEffect : Effect
{
    public RecoverEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            int recover = Mathf.RoundToInt((float)Value * (float)user.MEN / 100f);
            target.SetRecover(recover);
            logList.Add(new Log(user, target, this, hitType, recover.ToString()));
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
