using Explore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public RoomModel Data;
    public RectInt bounds;
    public List<Room> AdjList = new List<Room>();

    public bool Isolated 
    {
        get
        {
            return AdjList.Count == 1;
        }
    }

    private List<Vector2Int> _availableList = new List<Vector2Int>();
    private List<Vector2Int> _boundList = new List<Vector2Int>();

    public Room(Vector2Int location, Vector2Int size) 
    {
        Vector2Int pos;
        bounds = new RectInt(location, size);
        for (int i = bounds.xMin + 1; i < bounds.xMax; i++)
        {
            for (int j = bounds.yMin + 1; j < bounds.yMax; j++)
            {
                pos = new Vector2Int(i, j);
                _availableList.Add(pos);
                _boundList.Add(pos);
            }
        }
    }

    public static bool Intersect(Room a, Room b) {
        return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
            || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
    }

    public bool TryGetRandomPosition(out Vector2Int result)
    {
        result = new Vector2Int();
        if(_availableList.Count>0)
        {
            result = _availableList[Random.Range(0, _availableList.Count)]; ;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetNotAvailable(Vector2Int v2)
    {
        _availableList.Remove(v2);
    }

    public bool IsAvailable(Vector2Int v2) 
    {
        return _availableList.Contains(v2);
    }

    public bool InBound(Vector2Int v2)
    {
        return _boundList.Contains(v2);
    }
}