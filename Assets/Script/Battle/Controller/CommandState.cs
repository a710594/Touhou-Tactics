using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle 
{
    public partial class BattleController
    {

        public class CommandState : BattleControllerState
        {
            private Vector2Int? _lastPosition = null;
            private List<Vector2Int> _stepList = new List<Vector2Int>();

            public CommandState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                if (Instance.CommandStateBeginHandler != null)
                {
                    Instance.CommandStateBeginHandler();
                }

                _stepList.Clear();
                _selectedCharacter = Instance.SelectedCharacter;
                _characterList = Instance.CharacterAliveList;
                Instance.BattleUI.CommandGroup.SetData(_selectedCharacter.Info);
                Instance.BattleUI.CommandGroup.Reset();
                Instance.CharacterInfoUIGroup.ShowCharacterInfoUI_1(_selectedCharacter.Info, Utility.ConvertToVector2Int(_selectedCharacter.transform.position));
                Instance._line.gameObject.SetActive(false);

                bool sleep = false;
                for (int i = 0; i < _selectedCharacter.Info.StatusList.Count; i++)
                {
                    if (_selectedCharacter.Info.StatusList[i] is Sleep)
                    {
                        sleep = true;
                        break;
                    }
                }

                if (sleep)
                {
                    _context.SetState<EndState>();
                }
                else
                {
                    Instance.BattleUI.SetCommandVisible(true);
                }
            }

            public void SetCommand(Command command)
            {
                _selectedCharacter.Info.SelectedCommand = command;
                _context.SetState<RangeState>();
            }

            public override void Update()
            {
                if (Instance.UpdatePosition(out Vector2Int? position))
                {
                    if (_lastPosition != null)
                    {
                        Instance.SetSelect((Vector2Int)_lastPosition, false);
                    }

                    if (position != null)
                    {
                        Instance.ClearQuad(_stepList);
                        Instance.SetSelect((Vector2Int)position, true);
                        BattleCharacterController character = Instance.GetCharacterByPosition((Vector2Int)position);
                        if (character != null)
                        {
                            _stepList = Instance.GetStepList(character);
                            Instance.SetQuad(_stepList, _white);
                            if (character != _selectedCharacter) 
                            {
                                Instance.CharacterInfoUIGroup.ShowCharacterInfoUI_2(character.Info, Utility.ConvertToVector2Int(character.transform.position));
                            }
                        }
                        else
                        {
                            _stepList.Clear();
                            Instance.CharacterInfoUIGroup.HideCharacterInfoUI_2();
                        }
                    }
                    _lastPosition = position;
                }

                if (Utility.GetMouseButtonDoubleClick(0)) 
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100, _battleTileLayer)) 
                    {
                        Vector2Int v2 = Utility.ConvertToVector2Int(hit.transform.position);
                        BattleCharacterController character = Instance.GetCharacterByPosition(v2);
                        CharacterDetailUI characterDetailUI = CharacterDetailUI.Open(false);
                        characterDetailUI.SetData(character.Info, v2);
                    }
                }
            }

            public override void End()
            {
                Instance.ClearQuad(_stepList);
            }
        }
    }
}
