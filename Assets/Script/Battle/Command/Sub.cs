using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Sub : Command
    {
        public int CD;
        public int CurrentCD;
        public Sub(SupportModel data)
        {
            ID = data.ID;
            Name = data.Name;
            Comment = data.Comment;
            Hit = data.Hit;
            Range = data.Range;
            RangeTarget = data.RangeTarget;
            AreaTarget = data.AreaTarget;
            AreaType = data.AreaType;
            Track = data.Track;
            ArrayList = Utility.GetAreaList(data.Array);
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