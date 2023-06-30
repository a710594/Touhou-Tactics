using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AI
{
    protected AIContext _context = new AIContext();

    public virtual void Start(List<Vector2> stepList) 
    {
        ((AIState)_context.CurrentState).Start(stepList);
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
        protected List<Vector2> _stepList;
        protected Skill _selectedSkill;

        public AIState(StateContext context) : base(context)
        {
            _aiContext = (AIContext)context;
        }

        public virtual void Start(List<Vector2> stepList)
        {
            _stepList = stepList;
        }

        public virtual void OnMoveEnd() 
        {        
        }
    }
}

