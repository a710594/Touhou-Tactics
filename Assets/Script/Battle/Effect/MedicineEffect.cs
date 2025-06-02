using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�T�w�^��
public class MedicineEffect : Effect
{
    public MedicineEffect(EffectModel data) : base(data)
    {
    }

    //for food
    public MedicineEffect(int value)
    {
        Value = value;
        Type = EffectModel.TypeEnum.Medicine;
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic)
    {
        string text = "";
        if (hitType != HitType.Miss)
        {
            int recover = Value;
            target.Info.SetRecover(recover);
            text = recover.ToString();
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
