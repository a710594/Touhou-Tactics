using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.UI;

public class LogGroup : MonoBehaviour
{
    public float FadeTime;
    public Button OpenButton;
    public Button CloseButton;
    public ScrollView ScrollView;
    public LogPanel[] LogPanel;

    private bool _isInit = false;
    private int _index = 0;

    public void AddLog(string text)
    {
        LogPanel[_index].SetLabel(text);
        if(LogPanel[_index].gameObject.activeSelf)
        {
            LogPanel[_index].transform.SetAsLastSibling();
        }
        else
        {
            LogPanel[_index].gameObject.SetActive(true);
            LogPanel[_index].Fade(FadeTime, (logPanel)=>
            {
                logPanel.transform.SetAsLastSibling();
            });
        }
        _index = (_index + 1) % LogPanel.Length;
    }

    private void OpenButtonOnClick()
    {
        ScrollView.gameObject.SetActive(true);
        OpenButton.gameObject.SetActive(false);
        ScrollView.SetData(new List<object>(BattleController.Instance.LogList));
    }

    private void CloseButtonOnClick()
    {
        ScrollView.gameObject.SetActive(false);
        OpenButton.gameObject.SetActive(true);
    }

    void Awake()
    {
        ScrollView.gameObject.SetActive(false);
        OpenButton.onClick.AddListener(OpenButtonOnClick);
        CloseButton.onClick.AddListener(CloseButtonOnClick);
    }
}
