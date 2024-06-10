using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Support : Command
    {
        public int CD;
        public int CurrentCD;
        public SupportModel Data;

        public Support(SupportModel data)
        {
            Name = data.Name;
            Comment = data.Comment;
            Hit = data.Hit;
            Range = data.Range;
            CastTarget = data.CastTarget;
            EffectTarget = data.EffectTarget;
            Track = data.Track;
            AreaList = Utility.GetAreaList(data.Area);
            CD = data.CD;
            CurrentCD = 0;
            Effect = EffectFactory.GetEffect(data.EffectID);
        }

        public virtual void UseEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
        {
            Effect.Use(user, target, floatingList, characterList);
            user.HasUseSupport = true;
        }

        public void CheckCD()
        {
            if (CurrentCD > 0)
            {
                CurrentCD--;
            }
        }
    }
}