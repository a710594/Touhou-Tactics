using System;
using System.Collections.Generic;
using Explore;
using Graphs;
using UnityEngine;

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
    private List<Room> _roomList = new List<Room>();
    private List<Room> _isolatedList = new List<Room>(); //孤立的房間
    private List<Room> _otherList = new List<Room>(); //其他的房間
    private List<ExploreFileDoor> _doorList = new List<ExploreFileDoor>();

    public ExploreFile Create(RandomFloorModel floorData, int seed = 0)
    {
        _roomList.Clear();
        _isolatedList.Clear();
        _otherList.Clear();
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
        SetTreasure();
        PathfindHallways();
        PlaceWall();
        PlaceStartAndGoal();
        SetEnemy(floorData);

        return File;
    }

    private void PlaceRooms(RandomFloorModel floorData)
    {
        RoomModel roomData;
        for (int i = 0; i < floorData.RoomCount * 2; i++) 
        {
            roomData = DataTable.Instance.RoomDic[1];
            Vector2Int location = new Vector2Int(
                _random.Next(0, File.Size.x),
                _random.Next(0, File.Size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                _random.Next(roomData.MinWidth, roomData.MaxWidth + 1),
                _random.Next(roomData.MinHeight, roomData.MaxHeight + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in _roomList) {
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
                newRoom.Data = roomData;
                _roomList.Add(newRoom);
                Vector2Int pos;
                for (int j=newRoom.bounds.xMin + 1; j<newRoom.bounds.xMax; j++) 
                {
                    for (int k=newRoom.bounds.yMin + 1; k<newRoom.bounds.yMax; k++) 
                    {
                        pos = new Vector2Int(j, k);
                        grid[pos] = CellType.Room;
                        File.TileList.Add(new ExploreFileTile("Ground", pos));
                        _groundList.Add(pos);
                    }
                }
            }
        }
    }

    void Triangulate() {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in _roomList) {
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
        for (int i = 0; i < mst.Count; i++)
        {
            uRoom = ((Vertex<Room>)mst[i].U).Item;
            uPos = uRoom.bounds.position;

            vRoom = ((Vertex<Room>)mst[i].V).Item;
            vPos = vRoom.bounds.position;

            uRoom.AdjList.Add(vRoom);
            vRoom.AdjList.Add(uRoom);
        }

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
            if (UnityEngine.Random.Range(0f, 1f) < 0.25)
            {
                selectedEdges.Add(remainList[i]);
                uRoom = ((Vertex<Room>)remainList[i].U).Item;
                vRoom = ((Vertex<Room>)remainList[i].V).Item;
                uRoom.AdjList.Add(vRoom);
                vRoom.AdjList.Add(uRoom);
            }
        }

        _startRoom = BFS(_roomList, _roomList[0]);
        _endRoom = BFS(_roomList, _startRoom);

        for (int i = 0; i < _roomList.Count; i++) 
        {
            if (_roomList[i] != _startRoom)
            {
                if (_roomList[i].Isolated)
                {
                    _isolatedList.Add(_roomList[i]);
                }
                else
                {
                    _otherList.Add(_roomList[i]);
                }
            }
        }
    }

    private void SetTreasure() 
    {
        int count;
        Vector2Int pos;
        Room room;
        TreasureModel treasureData;
        ExploreFileTreasure treasureFile;

        //create key
        for (int i = 0; i < _isolatedList.Count; i++)
        {
            room = _otherList[UnityEngine.Random.Range(0, _otherList.Count)];
            if (room.TryGetRandomPosition(out pos))
            {
                treasureFile = new ExploreFileTreasure(ItemManager.KeyID, "Key", pos);
                File.TreasureList.Add(treasureFile);
                room.SetNotAvailable(pos);
            }
            else
            {
                i--;
            }
        }

        //create treasure
        List<Room> list = new List<Room>(_isolatedList);
        list.Remove(_endRoom);
        for (int i = 0; i < list.Count; i++)
        {
            room = list[i];
            if (room.TryGetRandomPosition(out pos))
            {
                treasureData = DataTable.Instance.TreasureDic[3];
                treasureFile = new ExploreFileTreasure(treasureData.GetItemID(), treasureData.Prefab, pos);
                File.TreasureList.Add(treasureFile);
                room.SetNotAvailable(pos);
            }
            else
            {
                i--;
            }
        }

        for (int i = 0; i < _otherList.Count; i++)
        {
            room = _otherList[i];
            count = UnityEngine.Random.Range(room.Data.MinTreasureCount, room.Data.MaxTreasureCount + 1);
            for (int j=0; j<count; j++) 
            {
                if (_otherList[i].TryGetRandomPosition(out pos))
                {
                    treasureData = DataTable.Instance.TreasureDic[room.Data.GetTreasure()];
                    treasureFile = new ExploreFileTreasure(treasureData.GetItemID(), treasureData.Prefab, pos);
                    File.TreasureList.Add(treasureFile);
                    _otherList[i].SetNotAvailable(pos);
                }
            }
        }
    }

    private Room DFS(List<Room> roomList, Room room, List<Room> visited) 
    {
        if(visited==null)
        {
            visited = new List<Room>();
        }
        visited.Add(room);
        Room last = room;
        for (int i=0; i<room.AdjList.Count; i++) 
        {
            if (!visited.Contains(room.AdjList[i])) 
            {
                last = DFS(roomList, room.AdjList[i], visited);
            }
        }
        return last;
    }

    private Room BFS(List<Room> roomList, Room start) 
    {
        Queue<Room> queue = new Queue<Room>();
        List<Room> visited = new List<Room>();
        queue.Enqueue(start);

        Room room = null;
        while (queue.Count != 0)
        {
            room = queue.Dequeue();
            visited.Add(room);
            for (int i=0; i<room.AdjList.Count; i++) 
            {
                if (!visited.Contains(room.AdjList[i])) 
                {
                    queue.Enqueue(room.AdjList[i]);
                }
            }      
        }

        return room;
    }

    void PathfindHallways() {
        ExploreFileDoor door;
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(new Vector2Int(File.Size.x, File.Size.y));

        foreach (var edge in selectedEdges) {
            var room_1 = (edge.U as Vertex<Room>).Item;
            var room_2 = (edge.V as Vertex<Room>).Item;

            Vector2Int pos_1 = Vector2Int.RoundToInt(room_1.bounds.center);
            Vector2Int pos_2 = Vector2Int.RoundToInt(room_2.bounds.center);
            Vector2Int temp;

            //門的位置在距離孤立房間最遠的相鄰走廊
            //所以 room_2.Isolated 的情況下 pos_1 和 pos_2 要交換
            Room isolatedRoom = null;
            if (room_1.Isolated && room_1 != _startRoom) 
            {
                isolatedRoom = room_1;
            }
            else if (room_2.Isolated && room_2 != _startRoom) 
            {
                isolatedRoom = room_2;
                temp = pos_1;
                pos_1 = pos_2;
                pos_2 = temp;
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
                List<Vector2Int> positionList = new List<Vector2Int>();
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                        File.TileList.Add(new ExploreFileTile("Ground", current));
                        _groundList.Add(current);

                        //find door
                        if (isolatedRoom != null)
                        {
                            if (isolatedRoom.InBound(current + Vector2Int.up) ||
                               isolatedRoom.InBound(current + Vector2Int.down) ||
                               isolatedRoom.InBound(current + Vector2Int.left) ||
                               isolatedRoom.InBound(current + Vector2Int.right))
                            {
                                positionList.Add(current);
                            }
                        }
                    }
                }

                if (positionList.Count > 0)
                {
                    while (positionList.Count > 1)
                    {
                        positionList.RemoveAt(0);
                    }
                    door = new ExploreFileDoor(positionList[0]);
                    _doorList.Add(door);
                }
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
            File.TileList.Add(new ExploreFileTile("Wall", position));
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
        _startRoom.TryGetRandomPosition(out File.Start);
        _startRoom.SetNotAvailable(File.Start);
        File.PlayerPositionX = File.Start.x;
        File.PlayerPositionZ = File.Start.y;

        if (_wallList.Contains(File.Start + Vector2Int.up))
        {
            File.PlayerRotationY = 90;
            if (_wallList.Contains(File.Start + Vector2Int.right))
            {
                File.PlayerRotationY = 180;
                if (_wallList.Contains(File.Start + Vector2Int.down))
                {
                    File.PlayerRotationY = 270;
                }
            }
        }

        _endRoom.TryGetRandomPosition(out File.Goal);
        _endRoom.SetNotAvailable(File.Goal);
    }

    private void SetEnemy(RandomFloorModel data)
    {
        Vector2Int pos;
        EnemyGroupModel enemyGroup;
        for (int i = 0; i < _otherList.Count; i++) //每個房間有一隻怪
        {
            enemyGroup = DataTable.Instance.EnemyGroupDic[data.GetEnemyGroupID()];
            if (_otherList[i].TryGetRandomPosition(out pos))
            {
                File.EnemyList.Add(new ExploreFileEnemy(enemyGroup, pos));
            }
        }
    }
}
