using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node(Vector2Int pos)
    {
        Position = pos;
    }

    public Vector2Int Position;
    public float G;
    public float H;
    public float F;
    public Node parent;
}
