using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挑釁
public class ProvocativeEffect : Effect
{
    public ProvocativeEffect(EffectModel data)
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
        BattleController.HitType hitType = BattleController.HitType.Hit; //挑釁的命中判定比較特別,以後再說...

        if (hitType != BattleController.HitType.Miss)
        {
            Status status = StatusFactory.GetStatus(Data.StatusType, Data.StatusID);
            ((ProvocativeStatus)status).Target = user;
            target.AddStatus(status);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Other, "挑釁");
        }
        else
        {
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Miss, "Miss");
        }
        floatingList.Add(floatingNumberData);


        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.SetEffect(user, target, floatingList, characterList);
        }
    }
}
