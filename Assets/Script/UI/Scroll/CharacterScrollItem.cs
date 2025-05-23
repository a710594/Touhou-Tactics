using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScrollItem : ScrollItem
{
    public Action<string> UseItemHandler;

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
        CharacterDetailUI characterDetailUI = CharacterDetailUI.Open(true);
        characterDetailUI.SetData((CharacterInfo)Data);
    }

    private void UseItemOnClick() 
    {
        BagUI bagUI = BagUI.Open();
        bagUI.SetUseState();
        bagUI.UseHandler += UseItem;
    }

    private void UseItem(object obj) 
    {
        int add = 0;
        if(obj is Battle.ItemCommand) 
        {
            Battle.ItemCommand item = (Battle.ItemCommand)obj;
            add = item.Effect.Value;
        }
        else if(obj is Food) 
        {
            Food food = (Food)obj;
            add = food.HP;
        }
        _characterInfo.SetRecover(add);
        HpBar.SetValueTween(_characterInfo.CurrentHP, _characterInfo.MaxHP, null);
        CharacterUI.SetTip(_characterInfo.Name + " �^�_�F " + add + " HP");
    }

    protected override void Awake()
    {
        base.Awake();

        DetailButton.onClick.AddListener(DetailOnClick);
        UseItemButton.onClick.AddListener(UseItemOnClick);
    }
}
