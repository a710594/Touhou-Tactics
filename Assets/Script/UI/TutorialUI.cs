using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Text CommentLabel;
    public Text BigCommentLabel;
    public Image Image;
    public Button CloseButton;

    private Action _confirmCallback;

    public static void Open(string commentText, string image, Action confirmCallback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/TutorialUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        TutorialUI tutorialUI = obj.GetComponent<TutorialUI>();
        tutorialUI.CommentLabel.gameObject.SetActive(true);
        tutorialUI.CommentLabel.text = commentText;
        tutorialUI.BigCommentLabel.gameObject.SetActive(false);
        tutorialUI.Image.gameObject.SetActive(true);
        tutorialUI.Image.sprite = Resources.Load<Sprite>("Image/" + image);
        tutorialUI._confirmCallback = confirmCallback;
    }

    public static void Open(string commentText, Action confirmCallback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/TutorialUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        TutorialUI tutorialUI = obj.GetComponent<TutorialUI>();
        tutorialUI.CommentLabel.gameObject.SetActive(false);
        tutorialUI.BigCommentLabel.gameObject.SetActive(true);
        tutorialUI.BigCommentLabel.text = commentText;
        tutorialUI.Image.gameObject.SetActive(false);
        tutorialUI._confirmCallback = confirmCallback;
    }

    private void CloseOnClick() 
    {
        if (_confirmCallback != null) 
        {
            _confirmCallback();
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        CloseButton.onClick.AddListener(CloseOnClick);
    }
}
