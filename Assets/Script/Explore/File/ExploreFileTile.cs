using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFileTile
{
    public Vector2Int Position;
    public string Prefab;
    public string Tag;

    public ExploreFileTile(Vector2Int position, string prefab, string tag)
    {
        Position = position;
        Prefab = prefab;
        Tag = tag;
    }
}
