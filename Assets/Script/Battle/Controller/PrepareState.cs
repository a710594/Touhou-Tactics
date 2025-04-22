using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class PrepareState : BattleControllerState 
        {
            private Vector2Int? _lastPosition = null;
            private Vector3 _originalPosition;
            private BattleCharacterController _dragCharacter = null;

            public PrepareState(StateContext context) : base(context)
            {
                Instance.TempList.Clear();
            }

            public override void Begin()
            {
                Instance.SelectBattleCharacterUI.Init(Instance.MinPlayerCount, Instance.MaxPlayerCount, Instance._candidateList);
                Instance.SetQuad(Instance.PlayerPositionList, _white);

                if(Instance.PrepareStateBeginHandler!=null)
                {
                    Instance.PrepareStateBeginHandler();
                }
            }

            public override void End()
            {
                for(int i=0; i<Instance.TempList.Count; i++)
                {
                    Instance._maxIndex++;
                    Instance.TempList[i].Index = Instance._maxIndex;
                    Instance.TempList[i].Outline.OutlineColor = Color.blue;
                    Instance.CharacterAliveList.Add(Instance.TempList[i]);

                    Instance.TempList[i].MoveEndHandler += Instance.OnMoveEnd;
                    Instance.BattleUI.CharacterListGroup.Add(Instance.TempList[i]);
                    Instance.BattleUI.SetFloatingNumberPoolAnchor(Instance.TempList[i]);
                }
                Instance.BattleUI.gameObject.SetActive(true);
                Instance.SelectBattleCharacterUI.gameObject.SetActive(false);
                Instance.SortCharacterList(true);
                Instance.ClearQuad(Instance.PlayerPositionList);

                if (_lastPosition != null)
                {
                    Instance.SetSelect((Vector2Int)_lastPosition, false);
                }
            }

            public bool PlaceCharacter(BattleCharacterController character)
            {
                Vector2Int position = Utility.ConvertToVector2Int(character.transform.position);
                if (Instance.PlayerPositionList.Contains(position))
                {
                    for (int i = 0; i < Instance.TempList.Count; i++)
                    {
                        if (Utility.ConvertToVector2Int(Instance.TempList[i].transform.position) == position)
                        {
                            GameObject.Destroy(character.gameObject);
                            return false;
                        }
                    }
                    Instance.TempList.Add(character);
                    return true;
                }
                else
                {
                    GameObject.Destroy(character.gameObject);
                    return false;
                }
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

                if (Input.GetMouseButtonDown(0)) 
                {
                    OnDragBegin();
                }
                else if (Input.GetMouseButton(0)) 
                {
                    OnDrag();
                }
                else if (Input.GetMouseButtonUp(0)) 
                {
                    OnDragEnd();
                }

                if (Utility.GetMouseButtonDoubleClick(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100, _battleTileLayer))
                    {
                        Vector2Int v2 = Utility.ConvertToVector2Int(hit.transform.position);
                        BattleCharacterController character = Instance.GetCharacterByPosition(Utility.ConvertToVector2Int(hit.transform.position));
                        CharacterDetailUI characterDetailUI = CharacterDetailUI.Open(false);
                        characterDetailUI.SetData(character.Info, v2);
                    }
                }
            }

            private void OnDragBegin() 
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100, _battleTileLayer))
                {
                    _dragCharacter = Instance.GetCharacterByPosition(Utility.ConvertToVector2Int(hit.transform.position));
                    if (_dragCharacter != null) 
                    {
                        if (Instance.TempList.Contains(_dragCharacter))
                        {
                            _originalPosition = _dragCharacter.transform.position;
                        }
                        else
                        {
                            _dragCharacter = null;
                        }    
                    }
                }
            }

            private void OnDrag()
            {
                if (_dragCharacter != null)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100, _battleTileLayer))
                    {
                        if (hit.collider.tag == "BattleTile" && Instance.PlayerPositionList.Contains(Utility.ConvertToVector2Int(hit.transform.position)))
                        {
                            BattleInfoTile tile = BattleController.Instance.TileDic[Utility.ConvertToVector2Int(hit.transform.position)];
                            _dragCharacter.transform.position = hit.transform.position + hit.transform.up * tile.TileData.Height;
                        }
                        else
                        {
                            Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                            _dragCharacter.transform.position = screenPos;
                        }
                    }
                    else
                    {
                        Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                        _dragCharacter.transform.position = screenPos;
                    }
                }
            }

            private void OnDragEnd()
            {
                if (_dragCharacter != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, _battleTileLayer))
                    {
                        Vector2Int position = Utility.ConvertToVector2Int(hit.transform.position);
                        if (Instance.PlayerPositionList.Contains(position))
                        {
                            for (int i = 0; i < Instance.TempList.Count; i++)
                            {
                                if (_dragCharacter != Instance.TempList[i] && Utility.ConvertToVector2Int(Instance.TempList[i].transform.position) == position)
                                {
                                    _dragCharacter.transform.position = _originalPosition;
                                }
                            }
                        }
                        else
                        {
                            _dragCharacter.transform.position = _originalPosition;
                        }
                    }
                    else
                    {
                        _dragCharacter.transform.position = _originalPosition;
                    }
                    _dragCharacter = null;
                }
            }
        }
    }
}
