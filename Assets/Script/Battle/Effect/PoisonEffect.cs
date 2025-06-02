using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    public PoisonEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic)
    {
        string text = "";
        if (hitType != HitType.Miss)
        {
            target.Info.AddStatus(Status);
            text = Status.Name;
        }

        if (!floatingNumberDic.ContainsKey(target))
        {
            floatingNumberDic.Add(target, new List<FloatingNumberData>());
        }
        floatingNumberDic[target].Add(new FloatingNumberData(text, Type, hitType));


        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, floatingNumberDic);
        }
    }
}
