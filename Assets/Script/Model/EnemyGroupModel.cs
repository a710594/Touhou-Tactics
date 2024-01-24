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
        if(Enemy_1 != -1)
            EnemyList.Add(Enemy_1);
        if (Enemy_2 != -1)
            EnemyList.Add(Enemy_2);
        if (Enemy_3 != -1)
            EnemyList.Add(Enemy_3);
        if (Enemy_4 != -1)
            EnemyList.Add(Enemy_4);
        if (Enemy_5 != -1)
            EnemyList.Add(Enemy_5);
    }
}
