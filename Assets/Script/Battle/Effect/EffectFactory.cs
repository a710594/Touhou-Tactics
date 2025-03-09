using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class EffectFactory
    {
        public static Effect GetEffect(int id)
        {
            return GetEffect(DataTable.Instance.EffectDic[id]);
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
                effect = new MagicAttackEffect(data);
            }
            else if (data.Type == EffectModel.TypeEnum.Provocative)
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
            else if (data.Type == EffectModel.TypeEnum.Summon)
            {
                effect = new SummonEffect(data);
            }
            else if (data.Type == EffectModel.TypeEnum.RatioDamage)
            {
                effect = new RatioDamageEffect(data);
            }
            else if (data.Type == EffectModel.TypeEnum.RecoverAll)
            {
                effect = new RecoverAllEffect(data);
            }

            return effect;
        }

        public static Effect GetEffect(int id, int addValue)
        {
            Effect effect = null;
            EffectModel data = DataTable.Instance.EffectDic[id];

            if (data.Type == EffectModel.TypeEnum.Medicine)
            {
                effect = new MedicineEffect(addValue);
            }

            return effect;
        }
    }
}