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
            Vector2Int start = Utility.ConvertToVector2Int(_info.Position);
            _stepList = BattleController.Instance.GetStepList(_info);
            List<BattleCharacterInfo> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
            Dictionary<BattleCharacterInfo, List<Vector2Int>> canHitDic = GetCanHitDic(targetList);
            Vector2Int moveTo;
            if (canHitDic.Count > 0) //���i�H�������ؼ�
            {
                _useSkill = true;
                _target = GetAttackTarget(new List<BattleCharacterInfo>(canHitDic.Keys));
                moveTo = GetMoveTo(MoveToEnum.Far, start, canHitDic[_target]); //��ܶZ���ؼл�����m
            }
            else //�ɶq�a��Q�������ؼ�
            {
                _useSkill = false;
                int distance;
                Vector2Int targetPosition;
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetPosition = Utility.ConvertToVector2Int(targetList[i].Position);
                    distance = BattleController.Instance.GetDistance(start, targetPosition, _info.Faction);
                    if (distance == -1)
                    {
                        targetList.RemoveAt(i);
                        i--;
                    }
                }
                _target = GetAttackTarget(targetList);
                moveTo = GetMoveTo(MoveToEnum.Near, _stepList, Utility.ConvertToVector2Int(_target.Position));
            }

            BattleController.Instance.SetState<BattleController.MoveState>();
            BattleController.Instance.Click(moveTo);
            BattleController.Instance.Click(moveTo);
        }
    }
}