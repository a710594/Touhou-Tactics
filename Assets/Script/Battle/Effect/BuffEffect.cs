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

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic)
    {
        if (hitType != HitType.Miss)
        {
            target.Info.AddStatus(Status);
        }
        else
        {
        }

        if(!floatingNumberDic.ContainsKey(target))
        {
            floatingNumberDic.Add(target, new List<FloatingNumberData>());
        }
        floatingNumberDic[target].Add(new FloatingNumberData(Status.Name, Type, hitType));


        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, floatingNumberDic);
        }
    }
}
