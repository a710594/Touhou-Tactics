using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : Effect
{
    public BuffEffect(EffectModel data) : base(data)
    {
    }

    public BuffEffect(StatusModel.TypeEnum type, int value, int time)
    {
        Value = value;
        Type = EffectModel.TypeEnum.Medicine;
        Status = new Status(type, value, time);
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            target.Info.AddStatus(Status);
        }
        else
        {
        }
        logList.Add(new Log(user, target, this, hitType, Status.Name));


        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, logList);
        }
    }
}
