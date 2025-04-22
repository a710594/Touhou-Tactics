using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleAI : MonoBehaviour
    {
        public Skill SelectedSkill;

        protected BattleCharacterController _character;

        public virtual void Init(BattleCharacterController character)
        {
            _character = character;
            SelectedSkill = character.Info.SkillList[0];
        }

        public virtual void Begin()
        {
        }

        protected List<BattleCharacterController> GetTargetList(BattleCharacterInfo.FactionEnum faction)
        {
            List<BattleCharacterController> list = BattleController.Instance.GetAliveAndDyingCharacterList();
            List<BattleCharacterController> targetList = new List<BattleCharacterController>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Info.Faction == faction)
                {
                    targetList.Add(list[i]);
                }
            }

            return targetList;
        }

        protected Dictionary<BattleCharacterController, List<Vector2Int>> GetCanHitDic(Skill skill, List<Vector2Int> stepList, List<BattleCharacterController> targetList)
        {
            Vector2Int targetPosition;
            Vector3 myPosition;
            List<Vector2Int> rangeList;
            Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic = new Dictionary<BattleCharacterController, List<Vector2Int>>(); //<目標,可以打到目標的位置>

            for (int i = 0; i < stepList.Count; i++) //我可以移動的範圍
            {
                rangeList = BattleController.Instance.GetRangeList(skill.Range, stepList[i]);
                for (int j = 0; j < targetList.Count; j++)
                {
                    targetPosition = Utility.ConvertToVector2Int(targetList[j].transform.position);
                    myPosition = new Vector3(stepList[i].x, BattleController.Instance.TileDic[stepList[i]].TileData.Height, stepList[i].y);
                    if (targetPosition != stepList[i] && rangeList.Contains(targetPosition)) //�ؼЦ�m�P�ڤ��P�B�b�g�{��
                    {
                        bool isBlock = false;
                        //檢查射擊是否會被地形阻礙
                        if (skill.Track == TrackEnum.Straight)
                        {
                            Utility.CheckLine(myPosition, targetList[j].transform.position, BattleController.Instance.CharacterAliveList, BattleController.Instance.TileDic, out isBlock, out Vector3 result);
                        }
                        else if (skill.Track == TrackEnum.Parabola)
                        {
                            Utility.CheckParabola(myPosition, targetList[j].transform.position, 4, BattleController.Instance.CharacterAliveList, BattleController.Instance.TileDic, out isBlock, out List<Vector3> result); //�n�ɩߪ��u������
                        }
                        else
                        {
                            //Instance._selectedPosition = position;
                        }

                        if (!isBlock)
                        {
                            if (!canHitDic.ContainsKey(targetList[j]))
                            {
                                canHitDic.Add(targetList[j], new List<Vector2Int>());
                            }
                            if (!canHitDic[targetList[j]].Contains(stepList[i]))
                            {
                                canHitDic[targetList[j]].Add(stepList[i]);
                            }
                        }
                    }
                }
            }

            return canHitDic;
        }

        protected virtual Vector2Int GetMoveTo(List<Vector2Int> stepList, List<BattleCharacterController> targetList, Dictionary<BattleCharacterController, List<Vector2Int>> canHitDic)
        {
            return Vector2Int.zero;
        }

        protected virtual BattleCharacterController GetTarget(List<BattleCharacterController> list)
        {
            return null;
        }
    }
}