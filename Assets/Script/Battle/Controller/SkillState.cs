using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class SkillState : BattleControllerState
        {
            public SkillState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                _character = Instance._selectedCharacter;
                if (!_character.IsAuto)
                {
                    Instance._battleUI.SetSkillVisible(true);
                    Instance._battleUI.SetSkillData(_character.SkillList);
                }
            }

            public override void Click(Vector2Int position)
            {
                Instance.SetCharacterInfoUI_2(position);
            }
        }
    }
}
