using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleAI : MonoBehaviour
    {
        public Skill SelectedSkill;

        protected BattleCharacterController _character;
        protected bool _canAttack = false;
        protected BattleCharacterController _target;

        public virtual void Init(BattleCharacterController character)
        {
            _character = character;
            SelectedSkill = character.Info.SkillList[0];
        }

        public virtual void Begin()
        {
            Move();
        }

        private void Move()
        {
            BattleController.Instance.ShowStepList(_character);
            List<BattleCharacterController> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
            Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic = GetCanHitDic(SelectedSkill, _character.Info.StepList, targetList);
            Vector2Int moveTo = GetMoveTo(_character.Info.StepList, targetList, canHitDic);
            BattleController.Instance.SetState<BattleController.MoveState>();
            BattleController.Instance.AfterMoveHandler += AfterMove;
            BattleController.Instance.Move(moveTo);
        }

        private void AfterMove()
        {
            BattleController.Instance.AfterMoveHandler -= AfterMove;
            if (_canAttack)
            {
                Attack();
            }
            else
            {
                SetDirection();
            }
        }

        private void Attack()
        {
            //BattleController.Instance.SetState<BattleController.CommandState>();
            BattleController.Instance.SetSelectedCommand(SelectedSkill);
            Vector2Int userPosition = Utility.ConvertToVector2Int(_character.transform.position);
            Vector2Int targetPosition = Utility.ConvertToVector2Int(_target.transform.position);
            List<Vector2Int> areaList = BattleController.Instance.GetAreaList(userPosition, targetPosition, SelectedSkill);
            BattleController.Instance.UseCommand(targetPosition, areaList);
            BattleController.Instance.AfterCheckResultHandler += SetDirection;
        }

        private void SetDirection()
        {
            BattleController.Instance.AfterCheckResultHandler -= SetDirection;
            BattleController.Instance.SetState<BattleController.DirectionState>();
            Vector3 v3 = _target.transform.position - transform.position;
            Vector2Int v2;
            if (Mathf.Abs(v3.x) > Mathf.Abs(v3.z))
            {
                if (v3.x > 0)
                {
                    v2 = Vector2Int.right;
                }
                else
                {
                    v2 = Vector2Int.left;
                }
            }
            else
            {
                if (v3.z > 0)
                {
                    v2 = Vector2Int.up;
                }
                else
                {
                    v2 = Vector2Int.down;
                }
            }
            BattleController.Instance.SetDirection(v2);
        }

        protected List<BattleCharacterController> GetTargetList(BattleCharacterInfo.FactionEnum faction)
        {
            List<BattleCharacterController> list = new List<BattleCharacterController>(BattleController.Instance.CharacterAliveList);
            list.AddRange(BattleController.Instance.CharacterDyingList);
            List<BattleCharacterController> targetList = new List<BattleCharacterController>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Info.Faction == faction)
                {
                    targetList.Add(list[i]);
                }
            }

            return targetList;
        }

        protected Dictionary<BattleCharacterController, List<Vector2Int>> GetCanHitDic(Skill skill, List<Vector2Int> stepList, List<BattleCharacterController> targetList)
        {
            Vector2Int targetPosition;
            Vector3 myPosition;
            List<Vector2Int> rangeList;
            Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic = new Dictionary<BattleCharacterController, List<Vector2Int>>(); //<目標,可以打到目標的位置>

            for (int i = 0; i < stepList.Count; i++) //我可以移動的範圍
            {
                rangeList = BattleController.Instance.GetPositionList(skill.Range, stepList[i]);
                for (int j = 0; j < targetList.Count; j++)
                {
                    targetPosition = Utility.ConvertToVector2Int(targetList[j].transform.position);
                    myPosition = new Vector3(stepList[i].x, BattleController.Instance.TileDic[stepList[i]].TileData.Height, stepList[i].y);
                    if (targetPosition != stepList[i] && rangeList.Contains(targetPosition)) //�ؼЦ�m�P�ڤ��P�B�b�g�{��
                    {
                        bool isBlock = false;
                        //檢查射擊是否會被地形阻礙
                        if (skill.Track == TrackEnum.Straight)
                        {
                            BattleController.Instance.CheckLine(myPosition, targetList[j].transform.position, out isBlock, out Vector3 result);
                        }
                        else if (skill.Track == TrackEnum.Parabola)
                        {
                            BattleController.Instance.CheckParabola(myPosition, targetList[j].transform.position, 4, out isBlock, out List<Vector3> result); //�n�ɩߪ��u������
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
                            if (!canHitDic[targetList[j]].Contains(stepList[i]))
                            {
                                canHitDic[targetList[j]].Add(stepList[i]);
                            }
                        }
                    }
                }
            }

            return canHitDic;
        }

        protected virtual Vector2Int GetMoveTo(List<Vector2Int> stepList, List<BattleCharacterController> targetList, Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic)
        {
            return Vector2Int.zero;
        }

        protected virtual BattleCharacterController GetTarget(List<BattleCharacterController> list)
        {
            return null;
        }
    }
}