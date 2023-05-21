using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AttackEffect : Effect
{
    public AttackEffect(EffectModel data, BattleCharacterInfo user)
    {
        Data = data;
        _user = user;
        if (data.SubType != EffectModel.TypeEnum.None)
        {
            EffectModel subData = DataContext.Instance.EffectDic[data.SubType][data.SubID];
            _subEffect = EffectFactory.GetEffect(subData, user);
        }
    }

    public override void SetEffect(BattleCharacterInfo target)
    {
        target.SetDamage(Mathf.RoundToInt((float)_user.ATK / (float)target.DEF * Data.Value));

        Console.WriteLine(target.Name + ":" + target.CurrentHP);

        base.SetEffect(target);
    }
}