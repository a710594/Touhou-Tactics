using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Explore;
using Graphs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Generator2D;
using static UnityEditor.PlayerSettings;

public class ExploreFileRandomGenerator
{
    private static ExploreFileRandomGenerator _instance;
    public static ExploreFileRandomGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ExploreFileRandomGenerator();
            }
            return _instance;
        }
    }

    public enum CellType
    {
        None,
        Room,
        Hallway,
        Wall,
    }

    public ExploreFile File;
    List<Room> roomList = new List<Room>();
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    private System.Random _random;
    private LayerMask _mapLayer = LayerMask.NameToLayer("Map");
    private Room _startRoom;
    private Room _endRoom;
    private Grid2D<CellType> grid;
    private List<Vector2Int> _groundList = new List<Vector2Int>();
    private List<Vector2Int> _wallList = new List<Vector2Int>();
    private List<Vector2Int> _treasurePositionList = new List<Vector2Int>();
    private List<ExploreFIleDoor> _doorList = new List<ExploreFIleDoor>();

    public ExploreFile Create(RandomFloorModel floorData, int seed = 0)
    {
        roomList.Clear();
        _groundList.Clear();
        _wallList.Clear();
        _treasurePositionList.Clear();
        _doorList.Clear();

        if (seed == 0)
        {
            _random = new System.Random(Guid.NewGuid().GetHashCode());
        }
        else
        {
            _random = new System.Random(seed);
        }

        File = new ExploreFile();
        File.Floor = floorData.Floor;
        File.Size = new Vector2Int(floorData.Width, floorData.Height);
        grid = new Grid2D<CellType>(File.Size, Vector2Int.zero);
        PlaceRooms(floorData);
        Triangulate();
        CreateHallways();
        PathfindHallways();
        PlaceWall();
        PlaceStartAndGoal();
        //SetTreasure();
        //SetEnemy(floorData);

        return File;
    }

    private void PlaceRooms(RandomFloorModel floorData)
    {
        RoomModel roomData;
        for (int i = 0; i < floorData.RoomCount * 2; i++) 
        {
            roomData = DataContext.Instance.RoomDic[floorData.GetRoomID()];
            Vector2Int location = new Vector2Int(
                //Random.Range(0, File.Size.x),
                //Random.Range(0, File.Size.y)
                _random.Next(0, File.Size.x),
                _random.Next(0, File.Size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                //Random.Range(roomData.MinWidth, roomData.MaxWidth + 1),
                //Random.Range(roomData.MinHeight, roomData.MaxHeight + 1)
                _random.Next(roomData.MinWidth, roomData.MaxWidth + 1),
                _random.Next(roomData.MinHeight, roomData.MaxHeight + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in roomList) {
                if (Room.Intersect(room, buffer)) {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= floorData.Width
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= floorData.Height) {
                add = false;
            }

            if (add) {
                roomList.Add(newRoom);
                Vector2Int pos;
                for (int j=newRoom.bounds.xMin + 1; j<newRoom.bounds.xMax; j++) 
                {
                    for (int k=newRoom.bounds.yMin + 1; k<newRoom.bounds.yMax; k++) 
                    {
                        pos = new Vector2Int(j, k);
                        grid[pos] = CellType.Room;
                        File.TileList.Add(new ExploreFileTile(true, false, "Ground", pos));
                        _groundList.Add(pos);
                    }
                }
                //foreach (var pos in newRoom.bounds.allPositionsWithin)
                //{
                //    grid[pos] = CellType.Room;
                //    File.TileList.Add(new ExploreFileTile(true, false, "Ground", pos));
                //    _groundList.Add(pos);
                //}
            }
        }
    }

    void Triangulate() {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in roomList) {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways() {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges) {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        //get nodeList
        Vector2Int uPos;
        Vector2Int vPos;
        Room uRoom;
        Room vRoom;
        Dictionary<Vector2Int, Room> nodeDic = new Dictionary<Vector2Int, Room>();
        for (int i = 0; i < mst.Count; i++)
        {
            uRoom = ((Vertex<Room>)mst[i].U).Item;
            uPos = uRoom.bounds.position;
            if (!nodeDic.ContainsKey(uPos))
            {
                nodeDic.Add(uPos, uRoom);
            }

            vRoom = ((Vertex<Room>)mst[i].V).Item;
            vPos = vRoom.bounds.position;
            if (!nodeDic.ContainsKey(vPos))
            {
                nodeDic.Add(vPos, vRoom);
            }

            nodeDic[uPos].AdjList.Add(nodeDic[vPos]);
            nodeDic[vPos].AdjList.Add(nodeDic[uPos]);
        }

        List<Room> roomList = new List<Room>(nodeDic.Values);
        List<Room> isolatedList = new List<Room>(); //孤立的房間
        List<Room> otherList = new List<Room>(); //其他的房間

        selectedEdges = new HashSet<Prim.Edge>(mst);
        HashSet<Prim.Edge> remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);
        List<Prim.Edge> remainList = new List<Prim.Edge>(remainingEdges);

        //去除孤立點的邊
        for (int i = 0; i < remainList.Count; i++)
        {
            if (((Vertex<Room>)remainList[i].U).Item.Isolated || ((Vertex<Room>)remainList[i].V).Item.Isolated)
            {
                remainList.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < remainList.Count; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.125)
            //if (_random.NextDouble() < 0.125)
            {
                selectedEdges.Add(remainList[i]);
                uRoom = ((Vertex<Room>)remainList[i].U).Item;
                vRoom = ((Vertex<Room>)remainList[i].V).Item;
                uRoom.AdjList.Add(vRoom);
                vRoom.AdjList.Add(uRoom);
            }
        }

        _startRoom = DFS(roomList, roomList[0]);
        _endRoom = DFS(roomList, _startRoom);

        for (int i = 0; i < roomList.Count; i++) 
        {
            if(roomList[i] != _startRoom && roomList[i] != _endRoom)
            if (roomList[i].Isolated) 
            {
                isolatedList.Add(roomList[i]);
            }
            else
            {
                otherList.Add(roomList[i]);
            }
        }

        Vector2Int pos;
        TreasureModel treasureData;
        ExploreFileTreasure treasureFile;
        List<TreasureModel> treasureList = new List<TreasureModel>();

        Room room;
        for (int i=0; i<isolatedList.Count; i++) 
        {
            treasureList = DataContext.Instance.TreasureDic[TreasureModel.TypeEnum.Special];
            treasureData = treasureList[UnityEngine.Random.Range(0, treasureList.Count)];

            pos = isolatedList[i].GetRandomPosition();
            treasureFile = new ExploreFileTreasure(treasureData.GetID(), treasureData.Prefab, treasureData.Height, pos, 0);
            File.TreasureList.Add(treasureFile);
            isolatedList[i].SetNotAvailable(pos);
            

            //create key
            room = otherList[UnityEngine.Random.Range(0, otherList.Count)];
            pos = room.GetRandomPosition();
            treasureFile = new ExploreFileTreasure(ItemManager.KeyID, "Key", 1, pos, 0);
            File.TreasureList.Add(treasureFile);
            room.SetNotAvailable(pos);
        }

        for (int i = 0; i < otherList.Count; i++)
        {
            treasureList = DataContext.Instance.TreasureDic[TreasureModel.TypeEnum.Normal];
            treasureData = treasureList[UnityEngine.Random.Range(0, treasureList.Count)];

            pos = otherList[i].GetRandomPosition();
            treasureFile = new ExploreFileTreasure(treasureData.GetID(), treasureData.Prefab, treasureData.Height, pos, 0);
            File.TreasureList.Add(treasureFile);
            otherList[i].SetNotAvailable(pos);
        }
    }

    //private class Node
    //{
    //    public Vector2Int Position;
    //    public List<Node> AdjList = new List<Node>();

    //    public Node(Vector2Int position) 
    //    {
    //        Position = position;
    //    }
    //}

    private Room DFS(List<Room> roomList, Room start) 
    {
        Room current = null;
        List<Room> visited = new List<Room>();
        Queue<Room> unVisited = new Queue<Room>();
        unVisited.Enqueue(start);

        while (unVisited.Count != 0)
        {
            current = unVisited.Dequeue(); //当前访问的
            visited.Add(current);
            for(int i=0; i<current.AdjList.Count; i++)
            {
                if (!visited.Contains(current.AdjList[i]))
                {
                    unVisited.Enqueue(current.AdjList[i]);
                }
            }         
        }

        return current;
    }

    void PathfindHallways() {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(new Vector2Int(File.Size.x, File.Size.y));

        foreach (var edge in selectedEdges) {
            var room_1 = (edge.U as Vertex<Room>).Item;
            var room_2 = (edge.V as Vertex<Room>).Item;

            Vector2Int pos_1 = Vector2Int.RoundToInt(room_1.bounds.center);
            Vector2Int pos_2 = Vector2Int.RoundToInt(room_2.bounds.center);

            Room isolatedRoom = null;
            if (room_1.Isolated && room_1 != _startRoom && room_1 != _endRoom) 
            {
                isolatedRoom = room_1;
            }
            else if (room_2.Isolated && room_2 != _startRoom && room_2 != _endRoom) 
            {
                isolatedRoom = room_2;
            }

            var path = aStar.FindPath(pos_1, pos_2, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
                var pathCost = new DungeonPathfinder2D.PathCost();
                
                pathCost.cost = Vector2Int.Distance(b.Position, pos_2);    //heuristic

                if (grid[b.Position] == CellType.Room)
                {
                    pathCost.cost += 10;
                }
                else if (grid[b.Position] == CellType.None)
                {
                    pathCost.cost += 5;
                }
                else if (grid[b.Position] == CellType.Hallway)
                {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null) {
                ExploreFIleDoor door = new ExploreFIleDoor();
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                        File.TileList.Add(new ExploreFileTile(true, false, "Ground", current));
                        _groundList.Add(current);

                        //find door
                        if (isolatedRoom != null)
                        {
                            if (isolatedRoom.InBound(current + Vector2Int.up) ||
                               isolatedRoom.InBound(current + Vector2Int.down) ||
                               isolatedRoom.InBound(current + Vector2Int.left) ||
                               isolatedRoom.InBound(current + Vector2Int.right))
                            {
                                door.PositionList.Add(current);
                            }
                        }
                    }

                    if (i > 0) {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }
                _doorList.Add(door);
            }
        }
        File.DoorList = _doorList;
    }

    void PlaceWall()
    {
        for (int i=0; i<_groundList.Count; i++) 
        {
            //左
            CheckWall(new Vector2Int(_groundList[i].x - 1, _groundList[i].y));

            //右
            CheckWall(new Vector2Int(_groundList[i].x + 1, _groundList[i].y));

            //下
            CheckWall(new Vector2Int(_groundList[i].x, _groundList[i].y - 1));

            //上
            CheckWall(new Vector2Int(_groundList[i].x, _groundList[i].y + 1));

            //左下
            CheckWall(new Vector2Int(_groundList[i].x - 1, _groundList[i].y - 1));

            //左上
            CheckWall(new Vector2Int(_groundList[i].x - 1, _groundList[i].y + 1));

            //右下
            CheckWall(new Vector2Int(_groundList[i].x + 1, _groundList[i].y - 1));

            //右上
            CheckWall(new Vector2Int(_groundList[i].x + 1, _groundList[i].y + 1));
        }
    }

    private bool CheckWall(Vector2Int position)
    {
        if (!_groundList.Contains(position) && !_wallList.Contains(position))
        {
            File.TileList.Add(new ExploreFileTile(false, false, "Wall", position));
            _wallList.Add(position);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PlaceStartAndGoal()
    {
        File.Start = _startRoom.GetRandomPosition();
        File.PlayerPosition = File.Start;
        _startRoom.SetNotAvailable(File.Start);

        File.Goal = _endRoom.GetRandomPosition();
        _endRoom.SetNotAvailable(File.Goal);
    }
}
