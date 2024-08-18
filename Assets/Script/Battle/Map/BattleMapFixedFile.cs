using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapFixedFile : BattleMapFile
{
    public bool MustBeEqualToMaxCount;
    public int Exp;
    public KeyValuePair<Vector3Int, int> EnemyList;
}
