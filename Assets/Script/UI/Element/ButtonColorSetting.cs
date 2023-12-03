using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorSetting : MonoBehaviour
{
    public Color CanUseColor;
    public Color NotUseColor;
    public Image Image;

    public void SetColor(bool canUse) 
    {
        if (canUse) 
        {
            Image.color = CanUseColor;
        }
        else
        {
            Image.color = NotUseColor;
        }
    }
}
