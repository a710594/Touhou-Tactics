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

    public ExploreInfo Info;
    List<Room> rooms;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    public void Generate(RandomFloorModel floorData)
    {
        Info = new ExploreInfo();
        Info.Floor = floorData.Floor;
        Info.Size = new Vector2Int(floorData.Width, floorData.Height);
        PlaceRooms(floorData);
        Triangulate();
        CreateHallways();
        PathfindHallways();
        PlaceWall();
    }

    private void PlaceRooms(RandomFloorModel floorData)
        {
        RoomModel roomData;
        for (int i = 0; i < floorData.RoomCount; i++) 
        {
            roomData = DataContext.Instance.RoomDic[floorData.GetRoomID()];
            Vector2Int location = new Vector2Int(
                Random.Range(0, floorData.Width),
                Random.Range(0, floorData.Height)
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
                ExploreInfoTile tile;
                foreach (var pos in newRoom.bounds.allPositionsWithin) {
                    if(!Info.TileDic.ContainsKey(pos))
                    {
                        tile = new ExploreInfoTile();
                        tile.IsVisited = false;
                        tile.IsWalkable = true;
                        tile.Event = null;
                        tile.Type = ExploreInfoTile.CellType.Room;
                        Info.TileDic.Add(pos, tile);
                    }
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
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(Info.Size);

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

                if (Info.TileDic[b.Position].Type == ExploreInfoTile.CellType.Room) {
                    pathCost.cost += 10;
                } else if (Info.TileDic[b.Position].Type == ExploreInfoTile.CellType.None) {
                    pathCost.cost += 5;
                } else if (Info.TileDic[b.Position].Type == ExploreInfoTile.CellType.Hallway) {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null) {
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (Info.TileDic[current].Type == ExploreInfoTile.CellType.None) {
                        Info.TileDic[current].Type = ExploreInfoTile.CellType.Hallway;
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
        foreach(KeyValuePair<Vector2Int, ExploreInfoTile> pair in Info.TileDic)
        {
            //左
            x = pair.Key.x - 1;
            y = pair.Key.y;
            CheckWall(new Vector2Int(x, y));

            //右
            x = pair.Key.x + 1;
            y = pair.Key.y;
            CheckWall(new Vector2Int(x, y));

            //下
            x = pair.Key.x;
            y = pair.Key.y - 1;
            CheckWall(new Vector2Int(x, y));

            //上
            x = pair.Key.x;
            y = pair.Key.y + 1;
            CheckWall(new Vector2Int(x, y));

            //左下
            x = pair.Key.x - 1;
            y = pair.Key.y - 1;
            CheckWall(new Vector2Int(x, y));

            //左上
            x = pair.Key.x - 1;
            y = pair.Key.y + 1;
            CheckWall(new Vector2Int(x, y));

            //右下
            x = pair.Key.x + 1;
            y = pair.Key.y - 1;
            CheckWall(new Vector2Int(x, y));

            //右上
            x = pair.Key.x + 1;
            y = pair.Key.y + 1;
            CheckWall(new Vector2Int(x, y));
        }
    }

    private bool CheckWall(Vector2Int position)
    {
        ExploreInfoTile tile;
        if(!Info.TileDic.ContainsKey(position))
        {
            tile = new ExploreInfoTile();
            tile.IsVisited = false;
            tile.IsWalkable = true;
            tile.Event = null;
            tile.Type = ExploreInfoTile.CellType.None;
            Info.TileDic.Add(position, tile);
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
    }

    private void SetTreasure()
    {
        int treasureCount = 10;
        Vector2Int v2;
        Room room;
        ExploreInfoTreasure treasure;
        for (int i = 0; i < treasureCount; i++)
        {
            room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            v2 =room.GetRandomPosition(); 
            if (Info.TileDic[v2].Treasure == null)
            {
                treasure = new ExploreInfoTreasure();
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
        Vector2Int v2;
        Room room;
        EnemyGroupModel groupData;
        ExploreInfoEnemy enemy;
        for (int i = 0; i < data.EnemyCount; i++)
        {
            room = rooms[UnityEngine.Random.Range(0, rooms.Count)];
            v2 = room.GetRandomPosition(); 
            room.SetNotAvailable(v2);
            //todo
        }
    }
}
