using System.Collections.Generic;

public class EnemyGroupModel
{
    public enum AiEnum
    {
        NotMove = 0,
        Move,
        Trace,
    }

    public int ID;
    public int Lv;
    public int Exp;
    public int MinPlayerCount;
    public int MaxPlayerCount;
    public string Prefab;
    public AiEnum AI;

    public int Enemy_1;
    public int Enemy_2;
    public int Enemy_3;
    public int Enemy_4;
    public int Enemy_5;
    public List<int> EnemyList = new List<int>();

    public string Map_1;
    public string Map_2;
    public string Map_3;
    public string Map_4;
    public string Map_5;
    public string Map_6;
    public string Map_7;
    public string Map_8;
    public List<string> MapPool = new List<string>();


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

    public void GetMapPool()
    {
        if (Map_1 != "x")
        {
            MapPool.Add(Map_1);
        }

        if (Map_2 != "x")
        {
            MapPool.Add(Map_2);
        }

        if (Map_3 != "x")
        {
            MapPool.Add(Map_3);
        }

        if (Map_4 != "x")
        {
            MapPool.Add(Map_4);
        }

        if (Map_5 != "x")
        {
            MapPool.Add(Map_5);
        }

        if (Map_6 != "x")
        {
            MapPool.Add(Map_6);
        }

        if (Map_7 != "x")
        {
            MapPool.Add(Map_7);
        }

        if (Map_8 != "x")
        {
            MapPool.Add(Map_8);
        }
    }

    public string GetMap() 
    {
        return MapPool[UnityEngine.Random.Range(0, MapPool.Count)];
    }
}
