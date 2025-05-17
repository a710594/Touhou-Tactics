using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SpellTutorial : BattleTutorial
    {
        public SpellTutorial()
        {
            _context.Parent = this;
            _context.ClearState();
            _context.AddState(new State_1(_context));
        }

        public override void Start()
        {
            BattleController.Instance.SetState<BattleController.PrepareState>();
            _context.SetState<State_1>();
        }

        public override void Deregister()
        {
            //((TutorialState)_context.CurrentState).Deregister();
        }

        private class State_1 : TutorialState
        {
            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                ((SpellTutorial)_context.Parent)._conversationUI = ConversationUI.Open(13, false, () =>
                {
                    BattleController.Instance.EndTutorial();
                });
            }
        }
    }
}