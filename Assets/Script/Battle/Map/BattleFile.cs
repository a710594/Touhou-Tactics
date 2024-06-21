using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFile
{
    public int MinX;
    public int MaxX;
    public int MinY;
    public int MaxY;
    public int PlayerCount;
    public bool MustBeEqualToNeedCount;
    public int Exp;
    public List<string[]> TileList = new List<string[]>();
    public List<int[]> NoAttachList = new List<int[]>(); //�T�ذ�,���|�����[����,��m���a���⪺�ϰ�
    public List<int[]> EnemyList = new List<int[]>(); //�ĤH����m�MID
}
