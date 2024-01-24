using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public enum StateEnum 
    {
        Normal,
        Equip,
        Use,
    }

    public Action<object> UseHandler;
    public Action CloseHandler;

    public Button ConsumablesButton;
    public Button FoodButton;
    public Button CardButton;
    public Button ItemButton;
    public Button EquipButton;
    public Button CloseButton;
    public Button UseButton;
    public ButtonColorSetting UseButtonColorSetting;
    public BagItemGroup BagItemGroup;
    public BagEquipGroup BagEquipGroup;
    public RectTransform RectTransform;
    public ButtonGroup ButtonGroup;
    public TipLabel TipLabel;

    private bool _canUse;
    private StateEnum _currentState;
    private object _selectedObj = null;
    private static BagUI _bagUI;

    public static BagUI Open() 
    {
        if (_bagUI == null)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/BagUI"), Vector3.zero, Quaternion.identity);
            GameObject canvas = GameObject.Find("Canvas");
            obj.transform.SetParent(canvas.transform);
            _bagUI = obj.GetComponent<BagUI>();
            _bagUI.RectTransform.offsetMax = Vector3.zero;
            _bagUI.RectTransform.offsetMin = Vector3.zero;
        }
        return _bagUI;
    }

    public void SetNormalState() 
    {
        _currentState = StateEnum.Normal;
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Consumables);
        ButtonGroup.SetSelect(ConsumablesButton.gameObject);
    }

    public void SetEquipState(EquipModel.CategoryEnum category, CharacterInfo character) 
    {
        _currentState = StateEnum.Equip;
        BagItemGroup.gameObject.SetActive(false);
        BagEquipGroup.gameObject.SetActive(true);
        BagEquipGroup.SetScrollView(category, character);
        BagEquipGroup.SetDetail(null);
        ButtonGroup.gameObject.SetActive(false);
    }

    public void SetUseState()
    {
        _currentState = StateEnum.Use;
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        CardButton.gameObject.SetActive(false);
        ItemButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Consumables);
        ButtonGroup.SetSelect(ConsumablesButton.gameObject);
    }


    private void ConsumablesOnClick() 
    {
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.ScrollView.CancelSelect();
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Consumables);
        BagItemGroup.SetComment("");
    }

    private void FoodOnClick()
    {
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.ScrollView.CancelSelect();
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Food);
        BagItemGroup.SetComment("");
    }

    private void CardOnClick()
    {
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.ScrollView.CancelSelect();
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Card);
        BagItemGroup.SetComment("");
    }

    private void ItemOnClick()
    {
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.ScrollView.CancelSelect();
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Item);
        BagItemGroup.SetComment("");
    }

    private void EquipOnClick()
    {
        BagItemGroup.gameObject.SetActive(false);
        BagEquipGroup.gameObject.SetActive(true);
        BagEquipGroup.SetScrollView();
        BagEquipGroup.SetDetail(null);
    }

    private void ScrollOnClick(object obj) 
    {
        if (_currentState == StateEnum.Equip)
        {
            UseButton.gameObject.SetActive(true);
            if (obj is BagScrollItem.Data)
            {
                BagScrollItem.Data data = (BagScrollItem.Data)obj;
                _selectedObj = data.Equip;
                _canUse = (data.Weight >= data.Equip.Weight);
                UseButtonColorSetting.SetColor(_canUse);
            }
        }
        else if(_currentState == StateEnum.Use) 
        {
            _selectedObj = obj;
            _canUse = true;
            UseButton.gameObject.SetActive(true);
        }
    }

    private void UseOnClick() 
    {
        if (_canUse)
        {
            if (_selectedObj is Equip)
            {
                ItemManager.Instance.MinusEquip((Equip)_selectedObj);
            }
            else if (_selectedObj is Food)
            {
                ItemManager.Instance.MinusFood((Food)_selectedObj);
            }
            else if (_selectedObj is Item)
            {
                Item item = (Item)_selectedObj;
                ItemManager.Instance.MinusItem(item.ID, 1);
            }
            else if (_selectedObj is Consumables)
            {
                Consumables consumables = (Consumables)_selectedObj;
                ItemManager.Instance.MinusItem(consumables.ID, 1);
            }
            else if (_selectedObj is Card)
            {
                Card card = (Card)_selectedObj;
                ItemManager.Instance.MinusItem(card.ID, 1);
            }

            if (UseHandler != null)
            {
                UseHandler(_selectedObj);
            }
            Destroy(gameObject);
        }
        else
        {
            TipLabel.SetLabel("該角色無法使用這麼重的裝備");
        }
    }

    public void Close() 
    {
        if (CloseHandler != null) 
        {
            CloseHandler();
        }

        TipLabel.Stop();
        Destroy(gameObject);
        _bagUI = null;
    }

    private void Update()
    {
    }

    private void Awake()
    {
        ConsumablesButton.onClick.AddListener(ConsumablesOnClick);
        FoodButton.onClick.AddListener(FoodOnClick);
        CardButton.onClick.AddListener(CardOnClick);
        ItemButton.onClick.AddListener(ItemOnClick);
        EquipButton.onClick.AddListener(EquipOnClick);
        BagItemGroup.ScrollHandler += ScrollOnClick;
        BagEquipGroup.ScrollHandler += ScrollOnClick;

        UseButton.onClick.AddListener(UseOnClick);
        CloseButton.onClick.AddListener(Close);
    }
}
