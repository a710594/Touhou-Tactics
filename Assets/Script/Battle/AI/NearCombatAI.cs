using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class NearCombatAI : BattleAI
    {
        private bool _canAttack = false;
        private BattleCharacterController _target;

        public override void Begin() 
        {
            List<Vector2Int> stepList = BattleController.Instance.GetStepList(_character);
            List<BattleCharacterController> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
            Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic = GetCanHitDic(SelectedSkill, stepList, targetList);
            Vector2Int moveTo = GetMoveTo(stepList, targetList, canHitDic);

            BattleController.Instance.SetState<BattleController.MoveState>();
            BattleController.Instance.Move(moveTo, ()=> 
            {
                if (_canAttack) 
                {
                    Attack();
                }
                else
                {
                    SetDirection();
                }
            });

        }

        private void Attack() 
        {
            BattleController.Instance.SetState<BattleController.CommandState>();
            BattleController.Instance.SetSelectedCommand(SelectedSkill);
            List<Vector2Int> areaList = BattleController.Instance.GetAreaList(Utility.ConvertToVector2Int(_character.transform.position), Utility.ConvertToVector2Int(_target.transform.position), SelectedSkill);
            BattleController.Instance.SetTargetList(areaList);
            BattleController.Instance.SetState<BattleController.SkillState>();
            BattleController.Instance.CommandStateBeginHandler += SetDirection;
        }

        private void SetDirection() 
        {
            BattleController.Instance.CommandStateBeginHandler -= SetDirection;
            BattleController.Instance.SetState<BattleController.DirectionState>();
            Vector3 v3 = _target.transform.position - transform.position;
            Vector2Int v2;
            if(Mathf.Abs(v3.x) > Mathf.Abs(v3.z)) 
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

        protected override Vector2Int GetMoveTo(List<Vector2Int> stepList, List<BattleCharacterController> targetList, Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic)
        {
            int distance;
            int minDistance = int.MaxValue;
            Vector2Int start = Utility.ConvertToVector2Int(transform.position);
            Vector2Int targetPosition;
            Vector2Int moveTo = new Vector2Int();
            if(canHitDic.Count> 0) 
            {
                _canAttack = true;
                _target = GetTarget(new List<BattleCharacterController>(canHitDic.Keys));
                for (int i = 0; i < canHitDic[_target].Count; i++)
                {
                    distance = BattleController.Instance.GetDistance(start, canHitDic[_target][i], _character.Info.Faction);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        moveTo = canHitDic[_target][i];
                    }
                }
            }
            else
            {
                _canAttack = false;
                _target = GetTarget(targetList);
                targetPosition = Utility.ConvertToVector2Int(_target.transform.position);
                for (int i = 0; i < stepList.Count; i++)
                {
                    distance = BattleController.Instance.GetDistance(stepList[i], targetPosition, _character.Info.Faction);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        moveTo = stepList[i];
                    }
                }
            }
            return moveTo;
        }

        protected override BattleCharacterController GetTarget(List<BattleCharacterController> list)
        {
            int damage;
            int maxDamage = -1;
            BattleCharacterController target = _character.Info.GetProvocativeTarget();

            if (target == null) 
            {
                for (int i = 0; i < list.Count; i++)
                {
                    damage = BattleController.Instance.GetDamage(SelectedSkill.Effect, _character, list[i]);
                    if (damage > maxDamage)
                    {
                        maxDamage = damage;
                        target = list[i];
                    }
                }
            }

            return target;
        }
    }
}
