using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : Effect
{
    public BuffEffect(EffectModel data)
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

        if(Data.Target == EffectModel.TargetEnum.Us) 
        {
            hitType = BattleCalculator.HitType.Hit;
        }
        else 
        {
            hitType = BattleCalculator.CheckHit(this, user, target);
        }

        if (hitType != BattleCalculator.HitType.Miss)
        {
            Status status = StatusFactory.GetStatus(Data.StatusType, Data.StatusID);
            target.AddStatus(status);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Other, status.Data.Name);
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
