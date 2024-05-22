using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;

public class LogGroup : MonoBehaviour
{
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
        }
        _index = (_index + 1) % LogPanel.Length;
    }
}
