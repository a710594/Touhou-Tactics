using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            public override void Begin()
            {
                Vector2Int selectedPosition = Instance._selectedPosition;
                List<BattleCharacterInfo> characterList = Instance.CharacterList;
                List<Vector2Int> effectPositionList;
                Dictionary<Command, List<Vector2Int>> effectPositionDic = new Dictionary<Command, List<Vector2Int>>();

                Command command = _character.SelectedCommand;
                while(command != null)
                {
                    effectPositionList = Instance.SetEffectPosition(command);
                    effectPositionDic.Add(command, effectPositionList);
                    command = command.SubCommand;
                }

                //character in main command effectPositionList
                int predictionHp;
                BattleCharacterInfo target = null;
                command = effectPositionDic.First().Key;
                effectPositionList = effectPositionDic.First().Value;
                for (int i = 0; i < characterList.Count; i++)
                {
                    if (effectPositionList.Contains(Utility.ConvertToVector2Int(characterList[i].Position)))
                    {
                        target = characterList[i];
                        break;
                    }
                }
                predictionHp = Instance.GetPredictionHp(_character, target, target.CurrentHP, command.Effect);
                Instance._battleUI.SetCharacterInfoUI_2(target);
                Instance._battleUI.SetPredictionInfo_2(target, predictionHp);
                float hitRate = Instance.GetHitRate(command.Hit, _character, characterList[i]);
                Instance._battleUI.SetHitRate(Mathf.RoundToInt(hitRate * 100));

                for (int i = 0; i < characterList.Count; i++)
                {
                    foreach(KeyValuePair<Command, List<Vector2Int>> pair in effectPositionDic)
                    {
                        if (pair.Value.Contains(Utility.ConvertToVector2Int(characterList[i].Position)))
                        {
                            predictionHp = Instance.GetPredictionHp(_character, characterList[i], characterList[i].CurrentHP, pair.Key.Effect);
                            Instance._battleUI.SetPredictionLittleHpBar(characterList[i], predictionHp);
                        }
                    }
                }

                BattleCharacterController _controller = Instance._controllerDic[_character.Index];
                _controller.SetDirection(selectedPosition - Utility.ConvertToVector2Int(_controller.transform.position));
                _controller.SetSprite();

                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;

                Instance.Info.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(true);
            }

            public override void Click(Vector2Int position)
            {
                for (int i=0; i<_characterList.Count; i++) 
                {
                    Instance._battleUI.StopPredictionLittleHpBar(_characterList[i]);
                }
                Instance._battleUI.StopPredictionInfo();
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
