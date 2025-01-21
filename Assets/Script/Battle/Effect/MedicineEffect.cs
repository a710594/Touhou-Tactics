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

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            int recover = Value;
            target.Info.SetRecover(recover);
            logList.Add(new Log(user, target, this, hitType, recover.ToString()));
        }
        else
        {
            logList.Add(new Log(user, target, this, hitType, "Miss"));
        }

        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, logList);
        }        
    }
}
