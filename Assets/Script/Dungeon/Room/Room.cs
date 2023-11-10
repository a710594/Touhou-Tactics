using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public RectInt bounds;
    public RoomModel Data;

    public Room(Vector2Int location, Vector2Int size)
    {
        bounds = new RectInt(location, size);
    }

    public Room(Vector2Int location, RoomModel data) 
    {
        Data = data;
        Vector2Int size = new Vector2Int(Random.Range(data.MinWidth, data.MaxWidth + 1), Random.Range(data.MinHeight, data.MaxHeight + 1));
        bounds = new RectInt(location, size);
    }

    public static bool Intersect(Room a, Room b)
    {
        return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
            || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
    }

    public Dictionary<Vector2Int, Treasure> GetTreasures()
    {
        Dictionary<Vector2Int, Treasure> treasures = new Dictionary<Vector2Int, Treasure>();
        if (Data.TreasureID != -1)
        {
            TreasureModel treasureData = DataContext.Instance.TreasureDic[Data.TreasureID];
            int treasureCount = Random.Range(Data.MinTreasureCount, Data.MaxTreasureCount + 1);
            Vector2Int treasurePosition;
            List<Vector2Int> positionList = new List<Vector2Int>();
            int random;
            for (int i = 0; i < bounds.size.x; i++)
            {
                for (int j = 0; j < bounds.size.y; j++)
                {
                    positionList.Add(new Vector2Int(i, j));
                }
            }
            for (int i = 0; i < treasureCount; i++)
            {
                if (treasureCount == 1)
                {
                    treasurePosition = new Vector2Int(bounds.x + bounds.size.x / 2, bounds.y + bounds.size.y / 2);
                }
                else
                {
                    random = Random.Range(0, positionList.Count);
                    treasurePosition = bounds.min + positionList[random];
                    positionList.RemoveAt(random);
                }
                treasures.Add(treasurePosition, new Treasure(treasurePosition, treasureData));
            }
        }
        return treasures;
    }
}