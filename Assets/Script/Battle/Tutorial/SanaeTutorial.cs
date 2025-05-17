using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SanaeTutorial : BattleTutorial
    {
        public SanaeTutorial()
        {
            IsActive = false;
            _context.Parent = this;
            _context.AddState(new State_1(_context));
        }

        public override void Start()
        {
            BattleController.Instance.SetState<BattleController.PrepareState>();
            BattleController.Instance.CharacterStateBeginHandler += CheckCharacter;
        }

        public override void Deregister()
        {
            BattleController.Instance.CharacterStateBeginHandler -= CheckCharacter;
        }

        public void CheckCharacter()
        {
            if(BattleController.Instance.SelectedCharacter.Info is BattlePlayerInfo && ((BattlePlayerInfo)BattleController.Instance.SelectedCharacter.Info).Job.ID == 7) 
            {
                IsActive = true;
                BattleController.Instance.CharacterStateBeginHandler -= CheckCharacter;
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
                ((SanaeTutorial)_context.Parent)._conversationUI = ConversationUI.Open(10, false, () =>
                {
                    BattleController.Instance.EndTutorial();
                });
            }
        }
    }
}