using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Sub : Command
    {
        public int CD;
        public int CurrentCD;
        public Sub(SubModel data)
        {
            ID = data.ID;
            Name = data.Name;
            Comment = data.Comment;
            Hit = data.Hit;
            Range = data.Range;
            Target = data.Target;
            AreaType = data.AreaType;
            Track = data.Track;
            ArrayList = Utility.GetAreaList(data.AreaArray);
            Particle = data.Particle;
            Shake = data.Shake;
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