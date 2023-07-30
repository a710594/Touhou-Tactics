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
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                _character = Instance._selectedCharacter;
                if (!_character.IsAuto)
                {
                    Instance._battleUI.SetItemScrollView();
                }
            }

            public override void Click(Vector2Int position)
            {
                Instance.SetCharacterInfoUI_2(position);
            }
        }
    }
}
