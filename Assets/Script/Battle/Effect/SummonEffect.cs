using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SummonEffect : Effect
    {
        public SummonEffect(EffectModel data) : base(data)
        {
        }

        public override void Use(BattleCharacterInfo user, Vector3 position)
        {
            BattleController.Instance.CreateCharacter(Value, user.Lv, position);
        }
    }
}