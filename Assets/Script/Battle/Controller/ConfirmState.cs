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
                _character = Instance._selectedCharacter;
                _characterList = Instance.CharacterList;

                Effect effect = null;
                if (_character.SelectedSkill != null) 
                {
                    effect = _character.SelectedSkill.Effect;
                }
                else if (_character.SelectedSupport != null) 
                {
                    effect = _character.SelectedSupport.Effect;
                }
                else if (_character.SelectedItem != null)
                {
                    effect = _character.SelectedItem.Effect;
                }

                Instance.SetSkillArea(effect);
                Vector2Int v = Instance._selectedPosition;

                Instance.BattleInfo.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(true);
            }

            public override void Click(Vector2Int position)
            {
                Instance._battleUI.StopHpPrediction();

                if (position == Instance._selectedPosition)
                {
                    _context.SetState<EffectState>();
                }
                else
                {
                    Instance._battleUI.SetCharacterInfoUI_2(null);
                    Instance.ClearQuad();
                    _context.SetState<SkillState>();
                }
            }

            public override void End()
            {
                Instance._cameraController.Clear();
                Instance.BattleInfo.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(false);
            }
        }
    }
}
