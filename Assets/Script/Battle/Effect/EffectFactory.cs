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
        Effect effect = null;

        if (data.Type == EffectModel.TypeEnum.PhysicalAttack)
        {
            effect = new PhysicalAttackEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.MagicAttack)
        {
            effect = new PhysicalAttackEffect(data);
        }
        else if(data.Type == EffectModel.TypeEnum.Provocative) 
        {
            effect = new ProvocativeEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.Buff)
        {
            effect = new BuffEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.Recover)
        {
            effect = new RecoverEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.Poison)
        {
            effect = new PoisonEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.Purify)
        {
            effect = new PurifyEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.Medicine)
        {
            effect = new MedicineEffect(data);
        }
        else if (data.Type == EffectModel.TypeEnum.Sleep)
        {
            effect = new SleepEffect(data);
        }

        return effect;
    }
}