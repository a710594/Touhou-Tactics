using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFactory
{
    public static Room GetRoom(int floor, Vector2Int location, System.Random random) 
    {
        List<int> list = DataContext.Instance.RoomPool[floor];
        int id = list[random.Next(0, list.Count)];
        RoomModel data = DataContext.Instance.RoomDic[id];
        return new Room(location, data, random);
    }
}
