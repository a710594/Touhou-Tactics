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
    public string SeedFile_1;
    public string SeedFile_2;
    public string SeedFile_3;
    public string SeedFile_4;
    public string SeedFile_5;

    public string EnemyName;
    public int EnemyCount;
    public List<int> RoomPool = new List<int>();
    public List<string> BattleSeedList = new List<string>();
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

    public void GetBattleSeedList() 
    {
        if (SeedFile_1 != "x") 
        {
            BattleSeedList.Add(SeedFile_1);
        }
        if (SeedFile_2 != "x")
        {
            BattleSeedList.Add(SeedFile_2);
        }
        if (SeedFile_3 != "x")
        {
            BattleSeedList.Add(SeedFile_3);
        }
        if (SeedFile_4 != "x")
        {
            BattleSeedList.Add(SeedFile_4);
        }
        if (SeedFile_5 != "x")
        {
            BattleSeedList.Add(SeedFile_5);
        }
    }
}
