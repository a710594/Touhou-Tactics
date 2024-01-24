using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFile
{
    public bool IsTutorial;
    public int PlayerCount;
    public int Exp;
    public List<string[]> TileList = new List<string[]>();
    public List<int[]> NoAttachList = new List<int[]>(); //�T�ذ�,���|�����[����,��m���a���⪺�ϰ�
    public List<int[]> EnemyList = new List<int[]>(); //�ĤH����m�MID
}
