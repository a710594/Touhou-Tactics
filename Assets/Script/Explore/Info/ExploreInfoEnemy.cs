using System.Collections;
using System.Collections.Generic;
using Explore;
using UnityEngine;

public class ExploreInfoEnemy
{
    public string Prefab;
    public Vector3 Position;
    public Vector3 Rotation;
    public ExploreFileEnemy.TypeEnum Type;
    public ExploreFileEnemy.AiEnum AI;
    public ExploreEnemyController Controller;

    //fixed
    public string Map;
    public string Tutorial;

    //random
    public int EnemyGroupId;

    public ExploreInfoEnemy(){}

    public ExploreInfoEnemy(ExploreFileEnemy file)
    {
        Type = file.Type;
        AI = file.AI;
        Map = file.Map;
        Tutorial = file.Tutorial;
        EnemyGroupId = file.EnemyGroupId;
        Prefab = file.Prefab;
        Position = new Vector3(file.Position.x, 1, file.Position.y);
        Rotation = new Vector3(0, file.RotationY, 0);
    }
}
