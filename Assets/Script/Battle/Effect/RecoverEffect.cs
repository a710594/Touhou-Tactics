using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverEffect : Effect
{
    public RecoverEffect(EffectModel data)
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
        BattleCalculator.HitType hitType;

        if (Data.Target == EffectModel.TargetEnum.Us)
        {
            hitType = BattleCalculator.HitType.Hit;
        }
        else
        {
            hitType = BattleCalculator.CheckHit(this, user, target);
        }

        if (hitType != BattleCalculator.HitType.Miss)
        {
            int recover = Mathf.RoundToInt((float)Data.Value * (float)user.MEF / 100f);
            target.SetRecover(recover);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Recover, recover.ToString());
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
