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
            int recover = Mathf.RoundToInt((float)Value * (float)user.Info.MEN / 100f);
            if (Passive.Contains<MiraclePassive>(user.Info.PassiveList))
            {
                recover = MiraclePassive.GetValue(recover);
            }
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

    public override void Use(HitType hitType, BattleCharacterController user, List<Log> logList)
    {
        BattleCharacterController target = null;
        if (hitType != HitType.Miss)
        {
            if (Target == EffectModel.TargetEnum.MinHP) 
            {
                List<BattleCharacterController> list = BattleController.Instance.CharacterAliveList;
                for (int i=0; i<list.Count; i++) 
                {
                    if(list[i].Info.Faction == user.Info.Faction) 
                    {
                        if(target == null || list[i].Info.CurrentHP < target.Info.CurrentHP) 
                        {
                            target = list[i];
                        }
                    }
                }

            }

            int recover = Mathf.RoundToInt((float)Value * (float)user.Info.MEN / 100f);
            if (Passive.Contains<MiraclePassive>(user.Info.PassiveList))
            {
                recover = MiraclePassive.GetValue(recover);
            }
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
