using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;

public class Generator2D {
    public enum CellType {
        None,
        Room,
        Hallway
    }

    public class Room {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size) {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b) {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }

        public Vector2Int GetRandomPosition()
        {
            Vector2Int position =  bounds.position + new Vector2Int(UnityEngine.Random.Range(0, bounds.size.x),UnityEngine.Random.Range(0, bounds.size.y));
            return position;
        }

        /*public Dictionary<Vector2Int, Treasure> GetTreasures()
        {
            Dictionary<Vector2Int, Treasure> treasures = new Dictionary<Vector2Int, Treasure>();
            if (Data.TreasurePool.Count > 0)
            {
                List<Vector2Int> positionList = new List<Vector2Int>();
                int random;
                for (int i = 0; i < bounds.size.x; i++)
                {
                    for (int j = 0; j < bounds.size.y; j++)
                    {
                        positionList.Add(new Vector2Int(i, j));
                    }
                }

                int treasureCount = Random.Range(Data.MinTreasureCount, Data.MaxTreasureCount + 1);
                Vector2Int treasurePosition;
                TreasureModel treasureData;
                for (int i = 0; i < treasureCount; i++)
                {
                    random = Random.Range(0, positionList.Count);
                    treasurePosition = bounds.min + positionList[random];
                    positionList.RemoveAt(random);
                    treasureData = DataContext.Instance.TreasureDic[Data.TreasurePool[Random.Range(0, Data.TreasurePool.Count)]];
                    treasures.Add(treasurePosition, new Treasure(treasurePosition, treasureData));
                }
            }
            return treasures;
        }*/
    }

    Random random;
    Grid2D<CellType> grid;
    List<Room> rooms;
    List<List<Vector2Int>> paths;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    public void Generate(Vector2Int size, int roomCount, Vector2Int roomMaxSize, out List<Room> roomList, out List<List<Vector2Int>> pathList) {
        int seed = (int)System.DateTime.Now.Ticks;
        random = new Random(seed);
        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        rooms = new List<Room>();
        paths = new List<List<Vector2Int>>();

        PlaceRooms(size, roomCount, roomMaxSize);
        Triangulate();
        CreateHallways();
        PathfindHallways(size);

        roomList = rooms;
        pathList = paths;
    }

    void PlaceRooms(Vector2Int size, int roomCount, Vector2Int roomMaxSize) {
        for (int i = 0; i < roomCount; i++) {
            Vector2Int location = new Vector2Int(
                random.Next(0, size.x),
                random.Next(0, size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                random.Next(1, roomMaxSize.x + 1),
                random.Next(1, roomMaxSize.y + 1)
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

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y) {
                add = false;
            }

            if (add) {
                rooms.Add(newRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin) {
                    grid[pos] = CellType.Room;
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
            if (random.NextDouble() < 0.125) {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways(Vector2Int size) {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

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

                if (grid[b.Position] == CellType.Room) {
                    pathCost.cost += 10;
                } else if (grid[b.Position] == CellType.None) {
                    pathCost.cost += 5;
                } else if (grid[b.Position] == CellType.Hallway) {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null) {
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None) {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0) {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }

                paths.Add(path);
            }
        }
    }
}