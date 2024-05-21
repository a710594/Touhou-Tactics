using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackEffect : Effect
{
    public MagicAttackEffect(EffectModel data) : base(data)
    {
        //Data = data;
        //if (data.SubID != -1)
        //{
        //    EffectModel subData = DataContext.Instance.EffectDic[data.SubID];
        //    SubEffect = EffectFactory.GetEffect(subData);
        //}
    }

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
        }

        logList.Add(new Log(this, hitType, ""));


        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, logList);
        }
    }
}
