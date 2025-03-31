using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatioDamageEffect : Effect
{
    public RatioDamageEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        // if(user!=target)
        // {
        //     hitType = BattleController.Instance.CheckHit(hit, user, target, true);
        // }
        // else
        // {
        //     hitType = BattleController.HitType.Hit;
        // }

        if (hitType == HitType.Hit)
        {
            int damage = Mathf.RoundToInt((float)Value * (float)target.Info.MaxHP / 100f);
            target.Info.SetDamage(damage);
            logList.Add(new Log(user, target, this, hitType, damage.ToString()));
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
