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
            Vector2Int targetPosition;
            Vector2Int moveTo = new Vector2Int();
            if (canHitDic.Count > 0)
            {
                _canAttack = true;
                _target = GetTarget(new List<BattleCharacterController>(canHitDic.Keys));
                targetPosition = Utility.ConvertToVector2Int(_target.transform.position);
                for (int i = 0; i < canHitDic[_target].Count; i++)
                {
                    distance = BattleController.Instance.GetDistance(targetPosition, canHitDic[_target][i], _character.Info.Faction); //盡量遠離目標
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
                    distance = BattleController.Instance.GetDistance(stepList[i], targetPosition, _character.Info.Faction); //盡量靠近目標
                    if (distance < maxDistance)
                    {
                        maxDistance = distance;
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