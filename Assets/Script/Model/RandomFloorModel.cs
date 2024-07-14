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
    public int Probability_1;
    public int Room_2;
    public int Probability_2;

    public string EnemyName;
    public int EnemyCount;
    public List<int> RoomPool = new List<int>();
    public void GetRoomPool() 
    {
        for (int j = 0; j < Probability_1; j++)
        {
            RoomPool.Add(Room_1);
        }

        for (int j = 0; j < Probability_2; j++)
        {
            RoomPool.Add(Room_2);
        }
    }
}
