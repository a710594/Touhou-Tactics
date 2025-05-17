using System;
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

            private bool _isMoving = false;
            private Vector2Int? _lastPosition = null;
            private List<Vector2Int> _stepList;

            public MoveState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                if (Instance.MoveStateBeginHandler != null)
                {
                    Instance.MoveStateBeginHandler();
                }

                _isMoving = false;
                _selectedCharacter = Instance.SelectedCharacter;
                _characterList = Instance.CharacterAliveList;
                Instance.BattleUI.SetCommandVisible(false);
                _stepList = Instance.GetStepList(_selectedCharacter);
                Instance.SetQuad(_stepList, _white);
            }

            public override void End()
            {
                Instance.ClearQuad(_stepList);    
            }

            public override void Update()
            {
                if (_isMoving || _selectedCharacter.Info.IsAuto) 
                {
                    return;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _context.SetState<CommandState>();
                    _selectedCharacter.Info.HasMove = false;
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
                        Instance.SetSelect((Vector2Int)position, true);
                        BattleCharacterController character = Instance.GetCharacterByPosition((Vector2Int)position);
                        if (character != null && character != _selectedCharacter)
                        {
                            Instance.CharacterInfoUIGroup.ShowCharacterInfoUI_2(character.Info, Utility.ConvertToVector2Int(character.transform.position));
                        }
                        else
                        {
                            Instance.CharacterInfoUIGroup.HideCharacterInfoUI_2();
                        }
                    }
                    _lastPosition = position;
                }

                if (position != null && _stepList.Contains((Vector2Int)position) && Input.GetMouseButtonDown(0) &&
                    (Instance.Tutorial == null || !Instance.Tutorial.IsActive || position == Instance.Tutorial.MovePosition))
                {
                    _isMoving = true;
                    Instance.SetSelect((Vector2Int)position, false);
                    Instance.Move((Vector2Int)position, () =>
                    {
                        _context.SetState<CommandState>();
                    });
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
        }
    }
}
