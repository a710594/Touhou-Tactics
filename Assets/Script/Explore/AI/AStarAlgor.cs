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
                List<Node> closedset = new List<Node>(); //�w�Q���⪺�`�I���X
                List<Node> openset = new List<Node>(); //�N�n�Q���⪺�`�I���X�A��l�u�]�tstart
                Node startNode = new Node(start);
                openset.Add(startNode);
                startNode.G = 0; //g(n)
                startNode.H = Vector2.Distance(start, goal); //�q�L���p��� ���ph(start)
                startNode.F = startNode.H; //f(n)=h(n)+g(n)�A�ѩ�g(n)=0�A�ҥH�ٲ�

                while (openset.Count > 0) //���N�Q���⪺�`�I�s�b�ɡA����`��
                {
                    Node x = openset[0];
                    for (int i = 1; i < openset.Count; i++) //�b�N�Q���p�����X�����f(x)�̤p���`�I
                    {
                        if (openset[i].F < x.F)
                        {
                            x = openset[i];
                        }
                    }

                    if (x.Position == goal)
                    {
                        List<Vector2Int> result = ReconstructPath(x);
                        return result;   //��^��x���̨θ��|
                    }

                    openset.Remove(x); //�Nx�`�I�q�N�Q���⪺�`�I���R��
                    closedset.Add(x); //�Nx�`�I���J�w�g�Q���⪺�`�I

                    bool isBetter;
                    List<Vector2Int> neighborList = GetNeighborPos(x.Position);
                    for (int i = 0; i < neighborList.Count; i++)  //�`���M���Px�۾F�`�I
                    {
                        Node y = new Node(neighborList[i]);

                        bool contains = false;
                        for (int j = 0; j < closedset.Count; j++) //�Yy�w�Q���ȡA���L
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

                        float g = x.G + MoveCost(x.Position, y.Position);    //�q�_�I��`�Iy���Z��

                        for (int j = 0; j < openset.Count; j++) //�Yy�w�Q���ȡA���L
                        {
                            if (openset[j].Position == y.Position)
                            {
                                y = openset[j];
                                break;
                            }
                        }

                        if (!openset.Contains(y)) //�Yy���O�N�Q���⪺�`�I
                        {
                            isBetter = true; //�ȮɧP�_����n
                        }
                        else if (g < y.G)
                        {
                            isBetter = true; //�ȮɧP�_����n
                        }
                        else
                        {
                            isBetter = false; //�ȮɧP�_����t
                        }

                        if (isBetter)
                        {
                            y.parent = x; //�Nx�]��y�����`�I
                            y.G = g; //��sy����I���Z��
                            y.H = Vector2.Distance(y.Position, goal); //���py����I���Z��
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
            if (!File.WalkableList.Contains(goal))
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

            if (File.WalkableList.Contains(current + Vector2Int.left))
            {
                list.Add(current + Vector2Int.left);
            }

            if (File.WalkableList.Contains(current + Vector2Int.right))
            {
                list.Add(current + Vector2Int.right);
            }

            if (File.WalkableList.Contains(current + Vector2Int.up))
            {
                list.Add(current + Vector2Int.up);
            }

            if (File.WalkableList.Contains(current + Vector2Int.down))
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