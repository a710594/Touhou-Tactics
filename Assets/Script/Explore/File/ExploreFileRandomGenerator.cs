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
    private Grid2D<CellType> grid;
    private List<Vector2Int> _groundList = new List<Vector2Int>();
    private List<Vector2Int> _wallList = new List<Vector2Int>();
    private List<Vector2Int> _treasurePositionList = new List<Vector2Int>();

    public ExploreFile Create(RandomFloorModel floorData, int seed = 0)
    {
        roomList.Clear();
        _groundList.Clear();
        _wallList.Clear();
        _treasurePositionList.Clear();

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
        Vector2Int v2;
        TreasureModel treasureData;
        ExploreFileTreasure treasureFile;
        int treasureCount;
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

        //尋找孤立的點
        List<Room> isolated = new List<Room>();
        for (int i=0; i<roomList.Count; i++) 
        {
            if (roomList[i].AdjList.Count == 1) 
            {
                isolated.Add(roomList[i]);
            }
        }

        //去除孤立點的邊
        for (int i=0; i< mst.Count; i++) 
        {
            for (int j=0; j<isolated.Count; j++) 
            {
                //if (isolated[j].Position == ((Vertex<Room>)mst[i].U).Item.bounds.position || isolated[j].Position == ((Vertex<Room>)mst[i].V).Item.bounds.position) 
                if (isolated[j] == ((Vertex<Room>)mst[i].U).Item || isolated[j] == ((Vertex<Room>)mst[i].V).Item)
                {
                    mst.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

        for (int i = 0; i < edges.Count; i++)
        {
            for (int j = 0; j < isolated.Count; j++)
            {
                //if (isolated[j].Position == ((Vertex<Room>)edges[i].U).Item.bounds.position || isolated[j].Position == ((Vertex<Room>)edges[i].V).Item.bounds.position)
                if (isolated[j] == ((Vertex<Room>)edges[i].U).Item || isolated[j] == ((Vertex<Room>)edges[i].V).Item)
                {
                    edges.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges)
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.125)
            //if (_random.NextDouble() < 0.125)
            {
                selectedEdges.Add(edge);
                Debug.Log(((Vertex<Room>)edge.U).Item.bounds.position + " " + ((Vertex<Room>)edge.V).Item.bounds.position);
            }
        }

        Room n1 = DFS(roomList, roomList[0]);
        Room n2 = DFS(roomList, n1);

        roomList.Remove(n1);
        roomList.Remove(n2);

        isolated.Remove(n1);
        isolated.Remove(n2);

        Vector2Int pos;
        TreasureModel treasureData;
        ExploreFileTreasure treasureFile;

        treasureData = DataContext.Instance.TreasureDic[1];
        for (int i = 0; i < roomList.Count; i++)
        {
            pos = roomList[i].GetRandomPosition();
            if (!_treasurePositionList.Contains(pos))
            {
                treasureFile = new ExploreFileTreasure(treasureData.GetItem(), treasureData.Prefab, treasureData.Height, pos, 0);
                File.TreasureList.Add(treasureFile);
                roomList[i].SetNotAvailable(pos);
            }
        }

        treasureData = DataContext.Instance.TreasureDic[2];
        for (int i=0; i< isolated.Count; i++) 
        {
            pos = isolated[i].GetRandomPosition();
            if (!_treasurePositionList.Contains(pos))
            {
                treasureFile = new ExploreFileTreasure(treasureData.GetItem(), treasureData.Prefab, treasureData.Height, pos, 0);
                File.TreasureList.Add(treasureFile);
                isolated[i].SetNotAvailable(pos);
            }
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
        Room startRoom = roomList[0];
        File.Start = startRoom.GetRandomPosition();
        File.PlayerPosition = File.Start;
        startRoom.SetNotAvailable(File.Start);

        List<Room> tempList = new List<Room>(roomList);
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
        goalRoom.SetNotAvailable(File.Goal);
    }
}
