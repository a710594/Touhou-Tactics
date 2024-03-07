using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class CureAI : BattleAI
    {
        public override void Start()
        {
            Vector2Int start = Utility.ConvertToVector2Int(_info.Position);
            SelectedSkill = _info.SkillList[0];
            _stepList = BattleController.Instance.GetStepList(_info);
            List<BattleCharacterInfo> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Enemy); //�M��٦�
            Dictionary<BattleCharacterInfo, List<Vector2Int>> canHitDic = GetCanHitDic(targetList);
            Vector2Int moveTo;
            if (canHitDic.Count > 0) //�����
            {
                _target = null;
                List<BattleCharacterInfo> list = new List<BattleCharacterInfo>(canHitDic.Keys);
                list.Remove(_info); //����v¡�ۤv
                _target = GetCurekTarget(list);
            }

            if(_target != null) //���i�v�����٦�
            {
                _useSkill = true;
                moveTo = GetMoveTo(MoveToEnum.Near, start, canHitDic[_target]); //��ܶZ���ؼЪ񪺦�m
            }
            else //�_�h�N�M��i�������ĤH
            {
                SelectedSkill = _info.SkillList[1];
                targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
                canHitDic = GetCanHitDic(targetList);
                if (canHitDic.Count > 0) //���i�H�������ؼ�
                {
                    _useSkill = true;
                    _target = GetAttackTarget(new List<BattleCharacterInfo>(canHitDic.Keys));
                    moveTo = GetMoveTo(MoveToEnum.Near, start, canHitDic[_target]); //��ܶZ���ؼЪ񪺦�m
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
            }

            BattleController.Instance.SetMoveState();
            BattleController.Instance.Click(moveTo);
            BattleController.Instance.Click(moveTo);
        }
    }
}