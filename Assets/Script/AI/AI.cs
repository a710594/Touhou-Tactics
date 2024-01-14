using Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AI
{
    protected AIContext _context = new AIContext();

    public virtual void Start()
    {
        ((AIState)_context.CurrentState).Start();
    }

    public void OnMoveEnd()
    {
        ((AIState)_context.CurrentState).OnMoveEnd();
    }

    protected virtual void OnDeath()
    {
    }

    public class AIContext : StateContext
    {
        public BattleCharacterInfo CharacterInfo;

        public void SetInfo(BattleCharacterInfo characterInfo)
        {
            CharacterInfo = characterInfo;
        }
    }

    public class AIState : State
    {
        protected AIContext _aiContext;
        protected BattleCharacterInfo _info;
        protected List<Vector2Int> _stepList;
        protected Skill _selectedSkill;

        public AIState(StateContext context) : base(context)
        {
            _aiContext = (AIContext)context;
            _info = _aiContext.CharacterInfo;
        }

        public virtual void Start()
        {
            BattleInfo info = BattleController.Instance.Info;
            List<BattleCharacterInfo> characterList = BattleController.Instance.CharacterList;
            _stepList = BattleController.Instance.GetStepList(Utility.ConvertToVector2Int(_info.Position), _info);
        }

        public virtual void OnMoveEnd()
        {
        }
    }
}

