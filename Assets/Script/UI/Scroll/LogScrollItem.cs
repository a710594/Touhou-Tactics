using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogScrollItem : ScrollItem
{
    public Text Label;

    public override void SetData(object obj)
    {
        base.SetData(obj);

        Label.text = (string)obj;
    }
}
