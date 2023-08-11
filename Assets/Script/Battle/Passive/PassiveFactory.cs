using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveFactory
{
    public static Passive GetPassive(int id) 
    {
        Passive passive = null;
        if(id == 1) 
        {
            passive = new ForwardPassive(DataContext.Instance.PassiveDic[id]);
        }

        return passive;
    }
}
