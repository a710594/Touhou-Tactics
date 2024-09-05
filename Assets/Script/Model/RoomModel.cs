using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomModel
{
    public int ID;
    public string Name;
    public int MinWidth;
    public int MaxWidth;
    public int MinHeight;
    public int MaxHeight;
    public int TreasureID_1;
    public int Probability_1;
    public int TreasureID_2;
    public int Probability_2;
    public int TreasureID_3;
    public int Probability_3;
    public int MinTreasureCount;
    public int MaxTreasureCount;
    public List<int> TreasurePool = new List<int>();

    public void GetTreasurePool()
    {
        for (int j = 0; j < Probability_1; j++)
        {
            TreasurePool.Add(TreasureID_1);
        }

        for (int j = 0; j < Probability_2; j++)
        {
            TreasurePool.Add(TreasureID_2);
        }

        for (int j = 0; j < Probability_3; j++)
        {
            TreasurePool.Add(TreasureID_3);
        }
    }

    public int GetTreasure()
    {

        return TreasurePool[Random.Range(0, TreasurePool.Count)];
    }
}
