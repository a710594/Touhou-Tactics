using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLvUI : MonoBehaviour
{
    public InputField LvField;
    public Button ConfirmButton;

    private void ConfirmOnClick()
    {
        try
        {
            int lv = int.Parse(LvField.text);
            CharacterManager.Instance.SetLv(lv);
            Debug.Log("Lv " + lv);
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
