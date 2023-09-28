using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupModel
{
    public int Floor;
    public int ID;
    public int Enemy_1;
    public int Enemy_2;
    public int Enemy_3;
    public int Enemy_4;
    public int Enemy_5;
    public int Exp;

    public List<int> EnemyList = new List<int>();


    public void GetEnemyList() 
    {
        EnemyList.Add(Enemy_1);
        EnemyList.Add(Enemy_2);
        EnemyList.Add(Enemy_3);
        EnemyList.Add(Enemy_4);
        EnemyList.Add(Enemy_5); //
    }
}
