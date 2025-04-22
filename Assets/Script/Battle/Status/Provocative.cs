using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public class Provocative : Status
    {
        public BattleCharacterController Target; //§ðÀ»¥Ø¼Ð

        public Provocative(StatusModel data) : base(data)
        {
        }
    }
}