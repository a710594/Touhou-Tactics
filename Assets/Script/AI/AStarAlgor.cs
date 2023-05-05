using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgor
{
    private static AStarAlgor _instance;
    public static AStarAlgor Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AStarAlgor();
            }
            return _instance;
        }
    }

    private Vector2 _realStart;
    private Vector2 _realGoal;
    private Dictionary<Vector2, int> _nodeDic = new Dictionary<Vector2, int>(); //position, moveCost

    public List<Vector2> GetPath(Vector2 start, Vector2 goal, Dictionary<Vector2, int> nodeDic, bool isRandom)
    {
        if (start == goal)
        {
            return new List<Vector2>();
        }
        else
        {
            _realStart = start;
            _realGoal = goal;
            _nodeDic = nodeDic;

            return GetPath(start, goal, isRandom);
        }
    }

    public List<Vector2> GetPath(Vector2 start, Vector2 goal, bool isRandom)
    {
        List<Node> closedset = new List<Node>(); //已被估算的節點集合
        List<Node> openset = new List<Node>(); //將要被估算的節點集合，初始只包含start
        Node startNode = new Node(start);
        openset.Add(startNode);
        startNode.G = 0; //g(n)
        startNode.H = Vector2.Distance(start, goal); //通過估計函數 估計h(start)
        startNode.F = startNode.H; //f(n)=h(n)+g(n)，由於g(n)=0，所以省略

        while (openset.Count > 0) //當將被估算的節點存在時，執行循環
        {
            Node x = openset[0];
            for (int i = 1; i < openset.Count; i++) //在將被估計的集合中找到f(x)最小的節點
            {
                if (openset[i].F < x.F)
                {
                    x = openset[i];
                }
                else if (openset[i].F == x.F && isRandom)
                {
                    if (Random.Range(0, 2) == 0) //可能是造成 stack overflow 的原因?
                    {
                        x = openset[i];
                    }
                }
            }

            if (x.Position == goal)
            {
                List<Vector2> result = ReconstructPath(x);
                return result;   //返回到x的最佳路徑
            }

            openset.Remove(x); //將x節點從將被估算的節點中刪除
            closedset.Add(x); //將x節點插入已經被估算的節點

            bool isBetter;
            List<Vector2> neighborList = GetNeighborPos(x.Position);
            for (int i = 0; i < neighborList.Count; i++)  //循環遍歷與x相鄰節點
            {
                Node y = new Node(neighborList[i]);

                bool contains = false;
                for (int j = 0; j < closedset.Count; j++) //若y已被估值，跳過
                {
                    if (closedset[j].Position == y.Position)
                    {
                        contains = true;
                        break;
                    }
                }
                if (contains)
                {
                    continue;
                }

                float g = x.G + _nodeDic[y.Position];    //從起點到節點y的距離

                for (int j = 0; j < openset.Count; j++) //若y已被估值，跳過
                {
                    if (openset[j].Position == y.Position)
                    {
                        y = openset[j];
                        break;
                    }
                }

                if (!openset.Contains(y)) //若y不是將被估算的節點
                {
                    isBetter = true; //暫時判斷為更好
                }
                else if (g < y.G)
                {
                    isBetter = true; //暫時判斷為更好
                }
                else
                {
                    isBetter = false; //暫時判斷為更差
                }

                if (isBetter)
                {
                    y.parent = x; //將x設為y的父節點
                    y.G = g; //更新y到原點的距離
                    y.H = Vector2.Distance(y.Position, goal); //估計y到終點的距離
                    y.F = y.G + y.H;
                    openset.Add(y);
                }
            }
        }

        return null;
    }

    private Vector2 GetClosestPosition(Vector2 position) 
    {
        float minDistance = -1;
        float currentPosition;
        Vector2 closestPosition = new Vector2();
        foreach (KeyValuePair<Vector2, int> pair in _nodeDic) 
        {
            currentPosition = Vector2.Distance(pair.Key, position);
            if (minDistance == -1 || currentPosition < minDistance) 
            {
                closestPosition = pair.Key;
                minDistance = currentPosition;
            }
        }

        return closestPosition;
    }

    private List<Vector2> ReconstructPath(Node currentNode)
    {
        List<Vector2> path = new List<Vector2>();

        if (currentNode.parent != null)
        {
            path = ReconstructPath(currentNode.parent);
        }

        path.Add(currentNode.Position);
        return path;
    }

    private List<Vector2> GetNeighborPos(Vector2 current)
    {
        List<Vector2> list = new List<Vector2>();

        if (_nodeDic.ContainsKey(current + Vector2.left))
        {
            list.Add(current + Vector2.left);
        }

        if (_nodeDic.ContainsKey(current + Vector2.right))
        {
            list.Add(current + Vector2.right);
        }

        if (_nodeDic.ContainsKey(current + Vector2.up))
        {
            list.Add(current + Vector2.up);
        }

        if (_nodeDic.ContainsKey(current + Vector2.down))
        {
            list.Add(current + Vector2.down);
        }

        return list;
    }
}