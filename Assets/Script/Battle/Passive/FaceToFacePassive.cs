using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToFacePassive : Passive
{
    //正面對決。不會因為從正面攻擊對手命中率就降低
    //一般來說，夾角90度的時候命中率為100%的話,夾角180度的時候命中率是50%
    //但有此特性的角色命中率都是 100%

    public FaceToFacePassive(PassiveModel data)
    {
        Data = data;
    }
}
