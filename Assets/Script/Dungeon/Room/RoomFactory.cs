using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFactory
{
    public static Room GetRoom(int floor, Vector2Int location, System.Random random) 
    {
        FloorModel floorData = DataContext.Instance.FloorDic[floor];
        int roomId = floorData.RoomPool[Random.Range(0, floorData.RoomPool.Count)];
        RoomModel data = DataContext.Instance.RoomDic[roomId];
        return new Room(location, data, random);
    }
}
