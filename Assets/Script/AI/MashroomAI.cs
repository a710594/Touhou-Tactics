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
        private int _skillId = 9;
        private Vector2Int _moveTo;
        private BattleCharacterInfo _target = null;
        private List<Vector2Int> _rangeList = new List<Vector2Int>();

        public NormalState(StateContext context) : base(context)
        {
        }

        public override void Start()
        {
            base.Start();

            _target = null;
            _rangeList.Clear();
            _selectedSkill = new Skill(DataContext.Instance.SkillDic[_skillId]);

            GetRange();
            GetTarget(true);
            if (_target != null)
            {
                _inRange = true;
                GetMoveTo(true);
                BattleController.Instance.SetMoveState();
                BattleController.Instance.Click(_moveTo);
                BattleController.Instance.Click(_moveTo);
            }
            else 
            {
                _inRange = false;
                GetTarget(false);
                GetMoveTo(false);
                BattleController.Instance.SetMoveState();
                BattleController.Instance.Click(_moveTo);
                BattleController.Instance.Click(_moveTo);
            }
        }

        public override void OnMoveEnd()
        {
            if (_inRange)
            {
                BattleController.Instance.SetSelectSkillState();
                BattleController.Instance.SelectSkill(_selectedSkill);
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
                BattleController.Instance.Click(Utility.ConvertToVector2Int(_target.Position));
            }
            else
            {
                BattleController.Instance.Idle();
            }
        }

        private void GetRange()
        {
            Vector2Int start = Utility.ConvertToVector2Int(_aiContext.CharacterInfo.Position);
            Vector2Int position = new Vector2Int();
            List<Vector2Int> list = new List<Vector2Int>(); //先挑選出距離 <= step + range 的座標
            BattleInfo battleInfo = BattleController.Instance.BattleInfo;
            foreach (KeyValuePair<Vector2Int, TileInfo> pair in battleInfo.TileInfoDic)
            {
                position = pair.Key;
                if (Utility.ManhattanDistance(position, start) <= _aiContext.CharacterInfo.MOV + _selectedSkill.Effect.Data.Range)
                {
                    for (int j = 0; j < _stepList.Count; j++)
                    {
                        if (Utility.ManhattanDistance(position, _stepList[j]) <= _selectedSkill.Effect.Data.Range)
                        {
                            _rangeList.Add(position);
                        }
                    }
                }
            }
        }

        private void GetTarget(bool inRange) 
        {
            Vector2Int position = new Vector2Int();
            List<BattleCharacterInfo> characterList = BattleController.Instance.CharacterList;

            //如果受到挑釁,會優先攻擊挑釁者
            for (int i=0; i< _aiContext.CharacterInfo.StatusList.Count; i++) 
            {
                if(_aiContext.CharacterInfo.StatusList[i] is ProvocativeStatus) 
                {
                    _target = ((ProvocativeStatus)_aiContext.CharacterInfo.StatusList[i]).Target;
                    return;
                }
            }

            BattleCharacterInfo.FactionEnum faction;
            if(_aiContext.CharacterInfo.Faction == BattleCharacterInfo.FactionEnum.Player) 
            {
                faction = BattleCharacterInfo.FactionEnum.Enemy;
            }
            else 
            {
                faction = BattleCharacterInfo.FactionEnum.Player;
            }

            for (int i=0; i<characterList.Count; i++) 
            {
                if (characterList[i].Faction == faction) 
                {
                    position = Utility.ConvertToVector2Int(characterList[i].Position);
                    if (!inRange || _rangeList.Contains(position)) 
                    {
                        if(_target == null || characterList[i].CurrentHP < _target.CurrentHP) 
                        {
                            _target = characterList[i];
                        }
                    }
                }
            }
        }

        private void GetMoveTo(bool inRange) 
        {
            int distance;
            int minDistance = -1;
            Vector2Int myPosition = Utility.ConvertToVector2Int(_aiContext.CharacterInfo.Position);
            Vector2Int targetPosition = Utility.ConvertToVector2Int(_target.Position);
            BattleInfo battleInfo = BattleController.Instance.BattleInfo;
            List<BattleCharacterInfo> characterList = BattleController.Instance.CharacterList;

            if (inRange) //目標必需在射程之內
            {
                List<Vector2Int> rangeList;

                for (int i = 0; i < _stepList.Count; i++)
                {
                    rangeList = Utility.GetRange(_selectedSkill.Effect.Data.Range, battleInfo.Width, battleInfo.Height, _stepList[i]);
                    if (rangeList.Contains(targetPosition))
                    {
                        distance = AStarAlgor.Instance.GetDistance(myPosition, _stepList[i], _aiContext.CharacterInfo, characterList, battleInfo.TileInfoDic);
                        if (minDistance == -1 || distance < minDistance)
                        {
                            minDistance = distance;
                            _moveTo = _stepList[i];
                        }

                    }
                }
            }
            else
            {
                for (int i = 0; i < _stepList.Count; i++)
                {
                    distance = AStarAlgor.Instance.GetDistance(targetPosition, _stepList[i], _aiContext.CharacterInfo, characterList, battleInfo.TileInfoDic);
                    if (minDistance == -1 || distance < minDistance)
                    {
                        minDistance = distance;
                        _moveTo = _stepList[i];
                    }
                }
            }

            Debug.Log(_moveTo);
        }
    }
}
