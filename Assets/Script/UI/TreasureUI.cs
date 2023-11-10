using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureUI : MonoBehaviour
{
    public Action CloseHandler;

    public Text TitleLabel_1;
    public Text TitleLabel_2;
    public Text NameLabel;

    public void Open(int id) 
    {
        gameObject.SetActive(true);
        TitleLabel_1.gameObject.SetActive(true);
        TitleLabel_2.gameObject.SetActive(false);
        ItemModel data = DataContext.Instance.ItemDic[id];
        NameLabel.text = data.Name;
    }

    private void Close() 
    {
        if (CloseHandler != null)
        {
            CloseHandler();
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Close();
        }
    }
}
