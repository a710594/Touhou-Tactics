using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EffectFactory
{
    public static Effect GetEffect(EffectModel.TypeEnum type, int id)
    {
        return GetEffect(DataContext.Instance.EffectDic[type][id]);
    }

    public static Effect GetEffect(EffectModel data)
    {
        Effect skill = null;

        if (data.Type == EffectModel.TypeEnum.PhysicalAttack)
        {
            skill = new PhysicalAttackEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.MagicAttack)
        {
            skill = new PhysicalAttackEffect(data);
        }
        else if(data.Type == EffectModel.TypeEnum.Provocative) 
        {
            skill = new ProvocativeEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.Buff)
        {
            skill = new BuffEffect(data);
        }

        return skill;
    }
}