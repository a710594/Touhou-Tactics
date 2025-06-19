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
            private List<Vector2Int> _areaList = new List<Vector2Int>();
            private static readonly Color _red = new Color(1, 1, 0, 0.5f);

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
                _areaList.Clear();

                Instance.SetRangeList(_selectedCharacter, _selectedCharacter.Info.SelectedCommand);
            }

            public override void Update()
            {
                if (Instance.Tutorial == null || !Instance.Tutorial.IsActive) 
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        _context.SetState<MoveState>();
                        return;
                    }
                }

                if (Instance.UpdatePosition(out Vector2Int? position))
                {
                    if (position != null)
                    {
                        Instance.ClearQuad(_areaList);
                        Instance.SetRangeList(_selectedCharacter, _selectedCharacter.Info.SelectedCommand);
                        Instance._line.Hide();
                        BattleCharacterController target = Instance.GetCharacterByPosition((Vector2Int)position);

                        if (_selectedCharacter.Info.RangeList.Contains((Vector2Int)position))
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
                                        Instance._line.Show(result, _red);
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

                            _areaList = Instance.GetAreaList(Utility.ConvertToVector2Int(_selectedCharacter.transform.position), (Vector2Int)position, command);
                            Instance.SetQuad(_areaList, Color.red);
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
                }

                if (Input.GetMouseButtonDown(0) && position != null && _areaList.Count > 0)
                {
                    Instance.UseCommand((Vector2Int)position, _areaList);
                }
            }

            public override void End()
            {
                Instance.ClearRangeList(_selectedCharacter);
                Instance.ClearQuad(_areaList);
            }
        }
    }
}
