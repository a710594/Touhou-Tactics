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
                _allCharacterList = GetAllCharacterList();
                List<Vector2Int> commandPositionList;
                Dictionary<Command, List<Vector2Int>> effectPositionDic = new Dictionary<Command, List<Vector2Int>>();
                Instance._commandTargetDic.Clear();

                Command command = _character.SelectedCommand;
                while(command != null)
                {
                    commandPositionList = Instance.SetCommandPosition(command);
                    effectPositionDic.Add(command, commandPositionList);
                    command = command.SubCommand;
                }

                //one character in main commandPositionList
                int predictionHp;
                BattleCharacterInfo target = null;
                command = effectPositionDic.First().Key;
                commandPositionList = effectPositionDic.First().Value;
                for (int i = 0; i < _allCharacterList.Count; i++)
                {
                    if (commandPositionList.Contains(Utility.ConvertToVector2Int(_allCharacterList[i].Position)))
                    {
                        target = _allCharacterList[i];
                        break;
                    }
                }
                predictionHp = Instance.GetPredictionHp(_character, target, target.CurrentHP, command.Effect);
                Instance._battleUI.SetCharacterInfoUI_2(target);
                Instance._battleUI.SetPredictionInfo_2(target, predictionHp);
                float hitRate = Instance.GetHitRate(command.Hit, _character, target);
                Instance._battleUI.SetHitRate(Mathf.RoundToInt(hitRate * 100));

                //all character in main commandPositionList
                for (int i = 0; i < _allCharacterList.Count; i++)
                {
                    foreach(KeyValuePair<Command, List<Vector2Int>> pair in effectPositionDic)
                    {
                        Instance._commandTargetDic.Add(pair.Key, new List<BattleCharacterInfo>());
                        if (pair.Value.Contains(Utility.ConvertToVector2Int(_allCharacterList[i].Position)))
                        {
                            predictionHp = Instance.GetPredictionHp(_character, _allCharacterList[i], _allCharacterList[i].CurrentHP, pair.Key.Effect);
                            Instance._battleUI.SetPredictionLittleHpBar(_allCharacterList[i], predictionHp);
                            Instance._commandTargetDic[command].Add(_allCharacterList[i]);
                        }
                    }
                }

                BattleCharacterController _controller = Instance._controllerDic[_character.Index];
                _controller.SetDirection(selectedPosition - Utility.ConvertToVector2Int(_controller.transform.position));
                _controller.SetSprite();

                Instance.Info.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(true);
            }

            public override void Click(Vector2Int position)
            {
                for (int i=0; i< _allCharacterList.Count; i++) 
                {
                    Instance._battleUI.StopPredictionLittleHpBar(_allCharacterList[i]);
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
