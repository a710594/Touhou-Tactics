using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class TargetState : BattleControllerState
        {
            private Command _command;
            private List<Vector2Int> _rangeList;
            private CameraRotate _cameraRotate;

            public TargetState(StateContext context) : base(context)
            {
                _cameraRotate = Camera.main.GetComponent<CameraRotate>();
            }

            public override void Begin()
            {
                _info = Instance.Info;
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;
                _command = _character.SelectedCommand;

                if (Passive.Contains<ArcherPassive>(_character.PassiveList))
                {
                    _rangeList = ArcherPassive.GetRange(_character.SelectedCommand.Range, _info.Width, _info.Height, Utility.ConvertToVector2Int(_character.Position), _info.TileAttachInfoDic);
                }
                else
                {
                    _rangeList = Utility.GetRange(_character.SelectedCommand.Range, _info.Width, _info.Height, Utility.ConvertToVector2Int(_character.Position));
                }

                Instance.RemoveByFaction(_command.RangeTarget, _rangeList);

                Instance.ClearQuad();
                Instance.SetQuad(_rangeList, Instance._white);
                Instance._battleUI.SetActionVisible(false);
                Instance._battleUI.SetSkillVisible(false);
            }

            public override void Click(Vector2Int position)
            {
                if (_rangeList.Contains(position))          
                {
                    List<BattleCharacterInfo> characterList = Instance.CharacterList;
                    //draw line
                    Dictionary<Vector2Int, TileAttachInfo> tileDic = Instance.Info.TileAttachInfoDic;
                    Vector3 p = new Vector3(position.x, tileDic[position].Height, position.y);
                    if (_command.Track == TrackEnum.Straight)
                    {
                        Utility.CheckLine(_character.Position, p, characterList, tileDic, out bool isBlock, out Vector3 result);
                        Instance._cameraController.DrawLine(_character.Position, result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result);
                    }
                    else if (_command.Track == TrackEnum.Parabola)
                    {
                        Utility.CheckParabola(_character.Position, p, 4, characterList, tileDic, out bool isBlock, out List<Vector3> result); //?n?????u??????
                        Instance._cameraController.DrawParabola(result, isBlock);
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
                    _context.SetState<ActionState>();
                }
            }
        }
    }
}