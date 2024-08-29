using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFile
{
    public bool IsArrive = false; //��F���I
    public int Floor;
    public Vector2Int Size;
    public Vector2Int Start;
    public Vector2Int Goal;
    public Vector2Int PlayerPosition;
    public int PlayerRotation;
    public List<Vector2Int> VisitedList = new List<Vector2Int>();
    public List<Vector2Int> WalkableList = new List<Vector2Int>();
    public List<ExploreFileEnemy> EnemyInfoList = new List<ExploreFileEnemy>();
    public List<ExploreFileTile> TileList = new List<ExploreFileTile>();
    public List<ExploreFileTreasure> TreasureList = new List<ExploreFileTreasure>();
    public List<ExploreFileTrigger> TriggerList = new List<ExploreFileTrigger>();
}
