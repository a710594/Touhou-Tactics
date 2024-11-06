using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Dijkstra
{
    private int[,] adj = new int[6, 6];
    private int[] dis = new int[6];
    private List<int> nonVisitedList = new List<int>();

    public void Init() 
    {
        for(int i=0; i<adj.GetLength(0); i++) 
        {
            for (int j=0; j<adj.GetLength(1); j++) 
            {
                adj[i, j] = -1;
            }
        }

        adj[0, 0] = 0;
        adj[0, 1] = 1;
        adj[0, 2] = 12;
        adj[1, 1] = 0;
        adj[1, 2] = 9;
        adj[1, 3] = 3;
        adj[2, 2] = 0;
        adj[2, 4] = 5;
        adj[3, 2] = 4;
        adj[3, 3] = 0;
        adj[3, 4] = 13;
        adj[3, 5] = 15;
        adj[4, 4] = 0;
        adj[4, 5] = 4;
        adj[5, 5] = 0;

        for (int i=0; i<dis.GetLength(0); i++) 
        {
            dis[i] = -1;
        }
        dis[0] = 0;
        dis[1] = 1;
        dis[2] = 12;

        for (int i = 1; i < 6; i++)
        {
            nonVisitedList.Add(i);
        }
    }

    public void GetShortest(int start) 
    {
        int index;
        int minIndex;
        int minDis;
        while (nonVisitedList.Count > 0)
        {
            minIndex = -1;
            minDis = -1;
            for (int i = 0; i < nonVisitedList.Count; i++)
            {
                index = nonVisitedList[i];
                if (dis[index] != -1 && (minDis == -1 || minDis > dis[index]))
                {
                    minIndex = index;
                    minDis = dis[index];
                }
            }
            nonVisitedList.Remove(minIndex);
            for (int i = 0; i < adj.GetLength(1); i++)
            {
                if (adj[minIndex, i] != -1 && (dis[i] == -1 || dis[i] > dis[minIndex] + adj[minIndex, i]))
                {
                    int value = dis[minIndex] + adj[minIndex, i];
                    dis[i] = value;
                }
            }
        }

        Console.WriteLine(dis);
    }

    public void GetLongest(int start)
    {
        int index;
        int maxIndex;
        int maxDis;
        while (nonVisitedList.Count > 0)
        {
            maxIndex = -1;
            maxDis = -1;
            for (int i = 0; i < nonVisitedList.Count; i++)
            {
                index = nonVisitedList[i];
                if (dis[index] != -1 && (maxDis == -1 || maxDis < dis[index]))
                {
                    maxIndex = index;
                    maxDis = dis[index];
                }
            }
            nonVisitedList.Remove(maxIndex);
            for (int i = 0; i < adj.GetLength(1); i++)
            {
                if (adj[maxIndex, i] != -1 && (dis[i] == -1 || dis[i] < dis[maxIndex] + adj[maxIndex, i]))
                {
                    int value = dis[maxIndex] + adj[maxIndex, i];
                    dis[i] = value;
                }
            }
        }

        Console.WriteLine(dis);
    }
}

