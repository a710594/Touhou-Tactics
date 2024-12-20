using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive 
{
    public PassiveModel Data;

    public Passive() { }

    public Passive(PassiveModel data)
    {
        Data = data;
    }

    public static bool Contains<T>(List<Passive> list)
    {
        for (int i=0; i<list.Count; i++) 
        {
            if(list[i] is T) 
            {
                return true;
            }
        }
        return false;
    }
}
