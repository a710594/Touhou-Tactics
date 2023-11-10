using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public Action CloseHandler;

    public Button ConsumablesButton;
    public Button FoodButton;
    public Button CardButton;
    public Button ItemButton;
    public Button EquipButton;
    public Button CloseButton;
    public BagItemGroup BagItemGroup;
    public BagEquipGroup BagEquipGroup;

    public void Open()
    {
        gameObject.SetActive(true);
        BagItemGroup.gameObject.SetActive(true);
        BagEquipGroup.gameObject.SetActive(false);
        BagItemGroup.SetScrollView(ItemModel.CategoryEnum.Consumables);
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

    private void CloseOnClick() 
    {
        if (CloseHandler != null) 
        {
            CloseHandler();
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            CloseOnClick();
        }
    }

    private void Awake()
    {
        ConsumablesButton.onClick.AddListener(ConsumablesOnClick);
        FoodButton.onClick.AddListener(FoodOnClick);
        CardButton.onClick.AddListener(CardOnClick);
        ItemButton.onClick.AddListener(ItemOnClick);
        EquipButton.onClick.AddListener(EquipOnClick);
        CloseButton.onClick.AddListener(CloseOnClick);
    }
}
