using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class TargetState : BattleControllerState
        {
            private Effect _effect;
            private List<Vector2Int> _rangeList;

            public TargetState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                _info = Instance.BattleInfo;
                _character = Instance._selectedCharacter;
                if (_character.SelectedSkill != null)
                {
                    _effect = _character.SelectedSkill.Effect;
                }
                else if(_character.SelectedSupport != null) 
                {
                    _effect = _character.SelectedSupport.Effect;
                }
                else if (_character.SelectedItem != null)
                {
                    _effect = _character.SelectedItem.Effect;
                }
                _rangeList = Utility.GetRange(_effect.Data.Range, _info.Width, _info.Height, Utility.ConvertToVector2Int(_character.Position));
                Instance.SetQuad(_rangeList, Instance._white);
                Instance._battleUI.SetSkillVisible(false);
                Instance._battleUI.SetItemScrollViewVisible(false);
            }

            public override void Click(Vector2Int position)
            {
                if (_rangeList.Contains(position))
                {
                    //顯示角色資料
                    List<BattleCharacterInfo> characterList = Instance.CharacterList;
                    for (int i = 0; i < characterList.Count; i++)
                    {
                        if (characterList[i] != Instance._selectedCharacter && position == Utility.ConvertToVector2Int(characterList[i].Position))
                        {
                            Instance._battleUI.SetCharacterInfoUI_2(characterList[i]);
                            int predictionHp = GetPredictionHp(characterList[i].CurrentHP, _effect, _character, characterList[i], characterList);
                            if (predictionHp != -1)
                            {
                                Instance._battleUI.SetHpPrediction(characterList[i].CurrentHP, predictionHp, characterList[i].MaxHP);
                            }
                            break;
                        }
                    }

                    Dictionary<Vector2Int, TileInfo> tileDic = Instance.BattleInfo.TileInfoDic;
                    Vector3 p = new Vector3(position.x, tileDic[position].Height, position.y);
                    if (_effect.Data.Track == EffectModel.TrackEnum.Straight)
                    {
                        CheckLine(_character.Position, p, characterList, tileDic, out bool isBlock, out Vector3 result);
                        Instance._cameraController.DrawLine(_character.Position, result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result);
                    }
                    else if (_effect.Data.Track == EffectModel.TrackEnum.Parabola)
                    {
                        CheckParabola(_character.Position, p, 4, tileDic, out bool isBlock, out List<Vector3> result); //要補拋物線的高度
                        Instance._cameraController.DrawParabola(result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result.Last());
                    }
                    else
                    {
                        Instance._selectedPosition = position;
                    }

                    Instance._controllerDic[_character.ID].SetDirection(Instance._selectedPosition);

                    _context.SetState<ConfirmState>();
                }
                else
                {
                    Instance.ClearQuad();
                    _context.SetState<SkillState>();
                }
            }
        }
    }
}