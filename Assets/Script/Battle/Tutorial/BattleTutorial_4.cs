using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleTutorial_4 : BattleTutorial
    {
        public BattleTutorial_4()
        {
            IsActive = false;
            _context.Parent = this;
            _context.AddState(new State_1(_context));
        }

        public override void Start()
        {
            BattleController.Instance.SetState<BattleController.PrepareState>();
            BattleController.Instance.ChangeStateHandler += CheckState;

            Explore.ExploreManager.Instance.ReloadHandler += () => 
            {
                //Event_8 event_8 = new Event_8();
                //event_8.Start();
                Explore.ExploreManager.Instance.ReloadHandler = null;
            };
        }

        public override void CheckState(State state)
        {
            if(state is BattleController.CommandState && BattleController.Instance.SelectedCharacter.JobId == 2) 
            {
                IsActive = true;
                BattleController.Instance.ChangeStateHandler -= CheckState;
                _context.SetState<State_1>();
            }
        }

        private class State_1 : TutorialState
        {
            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                //if (BattleController.Instance.SelectedCharacter.JobId == 2)
                //{
                    //BattleController.Instance.CommandStateBeginHandler = null;
                    //BattleUI.Instance.SetArrowVisible(false);
                    ((BattleTutorial)_context.Parent).ConversationUI = ConversationUI.Open(10, false, null, () =>
                    {
                        BattleController.Instance.EndTutorial();
                    });
                //}
            }

            public override bool CanMove()
            {
                return true;
            }
        }
    }
}