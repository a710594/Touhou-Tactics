using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public class ProvocativeStatus : Status
    {
        public BattleCharacterInfo Target; //�����ؼ�

        public ProvocativeStatus(StatusModel data) : base(data)
        {
        }
    }
}