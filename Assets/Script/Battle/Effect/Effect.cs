using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    public class Effect
    {
        public EffectModel.TypeEnum Type;
        public int Value;
        public Status Status;

        public Effect SubEffect;

        public Effect() { }

        public Effect(EffectModel data)
        {
            Type = data.Type;
            Value = data.Value;
            if (data.StatusID != -1)
            {
                Status = StatusFactory.GetStatus(data.StatusID);
            }
            if (data.SubID != -1)
            {
                SubEffect = EffectFactory.GetEffect(data.SubID);
            }
        }

        public virtual void Use(BattleCharacterController user, Vector2Int position) { }

        public virtual void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
        {
        }

        public virtual void Use(BattleCharacterController user, SubTargetEnum subTarget, List<Log> logList)
        {
        }
    }
}