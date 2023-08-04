using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    public PoisonEffect(EffectModel data)
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
        BattleController.HitType hitType = BattleController.CheckHit(this, user, target);

        if (hitType != BattleController.HitType.Miss)
        {
            Poison poison = (Poison)StatusFactory.GetStatus(Data.StatusType, Data.StatusID);
            target.AddStatus(poison);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Poison, poison.Data.Name);
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
