using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Battle;

public class DragCharacterImage : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Action<CharacterInfo> DragBeginHandler;
    public Action<CharacterInfo> DragEndHandler;

    public Image Image;

    private bool _drag = false;
    private CharacterInfo _character;
    private Transform _anchor;

    public void SetData(CharacterInfo character) 
    {
        _character = character;
        Sprite sprite = Resources.Load<Sprite>("Image/" + character.Controller + "_F");
        Image.sprite = sprite;
        Image.transform.localPosition = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _drag = true;
        BattleController.Instance.HideCharacter(_character);
        Image.color = Color.white;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Image.rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _drag = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector2Int position = new Vector2Int();
        if (Physics.Raycast(ray, out hit, 100))
        {
            position = Utility.ConvertToVector2Int(hit.transform.position);
            GameObject obj = BattleController.Instance.PlaceCharacter(position, _character);
            if (obj != null)
            {
                _anchor = obj.transform;
                Image.color = Color.clear;

                if (DragEndHandler != null)
                {
                    DragEndHandler(_character);
                }
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }

    void Update() 
    {
        if (_anchor != null && !_drag)
        {
            this.transform.position = Camera.main.WorldToScreenPoint(_anchor.position);
        }
    }
}
