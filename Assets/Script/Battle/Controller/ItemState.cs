using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class ItemState : BattleControllerState 
        {
            public ItemState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                _character = Instance._selectedCharacter;
                if (!_character.IsAuto)
                {
                    Instance._battleUI.SetActionVisible(true);
                    Instance._battleUI.SetSkillVisible(true);
                    Instance._battleUI.SetItemData(_character);
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
        }
    }
}
