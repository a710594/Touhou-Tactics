using System.Collections;
using System.Collections.Generic;
using Explore;
using UnityEngine;

public class ExploreFileRandomGenerator : MonoBehaviour
{
    private Generator2D _generator2D = new Generator2D();
    private NewExploreFile _file;
    private List<Vector2Int> _tilePositionList;

    public void BuildFile()
    {
        ExploreManager.Instance.CreateNewFile(new Vector2Int(80, 80), 80, new Vector2Int(5, 7));
        ExploreManager.Instance.CreateObject();
    }


}
