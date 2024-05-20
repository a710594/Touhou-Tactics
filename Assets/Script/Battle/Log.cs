using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Log
    {
        public BattleController.HitType HitType;
        public EffectModel.TypeEnum Type;
        public string Text;

        public Log(Effect effect, BattleController.HitType hitType, string text) 
        {
            Type = effect.Type;
            HitType = hitType;
            Text = text;
        }
    }
}
