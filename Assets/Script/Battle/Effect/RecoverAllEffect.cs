using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverAllEffect : Effect
{
    public RecoverAllEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        BattleController.HitType hitType = BattleController.HitType.Hit;

        int recover = user.MaxHP - user.CurrentHP;
        target.SetRecover(recover);
        logList.Add(new Log(user, target, this, hitType, recover.ToString()));

        if (SubEffect != null && hitType != BattleController.HitType.Miss)
        {
            SubEffect.Use(user, target, logList);
        }
    }
}
