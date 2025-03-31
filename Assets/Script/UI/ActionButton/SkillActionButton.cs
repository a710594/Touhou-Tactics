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
            _tipText = "�o�^�X�w�g�ϥιL�ޯ�F";
            Image.color = _notUseColor;
        }
        else 
        {
            _canUse = true;
            Image.color = _canUseColor;
        }
    }
}
