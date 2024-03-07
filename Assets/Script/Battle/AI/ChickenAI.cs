using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class ChickenAI : BattleAI
    {
        private int _count = 0;
        private Timer _timer = new Timer();

        public override void Init(BattleCharacterInfo info)
        {
            _info = info;
        }

        public override void Start()
        {
            if (_count == 0) 
            {
                _count++;
                _info.Sprite = _info.Enemy.SpriteList[1];
                BattleController.Instance.ChangeSprite(_info.Index, _info.Sprite);
                _timer.Start(1f, ()=> 
                {
                    BattleController.Instance.Idle();
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
            if (canHitDic.Count > 0) //有可以攻擊的目標
            {
                _useSkill = true;
                _target = GetAttackTarget(new List<BattleCharacterInfo>(canHitDic.Keys));
                moveTo = GetMoveTo(MoveToEnum.Near, start, canHitDic[_target]); //選擇距離目標近的位置
            }
            else //盡量靠近想攻擊的目標
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

            BattleController.Instance.SetMoveState();
            BattleController.Instance.Click(moveTo);
            BattleController.Instance.Click(moveTo);
        }
    }
}
