using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Tutorial_1 : BattleTutorial
    {
        private Timer _timer = new Timer();

        public Tutorial_1() 
        {
            BattleController.Instance.PrepareStateBeginHandler += Step_1;
        }

        private void Step_1()
        {
            BattleController.Instance.PrepareStateBeginHandler -= Step_1;
            _timer.Start(0.5f, () =>
            {
                TutorialUI.Open(1, null); //將角色從下方拖曳至場景的白色區域中。\n白色的區域代表可放置角色的位置。\n將角色配置完後按開始戰鬥。
            });
            BattleController.Instance.PlaceCharacterHandler += Step_2;
        }

        private void Step_2() 
        {
            BattleController.Instance.PlaceCharacterHandler -= Step_2;
            BattleController.Instance.PrepareStateStopDrag();
            TutorialArrowUI.Open("", BattleController.Instance.SelectCharacterUI.ConfirmButton.transform, new Vector3(-150, 0, 0), Vector2Int.right);
            BattleController.Instance.CharacterStateBeginHandler += Step_3;
        }

        private void Step_3() 
        {
            BattleController.Instance.CharacterStateBeginHandler -= Step_3;
            TutorialArrowUI.Close();
            BattleController.Instance.CommandStateBeginHandler += Step_4;
        }

        private void Step_4()
        {
            CanMove = true;
            BattleController.Instance.CommandStateBeginHandler -= Step_4;
            TutorialUI.Open(2, () => //選擇移動後，白色的格子代表可移動的範圍
            {
                BattleController.Instance.BattleUI.HideArrow();
                TutorialArrowUI.Open("選擇移動。", BattleUI.Instance.CommandGroup.MoveButton.transform, new Vector3(-200, 0, 0), Vector2Int.right);
                BattleUI.Instance.CommandGroup.MainButton.Lock = true;
                BattleUI.Instance.CommandGroup.SubButton.Lock = true;
                BattleUI.Instance.CommandGroup.ItemButton.Lock = true;
                BattleController.Instance.MoveStateBeginHandler += Step_5;
            });
        }

        private void Step_5() 
        {
            CanMove = false;
            MovePosition = new Vector2Int(4, 3);
            BattleController.Instance.MoveStateBeginHandler -= Step_5;
            TutorialArrowUI.Close();
            TutorialArrowUI.Open("選擇移動。", new Vector3(4, 1, 3), Vector2Int.down);
            BattleController.Instance.CommandStateBeginHandler += Step_6;
        }

        private void Step_6() 
        {
            MovePosition = null;
            SubID = 2;
            BattleController.Instance.CommandStateBeginHandler -= Step_6;
            TutorialArrowUI.Close();
            //次要動作大多是強化自身或同伴的輔助性動作\n主要動作則是攻擊對手或治療同伴
            //使用次要動作中的全力一擊。
            TutorialUI.Open(3, () => 
            {
                BattleController.Instance.BattleUI.CommandGroup.SetTutorial(typeof(Sub), SubID);
                BattleUI.Instance.CommandGroup.SubButton.Lock = false;
                BattleController.Instance.RangeStateBeginHandler += Step_7;
            });
        }

        private void Step_7() 
        {
            SubID = null;
            CommandPosition = new Vector2Int(4, 3);
            BattleController.Instance.RangeStateBeginHandler -= Step_7;
            BattleUI.Instance.CommandGroup.SubButton.Lock = true;
            TutorialArrowUI.Close();
            TutorialArrowUI.Open("選擇目標。", new Vector3(4, 2, 3), Vector2Int.down);
            BattleController.Instance.CommandStateBeginHandler += Step_8;
        }

        private void Step_8() 
        {
            CommandPosition = null;
            //CanFinish = true;
            SkillID = 1;
            BattleController.Instance.CommandStateBeginHandler -= Step_8;
            TutorialArrowUI.Close();
            TutorialUI.Open(4, () => //妖夢的攻擊力提升了！ \n使用主要動作中的攻擊，白色的格子代表可攻擊的範圍
            {
                BattleController.Instance.BattleUI.CommandGroup.SetTutorial(typeof(Skill), SkillID);
                BattleUI.Instance.CommandGroup.MainButton.Lock = false;
                BattleController.Instance.RangeStateBeginHandler += Step_9;
                //TutorialArrowUI.Open("選擇結束。", BattleUI.Instance.CommandGroup.FinishButton.transform, new Vector3(-200, 0, 0), Vector2Int.right);
                //BattleUI.Instance.CommandGroup.MainButton.Lock = true;
                //BattleController.Instance.DirectionStateBeginHandler += Step_9;
            });
        }

        private void Step_9()
        {
            SkillID = null;
            CommandPosition = new Vector2Int(4, 4);
            BattleController.Instance.RangeStateBeginHandler -= Step_9;
            BattleUI.Instance.CommandGroup.MainButton.Lock = true;
            TutorialArrowUI.Close();
            TutorialArrowUI.Open("選擇目標。", new Vector3(4, 2, 4), Vector2Int.down);
            BattleController.Instance.CommandStateBeginHandler += Step_10;
        }

        private void Step_10()
        {
            CommandPosition = null;
            CanFinish = true;
            BattleController.Instance.CommandStateBeginHandler -= Step_10;
            TutorialArrowUI.Close();
            TutorialUI.Open(5, () => //如果沒有要做的事就按結束吧。回合結束後需要選擇角色面對的方向。 角色面對的方向會影響命中率。 比方說如果攻擊敵人的正面，命中率會比較低。 反之從背後偷襲，命中率就會變高。 盡量面向敵人，避免被偷襲吧。
            {
                TutorialArrowUI.Open("選擇結束。", BattleUI.Instance.CommandGroup.FinishButton.transform, new Vector3(-200, 0, 0), Vector2Int.right);
                BattleController.Instance.DirectionStateBeginHandler += Step_11;
            });
        }
        private void Step_11() 
        {
            CanFinish = false;
            Direction = Vector2Int.up;
            BattleController.Instance.DirectionStateBeginHandler -= Step_11;
            TutorialArrowUI.Close();
            TutorialArrowUI.Open("選擇方向。", new Vector3(4, 1, 4), Vector2Int.down);
            BattleController.Instance.CharacterStateBeginHandler += Step_12;
        }

        private void Step_12() 
        {
            Direction = null;
            IsActive = false;
            TutorialArrowUI.Close();

            if (BattleController.Instance.SelectedCharacter.Info.Faction == BattleCharacterInfo.FactionEnum.Player) 
            {
                IsActive = true;
                BattleController.Instance.CharacterStateBeginHandler -= Step_12;
                BattleController.Instance.CommandStateBeginHandler += Step_13;
            }
        }

        private void Step_13() 
        {
            CanMove = true;
            BattleController.Instance.CommandStateBeginHandler -= Step_13;
            TutorialUI.Open(6, () => //選擇移動後，白色的格子代表可移動的範圍
            {
                BattleController.Instance.BattleUI.HideArrow();
                TutorialArrowUI.Open("選擇移動。", BattleUI.Instance.CommandGroup.MoveButton.transform, new Vector3(-200, 0, 0), Vector2Int.right);

                BattleController.Instance.MoveStateBeginHandler += Step_14;
            });

        }

        private void Step_14()
        {
            CanMove = false;
            MovePosition = new Vector2Int(6, 3);
            BattleController.Instance.MoveStateBeginHandler -= Step_14;
            TutorialArrowUI.Close();
            TutorialArrowUI.Open("選擇移動。", new Vector3(6, 1, 3), Vector2Int.down);
            BattleController.Instance.CommandStateBeginHandler += Step_15;
        }

        private void Step_15()
        {
            MovePosition = null;
            SkillID = 1;
            BattleController.Instance.CommandStateBeginHandler -= Step_15;
            TutorialArrowUI.Close();
            //次要動作大多是強化自身或同伴的輔助性動作\n主要動作則是攻擊對手或治療同伴
            //使用次要動作中的全力一擊。
            //TutorialUI.Open(3, () =>
            //{
                BattleController.Instance.BattleUI.CommandGroup.SetTutorial(typeof(Skill), SkillID);
                BattleUI.Instance.CommandGroup.MainButton.Lock = false;
                BattleController.Instance.RangeStateBeginHandler += Step_16;
            //});
        }

        private void Step_16()
        {
            SkillID = null;
            CommandPosition = new Vector2Int(5, 3);
            Critical = true;
            BattleController.Instance.RangeStateBeginHandler -= Step_16;
            BattleUI.Instance.CommandGroup.SubButton.Lock = true;
            TutorialArrowUI.Close();
            TutorialArrowUI.Open("選擇目標。", new Vector3(5, 2, 3), Vector2Int.down);
            BattleController.Instance.WinStateBeginHandler += Step_17;
        }

        private void Step_17()
        {
            CommandPosition = null;
            Critical = false;
            BattleController.Instance.WinStateBeginHandler -= Step_17;
            TutorialArrowUI.Close();
            BattleController.Instance.EndTutorial();
        }
    }
}