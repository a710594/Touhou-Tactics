using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class RangeState : BattleControllerState
        {
            private List<Vector2Int> _rangeList = new List<Vector2Int>();
            Dictionary<Command, List<Vector2Int>> _areaDic = new Dictionary<Command, List<Vector2Int>>();

            public RangeState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                _rangeList.Clear();
                _areaDic.Clear();

                if (Passive.Contains<ArcherPassive>(_character.Info.PassiveList))
                {
                    _rangeList = ArcherPassive.GetRange(_character.Info.SelectedCommand.Range, Utility.ConvertToVector2Int(_character.transform.position));
                }
                else
                {
                    _rangeList = Instance.GetRangeList(_character.Info.SelectedCommand.Range, Utility.ConvertToVector2Int(_character.transform.position));
                }

                Instance.RemoveRange(_character.Info.SelectedCommand.RangeTarget, _rangeList);

                Instance.ClearQuad();
                Instance.SetQuad(_rangeList, Instance._white);
                Instance.BattleUI.SetCommandVisible(false);
            }

            public override void Update()
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _context.SetState<CommandState>();
                    _character.Info.HasMove = false;
                    return;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Vector2Int v2 = Utility.ConvertToVector2Int(hit.point);

                    ResetQuad();

                    if (_rangeList.Contains(v2))
                    {
                        _areaDic.Clear();
                        Command command = _character.Info.SelectedCommand;
                        List<Vector2Int> areaList;
                        while (command != null)
                        {
                            areaList = Instance.GetAreaList(Utility.ConvertToVector2Int(_character.transform.position), v2, command);
                            _areaDic.Add(command, areaList);
                            command = command.SubCommand;
                        }

                        foreach (KeyValuePair<Command, List<Vector2Int>> pair in _areaDic)
                        {
                            Instance.SetQuad(pair.Value, Color.yellow);
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            BattleCharacterController character;
                            foreach (KeyValuePair<Command, List<Vector2Int>> pair1 in _areaDic)
                            {
                                Instance._commandTargetDic.Add(pair1.Key, new List<BattleCharacterController>());
                                foreach (Vector2Int pair2 in pair1.Value)
                                {
                                    character = Instance.GetCharacterByPosition(pair2);
                                    if (character != null)
                                    {
                                        Instance._commandTargetDic[pair1.Key].Add(character);
                                    }
                                }
                            }

                            if (_character.Info.SelectedCommand is Sub)
                            {
                                _context.SetState<SubState>();
                            }
                            else if (_character.Info.SelectedCommand is Skill)
                            {
                                _context.SetState<SkillState>();
                            }
                            else if (_character.Info.SelectedCommand is ItemCommand)
                            {
                                _context.SetState<ItemState>();
                            }
                            else if (_character.Info.SelectedCommand is Spell)
                            {
                                _context.SetState<SpellState>();
                            }
                        }
                    }
                }
                else if(Input.GetMouseButtonDown(1))
                {
                    ResetQuad();
                    _context.SetState<CommandState>();
                }
            }

            private void ResetQuad()
            {
                foreach (KeyValuePair<Command, List<Vector2Int>> pair1 in _areaDic)
                {
                    foreach (Vector2Int pair2 in pair1.Value)
                    {
                        if (_rangeList.Contains(pair2))
                        {
                            Instance.SetQuad(pair1.Value, Color.white);
                        }
                        else
                        {
                            Instance.ClearQuad(pair2);
                        }
                    }
                }
            }
        }
    }
}
