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
            _tipText = "�o�^�X�w�g�ϥιL�ťd�F";
            Image.color = _notUseColor;
        }
        else if (!character.CanUseSpell) 
        {
            _canUse = false;
            _tipText = "�U����ʤ~��ϥβťd";
            Image.color = _notUseColor;
        }
        else
        {
            _canUse = true;
            Image.color = _canUseColor;
        }
    }
}
