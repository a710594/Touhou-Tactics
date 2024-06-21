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

            public override void Begin()
            {
                _originalPosition = new Vector2Int(int.MaxValue, int.MaxValue);
                BattleInfo info = Instance.Info;
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                Instance._controllerDic[_character.Index].transform.position = _character.Position;
                Instance._battleUI.SetActionVisible(false);
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

            public override void Click(Vector2Int position)
            {
                if (_stepList.Contains(position))
                {
                    if (position == _originalPosition) //�T�w����
                    {
                        Instance._canClick = false;
                        Instance._controllerDic[_character.Index].transform.position = _character.Position;
                        Instance._battleUI.SetSkillVisible(false);
                        Instance.Info.TileComponentDic[_originalPosition].Select.gameObject.SetActive(false);
                        Instance.ClearQuad();
                        List<Vector2Int> path = Instance.GetPath(Utility.ConvertToVector2Int(_character.Position), position, _character.Faction);
                        Instance._controllerDic[_character.Index].Move(path);
                        _character.LastPosition = _character.Position;
                        _character.Position = new Vector3(position.x, Instance.Info.TileAttachInfoDic[position].Height, position.y);
                        _character.HasMove = true;
                    }
                    else
                    {
                        if (Instance.Info.TileComponentDic.ContainsKey(_originalPosition))
                        {
                            Instance.Info.TileComponentDic[_originalPosition].Select.gameObject.SetActive(false);
                        }
                        _originalPosition = position;
                        Instance.Info.TileComponentDic[_originalPosition].Select.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (Instance.Info.TileComponentDic.ContainsKey(_originalPosition))
                    {
                        Instance.Info.TileComponentDic[_originalPosition].Select.gameObject.SetActive(false);
                    }
                    Instance._controllerDic[_character.Index].transform.position = _character.Position;
                    _context.SetState<ActionState>();
                }
            }
        }
    }
}
