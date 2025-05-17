using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class RangeState : BattleControllerState
        {
            private Vector2Int? _lastPosition = null;
            private List<Vector2Int> _rangeList = new List<Vector2Int>();
            private List<Vector2Int> _areaList = new List<Vector2Int>();

            public RangeState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                if (Instance.RangeStateBeginHandler != null)
                {
                    Instance.RangeStateBeginHandler();
                }

                _selectedCharacter = Instance.SelectedCharacter;
                _characterList = Instance.CharacterAliveList;
                _rangeList.Clear();
                _areaList.Clear();

                if (Passive.Contains<ArcherPassive>(_selectedCharacter.Info.PassiveList))
                {
                    _rangeList = ArcherPassive.GetRange(_selectedCharacter.Info.SelectedCommand.Range, Utility.ConvertToVector2Int(_selectedCharacter.transform.position));
                }
                else
                {
                    _rangeList = Instance.GetRangeList(_selectedCharacter.Info.SelectedCommand.Range, Utility.ConvertToVector2Int(_selectedCharacter.transform.position));
                }

                Instance.RemoveRange(_selectedCharacter.Info.SelectedCommand.Target, _rangeList);

                Instance.SetQuad(_rangeList, _white);
                Instance.BattleUI.SetCommandVisible(false);
            }

            public override void Update()
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _context.SetState<CommandState>();
                    return;
                }

                if (Instance.UpdatePosition(out Vector2Int? position))
                {
                    if (_lastPosition != null)
                    {
                        Instance.SetSelect((Vector2Int)_lastPosition, false);
                    }

                    if (position != null)
                    {
                        Instance.ClearQuad(_areaList);
                        Instance.SetQuad(_rangeList, _white);
                        Instance._line.Hide();
                        BattleCharacterController target = Instance.GetCharacterByPosition((Vector2Int)position);

                        if (_rangeList.Contains((Vector2Int)position))
                        {
                            Command command = _selectedCharacter.Info.SelectedCommand;
                            if (target != null)
                            {
                                if (command.Track == TrackEnum.Straight)
                                {
                                    Instance.CheckLine(_selectedCharacter.transform.position, target.transform.position, out bool isBlock, out Vector3 result);
                                    if (isBlock)
                                    {
                                        Instance._line.Show(_selectedCharacter.transform.position, result, Color.red);
                                    }
                                    else
                                    {
                                        Instance._line.Show(_selectedCharacter.transform.position, result, Color.blue);
                                    }
                                    position = Utility.ConvertToVector2Int(result);
                                    target = Instance.GetCharacterByPosition((Vector2Int)position);
                                }
                                else if (command.Track == TrackEnum.Parabola)
                                {
                                    Instance.CheckParabola(_selectedCharacter.transform.position, target.transform.position, Instance.TileDic[Utility.ConvertToVector2Int(_selectedCharacter.transform.position)].TileData.Height + 1, out bool isBlock, out List<Vector3> result);
                                    if (isBlock)
                                    {
                                        Instance._line.Show(result, Color.red);
                                    }
                                    else
                                    {
                                        Instance._line.Show(result, Color.blue);
                                    }
                                    position = Utility.ConvertToVector2Int(result.Last());
                                    target = Instance.GetCharacterByPosition((Vector2Int)position);
                                }

                                if (target != null && target != _selectedCharacter)
                                {
                                    Instance.CharacterInfoUIGroup.ShowCharacterInfoUI_2(target.Info, Utility.ConvertToVector2Int(target.transform.position));
                                    float hitRate = Instance.GetHitRate(command.Hit, _selectedCharacter, target);
                                    Instance.CharacterInfoUIGroup.SetHitRate(Mathf.RoundToInt(hitRate * 100));
                                    int predictionHp = Instance.GetPredictionHp(_selectedCharacter, target, target.Info.CurrentHP, command.Effect);
                                    Instance.CharacterInfoUIGroup.SetPredictionInfo_2(target.Info, predictionHp);
                                }
                                else
                                {
                                    Instance.CharacterInfoUIGroup.HideCharacterInfoUI_2();
                                }
                            }
                            else 
                            {
                                Instance.CharacterInfoUIGroup.HideCharacterInfoUI_2();
                            }

                            Instance.SetSelect((Vector2Int)position, true);
                            _areaList = Instance.GetAreaList(Utility.ConvertToVector2Int(_selectedCharacter.transform.position), (Vector2Int)position, command);
                            Instance.SetQuad(_areaList, Color.yellow);
                        }
                        else
                        {
                            if (target != null && target != _selectedCharacter)
                            {
                                Instance.CharacterInfoUIGroup.ShowCharacterInfoUI_2(target.Info, Utility.ConvertToVector2Int(target.transform.position));
                            }
                            else
                            {
                                Instance.CharacterInfoUIGroup.HideCharacterInfoUI_2();
                            }
                        }
                    }
                    _lastPosition = position;
                }

                if (Input.GetMouseButtonDown(0) && position != null && _areaList.Count > 0)
                {
                    Instance.UseCommand((Vector2Int)position, _areaList);
                }

                if (Utility.GetMouseButtonDoubleClick(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100, _battleTileLayer))
                    {
                        Vector2Int v2 = Utility.ConvertToVector2Int(hit.transform.position);
                        BattleCharacterController character = Instance.GetCharacterByPosition(v2);
                        if (character != null)
                        {
                            CharacterDetailUI characterDetailUI = CharacterDetailUI.Open(false);
                            characterDetailUI.SetData(character.Info, v2);
                        }
                    }
                }
            }

            public override void End()
            {
                Instance.ClearQuad(_rangeList);
                Instance.ClearQuad(_areaList);
                if (_lastPosition != null)
                {
                    Instance.SetSelect((Vector2Int)_lastPosition, false);
                }
            }
        }
    }
}
