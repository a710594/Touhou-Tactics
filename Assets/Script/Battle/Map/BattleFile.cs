using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFile
{
    public bool IsTutorial;
    public int PlayerCount;
    public int Exp;
    public List<string[]> TileList = new List<string[]>();
    public List<int[]> NoAttachList = new List<int[]>(); //禁建區,不會有附加物件,放置玩家角色的區域
    public List<int[]> EnemyList = new List<int[]>(); //敵人的位置和ID
}
