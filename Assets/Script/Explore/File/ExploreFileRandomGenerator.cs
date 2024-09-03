using System.Collections;
using System.Collections.Generic;
using Explore;
using Graphs;
using UnityEngine;
using UnityEngine.Rendering;

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
    List<Room> rooms = new List<Room>();
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    private Transform _root;
    private LayerMask _mapLayer = LayerMask.NameToLayer("Map");
    private Grid2D<CellType> grid;
    private List<Vector2Int> _groundList = new List<Vector2Int>();
    private List<Vector2Int> _wallList = new List<Vector2Int>();
    private List<Vector2Int> _treasurePositionList = new List<Vector2Int>();

    public ExploreFile Create(RandomFloorModel floorData)
    {
        File = new ExploreFile();
        File.Floor = floorData.Floor;
        File.Size = new Vector2Int(floorData.Width, floorData.Height);
        grid = new Grid2D<CellType>(File.Size, Vector2Int.zero);
        _root = GameObject.Find("Generator2D").transform;
        PlaceRooms(floorData);
        Triangulate();
        CreateHallways();
        PathfindHallways();
        PlaceWall();
        PlaceStartAndGoal();
        SetTreasure();
        SetEnemy(floorData);

        return File;
    }

    private void PlaceRooms(RandomFloorModel floorData)
    {
        RoomModel roomData;
        for (int i = 0; i < floorData.RoomCount; i++) 
        {
            roomData = DataContext.Instance.RoomDic[floorData.GetRoomID()];
            Vector2Int location = new Vector2Int(
                Random.Range(0, File.Size.x),
                Random.Range(0, File.Size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                Random.Range(roomData.MinWidth, roomData.MaxWidth + 1),
                Random.Range(roomData.MinHeight, roomData.MaxHeight + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms) {
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
                rooms.Add(newRoom);
                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    grid[pos] = CellType.Room;
                    File.TileList.Add(new ExploreFileTile(true, false, "Ground", pos));
                    _groundList.Add(pos);
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

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges) {
            if (Random.Range(0f, 1f) < 0.125) {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways() {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(new Vector2Int(File.Size.x, File.Size.y));

        foreach (var edge in selectedEdges) {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
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

            if (path != null) {
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                        File.TileList.Add(new ExploreFileTile(true, false, "Ground", current));
                        _groundList.Add(current);
                    }

                    if (i > 0) {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }
            }
        }
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
        Room startRoom = rooms[0];
        File.Start = startRoom.GetRandomPosition();
        startRoom.SetNotAvailable(File.Start);

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

        File.Goal = goalRoom.GetRandomPosition();
        File.PlayerPosition = File.Goal;
        goalRoom.SetNotAvailable(File.Goal);
    }

    private void SetTreasure()
    {
        int treasureCount = 10;
        Vector2Int v2;
        Room room;
        TreasureObject treasure;
        GameObject obj;
        for (int i = 0; i < treasureCount; i++)
        {
            room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            v2 =room.GetRandomPosition(); 
            if (!_treasurePositionList.Contains(v2))
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + "TreasureBox"), Vector3.zero, Quaternion.identity);
                treasure = obj.GetComponent<TreasureObject>();
                treasure.ItemID = 1;
                treasure.Prefab = "TreasureBox";
                File.TreasureList.Add(new ExploreFileTreasure(1, "TreasureBox", 1, v2, Vector3Int.zero));
                room.SetNotAvailable(v2);
            }     
        }
    }

    private void SetEnemy(RandomFloorModel data)
    {
        int groupId;
        Vector2Int position;
        Room room;
        EnemyGroupModel groupData;
        ExploreFileEnemy enemy;
        for (int i = 0; i < data.EnemyCount; i++)
        {
            room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            position = room.GetRandomPosition();
            room.SetNotAvailable(position);

            groupId = data.EnemyGroupPool[UnityEngine.Random.Range(0, data.EnemyGroupPool.Count)];
            groupData = DataContext.Instance.EnemyGroupDic[groupId];
            enemy = new ExploreFileEnemy();
            enemy.Type = ExploreFileEnemy.TypeEnum.Random;
            enemy.AI = ExploreFileEnemy.AiEnum.Default;
            enemy.Position = position;
            enemy.RotationY = 0;
            enemy.EnemyGroupId = groupId;
            enemy.Prefab = groupData.Prefab;
            File.EnemyList.Add(enemy);   
        }
    }
}
