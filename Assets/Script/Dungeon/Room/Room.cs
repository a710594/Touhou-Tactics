using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Room 
    {
        public RectInt bounds;

        private List<Vector2Int> AvailableList;

        public Room(Vector2Int location, Vector2Int size) 
        {
            bounds = new RectInt(location, size);
            foreach (var pos in bounds.allPositionsWithin) 
            {
                AvailableList.Add(pos);
            }
        }

        public Room(Vector2Int location, RoomModel data)
        {
            Vector2Int size = new Vector2Int(Random.Range(data.MinWidth, data.MaxWidth), Random.Range(data.MinHeight, data.MaxHeight));
            bounds = new RectInt(location, size);
            foreach (var pos in bounds.allPositionsWithin) 
            {
                AvailableList.Add(pos);
            }
        }

        public static bool Intersect(Room a, Room b) {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }

        public Vector2Int GetRandomPosition()
        {
            return AvailableList[Random.Range(0, AvailableList.Count)];
        }

        public void SetNotAvailable(Vector2Int v2)
        {
            AvailableList.Remove(v2);
        }
    }