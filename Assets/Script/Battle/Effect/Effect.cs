using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Effect
{
    public EffectModel Data;

    protected BattleCharacterInfo _user;
    protected Effect _subEffect;

    public Effect() { }

    public Effect(EffectModel data, BattleCharacterInfo user)
    {
        Data = data;
        _user = user;
        if (data.SubType != EffectModel.TypeEnum.None)
        {
            EffectModel subData = DataContext.Instance.EffectDic[data.SubType][data.SubID];
            _subEffect = EffectFactory.GetEffect(subData.Type, subData.ID, user);
        }
    }

    public bool CheckArea(Vector2 center, Vector2 target)
    {
        for (int i = 0; i < Data.AreaList.Count; i++)
        {
            if (target == center + Data.AreaList[i])
            {
                return true;
            }
        }
        return false;
    }

    public virtual void SetEffect(BattleCharacterInfo target)
    {
        if (_subEffect != null)
        {
            _subEffect.SetEffect(target);
        }
    }
}