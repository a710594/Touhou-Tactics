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
            private BattleCharacterDetailUI _battleCharacterDetailUI;
            private BattleCharacterController _tempCharacter;

            public MoveState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                if (Instance.MoveStateBeginHandler != null)
                {
                    Instance.MoveStateBeginHandler();
                }

                _selectedCharacter = Instance.SelectedCharacter;
                _characterList = Instance.CharacterAliveList;
                Instance.BattleUI.CommandGroup.SetData(_selectedCharacter);
                Instance.BattleUI.CommandGroup.Reset();
                Instance.CharacterInfoUIGroup.ShowCharacterInfoUI_1(_selectedCharacter.Info, Utility.ConvertToVector2Int(_selectedCharacter.transform.position));
                Instance._line.gameObject.SetActive(false);
                Instance._targetList.Clear();

                if (_selectedCharacter.Info.IsSleep())
                {
                    _context.SetState<EndState>();
                }
                else
                {
                    SetData();
                }
            }

            public override void End()
            {
                Instance.ClearStepList(_selectedCharacter);
                Instance.BattleUI.SetCommandVisible(false);
            }

            public override void Update()
            {
                if (_isMoving || _selectedCharacter.Info.IsAuto) 
                {
                    return;
                }

                if (Instance.Tutorial == null || !Instance.Tutorial.IsActive) 
                {
                    if (Utility.GetMouseButtonDoubleClick(0))
                    {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit, 100, _battleTileLayer))
                        {
                            Vector2Int v2 = Utility.ConvertToVector2Int(hit.transform.position);
                            _tempCharacter = Instance.GetCharacterByPosition(v2);
                            if (_tempCharacter != null && _battleCharacterDetailUI == null)
                            {
                                _battleCharacterDetailUI = Instance.OpenCharacterDetail(_tempCharacter.Info, v2);
                                _battleCharacterDetailUI.CloseHandler = ResetStepList;
                                Instance.ClearStepList(_selectedCharacter);
                                Instance.ShowStepList(_tempCharacter);
                                Instance._cameraController.SetMyGameObj(_tempCharacter.gameObject, true, null);

                                return;
                            }
                        }
                    }
                }

                if (Instance.UpdatePosition(out Vector2Int? position)) 
                {
                    if (position != null) 
                    {
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
                }

                if (Input.GetMouseButtonDown(0) &&
                    position != null && position != Utility.ConvertToVector2Int(_selectedCharacter.transform.position) && _selectedCharacter.Info.StepList.Contains((Vector2Int)position) &&
                    (Instance.Tutorial == null || !Instance.Tutorial.IsActive || position == Instance.Tutorial.MovePosition))
                {
                    _isMoving = true;
                    Instance.SetSelect((Vector2Int)position, false);
                    //Instance.Move((Vector2Int)position, () =>
                    //{
                    //    _isMoving = false;
                    //    SetData();

                    //    if (Instance.AfterMoveHandler != null) 
                    //    {
                    //        Instance.AfterMoveHandler();
                    //    }
                    //});
                    Move((Vector2Int)position);
                }
            }

            public void Move(Vector2Int position)
            {
                Instance.HideLastPosition();
                Instance.BattleUI.SetCommandVisible(false);
                Vector2Int start = Utility.ConvertToVector2Int(_selectedCharacter.transform.position);
                Vector2Int goal = position;
                List<Vector2Int> path = Instance.GetPath(start, goal, _selectedCharacter.Info.Faction);
                int distance = Instance.GetDistance(start, goal, _selectedCharacter.Info.Faction);
                _selectedCharacter.Info.CurrentMOV -= distance;
                _selectedCharacter.LastPosition = _selectedCharacter.transform.position;
                _selectedCharacter.Move(path, () =>
                {
                    _isMoving = false;
                    SetData();

                    if (Instance.AfterMoveHandler != null)
                    {
                        Instance.AfterMoveHandler();
                    }
                });
            }

            private void SetData() 
            {
                Instance.ClearStepList(_selectedCharacter);
                Instance.ShowStepList(_selectedCharacter);

                if (!_selectedCharacter.Info.IsAuto)
                {
                    Instance.BattleUI.SetCommandVisible(true);
                }
            }

            private void ResetStepList() 
            {
                Instance.ClearStepList(_tempCharacter);
                Instance.ShowStepList(_selectedCharacter);
                _battleCharacterDetailUI = null;
                Instance._cameraController.SetMyGameObj(_selectedCharacter.gameObject, true, null);
            }
        }
    }
}
