using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class MoveState : BattleControllerState
        {
            private Vector2Int _originalPosition;
            private List<Vector2Int> _stepList;

            public MoveState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                _originalPosition = new Vector2Int(int.MaxValue, int.MaxValue);
                BattleInfo info = Instance.BattleInfo;
                _character = Instance._selectedCharacter;
                _characterList = Instance.CharacterList;
                Instance._controllerDic[_character.ID].transform.position = _character.Position;
                Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_character.Position)].HasCharacter = false;
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                _stepList = Instance.GetStepList(Utility.ConvertToVector2Int(_character.Position), _character);
                Instance.SetQuad(_stepList, Instance._white);
            }

            public override void Click(Vector2Int position)
            {
                Instance.SetCharacterInfoUI_2(position);

                if (_stepList.Contains(position))
                {
                    if (position == _originalPosition) //½T©w²¾°Ê
                    {
                        Instance._canClick = false;
                        Instance._controllerDic[_character.ID].transform.position = _character.Position;
                        Instance._battleUI.SetSkillVisible(false);
                        Instance.BattleInfo.TileComponentDic[_originalPosition].Select.gameObject.SetActive(false);
                        Instance.ClearQuad();
                        List<Vector2Int> path = Instance.GetPath(Utility.ConvertToVector2Int(_character.Position), position, _character.Faction);
                        Instance._controllerDic[_character.ID].Move(path);
                        Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_character.Position)].HasCharacter = false;
                        _character.LastPosition = _character.Position;
                        _character.Position = new Vector3(position.x, Instance.BattleInfo.TileInfoDic[position].Height, position.y);
                        Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_character.Position)].HasCharacter = true;
                    }
                    else
                    {
                        if (Instance.BattleInfo.TileComponentDic.ContainsKey(_originalPosition))
                        {
                            Instance.BattleInfo.TileComponentDic[_originalPosition].Select.gameObject.SetActive(false);
                        }
                        _originalPosition = position;
                        Instance.BattleInfo.TileComponentDic[_originalPosition].Select.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (Instance.BattleInfo.TileComponentDic.ContainsKey(_originalPosition))
                    {
                        Instance.BattleInfo.TileComponentDic[_originalPosition].Select.gameObject.SetActive(false);
                    }
                    Instance._controllerDic[_character.ID].transform.position = _character.Position;
                    _context.SetState<ActionState>();
                }
            }
        }
    }
}
