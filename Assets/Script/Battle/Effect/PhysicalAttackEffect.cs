using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Battle;

public class PhysicalAttackEffect : Effect
{
    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        FloatingNumberData floatingNumberData;
        BattleController.HitType hitType = BattleController.Instance.CheckHit(this, user, target);

        if (hitType != BattleController.HitType.Miss)
        {
            int damage = BattleController.Instance.GetDamage(this, user, target);
            if (hitType == BattleController.HitType.Critical)
            {
                damage *= 2;
            }
            target.SetDamage(damage);

            if (hitType == BattleController.HitType.Hit)
            {
                floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Damage, damage.ToString());
            }
            else
            {
                floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Critical, damage.ToString());
            }
        }
        else
        {
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Miss, "Miss");
        }
        floatingList.Add(floatingNumberData);
        

        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, floatingList, characterList);
        }
    }

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        BattleController.HitType hitType = BattleController.Instance.CheckHit(this, user, target);

        if (hitType != BattleController.HitType.Miss)
        {
            int damage = BattleController.Instance.GetDamage(this, user, target);
            if (hitType == BattleController.HitType.Critical)
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


        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, logList);
        }

        if (SelfEffect != null && hitType != BattleController.HitType.Miss)
        {
            SelfEffect.Use(user, user, logList);
        }
    }
}