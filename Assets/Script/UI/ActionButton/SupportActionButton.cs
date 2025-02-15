using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportActionButton : ActionButton
{
    public override void SetColor(BattleCharacterInfo character)
    {
        if (character.HasUseSupport)
        {
            _canUse = false;
            _tipText = "�o�^�X�w�g�ϥιL�䴩�F";
            Image.color = _notUseColor;
        }
        else
        {
            _canUse = true;
            Image.color = _canUseColor;
        }
    }
}
