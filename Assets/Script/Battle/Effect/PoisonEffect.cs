using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    public PoisonEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            target.Info.AddStatus(Status);
            logList.Add(new Log(user, target, this, hitType, Status.Name));
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
