using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    public Text ConfirmLabel;
    public Text CancelLabel;
    public Text CommentLabel;
    public Button ConfirmButton;
    public Button CancelButton;

    private Action _onConfirmHandler;
    private Action _onCancelHandler;


    public static void Open(string commentText, string confirmText, Action confirmCallback)
    {
        GameObject obj  = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/ConfirmUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        ConfirmUI confirmUI = obj.GetComponent<ConfirmUI>();
        confirmUI.Init(commentText, confirmText, confirmCallback);
    }

    public static void Open(string commentText, string confirmText, string cancelText, Action confirmCallback, Action cancelCallback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/ConfirmUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        ConfirmUI confirmUI = obj.GetComponent<ConfirmUI>();
        confirmUI.Init(commentText, confirmText, cancelText, confirmCallback, cancelCallback);
    }

    private void Close()
    {
        Destroy(gameObject);
    }

    private void Init(string commentText, string confirmText, Action confirmCallback)
    {
        CommentLabel.text = commentText;
        ConfirmLabel.text = confirmText;
        CancelButton.gameObject.SetActive(false);
        _onConfirmHandler = confirmCallback;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Init(string commentText, string confirmText, string cancelText, Action confirmCallback, Action cancelCallback)
    {
        CommentLabel.text = commentText;
        ConfirmLabel.text = confirmText;
        CancelLabel.text = cancelText;
        _onConfirmHandler = confirmCallback;
        _onCancelHandler = cancelCallback;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ConfirmOnClick()
    {
        if (_onConfirmHandler != null)
        {
            _onConfirmHandler();
        }

        Close();
    }

    private void CancelOnClick()
    {
        if (_onCancelHandler != null)
        {
            _onCancelHandler();
        }

        Close();
    }

    void Awake()
    {
        ConfirmButton.onClick.AddListener(ConfirmOnClick);
        CancelButton.onClick.AddListener(CancelOnClick);
    }
}
