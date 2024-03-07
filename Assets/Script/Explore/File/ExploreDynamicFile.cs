using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreDynamicFile
{
    public Vector2Int PlayerPosition;
    public int PlayerRotation;
    public List<Vector2Int> VisitedList = new List<Vector2Int>();
    public List<ExploreEnemyInfo> EnemyInfoList = new List<ExploreEnemyInfo>();
    //json 不能以 Vector2Int 作為 dictionary 的 key
    //所以要把它的 keys 和 values 分開來儲存
    public List<Vector2Int> TreasureKeys = new List<Vector2Int>();
    public List<Treasure> TreasureValues = new List<Treasure>();
}
