using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugItemUI : MonoBehaviour
{
    public InputField IDField;
    public InputField AmountField;
    public Button ConfirmButton;

    private void ConfirmOnClick() 
    {
        try
        {
            int id = int.Parse(IDField.text);
            ItemModel data = DataContext.Instance.ItemDic[id];
            if (data.Category == ItemModel.CategoryEnum.Equip)
            {
                ItemManager.Instance.AddEquip(id);
            }
            else if(data.Category == ItemModel.CategoryEnum.Food) 
            {
                ItemManager.Instance.AddFood(new Food(id));
            }
            else
            {
                ItemManager.Instance.AddItem(id, int.Parse(AmountField.text));
            }
            Debug.Log(DataContext.Instance.ItemDic[id].Name + " " + AmountField.text);
        }
        catch(Exception ex) 
        {
            Debug.Log(ex.Message);
        }
    }

    private void Awake()
    {
        ConfirmButton.onClick.AddListener(ConfirmOnClick);
    }
}
