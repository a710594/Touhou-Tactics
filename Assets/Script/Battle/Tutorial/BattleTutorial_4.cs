using Battle;
using System.Collections;
using System.Collections.Generic;

namespace Battle
{

    public class BattleTutorial_4 : BattleTutorial
    {
        public BattleTutorial_4() 
        { 
        
        }

        public override void Start()
        {
            List<CharacterInfo> list = new List<CharacterInfo>();
            list.Add(CharacterManager.Instance.Info.CharacterList[0]);
            list.Add(CharacterManager.Instance.Info.CharacterList[3]);
            BattleController.Instance.SetCandidateList(list);
            BattleController.Instance.SetState<BattleController.PrepareState>();
            _context.SetState<State_1>();
        }

        private class State_1 : TutorialState
        {
            private Timer _timer = new Timer();
            public State_1(StateContext context) : base(context)
            {
            }
        }
    }
}