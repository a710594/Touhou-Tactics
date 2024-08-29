using System.Collections;
using System.Collections.Generic;
using Explore;
using UnityEngine;

public class ExploreInfoEnemy
{
    public ExploreFileEnemy.TypeEnum Type;
    public ExploreEnemyController Controller;

    //fixed
    public string Map;
    public string Tutorial;

    //random
    public int EnemyGroupId;

    public ExploreInfoEnemy(ExploreFileEnemy file)
    {
        Type = file.Type;
        Map = file.Map;
        Tutorial = file.Tutorial;
        EnemyGroupId = file.EnemyGroupId;
    }
}
