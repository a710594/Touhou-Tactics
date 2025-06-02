using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScrollItem : ScrollItem
{
    public Action<CharacterScrollItem> DetailHandler;
    public Action<CharacterScrollItem> UseItemHandler;

    public Text NameLabel;
    public ValueBar HpBar;
    public Button DetailButton;
    public Button UseItemButton;

    private CharacterInfo _characterInfo;

    public override void SetData(object obj)
    {
        base.SetData(obj);
        _characterInfo = (CharacterInfo)obj;
        Image.sprite = Resources.Load<Sprite>("Image/" + _characterInfo.Controller + "_F");
        NameLabel.text = _characterInfo.Name;
        HpBar.SetValue(_characterInfo.CurrentHP, _characterInfo.MaxHP);
    }

    private void DetailOnClick() 
    {
        if (DetailHandler != null) 
        {
            DetailHandler(this);
        }
    }

    private void UseItemOnClick() 
    {
        if (UseItemHandler != null) 
        {
            UseItemHandler(this);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        DetailButton.onClick.AddListener(DetailOnClick);
        UseItemButton.onClick.AddListener(UseItemOnClick);
    }
}
