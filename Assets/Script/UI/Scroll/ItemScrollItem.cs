using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class ItemScrollItem : ScrollItem
{
    public Text Label;

    public override void SetData(object obj)
    {
        base.SetData(obj);
        Item item = (Item)obj;
        Label.text = item.Data.Name;
    }
}
