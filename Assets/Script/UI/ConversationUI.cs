using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConversationUI : MonoBehaviour
{
    public Action<int> Handler;

    public Text NameLabel;
    public Typewriter NormalTypewriter;
    public Button NextButton;
    public Button SkipButton;
    public Image Background;
    public Image FadeImage;
    public Image FlashImage;
    public Image[] CharacterImage;
    public RectTransform RectTransform;

    private int _siblingIndex;
    private bool _isPlayingBGM = false;
    private bool _isClickable = true;
    //private bool _isFadeEnd = true; //結束時淡出
    private ConversationModel _data;
    private Timer _timer = new Timer();
    private Action _pauseCallback;
    private Action _finishCallback;
    private List<string> _conversationList = new List<string>();

    private static ConversationUI _conversationUI;

    public static ConversationUI Open(int id, bool canSkip, Action finishCallback, Action pauseCallback)
    {
        if (_conversationUI == null)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/ConversationUI"), Vector3.zero, Quaternion.identity);
            GameObject canvas = GameObject.Find("Canvas");
            obj.transform.SetParent(canvas.transform);
            _conversationUI = obj.GetComponent<ConversationUI>();
            _conversationUI.RectTransform.offsetMax = Vector3.zero;
            _conversationUI.RectTransform.offsetMin = Vector3.zero;
            _conversationUI.Init(id, canSkip, finishCallback, pauseCallback);
        }
        return _conversationUI;
    }

    public static void Close()
    {
        if (_conversationUI != null)
        {
            Destroy(_conversationUI.gameObject);
        }
    }

    public void Continue(Action pauseCallback = null) 
    {
        gameObject.SetActive(true);
        _pauseCallback = pauseCallback;
        NextConversationID(_data.ID, _data.Page + 1);
    }

    private void Init(int id, bool canSkip, Action finishCallback, Action pauseCallback)
    {
        ConversationModel data = DataContext.Instance.ConversationDic[id][1];
        NormalTypewriter.ClearText();
        SkipButton.gameObject.SetActive(canSkip);
        SetData(data);

        _data = data;
        _pauseCallback = pauseCallback;
        _finishCallback = finishCallback;
    }

    private void NextConversationID(int id, int page)
    {
        DataContext.Instance.ConversationDic[id].TryGetValue(page, out ConversationModel data);
        if (data == null)
        {
            Finish();
        }
        else
        {
            _data = data;
            SetData(data);
        }
    }

    private void SetData(ConversationModel data)
    {
        NameLabel.text = data.Name;
        NormalTypewriter.Show(data.Dialog);

        for (int i=0; i<data.ImageList.Count; i++)
        {
            if (data.ImageList[i] == "x")
            {
                CharacterImage[i].gameObject.SetActive(false);
            }
            else if (data.ImageList[i] == "-")
            {
                CharacterImage[i].color = Color.gray;
                CharacterImage[i].transform.SetSiblingIndex(_siblingIndex);
            }
            else
            {
                CharacterImage[i].gameObject.SetActive(true);
                CharacterImage[i].color = Color.white;
                CharacterImage[i].transform.SetSiblingIndex(_siblingIndex + 1);
                if (data.ImageList[i] != "o")
                {
                    if (i == 2) 
                    {
                        CharacterImage[i].sprite = Resources.Load<Sprite>("Image/" + data.ImageList[i]);
                        //CharacterImage[i].SetNativeSize();
                        //CharacterImage[i].transform.position = Vector3.zero;
                    }
                    else 
                    {
                        CharacterImage[i].sprite = Resources.Load<Sprite>("Image/Character/" + data.ImageList[i]);
                    }
                    CharacterImage[i].SetNativeSize();
                }
            }
        }

        for (int i = 0; i < data.MotionList.Count; i++)
        {
            if (data.MotionList[i] == ConversationModel.MotionEnum.Jump)
            {
                CharacterImage[i].transform.DOJump(CharacterImage[i].transform.position, 50, 1, 0.5f);
            }
            else if(data.MotionList[i] == ConversationModel.MotionEnum.Shake)
            {
                CharacterImage[i].transform.DOShakePosition(0.5f, 20);
            }
        }

        //SetSpeical(data.Special);

        if (Handler != null)
        {
            Handler(data.ID);
        }
    }
    private void SetSpeical(string special)
    {
        if (special == "RedFlash")
        {
            FlashImage.color = new Color(1, 0, 0, 0);
            FlashImage.DOFade(1, 0.2f).SetLoops(2, LoopType.Yoyo);
        }
    }

    private void Finish()
    {
        NameLabel.text = string.Empty;
        NormalTypewriter.ClearText();

        _isClickable = false;

        if (_finishCallback != null)
        {
            _finishCallback();
        }
        else if (_pauseCallback != null) 
        {
            _pauseCallback();
        }

        Close();
    }

    private void NextOnClick()
    {
        if (!_isClickable)
        {
            return;
        }

        if (!NormalTypewriter.IsTyping)
        {
            if (_data.Pause)
            {
                NormalTypewriter.SetText();
                gameObject.SetActive(false);
                if (_pauseCallback != null)
                {
                    _pauseCallback();
                }
                return;
            }
            else
            {
                NextConversationID(_data.ID, _data.Page + 1);
            }
        }
        else
        {
            NormalTypewriter.SetText();
        }
    }

    private void SkipOnClick()
    {
        Finish();
    }

    private void Awake()
    {
        _siblingIndex = CharacterImage[0].transform.GetSiblingIndex();

        for (int i=0; i<CharacterImage.Length; i++)
        {
            CharacterImage[i].gameObject.SetActive(false);
        }

        NextButton.onClick.AddListener(NextOnClick);
        SkipButton.onClick.AddListener(SkipOnClick);
    }
}