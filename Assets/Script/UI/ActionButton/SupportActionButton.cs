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
            _tipText = "這回合已經使用過支援了";
            Image.color = _notUseColor;
        }
        else
        {
            _canUse = true;
            Image.color = _canUseColor;
        }
    }
}
