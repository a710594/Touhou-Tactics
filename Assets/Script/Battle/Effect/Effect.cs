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
        //public EffectModel Data;
        public EffectModel.TypeEnum Type;
        public int Value;
        // public int Hit;
        // public int Range;
        // public EffectModel.TargetEnum Target;
        // public EffectModel.TrackEnum Track;
        // public string Area;
        // public List<Vector2Int> AreaList = new List<Vector2Int>();
        public Status Status;

        public Effect SubEffect;

        public Effect SelfEffect;

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
            if(data.SelfID != -1)
            {
                SelfEffect = EffectFactory.GetEffect(data.SelfID);
            }
        }

        public virtual void Use(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
        {
        }

        public virtual void Use(BattleCharacterInfo user, Vector3 position) { }

        public virtual void Use(HitType hitType, BattleCharacterInfo user, BattleCharacterInfo target, List<Log> logList)
        {
        }
    }
}