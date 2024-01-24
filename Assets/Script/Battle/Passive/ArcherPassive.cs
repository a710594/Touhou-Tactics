using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPassive : Passive
{
    public ArcherPassive(PassiveModel data)
    {
        Data = data;
    }

    //擁有狙擊者被動的角色專用,站的越高射程越遠
    public static List<Vector2Int> GetRange(int range, int width, int height, Vector2Int start, Dictionary<Vector2Int, TileAttachInfo> tileInfoDic)
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
            if (!list.Contains(newPosition) && newPosition.y < height)
            {
                heightDiff = tileInfoDic[start].Height - tileInfoDic[newPosition].Height;
                newRange = Mathf.Clamp(range + heightDiff, 0, int.MaxValue);
                if (Utility.ManhattanDistance(newPosition, start) <= newRange)
                {
                    queue.Enqueue(newPosition);
                }
            }

            newPosition = position + Vector2Int.down;
            if (!list.Contains(newPosition) && newPosition.y >= 0)
            {
                heightDiff = tileInfoDic[start].Height - tileInfoDic[newPosition].Height;
                newRange = Mathf.Clamp(range + heightDiff, 0, int.MaxValue);
                if (Utility.ManhattanDistance(newPosition, start) <= newRange)
                {
                    queue.Enqueue(newPosition);
                }
            }

            newPosition = position + Vector2Int.left;
            if (!list.Contains(newPosition) && newPosition.x >= 0)
            {
                heightDiff = tileInfoDic[start].Height - tileInfoDic[newPosition].Height;
                newRange = Mathf.Clamp(range + heightDiff, 0, int.MaxValue);
                if (Utility.ManhattanDistance(newPosition, start) <= newRange)
                {
                    queue.Enqueue(newPosition);
                }
            }

            newPosition = position + Vector2Int.right;
            if (!list.Contains(newPosition) && newPosition.x < width)
            {
                heightDiff = tileInfoDic[start].Height - tileInfoDic[newPosition].Height;
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
