using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifyEffect : Effect
{
    public PurifyEffect(EffectModel data)
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
            for (int i=0; i<target.StatusList.Count; i++) 
            {
                if(target.StatusList[i] is Poison) //以後還會有更多種類的異常狀態
                {
                    target.StatusList.RemoveAt(i);
                    i--;
                }
            }
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Recover, "淨化");
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
