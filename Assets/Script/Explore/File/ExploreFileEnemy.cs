using System;
using System.Collections;
using System.Collections.Generic;
using Explore;
using UnityEngine;

public class ExploreFileEnemy
{
    public enum TypeEnum
    {
        Fixed,
        Random,
    }

    public TypeEnum Type;
    public EnemyGroupModel.AiEnum AI;
    public Vector2Int Position;
    public int RotationY;
    public string Prefab;

    //fixed
    public string Map;
    public string Tutorial;

    //random
    public int EnemyGroupId;

    [NonSerialized]
    public ExploreEnemyController Controller;

    public ExploreFileEnemy() { }

    public ExploreFileEnemy(EnemyGroupModel data, Vector2Int pos) 
    {
        Type = TypeEnum.Random;
        AI = data.AI;
        Prefab = data.Prefab;
        Position = pos;
        EnemyGroupId = data.ID;
    }
}
