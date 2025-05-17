using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Battle;

public class PhysicalAttackEffect : Effect
{
        public PhysicalAttackEffect(EffectModel data) : base(data)
        {
        }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            int damage = BattleController.Instance.GetDamage(this, user, target);
            if (hitType == HitType.Critical)
            {
                damage *= 2;
            }
            target.Info.SetDamage(damage);
            logList.Add(new Log(user, target, Type, hitType, damage.ToString()));
        }
        else
        {
            logList.Add(new Log(user, target, Type, hitType, "Miss"));
        }


        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, logList);
        }
    }
}