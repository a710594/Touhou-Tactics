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
    public Action<int, object> SetEquipHandler;
    public Action CloseHandler;

    public Text MoneyLabel;
    public Text KeyLabel;
    public Button ConsumablesButton;
    public Button FoodButton;
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

    private int _equipIndex;
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
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Food);
        ButtonGroup.SetSelect(FoodButton.gameObject);
        MoneyLabel.text = "Money: " + ItemManager.Instance.Info.Money;
        KeyLabel.text = "Key: " + ItemManager.Instance.Info.Key;
    }

    public void SetEquipState(EquipModel.CategoryEnum category, CharacterInfo character, int index) 
    {
        _currentState = StateEnum.Equip;
        _equipIndex = index;
        BagItemGroup.gameObject.SetActive(false);
        BagEquipGroup.gameObject.SetActive(true);
        BagEquipGroup.SetScrollView(category, character);
        BagEquipGroup.SetDetail(null);
        ButtonGroup.gameObject.SetActive(false);
        MoneyLabel.text = "Money: " + ItemManager.Instance.Info.Money;
        KeyLabel.text = "Key: " + ItemManager.Instance.Info.Key;
    }

    public void SetUseState()
    {
        _currentState = StateEnum.Use;
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        ItemButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Food);
        ButtonGroup.SetSelect(FoodButton.gameObject);
        MoneyLabel.text = "Money: " + ItemManager.Instance.Info.Money;
        KeyLabel.text = "Key: " + ItemManager.Instance.Info.Key;
    }


    private void ConsumablesOnClick() 
    {
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Consumables);
        BagItemGroup.SetName("");
        BagItemGroup.SetComment("");
    }

    private void FoodOnClick()
    {
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Food);
        BagItemGroup.SetName("");
        BagItemGroup.SetComment("");
    }

    private void ItemOnClick()
    {
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Item);
        BagItemGroup.SetName("");
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
        /*if (_currentState == StateEnum.Equip)
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
        }*/

        if(_currentState == StateEnum.Equip || _currentState== StateEnum.Use) 
        {
            _selectedObj = obj;
            UseButton.gameObject.SetActive(true);
        }
    }

    private void UseOnClick() 
    {
        if (_selectedObj is Equip)
        {
            ItemManager.Instance.MinusEquip((Equip)_selectedObj);
            if (SetEquipHandler != null)
            {
                SetEquipHandler(_equipIndex, _selectedObj);
            }
        }
        else if (_selectedObj is Food)
        {
            ItemManager.Instance.MinusFood((Food)_selectedObj);
            if (UseHandler != null)
            {
                UseHandler(_selectedObj);
            }
        }
        else if (_selectedObj is Consumables)
        {
            Consumables consumables = (Consumables)_selectedObj;
            ItemManager.Instance.MinusItem(consumables.ID, 1);
            if (UseHandler != null)
            {
                UseHandler(_selectedObj);
            }
        }

        Destroy(gameObject);
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
        ItemButton.onClick.AddListener(ItemOnClick);
        EquipButton.onClick.AddListener(EquipOnClick);
        BagItemGroup.ScrollHandler += ScrollOnClick;
        BagEquipGroup.ScrollHandler += ScrollOnClick;

        UseButton.onClick.AddListener(UseOnClick);
        CloseButton.onClick.AddListener(Close);
    }
}
