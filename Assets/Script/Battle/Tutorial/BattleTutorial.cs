using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleTutorial : Tutorial
    {
        public ConversationUI ConversationUI;

        public virtual bool CheckClick(Vector2Int position)
        {
            if(_context.CurrentState!=null)
            {
                return ((TutorialState)_context.CurrentState).CheckClick(position);
            }
            else
            {
                return false;
            }
        }

        public virtual bool CheckScrollItem(object obj) 
        {
            return ((TutorialState)_context.CurrentState).CheckScrollItem(obj);
        }

        public virtual bool CanMove() 
        {
            return ((TutorialState)_context.CurrentState).CanMove();
        }

        public virtual bool CanSkill()
        {
            return ((TutorialState)_context.CurrentState).CanSkill();
        }

        public virtual bool CanSupport()
        {
            return ((TutorialState)_context.CurrentState).CanSupport();
        }

        public virtual bool CanSpell()
        {
            return ((TutorialState)_context.CurrentState).CanSpell();
        }

        public virtual bool CanItem()
        {
            return ((TutorialState)_context.CurrentState).CanItem();
        }

        public virtual bool CanIdle()
        {
            return ((TutorialState)_context.CurrentState).CanIdle();
        }

        public virtual bool CanReset()
        {
            return ((TutorialState)_context.CurrentState).CanReset();
        }

        public virtual void CheckState(State state)
        {

        }

        protected class TutorialState : State
        {
            public TutorialState(StateContext context) : base(context)
            {
            }

            public virtual void Next() { }

            public virtual bool CanMove() 
            {
                return false;
            }

            public virtual bool CanSkill()
            {
                return false;
            }

            public virtual bool CanSupport()
            {
                return false;
            }

            public virtual bool CanSpell()
            {
                return false;
            }

            public virtual bool CanItem()
            {
                return false;
            }

            public virtual bool CanIdle()
            {
                return false;
            }

            public virtual bool CanReset()
            {
                return false;
            }

            public virtual bool CheckClick(Vector2Int position) 
            {
                return false;
            }

            public virtual bool CheckScrollItem(object obj)
            {
                return false;
            }

            public virtual void Deregister()
            {
            }
        }
    }
}