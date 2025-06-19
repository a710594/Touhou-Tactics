using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class NearCombatAI : BattleAI
    {
        protected override Vector2Int GetMoveTo(List<Vector2Int> stepList, List<BattleCharacterController> targetList, Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic)
        {
            int distance;
            int minDistance = int.MaxValue;
            Vector2Int start = Utility.ConvertToVector2Int(transform.position);
            Vector2Int targetPosition;
            Vector2Int moveTo = new Vector2Int();
            BattleCharacterController provocativeTarget = _character.Info.GetProvocativeTarget();
            if (canHitDic.Count > 0 && (provocativeTarget == null || canHitDic.ContainsKey(provocativeTarget)))
            {
                _canAttack = true;
                if (provocativeTarget == null)
                {
                    _target = GetTarget(new List<BattleCharacterController>(canHitDic.Keys));
                }
                else
                {
                    _target = provocativeTarget;
                }
                for (int i = 0; i < canHitDic[_target].Count; i++)
                {
                    distance = BattleController.Instance.GetDistance(start, canHitDic[_target][i], _character.Info.Faction); //�ɶq����ؼ�
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        moveTo = canHitDic[_target][i];
                    }
                }

                return moveTo;
            }
            else
            {
                _canAttack = false;
                if (provocativeTarget == null)
                {
                    _target = GetTarget(new List<BattleCharacterController>(targetList));
                }
                else
                {
                    _target = provocativeTarget;
                }
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

        //��̪�
        protected override BattleCharacterController GetTarget(List<BattleCharacterController> list)
        {
            int distance;
            int minDistance = int.MaxValue;
            BattleCharacterController target = null;

            for (int i = 0; i < list.Count; i++)
            {
                distance = BattleController.Instance.GetDistance(Utility.ConvertToVector2Int(_character.transform.position), Utility.ConvertToVector2Int(list[i].transform.position), _character.Info.Faction);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    target = list[i];
                }
            }

            return target;
        }
    }
}