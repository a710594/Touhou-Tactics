using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionGroup : MonoBehaviour
{
    public Action<Vector2Int> ClickHandler;

    public DirectionButton[] DirectionButtons;

    private void DirectionOnClick(Vector2Int direction)
    {
        if(ClickHandler!=null)
        {
            ClickHandler(direction);
        }
    }

    void Awake()
    {
        for(int i=0; i<DirectionButtons.Length; i++)
        {
            DirectionButtons[i].ClickHandler += DirectionOnClick;
        }
    }
}
