using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class OldBattleAI
    {
        public Skill SelectedSkill;

        protected BattleCharacterController _controller;
        protected List<Vector2Int> _stepList;
        protected bool _useSkill;
        protected BattleCharacterController _target;
        protected Timer _timer = new Timer();

        public virtual void Init(BattleCharacterController controller)
        {
            _controller = controller;
            SelectedSkill = _controller.Info.SkillList[0];
        }

        public virtual void Start()
        {
        }

        public void OnMoveEnd()
        {
            if (_useSkill)
            {
                BattleController.Instance.SetSelectedCommand(SelectedSkill);
                //BattleController.Instance.SetState<BattleController.TargetState>();
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.transform.position));
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.transform.position));
            }
            else
            {
                BattleController.Instance.SetState<BattleController.EndState>();
            }
        }

        protected List<BattleCharacterController> GetTargetList(BattleCharacterInfo.FactionEnum faction) 
        {
            List<BattleCharacterController> targetList = new List<BattleCharacterController>();

            for (int i = 0; i < BattleController.Instance.CharacterAliveList.Count; i++)
            {
                if (BattleController.Instance.CharacterAliveList[i].Info.Faction == faction)
                {
                    targetList.Add(BattleController.Instance.CharacterAliveList[i]);
                }
            }

            if (faction == BattleCharacterInfo.FactionEnum.Player)
            {
                for (int i = 0; i < BattleController.Instance.CharacterDyingList.Count; i++)
                {
                    targetList.Add(BattleController.Instance.CharacterDyingList[i]);
                }
            }

            return targetList;
        }

        protected Dictionary<BattleCharacterController, List<Vector2Int>> GetCanHitDic(List<BattleCharacterController> targetList) 
        {
            Vector2Int targetPosition_v2;
            Vector3 step_v3;
            List<Vector2Int> rangeList = new List<Vector2Int>();
            Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic = new Dictionary<BattleCharacterController, List<Vector2Int>>(); //<�ؼ�,�i�H������ӥؼЪ��a�I>

            for (int i = 0; i < _stepList.Count; i++) //我可以移動的範圍
            {
                rangeList = BattleController.Instance.GetRangeList(SelectedSkill.Range, _stepList[i]);
                for (int j = 0; j < targetList.Count; j++)
                {
                    targetPosition_v2 = Utility.ConvertToVector2Int(targetList[j].transform.position);
                    step_v3 = new Vector3(_stepList[i].x, BattleController.Instance.TileDic[_stepList[i]].TileData.Height, _stepList[i].y);
                    if (targetPosition_v2 != _stepList[i] && rangeList.Contains(targetPosition_v2)) //�ؼЦ�m�P�ڤ��P�B�b�g�{��
                    {
                        bool isBlock = false;
                        //檢查射擊是否會被地形阻礙
                        if (SelectedSkill.Track == TrackEnum.Straight)
                        {
                            BattleController.Instance.CheckLine(step_v3, targetList[j].transform.position, out isBlock, out Vector3 result);
                        }
                        else if (SelectedSkill.Track == TrackEnum.Parabola)
                        {
                            Utility.CheckParabola(step_v3, targetList[j].transform.position, 4, BattleController.Instance.CharacterAliveList, BattleController.Instance.TileDic, out isBlock, out List<Vector3> result); //�n�ɩߪ��u������
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
                distance = BattleController.Instance.GetDistance(start, moveToionList[i], _controller.Info.Faction);
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
                distance = BattleController.Instance.GetDistance(stepList[i], target, _controller.Info.Faction);
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

        protected BattleCharacterController GetAttackTarget(List<BattleCharacterController> list)
        {
            int damage;
            int maxDamage = -1;
            BattleCharacterController target = null;

            //�p�G����D�],�|�u�������D�]��
            for (int i = 0; i < _controller.Info.StatusList.Count; i++)
            {
                if (_controller.Info.StatusList[i] is Provocative)
                {
                    target = ((Provocative)_controller.Info.StatusList[i]).Target;
                    if (list.Contains(target))
                    {
                        return target;
                    }
                }
            }

            //�D��ॴ�X�̤j�ˮ`���ؼ�
            for (int i = 0; i < list.Count; i++)
            {
                damage = BattleController.Instance.GetDamage(SelectedSkill.Effect, _controller, list[i]);
                if (damage > maxDamage)
                {
                    maxDamage = damage;
                    target = list[i];
                }
            }

            return target;
        }

        protected BattleCharacterController GetCurekTarget(List<BattleCharacterController> list)
        {
            int hp;
            int minHp = int.MaxValue;
            BattleCharacterController target = null;

            //�D��HP�̤֪��ؼ�
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Info.CurrentHP < list[i].Info.MaxHP)
                {
                    hp = list[i].Info.CurrentHP;
                    if (hp < minHp)
                    {
                        minHp = hp;
                        target = list[i];
                    }
                }
            }

            return target;
        }
    }
}