using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Battle;

public class PhysicalAttackEffect : Effect
{
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

        if (SelfEffect != null && hitType != HitType.Miss)
        {
            SelfEffect.Use(hitType, user, user, logList);
        }
    }
}