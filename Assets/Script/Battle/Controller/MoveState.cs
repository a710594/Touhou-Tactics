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
                BattleInfo info = Instance.Info;
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                _character.Controller.transform.position = _character.Position;
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
                        _character.Controller.transform.position = _character.Position;
                        Instance.BattleUI.SetSkillVisible(false);
                        Instance.Info.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(false);
                        Instance.ClearQuad();
                        List<Vector2Int> path = Instance.GetPath(Utility.ConvertToVector2Int(_character.Position), position, _character.Faction);
                        _character.Controller.Move(path);
                        _character.LastPosition = _character.Position;
                        _character.Position = new Vector3(position.x, Instance.Info.TileDic[position].TileData.Height, position.y);
                        _character.HasMove = true;
                    }
                    else
                    {
                        if (Instance.Info.TileDic.ContainsKey(_originalPosition))
                        {
                            Instance.Info.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(false);
                        }
                        _originalPosition = position;
                        Instance.Info.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (Instance.Info.TileDic.ContainsKey(_originalPosition))
                    {
                        Instance.Info.TileDic[_originalPosition].TileObject.Select.gameObject.SetActive(false);
                    }
                    _character.Controller.transform.position = _character.Position;
                    _context.SetState<CommandState>();
                }
            }
        }
    }
}
