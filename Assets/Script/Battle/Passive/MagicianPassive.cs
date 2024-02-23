using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class MagicianPassive : Passive
    {

        //�����j��,����ɶˮ`��
        public MagicianPassive(PassiveModel data)
        {
            Data = data;
        }

        public static float GetValue(BattleCharacterInfo character)
        {
            return (1 + ((character.CurrentHP / (float)character.MaxHP) * 0.2f));
        }
    }
}