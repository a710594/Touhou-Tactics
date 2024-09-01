using System.Collections;
using System.Collections.Generic;
using Explore;
using Graphs;
using UnityEngine;

public class ExploreInfoGenerator
{
    private static ExploreInfoGenerator _instance;
    public static ExploreInfoGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ExploreInfoGenerator();
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

    public ExploreInfo Info;
    List<Room> rooms = new List<Room>();
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    private Transform _root;
    private LayerMask _mapLayer = LayerMask.NameToLayer("Map");
    private Grid2D<CellType> grid;
    private List<Vector2Int> wallList = new List<Vector2Int>();

    public ExploreInfo Create(RandomFloorModel floorData)
    {
        Info = new ExploreInfo();
        Info.Floor = floorData.Floor;
        Info.Size = new Vector2Int(floorData.Width, floorData.Height);
        grid = new Grid2D<CellType>(Info.Size, Vector2Int.zero);
        _root = GameObject.Find("Generator2D").transform;
        PlaceRooms(floorData);
        Triangulate();
        CreateHallways();
        PathfindHallways();
        PlaceWall();
        PlaceTile();
        PlaceStartAndGoal();
        SetTreasure();
        SetEnemy(floorData);

        return Info;
    }

    private void PlaceRooms(RandomFloorModel floorData)
    {
        RoomModel roomData;
        ExploreInfoTile tile;
        for (int i = 0; i < floorData.RoomCount; i++) 
        {
            roomData = DataContext.Instance.RoomDic[floorData.GetRoomID()];
            Vector2Int location = new Vector2Int(
                Random.Range(0, Info.Size.x),
                Random.Range(0, Info.Size.y)
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
                    Info.TileDic.Add(pos, new ExploreInfoTile(true, false, "Ground"));
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
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(new Vector2Int(Info.Size.x, Info.Size.y));

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
                        Info.TileDic.Add(current, new ExploreInfoTile(true, false, "Ground"));
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
        int x;
        int y;

        for (int i=0; i<grid.Size.x; i++) 
        {
            for (int j=0; j<grid.Size.y; j++) 
            {
                //左
                x = i - 1;
                y = j;
                CheckWall(new Vector2Int(x, y));

                //右
                x = i + 1;
                y = j;
                CheckWall(new Vector2Int(x, y));

                //下
                x = i;
                y = j - 1;
                CheckWall(new Vector2Int(x, y));

                //上
                x = i;
                y = j + 1;
                CheckWall(new Vector2Int(x, y));

                //左下
                x = i - 1;
                y = j - 1;
                CheckWall(new Vector2Int(x, y));

                //左上
                x = i - 1;
                y = j + 1;
                CheckWall(new Vector2Int(x, y));

                //右下
                x = i + 1;
                y = j - 1;
                CheckWall(new Vector2Int(x, y));

                //右上
                x = i + 1;
                y = j + 1;
                CheckWall(new Vector2Int(x, y));
            }
        }
    }

    private bool CheckWall(Vector2Int position)
    {
        if (!Info.TileDic.ContainsKey(position))
        {
            wallList.Add(position);
            Info.TileDic.Add(position, new ExploreInfoTile(false, false, "Wall"));
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
        Info.Start = startRoom.GetRandomPosition();
        startRoom.SetNotAvailable(Info.Start);

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

        Info.Goal = goalRoom.GetRandomPosition();
        goalRoom.SetNotAvailable(Info.Goal);

        GameObject gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
        gameObj.transform.position = new Vector3(Info.Goal.x, 0, Info.Goal.y);
        gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
        gameObj.transform.SetParent(_root);
        Info.TileDic[Info.Goal].Object.Icon = gameObj;
        if (Info.TileDic[Info.Goal].IsVisited)
        {
            Info.TileDic[Info.Goal].Object.Icon.layer = _mapLayer;
        }
        Info.TileDic[Info.Goal].Object.Icon.GetComponent<Goal>().Red.SetActive(!Info.IsArrive);
        Info.TileDic[Info.Goal].Object.Icon.GetComponent<Goal>().Blue.SetActive(Info.IsArrive);
    }

    private void PlaceTile() 
    {
        TileObject obj;
        foreach (KeyValuePair<Vector2Int, ExploreInfoTile> pair in Info.TileDic) 
        {
            obj = ((GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.Prefab), Vector3.zero, Quaternion.identity)).GetComponent<TileObject>();
            obj.transform.SetParent(_root);
            obj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
            pair.Value.Object = obj;
        }
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
            if (Info.TileDic[v2].Treasure == null)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + "TreasureBox"), Vector3.zero, Quaternion.identity);
                treasure = obj.GetComponent<TreasureObject>();
                treasure.Type = TreasureModel.TypeEnum.Item;
                treasure.ItemID = 1;
                treasure.Prefab = "TreasureBox";
                Info.TileDic[v2].Treasure = treasure;
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
        ExploreInfoEnemy enemy;
        for (int i = 0; i < data.EnemyCount; i++)
        {
            room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            position = room.GetRandomPosition();
            room.SetNotAvailable(position);

            groupId = data.EnemyGroupPool[UnityEngine.Random.Range(0, data.EnemyGroupPool.Count)];
            groupData = DataContext.Instance.EnemyGroupDic[groupId];
            enemy = new ExploreInfoEnemy();
            enemy.Type = ExploreFileEnemy.TypeEnum.Random;
            enemy.EnemyGroupId = groupId;
            enemy.Controller = ((GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + groupData.Prefab), Vector3.zero, Quaternion.identity)).GetComponent<ExploreEnemyController>();
            enemy.Controller.transform.position = new Vector3(position.x, 1, position.y);
            enemy.Controller.Init(ExploreFileEnemy.AiEnum.Default);
            Info.EnemyInfoList.Add(enemy);   
        }
    }
}
