using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class FarCombatAI : BattleAI
    {
        protected override Vector2Int GetMoveTo(List<Vector2Int> stepList, List<BattleCharacterController> targetList, Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic)
        {
            int distance;
            int maxDistance = -1;
            int minDistance = int.MaxValue;
            Vector2Int targetPosition;
            Vector2Int moveTo = new Vector2Int();
            if (canHitDic.Count > 0)
            {
                _canAttack = true;
                _target = GetTarget(new List<BattleCharacterController>(canHitDic.Keys));
                targetPosition = Utility.ConvertToVector2Int(_target.transform.position);
                for (int i = 0; i < canHitDic[_target].Count; i++)
                {
                    distance = BattleController.Instance.GetDistance(targetPosition, canHitDic[_target][i], _character.Info.Faction); //�ɶq�����ؼ�
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
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
                    distance = BattleController.Instance.GetDistance(stepList[i], targetPosition, _character.Info.Faction); //�ɶq�a��ؼ�
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        moveTo = stepList[i];
                    }
                }
            }
            return moveTo;
        }

        //��HP�̤֪�
        protected override BattleCharacterController GetTarget(List<BattleCharacterController> list)
        {
            int minHP = int.MaxValue;
            BattleCharacterController target = _character.Info.GetProvocativeTarget();

            if (target == null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (minHP > list[i].Info.CurrentHP)
                    {
                        minHP = list[i].Info.CurrentHP;
                        target = list[i];
                    }
                }
            }

            return target;
        }
    }
}