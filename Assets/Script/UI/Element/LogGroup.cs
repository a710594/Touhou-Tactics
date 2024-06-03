using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.UI;

public class LogGroup : MonoBehaviour
{
    public float FadeTime;
    public Button Button;
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

    private void ButtonOnClick()
    {
        /*if(!_isInit)
        {
            ScrollView.Init();
            _isInit = true;
        }*/

        ScrollView.gameObject.SetActive(!ScrollView.gameObject.activeSelf);
        if(ScrollView.gameObject.activeSelf)
        {
            ScrollView.SetData(new List<object> (BattleController.Instance.LogList));
        }
    }

    void Awake()
    {
        ScrollView.gameObject.SetActive(false);
        Button.onClick.AddListener(ButtonOnClick);
    }
}
