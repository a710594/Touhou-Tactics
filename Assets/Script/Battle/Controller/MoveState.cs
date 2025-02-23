using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class MoveState : BattleControllerState
        {
            private Vector2Int _originalPosition;
            private List<Vector2Int> _stepList;

            public MoveState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _originalPosition = new Vector2Int(int.MaxValue, int.MaxValue);
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                Instance.BattleUI.SetActionVisible(false);
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
                        Instance.BattleUI.SetSkillVisible(false);
                        Instance.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(false);
                        Instance.ClearQuad();
                        List<Vector2Int> path = Instance.GetPath(Utility.ConvertToVector2Int(_character.transform.position), position, _character.Info.Faction);
                        _character.Move(path);
                        _character.LastPosition = _character.transform.position;
                        _character.Info.HasMove = true;
                    }
                    else
                    {
                        if (Instance.TileDic.ContainsKey(_originalPosition))
                        {
                            Instance.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(false);
                        }
                        _originalPosition = position;
                        Instance.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (Instance.TileDic.ContainsKey(_originalPosition))
                    {
                        Instance.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(false);
                    }
                    _context.SetState<CommandState>();
                }
            }
        }
    }
}
