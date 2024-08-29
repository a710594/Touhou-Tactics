using System.Collections;
using System.Collections.Generic;
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
    public string Prefab;
    public Vector2Int Position;
    public int RotationY;

    //fixed
    public string Map;
    public string Tutorial;

    //random
    public int EnemyGroupId;
}
