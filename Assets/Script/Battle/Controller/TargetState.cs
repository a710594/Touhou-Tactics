using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    /*public partial class BattleController
    {
        public class TargetState : BattleControllerState
        {
            private Command _command;
            private List<Vector2Int> _rangeList;

            public TargetState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                _command = _character.Info.SelectedCommand;

                if (Passive.Contains<ArcherPassive>(_character.Info.PassiveList))
                {
                    _rangeList = ArcherPassive.GetRange(_character.Info.SelectedCommand.Range, Utility.ConvertToVector2Int(_character.transform.position));
                }
                else
                {
                    _rangeList = Instance.GetRangeList(_character.Info.SelectedCommand.Range, Utility.ConvertToVector2Int(_character.transform.position));
                }

                Instance.RemoveByFaction(_command.RangeTarget, _rangeList);

                Instance.ClearQuad();
                Instance.SetQuad(_rangeList, Instance._white);
                Instance.BattleUI.SetActionVisible(false);
                Instance.BattleUI.SetSkillVisible(false);
            }

            public override void Click(Vector2Int position)
            {
                if (_rangeList.Contains(position))          
                {
                    //draw line
                    Vector3 p = new Vector3(position.x, Instance.TileDic[position].TileData.Height, position.y);
                    if (_command.Track == TrackEnum.Straight)
                    {
                        Utility.CheckLine(_character.transform.position, p, Instance.CharacterList, Instance.TileDic, out bool isBlock, out Vector3 result);
                        Instance._cameraDraw.DrawLine(_character.transform.position, result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result);
                    }
                    else if (_command.Track == TrackEnum.Parabola)
                    {
                        Utility.CheckParabola(_character.transform.position, p, 4, Instance.CharacterList, Instance.TileDic, out bool isBlock, out List<Vector3> result); //?n?????u??????
                        Instance._cameraDraw.DrawParabola(result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result.Last());
                    }
                    else
                    {
                        Instance._selectedPosition = position;
                    }

                    _context.SetState<ConfirmState>();
                }
                else
                {
                    Instance.ClearQuad();
                    _context.SetState<CommandState>();
                }
            }
        }
    }*/
}