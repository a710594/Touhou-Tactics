using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class KutakaAI : BattleAI
    {
        private int _count = 0;

        public override void Init(BattleCharacterInfo info)
        {
            _info = info;
        }

        public override void Start()
        {
            if (_count == 1)
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
            _info.ActionCount = 1;
            SelectedSkill = _info.SkillList[0];
            BattleController.Instance.SetSelectedCommand(SelectedSkill);
            BattleController.Instance.SetState<BattleController.TargetState>();
            List<BattleCharacterInfo> targetList = GetTargetList(BattleCharacterInfo.FactionEnum.Player);
            _target = GetAttackTarget(targetList);
            BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
            MoveCamera(_target.Position, () =>
            {
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
            });
        }

        private void LayEgg() 
        {
            _info.ActionCount = 1;
            SelectedSkill = _info.SkillList[1];
            BattleController.Instance.SetSelectedCommand(SelectedSkill);
            BattleController.Instance.SetState<BattleController.TargetState>();
            BattleInfo battleInfo = BattleController.Instance.Info;
            List<Vector2Int> rangeList = Utility.GetRange(SelectedSkill.Range, Utility.ConvertToVector2Int(_info.Position), battleInfo);
            BattleController.Instance.RemoveByFaction(SelectedSkill.RangeTarget, rangeList);
            Vector2Int targetPosition = rangeList[UnityEngine.Random.Range(0, rangeList.Count)];
            BattleController.Instance.Click(targetPosition);
            Vector3 v3 = new Vector3(targetPosition.x, BattleController.Instance.Info.TileDic[targetPosition].TileData.Height, targetPosition.y);
            MoveCamera(v3, () =>
            {
                BattleController.Instance.Click(targetPosition);
            });
        }
    }
}