using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackEffect : Effect
{
    public MagicAttackEffect(EffectModel data)
    {
        Data = data;
        if (data.SubType != EffectModel.TypeEnum.None)
        {
            EffectModel subData = DataContext.Instance.EffectDic[data.SubType][data.SubID];
            SubEffect = EffectFactory.GetEffect(subData);
        }
    }

    public override void SetEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        FloatingNumberData floatingNumberData;
        BattleCalculator.HitType hitType = BattleCalculator.CheckHit(this, user, target);

        if (hitType != BattleCalculator.HitType.Miss)
        {
            int damage = BattleCalculator.GetDamage(this, user, target, characterList);
            if (hitType == BattleCalculator.HitType.Critical)
            {
                damage *= 2;
            }
            target.SetDamage(damage);

            if (hitType == BattleCalculator.HitType.Hit)
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


        if (SubEffect != null && hitType != BattleCalculator.HitType.Miss)
        {
            SubEffect.SetEffect(user, target, floatingList, characterList);
        }
    }
}
