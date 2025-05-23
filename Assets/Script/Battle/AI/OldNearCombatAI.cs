using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class OldNearCombatAI : OldBattleAI
    {
        public override void Start() 
        {
            Vector2Int start = Utility.ConvertToVector2Int(_controller.transform.position);
            _stepList = BattleController.Instance.GetStepList(_controller);
            List<BattleCharacterController> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
            Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic = GetCanHitDic(targetList);
            Vector2Int moveTo;
            if (canHitDic.Count > 0) //有可以攻擊的目標
            {
                _useSkill = true;
                _target = GetAttackTarget(new List<BattleCharacterController>(canHitDic.Keys));
                moveTo = GetMoveTo(MoveToEnum.Near, start, canHitDic[_target]); //選擇距離目標近的位置
            }
            else //盡量靠近想攻擊的目標
            {
                _useSkill = false;
                int distance;
                Vector2Int targetPosition;
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetPosition = Utility.ConvertToVector2Int(targetList[i].transform.position);
                    distance = BattleController.Instance.GetDistance(start, targetPosition, _controller.Info.Faction);
                    if(distance == -1)
                    {
                        targetList.RemoveAt(i);
                        i--;
                    }
                }
                _target = GetAttackTarget(targetList);
                moveTo = GetMoveTo(MoveToEnum.Near, _stepList, Utility.ConvertToVector2Int(_target.transform.position));
            }

            List<Vector2Int> path = BattleController.Instance.GetPath(Utility.ConvertToVector2Int(_controller.transform.position), moveTo, _controller.Info.Faction);
            _controller.LastPosition = _controller.transform.position;
            _controller.Move(path, () =>
            {
                if (_useSkill) 
                {
                    HitType hitType = BattleController.Instance.CheckHit(SelectedSkill.Hit, _controller, _target);
                    List<Log> logList = new List<Log>();
                    SelectedSkill.Effect.Use(hitType, _controller, _target, logList);
                    for (int i = 0; i < logList.Count; i++)
                    {
                        BattleController.Instance.BattleUI.PlayFloatingNumberPool(_target, logList[i]);
                    }

                    _timer.Start(1, () => {
                        ResultType result = BattleController.Instance.GetResult();
                        if (result == ResultType.Win)
                        {
                            BattleController.Instance.SetState<BattleController.WinState>();
                        }
                        else if (result == ResultType.Lose)
                        {
                            BattleController.Instance.SetState<BattleController.LoseState>();
                        }
                        else
                        {
                            BattleController.Instance.SetState<BattleController.DirectionState>();
                            BattleController.Instance.SetDirection(Utility.ConvertToVector2Int(_controller.transform.forward));
                        }
                    });
                }
                else
                {
                    BattleController.Instance.SetState<BattleController.DirectionState>();
                    BattleController.Instance.SetDirection(Utility.ConvertToVector2Int(_controller.transform.forward));
                }
            });
        }
    }
}