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

        if(obj is Skill) 
        {
            Label.text = ((Skill)obj).Data.Name;
        }
        else if(obj is Support) 
        {
            Label.text = ((Support)obj).Data.Name;
        }
        else if(obj is Item) 
        {
            Label.text = ((Item)obj).Data.Name;
        }
    }
}
