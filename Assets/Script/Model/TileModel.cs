using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModel
{
    public int ID;
    public string Name;
    public string Comment;
    public int Height;
    public int MoveCost;
    public bool Enqueue; //是否會進入 wave function collapse 的 BFS queue 中

    public int LeftID_1;
    public int LeftProbability_1;
    public int LeftID_2;
    public int LeftProbability_2;
    public int LeftID_3;
    public int LeftProbability_3;
    public List<int> LeftPool = new List<int>();

    public int RightID_1;
    public int RightProbability_1;
    public int RightID_2;
    public int RightProbability_2;
    public int RightID_3;
    public int RightProbability_3;
    public List<int> RightPool = new List<int>();

    public int UpID_1;
    public int UpProbability_1;
    public int UpID_2;
    public int UpProbability_2;
    public int UpID_3;
    public int UpProbability_3;
    public List<int> UpPool = new List<int>();

    public int DownID_1;
    public int DownProbability_1;
    public int DownID_2;
    public int DownProbability_2;
    public int DownID_3;
    public int DownProbability_3;
    public List<int> DownPool = new List<int>();

    public string AttachName_1;
    public int AttachProbability_1;
    public string AttachName_2;
    public int AttachProbability_2;
    public string AttachName_3;
    public int AttachProbability_3;
    public List<string> AttachPool = new List<string>();

    public void GetPool()
    {
        for (int i = 0; i < LeftProbability_1; i++)
        {
            LeftPool.Add(LeftID_1);
        }

        for (int i = 0; i < LeftProbability_2; i++)
        {
            LeftPool.Add(LeftID_2);
        }

        for (int i = 0; i < LeftProbability_3; i++)
        {
            LeftPool.Add(LeftID_3);
        }

        for (int i = 0; i < RightProbability_1; i++)
        {
            RightPool.Add(RightID_1);
        }

        for (int i = 0; i < RightProbability_2; i++)
        {
            RightPool.Add(RightID_2);
        }

        for (int i = 0; i < RightProbability_3; i++)
        {
            RightPool.Add(RightID_3);
        }

        for (int i = 0; i < UpProbability_1; i++)
        {
            UpPool.Add(UpID_1);
        }

        for (int i = 0; i < UpProbability_2; i++)
        {
            UpPool.Add(UpID_2);
        }

        for (int i = 0; i < UpProbability_3; i++)
        {
            UpPool.Add(RightID_3);
        }

        for (int i = 0; i < DownProbability_1; i++)
        {
            DownPool.Add(DownID_1);
        }

        for (int i = 0; i < DownProbability_2; i++)
        {
            DownPool.Add(DownID_2);
        }

        for (int i = 0; i < DownProbability_3; i++)
        {
            DownPool.Add(DownID_3);
        }

        for (int i = 0; i < AttachProbability_1; i++)
        {
            AttachPool.Add(AttachName_1);
        }

        for (int i = 0; i < AttachProbability_2; i++)
        {
            AttachPool.Add(AttachName_2);
        }

        for (int i = 0; i < AttachProbability_3; i++)
        {
            AttachPool.Add(AttachName_3);
        }
    }
}
