using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Explore;

public class MapObject : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Quad;
    public GameObject Icon;

    private bool _isVisited = false;

    //private void CheckVidsit(Transform transform) 
    //{
    //    if (!_isVisited)
    //    {
    //        Vector2Int v2;
    //        for (int i = 0; i <= 1; i++)
    //        {
    //            v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i);
    //            CheckVidsited(v2);
    //            v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i + transform.right);
    //            CheckVidsited(v2);
    //            v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i - transform.right);
    //            CheckVidsited(v2);
    //        }
    //    }
    //}


    public void CheckVidsited(Vector2Int v2)
    {
        if (v2 == Utility.ConvertToVector2Int(transform.position))
        {
            _isVisited = true;
            Quad.layer = ExploreManager.Instance.MapLayer;
            if (Icon != null)
            {
                Icon.layer = ExploreManager.Instance.MapLayer;
            }
        }
    }

    private void Awake()
    {
        //ExploreManager.Instance.VisitedHandler += CheckVidsited;
    }
}
