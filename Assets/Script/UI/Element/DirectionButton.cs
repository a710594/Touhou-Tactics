using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionButton : MonoBehaviour, IPointerClickHandler
{
    public Action<Vector2Int> ClickHandler;

    public Vector2Int Direction;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ClickHandler != null)
        {
            ClickHandler(Direction);
        }
    }
}
