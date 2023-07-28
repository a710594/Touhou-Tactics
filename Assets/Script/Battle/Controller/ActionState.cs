using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle 
{
    public partial class BattleController
    {
        private class ActionState : BattleControllerState
        {
            public ActionState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                _character = Instance._selectedCharacter;
                _characterList = Instance.CharacterList;
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(true);
                Instance._battleUI.ActionButtonGroup.SetButton(_character);
                if (!_character.HasUseSkill && _character.LastPosition != BattleCharacterInfo.DefaultLastPosition && _character.ActionCount < 2)
                {
                    Instance._battleUI.ActionButtonGroup.ResetButton.interactable = true;
                }
                else
                {
                    Instance._battleUI.ActionButtonGroup.ResetButton.interactable = false;
                }
                Instance._battleUI.SetCharacterInfoUI_1(_character);
                Instance._battleUI.SetCharacterInfoUI_2(null);
                Instance.ClearQuad();

                if (_character.IsAuto)
                {
                    _character.AI.Start();
                }
            }

            public override void Click(Vector2Int position)
            {
                Instance.SetCharacterInfoUI_2(position);
            }
        }
    }
}
