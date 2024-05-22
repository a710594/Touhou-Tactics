using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    public PoisonEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        FloatingNumberData floatingNumberData;
        BattleController.HitType hitType = BattleController.Instance.CheckHit(this, user, target);

        if (hitType != BattleController.HitType.Miss)
        {
            target.AddStatus(Status);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Poison, Status.Name);
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
        BattleController.HitType hitType = BattleController.Instance.CheckHit(this, user, target);

        if (hitType != BattleController.HitType.Miss)
        {
            target.AddStatus(Status);
            logList.Add(new Log(this, hitType, Status.Name));
        }
        else
        {
            logList.Add(new Log(this, hitType, "Miss"));      
        }


        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, logList);
        }
    }
}
