using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /*public class Tutorial_2 : BattleTutorial
    {
        public Tutorial_2()
        {
            BattleController.Instance.PrepareStateBeginHandler += Step_1;
        }

        private void Step_1()
        {
            BattleController.Instance.PrepareStateBeginHandler -= Step_1;
            TutorialUI.Open(7, null);
            IsActive = false;
            BattleController.Instance.CommandStateBeginHandler += Step_2;
        }

        private void Step_2() 
        {
            if(BattleController.Instance.SelectedCharacter.Info is BattlePlayerInfo && ((BattlePlayerInfo)BattleController.Instance.SelectedCharacter.Info).Job.ID == 7) 
            {
                TutorialUI.Open(8, ()=> 
                {
                    BattleController.Instance.CommandStateBeginHandler -= Step_2;
                    BattleController.Instance.EndTutorial();
                });
            }
        }
    }*/
}