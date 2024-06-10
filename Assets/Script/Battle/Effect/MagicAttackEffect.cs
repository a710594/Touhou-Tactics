using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackEffect : Effect
{
    public MagicAttackEffect(EffectModel data) : base(data)
    {
    }
    public override void Use(HitType hitType, BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            int damage = BattleController.Instance.GetDamage(this, user, target);
            if (hitType == HitType.Critical)
            {
                damage *= 2;
            }
            target.SetDamage(damage);
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
