using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleTutorial : Tutorial
    {
        public bool IsActive = true;
        public bool CanMove = false;
        public Vector2Int? MovePosition = null;
        public int? SkillID = null;
        public int? SubID = null;
        public int? ItemID = null;
        public int? SpellID = null;
        public Vector2Int? CommandPosition = null;
        public bool CanFinish = false;
        public Vector2Int? Direction = null;
        public bool Hit = false;
        public bool Critical = false;

        protected ConversationUI _conversationUI;
        protected TutorialArrowUI _tutorialArrowUI;

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

        //public virtual bool CheckScrollItem(object obj) 
        //{
        //    return ((TutorialState)_context.CurrentState).CheckScrollItem(obj);
        //}

        //public virtual bool CheckMove() 
        //{
        //    return ((TutorialState)_context.CurrentState).CanMove();
        //}

        //public virtual bool CheckSkill()
        //{
        //    return ((TutorialState)_context.CurrentState).CanSkill();
        //}

        //public virtual bool CheckSupport()
        //{
        //    return ((TutorialState)_context.CurrentState).CanSupport();
        //}

        //public virtual bool CheckSpell()
        //{
        //    return ((TutorialState)_context.CurrentState).CanSpell();
        //}

        //public virtual bool CheckItem()
        //{
        //    return ((TutorialState)_context.CurrentState).CanItem();
        //}

        //public virtual bool CheckIdle()
        //{
        //    return ((TutorialState)_context.CurrentState).CanIdle();
        //}

        //public virtual bool CheckReset()
        //{
        //    return ((TutorialState)_context.CurrentState).CanReset();
        //}

        protected class TutorialState : State
        {
            protected bool _clickable = true;

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
        }
    }
}