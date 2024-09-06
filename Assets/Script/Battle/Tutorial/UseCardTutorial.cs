using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{
    public class UseCardTutorial : BattleTutorial
    {
        public UseCardTutorial()
        {
            _context.AddState(new State_1(_context));
        }

        public override void Start()
        {
            BattleController.Instance.SetState<BattleController.PrepareState>();
            BattleController.Instance.CharacterStateBeginHandler += SetState;
        }

        private void SetState() 
        {
            _context.SetState<State_1>();
            BattleController.Instance.CharacterStateBeginHandler -= SetState;
        }

        private class State_1 : TutorialState
        {
            private Timer _timer = new Timer();

            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _timer.Start(0.5f, () =>
                {
                    ConversationUI.Open(5, false, () =>
                    {
                        BattleController.Instance.EndTutorial();
                    }, null);
                });
            }
        }

    }
}