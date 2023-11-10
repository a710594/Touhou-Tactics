using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFactory
{
    public static Room GetRoom(int floor, Vector2Int location) 
    {
        List<int> list = DataContext.Instance.RoomPool[floor];
        int id = list[Random.Range(0, list.Count)];
        RoomModel data = DataContext.Instance.RoomDic[id];
        return new Room(location, data);
    }
}
