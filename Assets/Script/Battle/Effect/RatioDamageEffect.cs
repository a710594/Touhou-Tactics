using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatioDamageEffect : Effect
{
    public RatioDamageEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic)
    {
        string text = "";
        if (hitType == HitType.Hit)
        {
            int damage = Mathf.RoundToInt((float)Value * (float)target.Info.MaxHP / 100f);
            target.Info.SetDamage(damage);
            text = damage.ToString();
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
