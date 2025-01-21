using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class FarCombatAI : BattleAI
    {
        public override void Start()
        {
            Vector2Int start = Utility.ConvertToVector2Int(_controller.transform.position);
            _stepList = BattleController.Instance.GetStepList(_controller);
            List<BattleCharacterController> targetList = GetTargetList(BattleCharacterControllerData.FactionEnum.Player);
            Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic = GetCanHitDic(targetList);
            Vector2Int moveTo;
            if (canHitDic.Count > 0) //���i�H�������ؼ�
            {
                _useSkill = true;
                _target = GetAttackTarget(new List<BattleCharacterController>(canHitDic.Keys));
                moveTo = GetMoveTo(MoveToEnum.Far, start, canHitDic[_target]); //��ܶZ���ؼл�����m
            }
            else //�ɶq�a��Q�������ؼ�
            {
                _useSkill = false;
                int distance;
                Vector2Int targetPosition;
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetPosition = Utility.ConvertToVector2Int(targetList[i].transform.position);
                    distance = BattleController.Instance.GetDistance(start, targetPosition, _controller.Info.Faction);
                    if (distance == -1)
                    {
                        targetList.RemoveAt(i);
                        i--;
                    }
                }
                _target = GetAttackTarget(targetList);
                moveTo = GetMoveTo(MoveToEnum.Near, _stepList, Utility.ConvertToVector2Int(_target.transform.position));
            }

            BattleController.Instance.SetState<BattleController.MoveState>();
            BattleController.Instance.Click(moveTo);
            BattleController.Instance.Click(moveTo);
        }
    }
}