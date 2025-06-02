using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackEffect : Effect
{
    public MagicAttackEffect(EffectModel data) : base(data)
    {
    }
    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic)
    {
        string text = "";
        if (hitType != HitType.Miss)
        {
            int damage = BattleController.Instance.GetDamage(this, user, target);
            if (hitType == HitType.Critical)
            {
                damage *= 2;
            }
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
