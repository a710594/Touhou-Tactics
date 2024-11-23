using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class ChickenAI : BattleAI
    {
        private int _count = 0;

        public override void Start()
        {
            if (_count == 0) 
            {
                _count++;
                _info.Sprite = _info.Enemy.SpriteList[1];
                BattleController.Instance.ChangeSprite(_info, _info.Sprite);
                _timer.Start(1f, ()=> 
                {
                    BattleController.Instance.SelectedCharacter.ActionCount = 0;
                    BattleController.Instance.SetState<BattleController.EndState>();
                });
            }
            else 
            {
                NearCombat();
            }
        }

        private void NearCombat() 
        {
            Vector2Int start = Utility.ConvertToVector2Int(_info.Position);
            SelectedSkill = _info.SkillList[0];
            _stepList = BattleController.Instance.GetStepList(_info);
            List<BattleCharacterInfo> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
            Dictionary<BattleCharacterInfo, List<Vector2Int>> canHitDic = GetCanHitDic(targetList);
            Vector2Int moveTo;
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

            BattleController.Instance.SetState<BattleController.MoveState>();
            BattleController.Instance.Click(moveTo);
            BattleController.Instance.Click(moveTo);
        }
    }
}
