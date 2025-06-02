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
    public Action<object> ScrollItemHandler;

    public Text MoneyLabel;
    public Text KeyLabel;
    public Button ConsumablesButton;
    public Button FoodButton;
    public Button ItemButton;
    public Button EquipButton;
    public Button CloseButton;
    public Button UseButton;
    public ButtonColorSetting UseButtonColorSetting;
    public BagItemGroup ItemGroup;
    public BagEquipGroup EquipGroup;
    public RectTransform RectTransform;
    public ButtonGroup ButtonGroup;
    public TipLabel TipLabel;

    private static BagUI _bagUI;

    private int _equipIndex;
    private object _selectedObj = null;
    private StateEnum _currentState;
    private Action _callback;

    public static BagUI Open(Action callback) 
    {
        if (_bagUI == null)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/BagUI"), Vector3.zero, Quaternion.identity);
            GameObject canvas = GameObject.Find("Canvas");
            obj.transform.SetParent(canvas.transform);
            _bagUI = obj.GetComponent<BagUI>();
            _bagUI.RectTransform.offsetMax = Vector3.zero;
            _bagUI.RectTransform.offsetMin = Vector3.zero;
            _bagUI._callback = callback;
        }
        return _bagUI;
    }

    public void SetNormalState() 
    {
        _currentState = StateEnum.Normal;
        ItemGroup.gameObject.SetActive(true);
        EquipGroup.gameObject.SetActive(false);
        ItemGroup.SetScrollView(ItemModel.CategoryEnum.Material);
        ButtonGroup.SetSelect(ItemButton.gameObject);
        MoneyLabel.text = "Money: " + ItemManager.Instance.Info.Money;
        KeyLabel.text = "Key: " + ItemManager.Instance.Info.Key;
    }

    public void SetEquipState(EquipModel.CategoryEnum category, CharacterInfo character, int index) 
    {
        _currentState = StateEnum.Equip;
        _equipIndex = index;
        ItemGroup.gameObject.SetActive(false);
        EquipGroup.gameObject.SetActive(true);
        EquipGroup.SetScrollView(category, character);
        EquipGroup.SetDetail(null);
        ButtonGroup.gameObject.SetActive(false);
        MoneyLabel.text = "Money: " + ItemManager.Instance.Info.Money;
        KeyLabel.text = "Key: " + ItemManager.Instance.Info.Key;
    }

    public void SetUseState()
    {
        _currentState = StateEnum.Use;
        ItemGroup.gameObject.SetActive(true);
        EquipGroup.gameObject.SetActive(false);
        ItemButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        ItemGroup.SetScrollView(ItemModel.CategoryEnum.Food);
        ButtonGroup.SetSelect(FoodButton.gameObject);
        MoneyLabel.text = "Money: " + ItemManager.Instance.Info.Money;
        KeyLabel.text = "Key: " + ItemManager.Instance.Info.Key;
    }

    public void SetSelectObj(object obj) 
    {
        _selectedObj = obj;
        UseButton.gameObject.SetActive(true);
    }

    private void ConsumablesOnClick() 
    {
        ItemGroup.gameObject.SetActive(true);
        EquipGroup.gameObject.SetActive(false);
        ItemGroup.SetScrollView(ItemModel.CategoryEnum.Consumables);
        ItemGroup.SetName("");
        ItemGroup.SetComment("");
    }

    private void FoodOnClick()
    {
        ItemGroup.gameObject.SetActive(true);
        EquipGroup.gameObject.SetActive(false);
        ItemGroup.SetScrollView(ItemModel.CategoryEnum.Food);
        ItemGroup.SetName("");
        ItemGroup.SetComment("");
    }

    private void ItemOnClick()
    {
        ItemGroup.gameObject.SetActive(true);
        EquipGroup.gameObject.SetActive(false);
        ItemGroup.SetScrollView(ItemModel.CategoryEnum.Material);
        ItemGroup.SetName("");
        ItemGroup.SetComment("");
    }

    private void EquipOnClick()
    {
        ItemGroup.gameObject.SetActive(false);
        EquipGroup.gameObject.SetActive(true);
        EquipGroup.SetScrollView();
        EquipGroup.SetDetail(null);
    }

    private void ScrollOnClick(object obj) 
    {
        if(ScrollItemHandler!=null)
        {
            ScrollItemHandler(obj);
        }
        else if(_currentState == StateEnum.Equip || _currentState== StateEnum.Use) 
        {
            _selectedObj = obj;
            UseButton.gameObject.SetActive(true);

            Equip equip = null;
            if (obj is Equip)
            {
                equip = (Equip)obj;
                EquipGroup.SetDetail(equip);
            }
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
        else if (_selectedObj is Item)
        {
            Item consumables = (Item)_selectedObj;
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
        _bagUI = null;
        TipLabel.Stop();
        Destroy(gameObject);

        if (_callback != null)
        {
            _callback();
        }
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
        ItemGroup.ScrollHandler += ScrollOnClick;
        EquipGroup.ScrollHandler += ScrollOnClick;

        UseButton.onClick.AddListener(UseOnClick);
        CloseButton.onClick.AddListener(Close);
    }
}
