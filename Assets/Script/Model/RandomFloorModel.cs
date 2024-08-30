using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorModel
{
    public int Floor;
    public int Width;
    public int Height;

    public int RoomCount;
    public int Room_1;
    public int RoomProbability_1;
    public int Room_2;
    public int RoomProbability_2;
    public List<int> RoomPool = new List<int>();

    public int EnemyCount;
    public int EnemyGroup_1;
    public int EnemyGroupProbability_1;
    public int EnemyGroup_2;
    public int EnemyGroupProbability_2;
    public int EnemyGroup_3;
    public int EnemyGroupProbability_3;
    public int BossEnemyGroup;
    public List<int> EnemyGroupPool = new List<int>();

    public void GetRoomPool() 
    {
        for (int j = 0; j < RoomProbability_1; j++)
        {
            RoomPool.Add(Room_1);
        }

        for (int j = 0; j < RoomProbability_2; j++)
        {
            RoomPool.Add(Room_2);
        }
    }

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

    public int GetRoomID()
    {
        return RoomPool[Random.Range(0, RoomPool.Count)];
    }
}
