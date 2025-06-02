using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifyEffect : Effect
{
    public PurifyEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic)
    {
        string text = "";
        if (hitType != HitType.Miss)
        {
            for (int i=0; i<target.Info.StatusList.Count; i++) 
            {
                if(target.Info.StatusList[i] is Poison || target.Info.StatusList[i] is Sleep)
                {
                    target.Info.StatusList.RemoveAt(i);
                    i--;
                }
            }
            text = "淨化";
        }

        if (!floatingNumberDic.ContainsKey(target))
        {
            floatingNumberDic.Add(target, new List<FloatingNumberData>());
        }
        floatingNumberDic[target].Add(new FloatingNumberData(text, Type, hitType));

        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, floatingNumberDic);
        }
    }
}
