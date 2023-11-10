using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class ActionScrollItem : ScrollItem
{
    private Color _canUseColor = new Color32(255, 236, 191, 255);
    private Color _notUseColor = new Color32(220, 200, 160, 255);

    public Text Label;

    public override void SetData(object obj)
    {
        base.SetData(obj);

        if(obj is Skill) 
        {
            Skill skill = (Skill)obj;
            Label.text = skill.Data.Name;
            if (skill.CurrentCD == 0)
            {
                Background.color = _canUseColor;
            }
            else
            {
                Background.color = _notUseColor;
            }
        }
        else if(obj is Support) 
        {
            Support support = (Support)obj;
            Label.text = support.Data.Name;
            if (support.CurrentCD == 0)
            {
                Background.color = _canUseColor;
            }
            else
            {
                Background.color = _notUseColor;
            }
        }
        else if(obj is Consumables) 
        {
            Label.text = ((Consumables)obj).ItemData.Name;
            Background.color = _canUseColor;
        }
        else if (obj is Food)
        {
            Label.text = ((Food)obj).Name;
            Background.color = _canUseColor;
        }
        else if (obj is Card)
        {
            Card card = (Card)obj;
            Label.text = card.ItemData.Name;
            if (BattleController.Instance.SelectedCharacter.CurrentPP >= card.CardData.PP)
            {
                Background.color = _canUseColor;
            }
            else
            {
                Background.color = _notUseColor;
            }
        }
    }
}
