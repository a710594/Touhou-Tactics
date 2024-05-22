using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    public Text Label;

    public void SetLabel(string text)
    {
        Label.text = text;
    }
}
