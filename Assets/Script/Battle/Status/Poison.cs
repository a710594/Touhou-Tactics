using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Poison : Status
    {
        public Poison(StatusModel data) : base(data)
        {
        }

        public int GetDamage(BattleCharacterInfo target)
        {
            return Mathf.RoundToInt(target.MaxHP * Value / 100f);
        }
    }
}
