using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorialController
{
    private static BattleTutorialController _instance;
    public static BattleTutorialController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BattleTutorialController();
            }
            return _instance;
        }
    }

    private StateContext _context = new StateContext();

    public BattleTutorialController() 
    {
        _context.ClearState();
        _context.AddState(new State_1(_context));

        _context.SetState<State_1>();
    }

    public bool CanSetMoveState() 
    {
        return ((TutorialState)_context.CurrentState).CantSetMoveState();
    }

    private class TutorialState : State
    {
        protected BattleMapInfo _info;
        protected BattleCharacterInfo _character;
        protected List<BattleCharacterInfo> _characterList;

        public TutorialState(StateContext context) : base(context)
        {
        }

        public virtual bool CantSetMoveState() 
        {
            return true;
        }
    }

    //第一步:選擇移動
    private class State_1 : TutorialState 
    {
        public State_1(StateContext context) : base(context)
        {
        }

        public override bool CantSetMoveState()
        {
            return true;
        }
    }
}
