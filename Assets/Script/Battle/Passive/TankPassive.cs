using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPassive : Passive
{
    //衝鋒在前守護隊友。開場時可無視WT第一個行動。

    public TankPassive(PassiveModel data)
    {
        Data = data;
    }
}
