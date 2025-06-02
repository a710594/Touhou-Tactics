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

        public override void Use(BattleCharacterController user, Vector2Int position)
        {
            BattleController.Instance.CreateEnemy(Value, user.Info.Lv, position, user.transform.eulerAngles.y);
        }
    }
}