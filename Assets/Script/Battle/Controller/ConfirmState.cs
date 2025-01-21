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
                _character = Instance.SelectedCharacter;
                _allCharacterList = GetAllCharacterList();
                List<Vector2Int> commandPositionList;
                Dictionary<Command, List<Vector2Int>> commandPositionDic = new Dictionary<Command, List<Vector2Int>>();
                Instance._commandTargetDic.Clear();

                Command command = _character.Info.SelectedCommand;
                while(command != null)
                {
                    commandPositionList = Instance.SetCommandPosition(Utility.ConvertToVector2Int(_character.transform.position), selectedPosition, command);
                    commandPositionDic.Add(command, commandPositionList);
                    command = command.SubCommand;
                }

                //one character in main commandPositionList
                int predictionHp;
                BattleCharacterController target = null;
                command = commandPositionDic.First().Key;
                commandPositionList = commandPositionDic.First().Value;
                for (int i = 0; i < _allCharacterList.Count; i++)
                {
                    if (commandPositionList.Contains(Utility.ConvertToVector2Int(_allCharacterList[i].transform.position)))
                    {
                        target = _allCharacterList[i];
                        break;
                    }
                }

                if(target!=null)
                {
                    predictionHp = Instance.GetPredictionHp(_character, target, target.Info.CurrentHP, command.Effect);
                    Instance.BattleUI.SetCharacterInfoUI_2(target.Info);
                    Instance.BattleUI.SetPredictionInfo_2(target.Info, predictionHp);
                    float hitRate = Instance.GetHitRate(command.Hit, _character, target);
                    Instance.BattleUI.SetHitRate(Mathf.RoundToInt(hitRate * 100));
                }
                
                Instance.ClearQuad();
                foreach(KeyValuePair<Command, List<Vector2Int>> pair in commandPositionDic)
                {
                    Instance._commandTargetDic.Add(pair.Key, new List<BattleCharacterController>());
                    Instance.SetQuad(pair.Value, Instance._yellow); 
                    //all character in main commandPositionList
                    for (int i = 0; i < _allCharacterList.Count; i++)
                    {
                        if (pair.Value.Contains(Utility.ConvertToVector2Int(_allCharacterList[i].transform.position)))
                        {
                            predictionHp = Instance.GetPredictionHp(_character, _allCharacterList[i], _allCharacterList[i].Info.CurrentHP, pair.Key.Effect);
                            Instance.BattleUI.SetPredictionLittleHpBar(_allCharacterList[i], predictionHp);
                            Instance._commandTargetDic[pair.Key].Add(_allCharacterList[i]);
                        }
                    }
                }


                _character.SetDirection(selectedPosition - Utility.ConvertToVector2Int(_character.transform.position));
                _character.SetSprite();

                Instance.TileDic[Instance._selectedPosition].TileObject.Select.gameObject.SetActive(true);
            }

            public override void Click(Vector2Int position)
            {
                for (int i=0; i< _allCharacterList.Count; i++) 
                {
                    Instance.BattleUI.StopPredictionLittleHpBar(_allCharacterList[i]);
                }
                Instance.BattleUI.StopPredictionInfo();
                Instance.BattleUI.HideHitRate();

                if (position == Instance._selectedPosition)
                {
                    _context.SetState<EffectState>();
                }
                else
                {
                    Instance.BattleUI.SetCharacterInfoUI_2(null);
                    Instance.ClearQuad();
                    _context.SetState<CommandState>();
                }
            }

            public override void End()
            {
                Instance._cameraController.Clear();
                Instance.TileDic[Instance._selectedPosition].TileObject.Select.gameObject.SetActive(false);
            }
        }
    }
}
