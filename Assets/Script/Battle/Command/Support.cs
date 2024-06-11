using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Support : Command
    {
        public int CD;
        public int CurrentCD;
        public Support(SupportModel data)
        {
            ID = data.ID;
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

        public void CheckCD()
        {
            if (CurrentCD > 0)
            {
                CurrentCD--;
            }
        }
    }
}