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
                if (Instance.CommandStateBeginHandler != null)
                {
                    Instance.CommandStateBeginHandler();
                }

                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                Instance.BattleUI.CommandGroup.SetData(_character.Info);
                Instance.BattleUI.CommandGroup.Reset();
                Instance.CharacterInfoUIGroup.SetCharacterInfoUI_1(_character.Info);
                Instance.CharacterInfoUIGroup.SetCharacterInfoUI_2(null);
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
                    _context.SetState<EndState>();
                }
                else if (_character.Info.IsAuto)
                {
                    _character.AI.Start();
                    Instance.BattleUI.SetCommandVisible(false);
                }
                else
                {
                    Instance.BattleUI.SetCommandVisible(true);
                }
            }

            public void SetCommand(Command command)
            {
                _character.Info.SelectedCommand = command;
                _context.SetState<RangeState>();
            }
        }
    }
}
