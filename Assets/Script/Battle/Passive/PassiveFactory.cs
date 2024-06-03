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
                passive = new TankPassive(DataContext.Instance.PassiveDic[id]);
            }
            else if (id == 2)
            {
                passive = new MagicianPassive(DataContext.Instance.PassiveDic[id]);
            }
            else if (id == 3)
            {
                passive = new ArcherPassive(DataContext.Instance.PassiveDic[id]);
            }
            else if (id == 4)
            {
                passive = new SwordmanPassive(DataContext.Instance.PassiveDic[id]);
            }
            else if (id == 5)
            {
                passive = new DreamEaterPassive(DataContext.Instance.PassiveDic[id]);
            }
            else if (id == 6)
            {
                passive = new PhoenixPassive(DataContext.Instance.PassiveDic[id]);
            }

            return passive;
        }
    }
}