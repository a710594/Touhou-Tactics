using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfInjuryEffect : Effect
{
    public SelfInjuryEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        BattleController.HitType hitType = BattleController.HitType.Hit;

        int damage = Mathf.RoundToInt((float)Value * (float)user.MaxHP / 100f);
        user.SetDamage(damage);
        logList.Add(new Log(user, user, this, hitType, damage.ToString()));

        if (SubEffect != null)
        {
            SubEffect.Use(user, target, logList);
        }
    }
}
