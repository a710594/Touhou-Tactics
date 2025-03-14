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
    public int MapProbability_1;
    public string Map_2;
    public int MapProbability_2;
    public string Map_3;
    public int MapProbability_3;
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
            for (int j = 0; j < MapProbability_1; j++)
            {
                MapPool.Add(Map_1);
            }
        }

        if (Map_2 != "x")
        {
            for (int j = 0; j < MapProbability_2; j++)
            {
                MapPool.Add(Map_2);
            }
        }

        if (Map_3 != "x")
        {
            for (int j = 0; j < MapProbability_3; j++)
            {
                MapPool.Add(Map_3);
            }
        }
    }

    public string GetMap() 
    {
        return MapPool[UnityEngine.Random.Range(0, MapPool.Count)];
    }
}
