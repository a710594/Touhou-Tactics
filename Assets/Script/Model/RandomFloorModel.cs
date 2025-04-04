using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorModel
{
    public int Floor;
    public int Width;
    public int Height;

    public int RoomCount;

    public int EnemyCount;
    public int EnemyGroup_1;
    public int EnemyGroupProbability_1;
    public int EnemyGroup_2;
    public int EnemyGroupProbability_2;
    public int EnemyGroup_3;
    public int EnemyGroupProbability_3;
    public int BossEnemyGroup;
    public List<int> EnemyGroupPool = new List<int>();

    public void GetEnemyGroupPool() 
    {
        for (int j = 0; j < EnemyGroupProbability_1; j++)
        {
            EnemyGroupPool.Add(EnemyGroup_1);
        }

        for (int j = 0; j < EnemyGroupProbability_2; j++)
        {
            EnemyGroupPool.Add(EnemyGroup_2);
        }

        for (int j = 0; j < EnemyGroupProbability_3; j++)
        {
            EnemyGroupPool.Add(EnemyGroup_3);
        }
    }

    public int GetEnemyGroupID()
    {
        return EnemyGroupPool[Random.Range(0, EnemyGroupPool.Count)];
    }
}
