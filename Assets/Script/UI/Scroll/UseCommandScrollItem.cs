using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class UseCommandScrollItem : ScrollItem
{
    private Color _canUseColor = new Color32(255, 236, 191, 255);
    private Color _notUseColor = new Color32(200, 180, 140, 255);


    public override void SetData(object obj)
    {
        base.SetData(obj);

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter.Info;
        Command command = (Command)obj;
        if(obj is Skill) 
        {
            Skill skill = (Skill)obj;
            Label.text = command.Name;
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
            Label.text = support.Name;
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
            Label.text = ((Consumables)obj).Name;
            Background.color = _canUseColor;
        }
        else if (obj is Food)
        {
            Label.text = ((Food)obj).Name;
            Background.color = _canUseColor;
        }
        else if (obj is Spell)
        {
            Spell spell = (Spell)obj;
            Label.text = spell.Name;
            if (character.CanUseSpell)
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
