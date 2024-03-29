using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public RectInt bounds;
    public RoomModel Data;
    public List<Room> AdjRoomList = new List<Room>();

    public Room(Vector2Int location, Vector2Int size)
    {
        bounds = new RectInt(location, size);
    }

    public Room(Vector2Int location, RoomModel data, System.Random random) 
    {
        Data = data;
        Vector2Int size = new Vector2Int(random.Next(data.MinWidth, data.MaxWidth + 1), random.Next(data.MinHeight, data.MaxHeight + 1));
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
        if (Data.TreasurePool.Count > 0)
        {
            List<Vector2Int> positionList = new List<Vector2Int>();
            int random;
            for (int i = 0; i < bounds.size.x; i++)
            {
                for (int j = 0; j < bounds.size.y; j++)
                {
                    positionList.Add(new Vector2Int(i, j));
                }
            }

            int treasureCount = Random.Range(Data.MinTreasureCount, Data.MaxTreasureCount + 1);
            Vector2Int treasurePosition;
            TreasureModel treasureData;
            for (int i = 0; i < treasureCount; i++)
            {
                random = Random.Range(0, positionList.Count);
                treasurePosition = bounds.min + positionList[random];
                positionList.RemoveAt(random);
                treasureData = DataContext.Instance.TreasureDic[Data.TreasurePool[Random.Range(0, Data.TreasurePool.Count)]];
                treasures.Add(treasurePosition, new Treasure(treasurePosition, treasureData));
            }
        }
        return treasures;
    }
}