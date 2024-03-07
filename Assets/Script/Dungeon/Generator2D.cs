using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using Explore;

public class Generator2D : MonoBehaviour {
    private enum CellType {
        None,
        Room,
        Hallway,
        Wall,
    }

    Random random;
    Grid2D<CellType> grid;
    List<Room> rooms;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;
    ExploreInfo info;

    private Vector2Int _floorSize;
    private int _roomCount;

    void Start() {
        //Generate();
    }

    public ExploreInfo Generate(int floor)
    {
        FloorModel data = DataContext.Instance.FloorDic[floor];
        
        return Generate(data);
    }

    public ExploreInfo Generate(FloorModel data) {
        _floorSize = new Vector2Int(data.Width, data.Height);
        _roomCount = data.RoomCount;

        int seed = (int)System.DateTime.Now.Ticks;
        Debug.Log("Seed:" + seed.ToString());
        random = new Random(seed);
        grid = new Grid2D<CellType>(_floorSize, Vector2Int.zero);
        rooms = new List<Room>();
        info = new ExploreInfo();
        info.Size = _floorSize;
        info.Floor = data.Floor;

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
        PlaceWall();
        PlaceStartAndGoal();
        CreateEnemy(data);
        info.PlayerPosition = info.Start;

        return info;
    }

    //public void Relod(ExploreInfo info)
    //{
    //    this.info = info;
    //    _start = info.Start;
    //    _goal = info.Goal;
    //    PlaceObject();
    //    foreach(KeyValuePair<Vector2Int, TileObject> pair in info.TileDic) 
    //    {
    //        if (info.VisitedList.Contains(pair.Key)) 
    //        {
    //            if (pair.Value.Quad != null) 
    //            {
    //                pair.Value.Quad.layer = ExploreManager.Instance.MapLayer;
    //            }
    //            if (pair.Value.Icon != null) 
    //            {
    //                pair.Value.Icon.layer = ExploreManager.Instance.MapLayer;
    //            }
    //        }
    //    }
    //}

    void PlaceRooms() {
        for (int i = 0; i < _roomCount; i++) {
            Vector2Int location = new Vector2Int(
                random.Next(0, _floorSize.x),
                random.Next(0, _floorSize.y)
            );

            bool add = true;
            Room newRoom = RoomFactory.GetRoom(1, location, random);
            Room buffer = new Room(location + new Vector2Int(-1, -1), newRoom.bounds.size + new Vector2Int(2, 2));

            foreach (var room in rooms) {
                if (Room.Intersect(room, buffer)) {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= _floorSize.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= _floorSize.y) {
                add = false;
            }
            
            if (add) {
                rooms.Add(newRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin) {
                    grid[pos] = CellType.Room;
                    if (!info.TileDic.ContainsKey(pos)) 
                    {
                        info.TileDic.Add(pos, new TileObject("Ground"));
                        info.GroundList.Add(pos);
                    }
                }

                Dictionary<Vector2Int, Treasure> treasures = newRoom.GetTreasures();
                foreach(KeyValuePair<Vector2Int, Treasure> pair in treasures) 
                {
                    info.TreasureDic.Add(pair.Key, pair.Value);
                }
            }
        }
    }

    void Triangulate() {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms) {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways() {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges) {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        if (edges.Count > 0)
        {
            List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

            selectedEdges = new HashSet<Prim.Edge>(mst);
            var remainingEdges = new HashSet<Prim.Edge>(edges);
            remainingEdges.ExceptWith(selectedEdges);

            foreach (var edge in remainingEdges)
            {
                if (random.NextDouble() < 0.125) //額外的路徑
                {
                    selectedEdges.Add(edge);
                }
            }
        }
    }

    void PathfindHallways() {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(_floorSize);

        if (selectedEdges != null)
        {
            foreach (var edge in selectedEdges)
            {
                var startRoom = (edge.U as Vertex<Room>).Item;
                var endRoom = (edge.V as Vertex<Room>).Item;

                var startPosf = startRoom.bounds.center;
                var endPosf = endRoom.bounds.center;
                var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
                var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

                var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) =>
                {
                    var pathCost = new DungeonPathfinder2D.PathCost();

                    pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

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

                if (path != null)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        var current = path[i];

                        if (grid[current] == CellType.None)
                        {
                            grid[current] = CellType.Hallway;
                        }

                        if (i > 0)
                        {
                            var prev = path[i - 1];

                            var delta = current - prev;
                        }
                    }

                    foreach (var pos in path)
                    {
                        if (!info.TileDic.ContainsKey(pos))
                        {
                            if (grid[pos] == CellType.Hallway)
                            {
                                info.TileDic.Add(pos, new TileObject("Ground"));
                                info.GroundList.Add(pos);
                            }
                        }
                    }

                    //add adjRoom
                    if (!startRoom.AdjRoomList.Contains(endRoom))
                    {
                        startRoom.AdjRoomList.Add(endRoom);
                    }
                    if (!endRoom.AdjRoomList.Contains(startRoom))
                    {
                        endRoom.AdjRoomList.Add(startRoom);
                    }
                }
            }
        }
    }

    void PlaceWall()
    {
        Vector2Int position;
        for (int i=-1; i<=grid.Size.x; i++) 
        {
            for(int j=-1; j<=grid.Size.y; j++)
            {
                position = new Vector2Int(i, j);
                if (InBound(i, j))
                {
                    if (grid[i, j] == CellType.None && CheckWall(position))
                    {
                        grid[i, j] = CellType.Wall;

                        info.TileDic.Add(position, new TileObject("Wall"));
                    }
                }
                else
                {
                    if (CheckWall(position))
                    {
                        info.TileDic.Add(position, new TileObject("Wall"));
                    }
                }
            }
        }
    }

    private bool CheckWall(Vector2Int position)
    {
        int x;
        int y;
        CellType cellType;
        //左
        x = position.x - 1;
        y = position.y;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        //右
        x = position.x + 1;
        y = position.y;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        //下
        x = position.x;
        y = position.y - 1;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        //上
        x = position.x;
        y = position.y + 1;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        //左下
        x = position.x - 1;
        y = position.y - 1;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        //左上
        x = position.x - 1;
        y = position.y + 1;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        //右下
        x = position.x + 1;
        y = position.y - 1;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        //右上
        x = position.x + 1;
        y = position.y + 1;
        if (InBound(x, y))
        {
            cellType = grid[x, y];
            if (cellType != CellType.None && cellType != CellType.Wall)
            {
                return true;
            }
        }
        return false;
    }

    private void PlaceStartAndGoal()
    {
        Room startRoom = rooms[random.Next(0, rooms.Count)];
        List<Vector2Int> positionList = new List<Vector2Int>();
        for (int i = 0; i < startRoom.bounds.size.x; i++)
        {
            for (int j = 0; j < startRoom.bounds.size.y; j++)
            {
                positionList.Add(new Vector2Int(startRoom.bounds.position.x + i, startRoom.bounds.y + j));
            }
        }
        foreach(KeyValuePair<Vector2Int, Treasure> pair in info.TreasureDic)
        {
            positionList.Remove(pair.Key);
        }
        info.Start = positionList[random.Next(0, positionList.Count)];

        List<Room> tempList = new List<Room>(rooms);
        tempList.Remove(startRoom);
        Room goalRoom = null;
        for (int i = 0; i < tempList.Count; i++)
        {
            if (goalRoom == null || Vector3.Distance(tempList[i].bounds.center, startRoom.bounds.center) > Vector3.Distance(goalRoom.bounds.center, startRoom.bounds.center))
            {
                goalRoom = tempList[i];
            }
        }

        positionList.Clear();
        for (int i = 0; i < goalRoom.bounds.size.x; i++)
        {
            for (int j = 0; j < goalRoom.bounds.size.y; j++)
            {
                positionList.Add(new Vector2Int(goalRoom.bounds.position.x + i, goalRoom.bounds.y + j));
            }
        }
        foreach (KeyValuePair<Vector2Int, Treasure> pair in info.TreasureDic)
        {
            positionList.Remove(pair.Key);
        }

        info.Goal = positionList[random.Next(0, positionList.Count)];
    }

    private void CreateEnemy(FloorModel data) 
    {
        int random;
        List<Vector2Int> walkableList = new List<Vector2Int>(info.GroundList);
        walkableList.Remove(info.Start);
        walkableList.Remove(info.Goal);
        foreach (KeyValuePair<Vector2Int, Treasure> pair in info.TreasureDic)
        {
            walkableList.Remove(pair.Key);
        }

        ExploreEnemyInfo enemyInfo;
        for (int i = 0; i < data.EnemyCount; i++)
        {
            random = UnityEngine.Random.Range(0, walkableList.Count);
            enemyInfo = new ExploreEnemyInfo(data.EnemyName, null, walkableList[random], 0);
            info.EnemyInfoList.Add(enemyInfo);
            walkableList.RemoveAt(random);
        }
        enemyInfo = new ExploreEnemyInfo("FloorBOSS", null, info.Goal, 0);
        info.EnemyInfoList.Add(enemyInfo);
    }

    private bool InBound(int x, int y)
    {
        if (x >= 0 && x < grid.Size.x && y >= 0 && y < grid.Size.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
