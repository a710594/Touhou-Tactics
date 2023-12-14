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
                _info = Instance.Info;
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                _effect = Utility.GetEffect(_character.SelectedObject);

                if (Passive.Contains<ArcherPassive>(_character.PassiveList))
                {
                    _rangeList = ArcherPassive.GetRange(_effect.Range, _info.Width, _info.Height, Utility.ConvertToVector2Int(_character.Position), _info.TileInfoDic);
                }
                else
                {
                    _rangeList = Utility.GetRange(_effect.Range, _info.Width, _info.Height, Utility.ConvertToVector2Int(_character.Position));
                }

                Instance.RemoveByFaction(_effect, _rangeList);

                //Vector2Int v2;
                //for (int i=0; i< _characterList.Count; i++) 
                //{
                //    v2 = Utility.ConvertToVector2Int(_characterList[i].Position);
                //    if (_rangeList.Contains(v2)) 
                //    {
                //        if(_effect.Target == EffectModel.TargetEnum.None ||
                //           (_effect.Target == EffectModel.TargetEnum.Us && _character.Faction != _characterList[i].Faction) ||
                //           (_effect.Target == EffectModel.TargetEnum.Them && _character.Faction == _characterList[i].Faction)) 
                //        {
                //            _rangeList.Remove(v2);
                //            i--;
                //        }
                //    }
                //}

                Instance.SetQuad(_rangeList, Instance._white);
                Instance._battleUI.SetActionVisible(false);
                Instance._battleUI.SetSkillVisible(false);
            }

            public override void Click(Vector2Int position)
            {
                if (_rangeList.Contains(position))
                {
                    //��ܨ�����
                    List<BattleCharacterInfo> characterList = Instance.CharacterList;
                    for (int i = 0; i < characterList.Count; i++)
                    {
                        if (characterList[i] != Instance.SelectedCharacter && position == Utility.ConvertToVector2Int(characterList[i].Position))
                        {
                            Instance._battleUI.SetCharacterInfoUI_2(characterList[i]);
                            int predictionHp = Instance.GetPredictionHp(characterList[i].CurrentHP, _effect, _character, characterList[i], characterList);
                            if (predictionHp != -1)
                            {
                                Instance._battleUI.SetHpPrediction(characterList[i].CurrentHP, predictionHp, characterList[i].MaxHP);
                            }
                            float hitRate = Instance.GetHitRate(_effect, _character, characterList[i]);
                            Instance._battleUI.SetHitRate(Mathf.RoundToInt(hitRate * 100));
                            break;
                        }
                    }

                    Dictionary<Vector2Int, TileInfo> tileDic = Instance.Info.TileInfoDic;
                    Vector3 p = new Vector3(position.x, tileDic[position].Height, position.y);
                    if (_effect.Track == EffectModel.TrackEnum.Straight)
                    {
                        CheckLine(_character.Position, p, characterList, tileDic, out bool isBlock, out Vector3 result);
                        Instance._cameraController.DrawLine(_character.Position, result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result);
                    }
                    else if (_effect.Track == EffectModel.TrackEnum.Parabola)
                    {
                        CheckParabola(_character.Position, p, 4, tileDic, out bool isBlock, out List<Vector3> result); //�n�ɩߪ��u������
                        Instance._cameraController.DrawParabola(result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result.Last());
                    }
                    else
                    {
                        Instance._selectedPosition = position;
                    }

                    Instance._controllerDic[_character.Index].SetDirection(Instance._selectedPosition);

                    _context.SetState<ConfirmState>();
                }
                else
                {
                    Instance.ClearQuad();
                    _context.SetState<ActionState>();
                }
            }
        }
    }
}