using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialArrowUI : MonoBehaviour
{
    public Text CommentLabel;
    public GameObject Arrow;

    private Vector3 _worldPosition;
    private Vector3 _offset;
    private Transform _anchor;
    private static TutorialArrowUI _tutorialArrowUI;

    public static void Open(string commentText, Transform anchor, Vector3 offset, Vector2Int direction, Action confirmCallback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/TutorialArrowUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        _tutorialArrowUI = obj.GetComponent<TutorialArrowUI>();
        _tutorialArrowUI.CommentLabel.text = commentText;
        _tutorialArrowUI._anchor = anchor;
        _tutorialArrowUI._offset = offset;

        if(direction == Vector2Int.right) 
        {
            _tutorialArrowUI.Arrow.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (direction == Vector2Int.left)
        {
            _tutorialArrowUI.Arrow.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if (direction == Vector2Int.up)
        {
            _tutorialArrowUI.Arrow.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    public static void Open(string commentText, Vector3 position, Vector2Int direction, Action confirmCallback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/TutorialArrowUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        _tutorialArrowUI = obj.GetComponent<TutorialArrowUI>();
        _tutorialArrowUI.CommentLabel.text = commentText;
        _tutorialArrowUI._anchor = null;
        _tutorialArrowUI._worldPosition = position;

        if (direction == Vector2Int.right)
        {
            _tutorialArrowUI.Arrow.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (direction == Vector2Int.left)
        {
            _tutorialArrowUI.Arrow.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if (direction == Vector2Int.up)
        {
            _tutorialArrowUI.Arrow.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }

    public static void Close()
    {
        Destroy(_tutorialArrowUI.gameObject);
    }

    private void Update()
    {
        if (_anchor != null)
        {
            transform.position = _anchor.position + _offset;
        }
        else 
        {
            transform.position = Camera.main.WorldToScreenPoint(_worldPosition);
        }
    }
}
