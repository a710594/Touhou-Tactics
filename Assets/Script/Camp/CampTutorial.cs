using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorial : Tutorial
{
    public CampUI CampUI;

    public virtual bool CanCook()
    {
        return ((CampTutorialState)_context.CurrentState).CanCook();
    }

    public virtual bool CanShop()
    {
        return ((CampTutorialState)_context.CurrentState).CanShop();
    }

    public virtual bool CanExplore()
    {
        return ((CampTutorialState)_context.CurrentState).CanExplore();
    }

    protected class CampTutorialState : State 
    {
        public CampTutorialState(StateContext context) : base(context)
        {
        }

        public virtual void Next() { }

        public virtual bool CanCook()
        {
            return false;
        }

        public virtual bool CanShop()
        {
            return false;
        }

        public virtual bool CanExplore()
        {
            return false;
        }
    }
}
