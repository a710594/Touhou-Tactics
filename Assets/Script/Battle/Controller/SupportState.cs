using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        /*private class SupportState : BattleControllerState 
        {
            public SupportState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                //Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                _character = Instance.SelectedCharacter;
                if (!_character.IsAuto)
                {
                    Instance._battleUI.SetActionVisible(true);
                    Instance._battleUI.SetSkillVisible(true);
                    Instance._battleUI.SetSupportData(_character.SupportList, _character.CurrentSP);
                }
            }

            public override void End()
            {
                Instance._battleUI.SetSkillVisible(false);
            }

            public override void Click(Vector2Int position)
            {
                bool show = Instance.SetCharacterInfoUI_2(position);
                if (!show)
                {
                    _context.SetState<ActionState>();
                }
            }
        }*/
    }
}
