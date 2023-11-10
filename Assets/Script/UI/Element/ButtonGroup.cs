using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    public ButtonSingle[] Button;

    public void SetSelect(GameObject button) 
    {
        for (int i = 0; i < Button.Length; i++)
        {
            if (Button[i].gameObject.Equals(button))
            {
                Button[i].SetSelected(true);
            }
            else
            {
                Button[i].SetSelected(false);
            }
        }
    }

    private void ButtonOnClick(GameObject button)
    {
        SetSelect(button);
    }

    private void Awake()
    {
        for (int i=0; i<Button.Length; i++)
        {
            Button[i].ClickHandler = ButtonOnClick;
        }
    }
}
