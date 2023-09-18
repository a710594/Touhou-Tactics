using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Battle;

public class DragCharacter : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Action<CharacterInfo> DragEndHandler;

    public Image BG;
    public Image Drag;

    private CharacterInfo _character;

    public void SetData(CharacterInfo character) 
    {
        _character = character;
        Sprite sprite = Resources.Load<Sprite>("Image/" + character.Controller + "_F");
        BG.sprite = sprite;
        Drag.sprite = sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag.rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector2Int position = new Vector2Int();
        if (Physics.Raycast(ray, out hit, 100))
        {
            position = Utility.ConvertToVector2Int(hit.transform.position);
            BattleController.Instance.PlaceCharacter(position, _character);

            if(DragEndHandler != null) 
            {
                DragEndHandler(_character);
            }
        }
        Drag.transform.position = BG.transform.position;
    }
}
