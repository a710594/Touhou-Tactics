using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class ArcherPassive : Passive
    {
        public ArcherPassive(PassiveModel data)
        {
            Data = data;
        }

        //�֦������̳Q�ʪ�����M��,�����V���g�{�V��
        public static List<Vector2Int> GetRange(int range, Vector2Int start, BattleInfo info)
        {
            int heightDiff;
            int newRange;
            Vector2Int position;
            Vector2Int newPosition;
            List<Vector2Int> list = new List<Vector2Int>();

            //BFS
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                position = queue.Dequeue();
                list.Add(position);

                newPosition = position + Vector2Int.up;
                if (!list.Contains(newPosition) && newPosition.y <= info.MaxY)
                {
                    heightDiff = info.TileDic[start].TileData.Height - info.TileDic[newPosition].TileData.Height;
                    newRange = Mathf.Clamp(range + heightDiff, 0, int.MaxValue);
                    if (Utility.ManhattanDistance(newPosition, start) <= newRange)
                    {
                        queue.Enqueue(newPosition);
                    }
                }

                newPosition = position + Vector2Int.down;
                if (!list.Contains(newPosition) && newPosition.y >= info.MinY)
                {
                    heightDiff = info.TileDic[start].TileData.Height - info.TileDic[newPosition].TileData.Height;
                    newRange = Mathf.Clamp(range + heightDiff, 0, int.MaxValue);
                    if (Utility.ManhattanDistance(newPosition, start) <= newRange)
                    {
                        queue.Enqueue(newPosition);
                    }
                }

                newPosition = position + Vector2Int.left;
                if (!list.Contains(newPosition) && newPosition.x >= info.MinX)
                {
                    heightDiff = info.TileDic[start].TileData.Height - info.TileDic[newPosition].TileData.Height;
                    newRange = Mathf.Clamp(range + heightDiff, 0, int.MaxValue);
                    if (Utility.ManhattanDistance(newPosition, start) <= newRange)
                    {
                        queue.Enqueue(newPosition);
                    }
                }

                newPosition = position + Vector2Int.right;
                if (!list.Contains(newPosition) && newPosition.x <= info.MaxX)
                {
                    heightDiff = info.TileDic[start].TileData.Height - info.TileDic[newPosition].TileData.Height;
                    newRange = Mathf.Clamp(range + heightDiff, 0, int.MaxValue);
                    if (Utility.ManhattanDistance(newPosition, start) <= newRange)
                    {
                        queue.Enqueue(newPosition);
                    }
                }
            }
            return list;
        }
    }
}