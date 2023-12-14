using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class ConfirmState : BattleControllerState
        {
            public ConfirmState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                Instance.SetSkillArea(Utility.GetEffect(_character.SelectedObject));

                Instance.Info.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(true);
            }

            public override void Click(Vector2Int position)
            {
                Instance._battleUI.StopHpPrediction();
                Instance._battleUI.HideHitRate();

                if (position == Instance._selectedPosition)
                {
                    _context.SetState<EffectState>();
                }
                else
                {
                    Instance._battleUI.SetCharacterInfoUI_2(null);
                    Instance.ClearQuad();
                    _context.SetState<ActionState>();
                }
            }

            public override void End()
            {
                Instance._cameraController.Clear();
                Instance.Info.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(false);
            }
        }
    }
}
