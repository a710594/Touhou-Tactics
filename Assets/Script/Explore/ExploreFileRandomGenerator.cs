using System.Collections;
using System.Collections.Generic;
using Explore;
using UnityEngine;

public class ExploreFileRandomGenerator : MonoBehaviour
{
    public int Floor;

    private Generator2D _generator2D = new Generator2D();
    private NewExploreFile _file;
    private List<Vector2Int> _tilePositionList;

    public void BuildFile()
    {
        ExploreManager.Instance.CreateNewFile(Floor, new Vector2Int(80, 80), 80, new Vector2Int(5, 7), null); //temp, null нnзя
        ExploreManager.Instance.CreateObject();
    }


}
