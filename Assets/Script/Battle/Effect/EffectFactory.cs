using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EffectFactory
{
    public static Effect GetEffect(EffectModel.TypeEnum type, int id, BattleCharacterInfo user)
    {
        return GetEffect(DataContext.Instance.EffectDic[type][id], user);
    }

    public static Effect GetEffect(EffectModel data, BattleCharacterInfo user)
    {
        Effect skill = null;

        if (data.Type == EffectModel.TypeEnum.Attack)
        {
            skill = new AttackEffect(data, user);
        }

        return skill;
    }
}