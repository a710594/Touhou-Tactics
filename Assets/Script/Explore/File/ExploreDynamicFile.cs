using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreDynamicFile
{
    public Vector2Int PlayerPosition;
    public int PlayerRotation;
    public List<Vector2Int> VisitedList = new List<Vector2Int>();
    public List<ExploreEnemyInfo> EnemyInfoList = new List<ExploreEnemyInfo>();
    //json ����H Vector2Int �@�� dictionary �� key
    //�ҥH�n�⥦�� keys �M values ���}���x�s
    public List<Vector2Int> TreasureKeys = new List<Vector2Int>();
    public List<Treasure> TreasureValues = new List<Treasure>();
}
