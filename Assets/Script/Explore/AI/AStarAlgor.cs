using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public partial class ExploreManager
    {
        public List<Vector2Int> GetPath(Vector2Int start, Vector2Int goal)
        {
            if (start == goal)
            {
                return new List<Vector2Int>();
            }
            else
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
                    }

                    if (x.Position == goal)
                    {
                        List<Vector2Int> result = ReconstructPath(x);
                        return result;   //返回到x的最佳路徑
                    }

                    openset.Remove(x); //將x節點從將被估算的節點中刪除
                    closedset.Add(x); //將x節點插入已經被估算的節點

                    bool isBetter;
                    List<Vector2Int> neighborList = GetNeighborPos(x.Position);
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

                        float g = x.G + MoveCost(x.Position, y.Position);    //從起點到節點y的距離

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
            }

            return null;
        }

        public int GetDistance(Vector2Int start, Vector2Int goal)
        {
            int distance = 0;
            if (!Info.WalkableList.Contains(goal))
            {
                return -1;
            }
            else if (start == goal)
            {
                return 0;
            }
            else
            {
                List<Vector2Int> path = GetPath(start, goal);
                if (path != null)
                {
                    for (int i = 1; i < path.Count; i++)
                    {
                        distance += MoveCost(path[i - 1], path[i]);
                    }
                }
                else
                {
                    distance = -1;
                }

                return distance;
            }
        }

        private List<Vector2Int> ReconstructPath(Node currentNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();

            if (currentNode.parent != null && currentNode != currentNode.parent)
            {
                path = ReconstructPath(currentNode.parent);
            }

            path.Add(currentNode.Position);
            return path;
        }

        private List<Vector2Int> GetNeighborPos(Vector2Int current)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            if (IsWalkableNew(current + Vector2Int.left))
            {
                list.Add(current + Vector2Int.left);
            }

            if (IsWalkableNew(current + Vector2Int.right))
            {
                list.Add(current + Vector2Int.right);
            }

            if (IsWalkableNew(current + Vector2Int.up))
            {
                list.Add(current + Vector2Int.up);
            }

            if (IsWalkableNew(current + Vector2Int.down))
            {
                list.Add(current + Vector2Int.down);
            }

            return list;
        }

        private int MoveCost(Vector2Int from, Vector2Int to) 
        {
            return 1;
        }
    }
}