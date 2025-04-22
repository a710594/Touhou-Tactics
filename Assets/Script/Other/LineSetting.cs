using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSetting : MonoBehaviour
{
    public LineRenderer Line;
    public Material Red;
    public Material Blue;

    public void Show(Vector3 p1, Vector3 p2, Color color) 
    {
        gameObject.SetActive(true);
        Line.positionCount = 2;
        Line.SetPosition(0, p1);
        Line.SetPosition(1, p2);
        if(color == Color.red) 
        {
            Line.material = Red;
        }
        else if(color == Color.blue) 
        {
            Line.material = Blue;
        }
    }

    public void Show(List<Vector3> list, Color color)
    {
        gameObject.SetActive(true);
        Line.positionCount = list.Count;
        for (int i=0; i<list.Count; i++) 
        {
            Line.SetPosition(i, list[i]);
        }
        if (color == Color.red)
        {
            Line.material = Red;
        }
        else if (color == Color.blue)
        {
            Line.material = Blue;
        }
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }
}
