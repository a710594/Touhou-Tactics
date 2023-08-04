using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�D�]
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
        BattleController.HitType hitType = BattleController.HitType.Hit; //�D�]���R���P�w����S�O,�H��A��...

        if (hitType != BattleController.HitType.Miss)
        {
            Status status = StatusFactory.GetStatus(Data.StatusType, Data.StatusID);
            ((ProvocativeStatus)status).Target = user;
            target.AddStatus(status);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Other, "�D�]");
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
