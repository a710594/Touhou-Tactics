using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionButton : ActionButton
{
    public override void SetColor(BattleCharacterInfo character)
    {
        if (character.HasMain)
        {
            _canUse = false;
            _tipText = "�o�^�X�w�g�ϥιL�D��F";
            Image.color = _notUseColor;
        }
        else
        {
            _canUse = true;
            Image.color = _canUseColor;
        }
    }
}
