using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�D�]
public class ProvocativeEffect : Effect
{
    public ProvocativeEffect(EffectModel data) : base(data)
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
        BattleController.HitType hitType = BattleController.HitType.Hit; //�D�]���R���P�w����S�O,�H��A��...

        if (hitType != BattleController.HitType.Miss)
        {
            ((ProvocativeStatus)Status).Target = user;
            target.AddStatus(Status);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Other, "�D�]");
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
}
