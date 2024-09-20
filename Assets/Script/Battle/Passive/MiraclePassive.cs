using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class MiraclePassive : Passive
    {
        public MiraclePassive(PassiveModel data)
        {
            Data = data;
        }

        public static int GetValue(int recover)
        {
            int random = Random.Range(0, 10);
            if(random < 3)
            {
                recover = Mathf.RoundToInt(recover * 1.5f);
            }
            return recover;
        }
    }
}