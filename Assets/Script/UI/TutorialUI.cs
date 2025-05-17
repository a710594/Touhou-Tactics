using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Text TitleLabel;
    public Text CommentLabel;
    public Image Image;
    public Button PreviouButton;
    public Button NextButton;
    public Button CloseButton;

    private int _page;
    private Action _callback;
    private Dictionary<int, TutorialModel> _dataDic;

    public static void Open(int id, Action callback)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/TutorialUI"), Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        TutorialUI tutorialUI = obj.GetComponent<TutorialUI>();
        tutorialUI.Init(id, callback);
    }

    public void Init(int id, Action callback)
    {
        _page = 1;
        _dataDic = DataTable.Instance.TutorialDic[id];
        SetContent();
        PreviouButton.gameObject.SetActive(false);
        if (DataTable.Instance.TutorialDic[id].ContainsKey(_page + 1)) 
        {
            NextButton.gameObject.SetActive(true);
            CloseButton.gameObject.SetActive(false);
        }
        else
        {
            NextButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(true);
        }
        _callback = callback;
    }

    private void SetContent() 
    {
        TutorialModel data = _dataDic[_page];
        TitleLabel.text = data.Title;
        CommentLabel.text = data.Comment;
        if (data.Image != "x")
        {
            Image.gameObject.SetActive(true);
            Image.sprite = Resources.Load<Sprite>("Image/Tutorial/" + data.Image);
        }
        else
        {
            Image.gameObject.SetActive(false);
        }
    }

    private void PreviouOnClick() 
    {
        _page--;
        SetContent();
        NextButton.gameObject.SetActive(true);
        CloseButton.gameObject.SetActive(false);
        if (!_dataDic.ContainsKey(_page - 1)) 
        {
            PreviouButton.gameObject.SetActive(false);
        }
    }

    private void NextOnClick() 
    {
        _page++;
        SetContent();
        PreviouButton.gameObject.SetActive(true);
        if (!_dataDic.ContainsKey(_page + 1)) 
        {
            NextButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(true);
        }
    }

    private void CloseOnClick() 
    {
        if (_callback != null) 
        {
            _callback();
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        PreviouButton.onClick.AddListener(PreviouOnClick);
        NextButton.onClick.AddListener(NextOnClick);
        CloseButton.onClick.AddListener(CloseOnClick);
    }
}
