using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using DG.Tweening;

public class LogGroup : MonoBehaviour
{
    public float FadeTime;
    public LogPanel[] LogPanel;

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
}
