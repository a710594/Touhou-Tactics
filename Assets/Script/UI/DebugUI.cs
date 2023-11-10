using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public Dropdown Dropdown;
    public DebugItemUI DebugItemUI;
    public DebugMoneyUI DebugMoneyUI;

    private void DropdownOnValueChanged(int value)
    {
        if (value == 1)
        {
            DebugItemUI.gameObject.SetActive(true);
            DebugMoneyUI.gameObject.SetActive(false);
        }
        else if (value == 2) 
        {
            DebugItemUI.gameObject.SetActive(false);
            DebugMoneyUI.gameObject.SetActive(true);
        }
        else
        {
            DebugItemUI.gameObject.SetActive(false);
            DebugMoneyUI.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        DebugItemUI.gameObject.SetActive(false);
        Dropdown.onValueChanged.AddListener(DropdownOnValueChanged);
    }
}
