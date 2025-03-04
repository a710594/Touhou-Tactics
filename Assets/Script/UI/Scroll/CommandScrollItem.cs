using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class CommandScrollItem : ScrollItem
{
    public override void SetData(object obj)
    {
        base.SetData(obj);

        Command command = (Command)obj;
        if(obj is Skill) 
        {
            Skill skill = (Skill)obj;
            Label.text = command.Name;
        }
        else if(obj is Support) 
        {
            Support support = (Support)obj;
            Label.text = support.Name;
        }
        else if(obj is Consumables) 
        {
            Label.text = ((Consumables)obj).Name;
        }
        else if (obj is Food)
        {
            Label.text = ((Food)obj).Name;
        }
        else if (obj is Spell)
        {
            Spell spell = (Spell)obj;
            Label.text = spell.Name;
        }
    }
}
