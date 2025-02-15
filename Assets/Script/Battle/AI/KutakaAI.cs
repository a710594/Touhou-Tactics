using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class KutakaAI : BattleAI
    {
        private int _count = 0;

        public override void Start()
        {
            if (_count == 0)
            {
                LayEgg();
            }
            else
            {
                Attack();
            }
            _count = (_count + 1) % 3;
        }

        private void Attack() 
        {
            _controller.Info.ActionCount = 1;
            SelectedSkill = _controller.Info.SkillList[0];
            BattleController.Instance.SetSelectedCommand(SelectedSkill);
            BattleController.Instance.SetState<BattleController.TargetState>();
            List<BattleCharacterController> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
            _target = GetAttackTarget(targetList);
            BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.transform.position));
            MoveCamera(_target.transform.position, () =>
            {
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.transform.position));
            });
        }

        private void LayEgg() 
        {
            _controller.Info.ActionCount = 1;
            SelectedSkill = _controller.Info.SkillList[1];
            BattleController.Instance.SetSelectedCommand(SelectedSkill);
            BattleController.Instance.SetState<BattleController.TargetState>();
            List<Vector2Int> rangeList = BattleController.Instance.GetRange(SelectedSkill.Range, Utility.ConvertToVector2Int(_controller.transform.position));
            BattleController.Instance.RemoveByFaction(SelectedSkill.RangeTarget, rangeList);
            Vector2Int targetPosition = rangeList[UnityEngine.Random.Range(0, rangeList.Count)];
            BattleController.Instance.Click(targetPosition);
            Vector3 v3 = new Vector3(targetPosition.x, BattleController.Instance.TileDic[targetPosition].TileData.Height, targetPosition.y);
            MoveCamera(v3, () =>
            {
                BattleController.Instance.Click(targetPosition);
            });
        }
    }
}