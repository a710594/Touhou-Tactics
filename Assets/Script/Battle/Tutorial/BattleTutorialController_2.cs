using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleTutorialController_2 : BattleTutorialController
    {
        public BattleTutorialController_2()
        {
            _context.AddState(new State_1(_context));
            _context.AddState(new State_2(_context));
            _context.AddState(new State_3(_context));
            _context.AddState(new State_4(_context));
            _context.AddState(new State_5(_context));

            BattleController.Instance.ActionStateBeginHandler += Start;
        }

        public override List<CharacterInfo> GetCharacterList()
        {
            List<CharacterInfo> list = new List<CharacterInfo>();
            list.Add(CharacterManager.Instance.Info.CharacterList[0]);
            list.Add(CharacterManager.Instance.Info.CharacterList[3]);

            return list;
        }

        public override void Start() 
        {
            BattleController.Instance.ActionStateBeginHandler -= Start;
            _context.SetState<State_1>();
        }

        private class State_1 : TutorialState
        {
            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                BattleUI.Instance.SetArrowVisible(false);
                TutorialUI.Open("支援：\n強化自身，不消耗行動次數，一個回合只能使用一次。", ()=> 
                {
                    Vector3 offset = new Vector3(-200, 0, 0);
                    TutorialArrowUI.Open("選擇支援。", BattleUI.Instance.ActionButtonGroup.SupportButton.transform, offset, Vector2Int.right, null);
                });
            }

            public override bool CanSupport()
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

        //金剛結界
        private class State_2 : TutorialState
        {
            public State_2(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 offset = new Vector3(-200, 160, 0);
                TutorialArrowUI.Open("選擇金剛結界。", BattleUI.Instance.ActionButtonGroup.ScrollView.Background.transform, offset, Vector2Int.right, null);
            }

            public override bool CheckScrollItem(object obj)
            {
                if(obj is Support &&((Support)obj).ID == 3) 
                {
                    Next();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Next()
            {
                TutorialArrowUI.Close();
                _context.SetState<State_3>();
            }
        }

        //選擇目標
        private class State_3 : TutorialState
        {
            public State_3(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                TutorialArrowUI.Open("選擇目標。", new Vector3(1, 2, 1), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(1, 1))
                {
                    Next();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Next()
            {
                TutorialArrowUI.Close();
                _context.SetState<State_4>();
            }
        }

        //確認目標
        private class State_4 : TutorialState
        {
            public State_4(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                TutorialArrowUI.Open("再次點選同樣的位置確認。", new Vector3(1, 2, 1), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(1, 1))
                {
                    TutorialArrowUI.Close();
                    Next();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Next()
            {
               _context.SetState<State_5>();
            }
        }

        private class State_5 : TutorialState
        {
            public State_5(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                TutorialUI.Open("旁邊的灰色區塊代表受到支援影響的區域，站在該區域的友方也能得到防禦力上升的效果。", ()=>
                {
                    TutorialUI.Open("關於行動值，支援不消耗行動值。剩餘兩個行動值，要用來使用技能或移動都可以，順序隨意。", ()=>
                    {
                        TutorialUI.Open("另外要注意的是，支援有冷卻時間，也就是數回合才能再次使用。", ()=>
                        {
                            BattleController.Instance.EndTutorial();
                            BattleUI.Instance.SetArrowVisible(true);
                        });
                    });
                });
            }
        }
    }
}