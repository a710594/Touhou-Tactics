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
        public EffectModel.TargetEnum Target;

        public Effect SubEffect;

        public Effect() { }

        public Effect(EffectModel data)
        {
            Type = data.Type;
            Value = data.Value;
            Target = data.Target;
            if (data.StatusID != -1)
            {
                Status = StatusFactory.GetStatus(data.StatusID);
            }
            if (data.SubID != -1)
            {
                SubEffect = EffectFactory.GetEffect(data.SubID);
            }
        }

        public virtual void Use(BattleCharacterController user, Vector3 position) { }

        public virtual void Use(HitType hitType, BattleCharacterController user, BattleCharacterController target, List<Log> logList)
        {
        }

        public virtual void Use(HitType hitType, BattleCharacterController user, List<Log> logList)
        {
        }
    }
}