using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : Effect
{
    public BuffEffect(EffectModel data) : base(data)
    {
    }

    public BuffEffect(StatusModel.TypeEnum type, int value, int time)
    {
        Value = value;
        Type = EffectModel.TypeEnum.Medicine;
        Hit = 100;
        Target = EffectModel.TargetEnum.Us;
        Track = EffectModel.TrackEnum.None;
        Status = new Status(type, value, time);
    }

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        FloatingNumberData floatingNumberData;
        BattleController.HitType hitType;

        if(Target == EffectModel.TargetEnum.Us) 
        {
            hitType = BattleController.HitType.Hit;
        }
        else 
        {
            hitType = BattleController.Instance.CheckHit(this, user, target);
        }

        if (hitType != BattleController.HitType.Miss)
        {
            target.AddStatus(Status);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Other, Status.Name);
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

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        BattleController.HitType hitType;

        if (Target == EffectModel.TargetEnum.Us)
        {
            hitType = BattleController.HitType.Hit;
        }
        else
        {
            hitType = BattleController.Instance.CheckHit(this, user, target);
        }

        if (hitType != BattleController.HitType.Miss)
        {
            target.AddStatus(Status);
        }
        else
        {
        }
        logList.Add(new Log(user, target, this, hitType, Status.Name));


        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, logList);
        }
    }
}
