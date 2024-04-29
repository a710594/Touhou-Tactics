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
            private Effect _effect;
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
                _effect = Utility.GetEffect(_character.SelectedObject);

                if (Passive.Contains<ArcherPassive>(_character.PassiveList))
                {
                    _rangeList = ArcherPassive.GetRange(_effect.Range, _info.Width, _info.Height, Utility.ConvertToVector2Int(_character.Position), _info.TileAttachInfoDic);
                }
                else
                {
                    _rangeList = Utility.GetRange(_effect.Range, _info.Width, _info.Height, Utility.ConvertToVector2Int(_character.Position));
                }

                Instance.RemoveByFaction(_effect, _rangeList);

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
                    if (_effect.Track == EffectModel.TrackEnum.Straight)
                    {
                        Utility.CheckLine(_character.Position, p, characterList, tileDic, out bool isBlock, out Vector3 result);
                        Instance._cameraController.DrawLine(_character.Position, result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result);
                    }
                    else if (_effect.Track == EffectModel.TrackEnum.Parabola)
                    {
                        Utility.CheckParabola(_character.Position, p, 4, characterList, tileDic, out bool isBlock, out List<Vector3> result); //�n�ɩߪ��u������
                        Instance._cameraController.DrawParabola(result, isBlock);
                        Instance._selectedPosition = Utility.ConvertToVector2Int(result.Last());
                    }
                    else
                    {
                        Instance._selectedPosition = position;
                    }

                    List<Vector2Int> areaList = Instance.GetAreaList(Utility.GetEffect(_character.SelectedObject));
                    Instance.SetArea(Utility.GetEffect(_character.SelectedObject));

                    //��ܨ�����
                    for (int i = 0; i < characterList.Count; i++)
                    {
                        if (areaList.Contains(Utility.ConvertToVector2Int(characterList[i].Position)))
                        {
                            int predictionHp = Instance.GetPredictionHp(characterList[i].CurrentHP, _effect, _character, characterList[i], characterList);
                            Instance._battleUI.SetPredictionLittleHpBar(characterList[i], predictionHp);
                            if (characterList[i] != Instance.SelectedCharacter && position == Utility.ConvertToVector2Int(characterList[i].Position))
                            {
                                Instance._battleUI.SetCharacterInfoUI_2(characterList[i]);
                                Instance._battleUI.SetPredictionInfo(characterList[i], predictionHp);
                                float hitRate = Instance.GetHitRate(_effect, _character, characterList[i]);
                                Instance._battleUI.SetHitRate(Mathf.RoundToInt(hitRate * 100));
                            }
                        }
                    }

                    BattleCharacterController _controller = Instance._controllerDic[_character.Index];
                    _controller.SetDirection(position - Utility.ConvertToVector2Int(_controller.transform.position));
                    _controller.SetSprite();

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