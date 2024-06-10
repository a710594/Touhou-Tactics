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

    public override void Use(HitType hitType, BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            ((ProvocativeStatus)Status).Target = user;
            target.AddStatus(Status);
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
