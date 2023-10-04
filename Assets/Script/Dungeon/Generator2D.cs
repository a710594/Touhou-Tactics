using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using Explore;

public class Generator2D : MonoBehaviour {
    public enum CellType {
        None,
        Room,
        Hallway,
        Wall,
    }

    [SerializeField]
    Vector2Int size;
    [SerializeField]
    int roomCount;
    [SerializeField]
    Vector2Int roomMaxSize;
    [SerializeField]
    GameObject cubePrefab;
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    Material blueMaterial;
    [SerializeField]
    Material whiteMaterial;

    Random random;
    Grid2D<CellType> grid;
    List<Room> rooms;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    void Start() {
        //Generate();
    }

    public void Generate() {
        random = new Random(0);
        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
        PlaceWall();
        Explore.ExploreManager.Instance.SetData(grid, rooms);
    }

    public void Relod(Grid2D<Generator2D.CellType> grid, List<Room> rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            PlaceRoom(rooms[i].bounds.position, rooms[i].bounds.size);
        }

        for (int i = 0; i < grid.Size.x; i++)
        {
            for (int j = 0; j < grid.Size.y; j++)
            {
                if (grid[i, j] == CellType.Hallway)
                {
                    PlaceHallway(new Vector2Int(i, j));
                }
                else if (grid[i, j] == CellType.Wall)
                {
                    GameObject go = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                    go.transform.position = new Vector3(i, 2, j);
                    go.transform.localScale = new Vector3(1, 5, 1);
                    go.GetComponent<MeshRenderer>().material = whiteMaterial;
                    go.layer = LayerMask.NameToLayer("Wall");
                    go.transform.SetParent(transform);
                }
            }
        }
    }

    void PlaceRooms() {
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
                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);

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

        if (edges.Count > 0)
        {
            List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

            selectedEdges = new HashSet<Prim.Edge>(mst);
            var remainingEdges = new HashSet<Prim.Edge>(edges);
            remainingEdges.ExceptWith(selectedEdges);

            foreach (var edge in remainingEdges)
            {
                if (random.NextDouble() < 0.125)
                {
                    selectedEdges.Add(edge);
                }
            }
        }
    }

    void PathfindHallways() {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

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
                        if (grid[pos] == CellType.Hallway)
                        {
                            PlaceHallway(pos);
                        }
                    }
                }
            }
        }
    }

    void PlaceWall()
    {
        for (int i=0; i<grid.Size.x; i++) 
        {
            for(int j=0; j<grid.Size.y; j++)
            {
                if (grid[i, j] == CellType.None) 
                {
                    if(CheckWall(new Vector3(i, 0, j))) 
                    {
                        grid[i, j] = CellType.Wall;
                        GameObject go = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                        go.transform.position = new Vector3(i, 2, j);
                        go.transform.localScale = new Vector3(1, 5, 1);
                        go.GetComponent<MeshRenderer>().material = whiteMaterial;
                        go.layer = LayerMask.NameToLayer("Wall");
                        go.transform.SetParent(transform);
                    }
                }
            }
        }
    }

    private bool CheckWall(Vector3 position)
    {
        Vector3 newPosition;
        newPosition = new Vector3(position.x - 1, 0, position.z); //左
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        newPosition = new Vector3(position.x + 1, 0, position.z); //右
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        newPosition = new Vector3(position.x, 0, position.z - 1); //下
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        newPosition = new Vector3(position.x, 0, position.z + 1); //上
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        newPosition = new Vector3(position.x - 1, 0, position.z - 1); //左下
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        newPosition = new Vector3(position.x - 1, 0, position.z + 1); //左上
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        newPosition = new Vector3(position.x + 1, 0, position.z - 1); //右下
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        newPosition = new Vector3(position.x + 1, 0, position.z + 1); //右上
        if (InBound(newPosition) && grid[(int)newPosition.x, (int)newPosition.z] != CellType.None && grid[(int)newPosition.x, (int)newPosition.z] != CellType.Wall)
        {
            return true;
        }
        return false;
    }

    private bool InBound(Vector3 position)
    {
        if (position.x >= 0 && position.x < grid.Size.x && position.z >= 0 && position.z < grid.Size.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void PlaceCube(Vector2Int location, Vector2Int size, Material material) {
        for (int i=0; i<size.x; i++) 
        {
            for (int j=0; j<size.y; j++) 
            {
                GameObject go = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity);
                go.transform.position = new Vector3(location.x + i, 0, location.y + j);
                go.GetComponent<MeshRenderer>().material = material;
                go.transform.SetParent(transform);
            }
        }
    }

    void PlaceRoom(Vector2Int location, Vector2Int size) {
        PlaceCube(location, size, redMaterial);
    }

    void PlaceHallway(Vector2Int location) {
        PlaceCube(location, new Vector2Int(1, 1), blueMaterial);
    }
}
