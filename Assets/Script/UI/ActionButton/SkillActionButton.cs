using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionButton : ActionButton
{
    public override void SetColor(BattleCharacterInfo character)
    {
        if(character.HasMain)
        {
            _canUse = false;
            _tipText = "這回合已經使用過技能了";
            Image.color = _notUseColor;
        }
        else 
        {
            _canUse = true;
            Image.color = _canUseColor;
        }
    }
}
