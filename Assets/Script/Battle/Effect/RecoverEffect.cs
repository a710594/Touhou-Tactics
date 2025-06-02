using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverEffect : Effect
{
    public RecoverEffect(EffectModel data) : base(data)
    {
    }

    public override void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic)
    {
        string text = "";
        if (hitType != HitType.Miss)
        {
            int recover = BattleController.Instance.GetRecover(this, user);
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

    public override void Use(BattleCharacterController user, SubTargetEnum subTarget, Dictionary<BattleCharacterController, List<FloatingNumberData>> floatingNumberDic, out BattleCharacterController target)
    {
        base.Use(user, subTarget, floatingNumberDic, out target);

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

            if (!floatingNumberDic.ContainsKey(target))
            {
                floatingNumberDic.Add(target, new List<FloatingNumberData>());
            }
            floatingNumberDic[target].Add(new FloatingNumberData(recover.ToString(), Type, HitType.Hit));
        }
    }
}
