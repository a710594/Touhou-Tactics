using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�D�]
public class ProvocativeEffect : Effect
{
    public ProvocativeEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            ((Provocative)Status).Target = user;
            target.Info.AddStatus(Status);
            logList.Add(new Log(user, target, Type, hitType, Status.Name));
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
