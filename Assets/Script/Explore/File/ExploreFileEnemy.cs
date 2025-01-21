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

    public enum AiEnum 
    {
        NotMove = 0,
        Default,
        Trace,
    }

    public TypeEnum Type;
    public AiEnum AI;
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
        AI = AiEnum.Default;
        Prefab = data.Prefab;
        Position = pos;
        EnemyGroupId = data.ID;
    }
}
