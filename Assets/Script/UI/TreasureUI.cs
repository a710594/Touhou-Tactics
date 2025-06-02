using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureUI : MonoBehaviour
{
    public Text TitleLabel_1;
    public Text TitleLabel_2;
    public Text NameLabel;

    private Action _callback;

    public void Open(ItemModel data, Action callback) 
    {
        gameObject.SetActive(true);
        TitleLabel_1.gameObject.SetActive(true);
        TitleLabel_2.gameObject.SetActive(false);
        NameLabel.text = data.Name;
        _callback = callback;
    }

    private void Close() 
    {
        gameObject.SetActive(false);

        if (_callback != null)
        {
            _callback();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Close();
        }
    }
}
