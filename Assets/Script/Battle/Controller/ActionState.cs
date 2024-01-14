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
                _info = Instance.Info;
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(true);
                Instance._battleUI.ActionButtonGroup.SetButton(_character);
                Instance._battleUI.ActionButtonGroup.ResetButton.gameObject.SetActive(_character.HasMove);
                Instance._battleUI.SetCharacterInfoUI_1(_character);
                Instance._battleUI.SetCharacterInfoUI_2(null);
                Instance._battleUI.ActionButtonGroup.SkillInfoGroup.gameObject.SetActive(false);
                Instance.ClearQuad();

                bool sleep = false;
                for (int i = 0; i < _character.StatusList.Count; i++)
                {
                    if (_character.StatusList[i] is Sleep)
                    {
                        sleep = true;
                        break;
                    }
                }

                if (sleep)
                {
                    _character.ActionCount = 0;
                    _context.SetState<EndState>();
                }
                else if (_character.IsAuto)
                {
                    _character.AI.Start();
                }
                else if (_info.IsTutorial) 
                {
                    BattleTutorialController.Instance.Start();
                }
            }

            public override void Click(Vector2Int position)
            {
                Instance.SetCharacterInfoUI_2(position);
            }
        }
    }
}
