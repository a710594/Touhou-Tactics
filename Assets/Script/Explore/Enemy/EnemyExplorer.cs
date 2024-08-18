using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplorer
{
    public enum AiEnum 
    {
        NotMove = 0,
        Default,
        Trace,
    }

    public AiEnum AiType;
    public string Prefab;
    public Vector2Int Position;
    public int RotationY;
}
