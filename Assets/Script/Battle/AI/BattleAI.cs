using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleAI
    {
        public Skill SelectedSkill;

        protected BattleCharacterInfo _info;
        protected List<Vector2Int> _stepList;
        protected bool _useSkill;
        protected BattleCharacterInfo _target;
        protected Timer _timer = new Timer();

        private CameraMove _cameraMove;

        public virtual void Init(BattleCharacterInfo info)
        {
            _info = info;
            _cameraMove = Camera.main.transform.parent.gameObject.GetComponent<CameraMove>();
            SelectedSkill = _info.SkillList[0];
        }

        public virtual void Start()
        {
        }

        public void OnMoveEnd()
        {
            if (_useSkill)
            {
                BattleController.Instance.SetTargetState(SelectedSkill);
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
                MoveCamera(_target.Position, () =>
                {
                    BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
                });
            }
            else
            {
                BattleController.Instance.Idle();
            }
        }

        protected List<BattleCharacterInfo> GetTargetList(BattleCharacterInfo.FactionEnum faction) 
        {
            List<BattleCharacterInfo> targetList = new List<BattleCharacterInfo>();

            for (int i = 0; i < BattleController.Instance.CharacterList.Count; i++)
            {
                if (BattleController.Instance.CharacterList[i].Faction == faction)
                {
                    targetList.Add(BattleController.Instance.CharacterList[i]);
                }
            }

            if (faction == BattleCharacterInfo.FactionEnum.Player)
            {
                for (int i = 0; i < BattleController.Instance.DyingList.Count; i++)
                {
                    targetList.Add(BattleController.Instance.DyingList[i]);
                }
            }

            return targetList;
        }

        protected Dictionary<BattleCharacterInfo, List<Vector2Int>> GetCanHitDic(List<BattleCharacterInfo> targetList) 
        {
            Vector2Int targetPosition_v2;
            Vector3 step_v3;
            BattleInfo battleInfo = BattleController.Instance.Info;
            List<Vector2Int> rangeList = new List<Vector2Int>();
            Dictionary<BattleCharacterInfo, List<Vector2Int>> canHitDic = new Dictionary<BattleCharacterInfo, List<Vector2Int>>(); //<�ؼ�,�i�H������ӥؼЪ��a�I>

            for (int i = 0; i < _stepList.Count; i++) //我可以移動的範圍
            {
                rangeList = Utility.GetRange(SelectedSkill.Range, battleInfo.Width, battleInfo.Height, _stepList[i]);
                for (int j = 0; j < targetList.Count; j++)
                {
                    targetPosition_v2 = Utility.ConvertToVector2Int(targetList[j].Position);
                    step_v3 = new Vector3(_stepList[i].x, BattleController.Instance.Info.TileAttachInfoDic[_stepList[i]].Height, _stepList[i].y);
                    if (targetPosition_v2 != _stepList[i] && rangeList.Contains(targetPosition_v2)) //�ؼЦ�m�P�ڤ��P�B�b�g�{��
                    {
                        bool isBlock = false;
                        //檢查射擊是否會被地形阻礙
                        if (SelectedSkill.Track == TrackEnum.Straight)
                        {
                            Utility.CheckLine(step_v3, targetList[j].Position, BattleController.Instance.CharacterList, BattleController.Instance.Info.TileAttachInfoDic, out isBlock, out Vector3 result);
                            //Instance._cameraController.DrawLine(_character.Position, result, isBlock);
                            //Instance._selectedPosition = Utility.ConvertToVector2Int(result);
                        }
                        else if (SelectedSkill.Track == TrackEnum.Parabola)
                        {
                            Utility.CheckParabola(step_v3, targetList[j].Position, 4, BattleController.Instance.CharacterList, BattleController.Instance.Info.TileAttachInfoDic, out isBlock, out List<Vector3> result); //�n�ɩߪ��u������
                            //Instance._cameraController.DrawParabola(result, isBlock);
                            //Instance._selectedPosition = Utility.ConvertToVector2Int(result.Last());
                        }
                        else
                        {
                            //Instance._selectedPosition = position;
                        }

                        if (!isBlock)
                        {
                            if (!canHitDic.ContainsKey(targetList[j]))
                            {
                                canHitDic.Add(targetList[j], new List<Vector2Int>());
                            }
                            if (!canHitDic[targetList[j]].Contains(_stepList[i]))
                            {
                                canHitDic[targetList[j]].Add(_stepList[i]);
                            }
                        }
                    }
                }
            }

            return canHitDic;
        }

        protected enum MoveToEnum 
        {
            Near, 
            Far,
        }
        protected Vector2Int GetMoveTo(MoveToEnum moveToEnum, Vector2Int start, List<Vector2Int> moveToionList) 
        {
            int distance;
            int maxDistance = -1;
            int minDistance = int.MaxValue;
            Vector2Int moveTo = new Vector2Int();
            for (int i = 0; i < moveToionList.Count; i++)
            {
                distance = BattleController.Instance.GetDistance(start, moveToionList[i], _info.Faction);
                if (moveToEnum == MoveToEnum.Near)
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        moveTo = moveToionList[i];
                    }
                }
                else if(moveToEnum == MoveToEnum.Far) 
                {
                    if (distance > maxDistance)
                    {
                        minDistance = distance;
                        moveTo = moveToionList[i];
                    }
                }
            }

            return moveTo;
        }

        protected Vector2Int GetMoveTo(MoveToEnum moveToEnum, List<Vector2Int> stepList, Vector2Int target)
        {
            int distance;
            int maxDistance = -1;
            int minDistance = int.MaxValue;
            Vector2Int moveTo = new Vector2Int();
            for (int i = 0; i < stepList.Count; i++)
            {
                distance = BattleController.Instance.GetDistance(stepList[i], target, _info.Faction);
                if (moveToEnum == MoveToEnum.Near)
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        moveTo = stepList[i];
                    }
                }
                else if (moveToEnum == MoveToEnum.Far)
                {
                    if (distance > maxDistance)
                    {
                        minDistance = distance;
                        moveTo = stepList[i];
                    }
                }
            }

            return moveTo;
        }

        protected BattleCharacterInfo GetAttackTarget(List<BattleCharacterInfo> list)
        {
            int damage;
            int maxDamage = -1;
            BattleCharacterInfo target = null;

            //�p�G����D�],�|�u�������D�]��
            for (int i = 0; i < _info.StatusList.Count; i++)
            {
                if (_info.StatusList[i] is ProvocativeStatus)
                {
                    target = ((ProvocativeStatus)_info.StatusList[i]).Target;
                    if (list.Contains(target))
                    {
                        return target;
                    }
                }
            }

            //�D��ॴ�X�̤j�ˮ`���ؼ�
            for (int i = 0; i < list.Count; i++)
            {
                damage = BattleController.Instance.GetDamage(SelectedSkill.Effect, _info, list[i]);
                if (damage > maxDamage)
                {
                    maxDamage = damage;
                    target = list[i];
                }
            }

            return target;
        }

        protected BattleCharacterInfo GetCurekTarget(List<BattleCharacterInfo> list)
        {
            int hp;
            int minHp = int.MaxValue;
            BattleCharacterInfo target = null;

            //�D��HP�̤֪��ؼ�
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CurrentHP < list[i].MaxHP)
                {
                    hp = list[i].CurrentHP;
                    if (hp < minHp)
                    {
                        minHp = hp;
                        target = list[i];
                    }
                }
            }

            return target;
        }

        protected void MoveCamera(Vector3 position, Action callback) 
        {
            _cameraMove.Move(position, callback);
            //if (BattleController.Instance.CameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
            //{
            //    position += new Vector3(-10, 10, -10);
            //}
            //else
            //{
            //    position += new Vector3(0, 10, 0);
            //}
            //float distance = Vector3.Distance(Camera.main.transform.position, position);
            //Camera.main.transform.DOMove(position, 0.1f * distance).OnComplete(() =>
            //{
            //    callback();
            //});
        }
    }
}