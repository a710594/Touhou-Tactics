using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverAllEffect : Effect
{
    public RecoverAllEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
    {
        int recover = user.MaxHP - user.CurrentHP;
        target.SetRecover(recover);
        logList.Add(new Log(user, target, this, hitType, recover.ToString()));

        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, logList);
        }
    }
}
