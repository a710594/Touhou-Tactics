using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialArrowUI : MonoBehaviour
{
    public Text CommentLabel;
    public RectTransform Arrow;
    public RectTransform BG;

    private Vector3 _worldPosition;
    private Vector3 _offset;
    private Transform _anchor;
    private static TutorialArrowUI _tutorialArrowUI;

    public static TutorialArrowUI Open(string commentText, Transform anchor, Vector3 offset, Vector2Int direction)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/TutorialArrowUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        _tutorialArrowUI = obj.GetComponent<TutorialArrowUI>();
        _tutorialArrowUI.Init(commentText, anchor, offset, direction);

        return _tutorialArrowUI;
    }

    public static void Open(string commentText, Vector3 position, Vector2Int direction)
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
        if (_tutorialArrowUI != null)
        {
            Destroy(_tutorialArrowUI.gameObject);
        }
    }

    public void SetAnchor(Transform anchor, Vector3 offset) 
    {
        _anchor = anchor;
        _offset = offset;
    }

    private void Init(string commentText, Transform anchor, Vector3 offset, Vector2Int direction)
    {
        if (commentText == "")
        {
            CommentLabel.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            CommentLabel.transform.parent.gameObject.SetActive(true);
            CommentLabel.text = commentText;
        }
        _anchor = anchor;
        _offset = offset;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_tutorialArrowUI.BG.transform);
        StartCoroutine(SetPosition(direction));
    }

    private void Init(string commentText, Vector3 position, Vector2Int direction)
    {
        CommentLabel.text = commentText;
        _anchor = null;
        _worldPosition = position;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_tutorialArrowUI.BG.transform);
        StartCoroutine(SetPosition(direction));
    }

    private IEnumerator SetPosition(Vector2Int direction)
    {
        yield return new WaitForEndOfFrame();
        if (direction == Vector2Int.right)
        {
            Arrow.transform.localEulerAngles = new Vector3(0, 0, 90);
            BG.transform.localPosition = new Vector3(-((BG.sizeDelta.x + Arrow.sizeDelta.x) / 2f), 0, 0);
        }
        else if (direction == Vector2Int.left)
        {
            Arrow.transform.localEulerAngles = new Vector3(0, 0, -90);
            BG.transform.localPosition = new Vector3((BG.sizeDelta.x + Arrow.sizeDelta.x) / 2f, 0, 0);
        }
        else if (direction == Vector2Int.up)
        {
            _tutorialArrowUI.Arrow.transform.localEulerAngles = new Vector3(0, 0, 180);
            BG.transform.localPosition = new Vector3(0, (BG.sizeDelta.y + Arrow.sizeDelta.y) / 2f, 0);
        }
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
