using Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MashroomAI : AI
{

    public MashroomAI(BattleCharacterInfo characterInfo) 
    {
        _context.SetInfo(characterInfo);
        _context.AddState(new NormalState(_context));
        _context.SetState<NormalState>();
    }

    private class NormalState : AIState 
    {
        private bool _inRange;
        private int _skillId = 1;
        private Vector2Int _moveTo;
        private BattleCharacterInfo _target = null;
        private List<Vector2Int> _allRangeList = new List<Vector2Int>();
        private Dictionary<BattleCharacterInfo, List<Vector2Int>> _targetDic = new Dictionary<BattleCharacterInfo, List<Vector2Int>>();

        public NormalState(StateContext context) : base(context)
        {
        }

        public override void Start()
        {
            base.Start();

            _target = null;
            _allRangeList.Clear();
            _targetDic.Clear();
            _selectedSkill = new Skill(DataContext.Instance.SkillDic[_skillId]);

            Vector2Int v2;
            BattleInfo battleInfo = BattleController.Instance.Info;
            List<Vector2Int> rangeList = new List<Vector2Int>();
            List<BattleCharacterInfo> characterList = new List<BattleCharacterInfo>(BattleController.Instance.CharacterList);
            for (int i=0; i<characterList.Count; i++) 
            {
                if(characterList[i].Faction == BattleCharacterInfo.FactionEnum.Enemy) 
                {
                    characterList.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < _stepList.Count; i++)
            {
                rangeList = Utility.GetRange(_selectedSkill.Effect.Range, battleInfo.Width, battleInfo.Height, _stepList[i]);
                for (int j=0; j<characterList.Count; j++) 
                {
                    v2 = Utility.ConvertToVector2Int(characterList[j].Position);
                    if (v2 != _stepList[i] && rangeList.Contains(v2))
                    {
                        if (!_targetDic.ContainsKey(characterList[j]))
                        {
                            _targetDic.Add(characterList[j], new List<Vector2Int>());
                        }
                        if (!_targetDic[characterList[j]].Contains(_stepList[i]))
                        {
                            _targetDic[characterList[j]].Add(_stepList[i]);
                        }
                    }
                }
            }

            int damage;
            int maxDamage = -1;
            int distance;
            int maxDistance = -1;
            int minDistance = int.MaxValue;
            if (_targetDic.Count > 0)
            {
                _target = GetTarget(new List<BattleCharacterInfo>(_targetDic.Keys));

                List<Vector2Int> list = _targetDic[_target];
                Vector2Int start = Utility.ConvertToVector2Int(_info.Position);
                for (int i = 0; i < list.Count; i++)
                {
                    distance = BattleController.Instance.GetDistance(start, list[i], _info.Faction);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        _moveTo = list[i];
                    }
                }
                _inRange = true;
            }
            else
            {
                _target = GetTarget(characterList);

                v2 = Utility.ConvertToVector2Int(_target.Position);
                for (int i=0; i<_stepList.Count; i++) 
                {
                    distance = BattleController.Instance.GetDistance(_stepList[i], v2, _info.Faction);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _moveTo = _stepList[i];
                    }
                }
            }
            Debug.Log(_moveTo);
            BattleController.Instance.SetMoveState();
            BattleController.Instance.Click(_moveTo);
            BattleController.Instance.Click(_moveTo);
        }

        private BattleCharacterInfo GetTarget(List<BattleCharacterInfo> list)
        {
            int damage;
            int maxDamage = -1;
            BattleCharacterInfo target = null;

            //如果受到挑釁,會優先攻擊挑釁者
            for (int i = 0; i < _info.StatusList.Count; i++)
            {
                if (_info.StatusList[i] is ProvocativeStatus)
                {
                    target = ((ProvocativeStatus)_aiContext.CharacterInfo.StatusList[i]).Target;
                    return target;
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                damage = BattleController.Instance.GetDamage(_selectedSkill.Effect, _info, list[i]);
                if (damage > maxDamage)
                {
                    maxDamage = damage;
                    target = list[i];
                }
            }

            return target;
        }

        public override void OnMoveEnd()
        {
            if (_inRange)
            {
                BattleController.Instance.SelectObject(_selectedSkill);
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
            }
            else
            {
                BattleController.Instance.Idle();
            }
        }              
    }
}
