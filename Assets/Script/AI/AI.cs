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
        protected BattleCharacterInfo _character;
        protected List<Vector2Int> _stepList;
        protected Skill _selectedSkill;

        public AIState(StateContext context) : base(context)
        {
            _aiContext = (AIContext)context;
            _character = _aiContext.CharacterInfo;
        }

        public virtual void Start()
        {
            BattleInfo info = BattleController.Instance.BattleInfo;
            List<BattleCharacterInfo> characterList = BattleController.Instance.CharacterList;
            _stepList = _stepList = AStarAlgor.Instance.GetStepList(info.Width, info.Height, Utility.ConvertToVector2Int(_character.Position), _character, characterList, info.TileInfoDic);
        }

        public virtual void OnMoveEnd() 
        {        
        }
    }
}

