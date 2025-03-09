using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class PassiveFactory
    {
        public static Passive GetPassive(int id)
        {
            Passive passive = null;
            if (id == 1)
            {
                passive = new TankPassive(DataTable.Instance.PassiveDic[id]);
            }
            else if (id == 2)
            {
                passive = new MagicianPassive(DataTable.Instance.PassiveDic[id]);
            }
            else if (id == 3)
            {
                passive = new ArcherPassive(DataTable.Instance.PassiveDic[id]);
            }
            else if (id == 4)
            {
                passive = new SwordmanPassive(DataTable.Instance.PassiveDic[id]);
            }
            else if (id == 5)
            {
                passive = new DreamEaterPassive(DataTable.Instance.PassiveDic[id]);
            }
            else if (id == 6)
            {
                passive = new PhoenixPassive(DataTable.Instance.PassiveDic[id]);
            }
            else if (id == 7)
            {
                passive = new MiraclePassive(DataTable.Instance.PassiveDic[id]);
            }

            return passive;
        }
    }
}