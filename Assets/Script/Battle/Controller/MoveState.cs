using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class MoveState : BattleControllerState
        {
            private static readonly Vector2Int _maxVector2Int = new Vector2Int(int.MaxValue, int.MaxValue);

            private bool _canClick = true;
            private Vector2Int _lastPosition = _maxVector2Int;
            private List<Vector2Int> _stepList;

            public MoveState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                Instance.BattleUI.SetCommandVisible(false);
                _stepList = Instance.GetStepList(_character);
                Instance.ClearQuad();
                Instance.SetQuad(_stepList, Instance._white);
            }

            public override void End()
            {
                if(Instance.MoveStateEndHandler!=null)
                {
                    Instance.MoveStateEndHandler();
                }          
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
                if (_canClick && Physics.Raycast(ray, out hit, 100))
                {
                    Vector2Int v2 = Utility.ConvertToVector2Int(hit.point);

                    if(_lastPosition != _maxVector2Int && _lastPosition != v2) 
                    {
                        Instance.SetQuad(new List<Vector2Int>() { _lastPosition }, Color.white);
                    }

                    if (_stepList.Contains(v2)) 
                    {
                        Instance.SetQuad(new List<Vector2Int>() { v2 }, Color.yellow);
                        _lastPosition = v2;

                        if (Input.GetMouseButtonDown(0)) 
                        {
                            _canClick = false;
                            Instance.ClearQuad();
                            List<Vector2Int> path = Instance.GetPath(Utility.ConvertToVector2Int(_character.transform.position), v2, _character.Info.Faction);
                            _character.LastPosition = _character.transform.position;
                            _character.Move(path, ()=> 
                            {
                                _canClick = true;
                                
                                if (!_character.Info.HasMove)
                                {
                                    _character.Info.HasMove = true;
                                }
                                else if (!_character.Info.MoveAgain)
                                {
                                    _character.Info.MoveAgain = true;
                                }

                                if (_character.AI != null)
                                {
                                    _character.AI.OnMoveEnd();
                                }
                                else
                                {
                                    _context.SetState<CommandState>();
                                }
                            });
                        }
                    }
                }
                else
                {
                    if (_lastPosition != _maxVector2Int)
                    {
                        Instance.ClearQuad(_lastPosition);
                        _lastPosition = _maxVector2Int;
                    }
                }
            }
        }
    }
}
