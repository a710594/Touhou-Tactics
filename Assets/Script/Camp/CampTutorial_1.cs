using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorial_1 : CampTutorial
{
    public CampTutorial_1()
    {
        _context.AddState(new State_1(_context));
        _context.AddState(new State_2(_context));
    }

    public override void Start()
    {
        _context.SetState<State_1>();
    }

    private class State_1 : CampTutorialState
    {
        public State_1(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            CampUI campUI = GameObject.Find("CampUI").GetComponent<CampUI>();
            Vector3 offset = Vector3.zero;
            TutorialArrowUI.Open("選擇製作料理。", campUI.CookButton.transform, offset, Vector2Int.right);
        }

        public override bool CanCook()
        {
            Next();
            return true;
        }

        public override void Next()
        {
            TutorialArrowUI.Close();
            _context.SetState<State_2>();
        }
    }

    private class State_2 : CampTutorialState
    {
        public State_2(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            ConversationUI.Open(12, false, ()=> 
            {
                //EndTutorial();  
            }, null);
        }
    }
}
