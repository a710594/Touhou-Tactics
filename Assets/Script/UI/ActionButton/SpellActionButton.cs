using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellActionButton : ActionButton
{
    public override void SetColor(BattleCharacterInfo character)
    {
        if (character.HasMain)
        {
            _canUse = false;
            _tipText = "這回合已經使用過符卡了";
            Image.color = _notUseColor;
        }
        else if (!character.CanUseSpell) 
        {
            _canUse = false;
            _tipText = "下次行動才能使用符卡";
            Image.color = _notUseColor;
        }
        else
        {
            _canUse = true;
            Image.color = _canUseColor;
        }
    }
}
