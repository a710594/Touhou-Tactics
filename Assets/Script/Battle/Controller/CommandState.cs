using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle 
{
    public partial class BattleController
    {
        public class CommandState : BattleControllerState
        {
            public CommandState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                if(Instance.CommandStateBeginHandler != null)
                {
                    Instance.CommandStateBeginHandler();
                }

                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                Instance.BattleUI.SetActionVisible(true);
                Instance.BattleUI.ActionButtonGroup.SetButton(_character.Info);
                Instance.BattleUI.ActionButtonGroup.ResetButton.gameObject.SetActive(_character.Info.HasMove);
                Instance.BattleUI.SetCharacterInfoUI_1(_character.Info);
                Instance.BattleUI.SetCharacterInfoUI_2(null);
                Instance.BattleUI.ActionButtonGroup.SkillInfoGroup.gameObject.SetActive(false);
                Instance.ClearQuad();
                Instance.ShowTileBuff(_character);

                bool sleep = false;
                for (int i = 0; i < _character.Info.StatusList.Count; i++)
                {
                    if (_character.Info.StatusList[i] is Sleep)
                    {
                        sleep = true;
                        break;
                    }
                }

                if (sleep)
                {
                    _character.Info.ActionCount = 0;
                    _context.SetState<EndState>();
                }
                else if (_character.Info.IsAuto)
                {
                    _character.AI.Start();
                }
            }

            public override void Click(Vector2Int position)
            {
                Instance.ClearQuad();
                Instance.SetCharacterInfoUI_2(position);
            }
        }
    }
}
