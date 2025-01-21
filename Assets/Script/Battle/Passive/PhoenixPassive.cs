using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class PhoenixPassive : Passive
    {
        //HP�V�C���ɭԪ��z�����ˮ`�V��(�̰�+50%)
        public PhoenixPassive(PassiveModel data)
        {
            Data = data;
        }

        public static float GetValue(BattleCharacterControllerData character)
        {
            return (1 + ((character.MaxHP - character.CurrentHP) / (float)character.MaxHP * 0.5f));
        }
    }
}