using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMoneyUI : MonoBehaviour
{
    public InputField MoneyField;
    public Button ConfirmButton;

    private void ConfirmOnClick()
    {
        try
        {
            int money = int.Parse(MoneyField.text);
            ItemManager.Instance.Info.Money += money;
            Debug.Log("Add Money " + money);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void Awake()
    {
        ConfirmButton.onClick.AddListener(ConfirmOnClick);
    }
}
