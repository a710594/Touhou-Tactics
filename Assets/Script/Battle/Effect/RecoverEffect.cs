using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverEffect : Effect
{
    public RecoverEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        FloatingNumberData floatingNumberData;
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
            int recover = Mathf.RoundToInt((float)Value * (float)user.MEN / 100f);
            target.SetRecover(recover);
            floatingNumberData = new FloatingNumberData(FloatingNumberData.TypeEnum.Recover, recover.ToString());
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
            int recover = Mathf.RoundToInt((float)Value * (float)user.MEN / 100f);
            target.SetRecover(recover);
            logList.Add(new Log(user, target, this, hitType, recover.ToString()));
        }
        else
        {
            logList.Add(new Log(user, target, this, hitType, "Miss"));  
        }

        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, logList);
        }
    }
}
