using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverEffect : Effect
{
    public RecoverEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
    {
        if (hitType != HitType.Miss)
        {
            int recover = BattleController.Instance.GetRecover(this, user);
            target.Info.SetRecover(recover);
            logList.Add(new Log(user, target, Type, hitType, recover.ToString()));
        }
        else
        {
            logList.Add(new Log(user, target, Type, hitType, "Miss"));  
        }

        if (SubEffect != null && hitType != HitType.Miss)
        {
            SubEffect.Use(hitType, user, target, logList);
        }
    }

    public override void Use(BattleCharacterController user, SubTargetEnum subTarget, List<Log> logList)
    {
        BattleCharacterController target = null;

        if(subTarget == SubTargetEnum.MinHp) 
        {
            List<BattleCharacterController> list = new List<BattleCharacterController>(BattleController.Instance.CharacterAliveList);
            list.AddRange(BattleController.Instance.CharacterDyingList);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Info.Faction == user.Info.Faction)
                {
                    if (target == null || list[i].Info.CurrentHP < target.Info.CurrentHP)
                    {
                        target = list[i];
                    }
                }
            }

            int recover = BattleController.Instance.GetRecover(this, user);
            target.Info.SetRecover(recover);
            logList.Add(new Log(user, target, Type, HitType.Hit, recover.ToString()));
        }
    }
}
