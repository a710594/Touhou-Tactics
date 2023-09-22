using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class DragCharacterSprite : MonoBehaviour
{
    public Action<CharacterInfo> MouseDownHandler;

    private CharacterInfo _character;

    private void SetData(CharacterInfo characterInfo) 
    {
        _character = characterInfo;
    }

    void OnMouseDown()
    {
        //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        gameObject.SetActive(false);
        if (MouseDownHandler != null)
        {
            MouseDownHandler(_character);
        }
    }

    //void OnMouseDrag()
    //{
    //    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
    //    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    //    transform.position = curPosition;

    //}

    //void OnMouseUp()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    Vector2Int v2 = new Vector2Int();
    //    Vector3 v3 = new Vector3();
    //    int layerMask = LayerMask.GetMask("Ignore Raycast");
    //    if (Physics.Raycast(ray, out hit, 100, ~layerMask))
    //    {
    //        v3 = hit.transform.position;
    //        v2 = Utility.ConvertToVector2Int(v3);
    //        transform.position = new Vector3(v2.x, BattleController.Instance.Info.TileInfoDic[v2].Height, v2.y);
    //    }
    //}
}
