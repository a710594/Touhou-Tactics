using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFileFixed
{
    public int PlayerCount;
    public int Exp;
    public List<Vector2Int> PlayerPositionList = new List<Vector2Int>();
    public List<BattleFileEnemy> EnemyList = new List<BattleFileEnemy>();
    public List<BattleFileTile> TileList = new List<BattleFileTile>();
}
