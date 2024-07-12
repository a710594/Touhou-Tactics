using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleTutorial_3 : BattleTutorial
    {
        public BattleTutorial_3()
        {
            _context.AddState(new State_1(_context));
            _context.AddState(new State_2(_context));
            _context.AddState(new State_3(_context));
            _context.AddState(new State_4(_context));
            
            BattleController.Instance.ActionStateBeginHandler += Start;
        }

        public override List<CharacterInfo> GetCharacterList()
        {
            List<CharacterInfo> list = new List<CharacterInfo>();
            list.Add(CharacterManager.Instance.Info.CharacterList[0]);
            list.Add(CharacterManager.Instance.Info.CharacterList[2]);
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
                TutorialUI.Open("符卡：\n消耗符卡發動強大的效果。使用符卡後所有的我方成員都會進入冷卻時間，需要數回合後才能再次使用符卡。", ()=> 
                {
                    Vector3 offset = new Vector3(-200, -50, 0);
                    TutorialArrowUI.Open("選擇符卡。", BattleUI.Instance.ActionButtonGroup.SupportButton.transform, offset, Vector2Int.right, null);
                });
            }

            public override bool CanSpell()
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

        //夢想封印
        private class State_2 : TutorialState
        {
            public State_2(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 offset = new Vector3(-200, 160, 0);
                TutorialArrowUI.Open("選擇夢想封印。", BattleUI.Instance.ActionButtonGroup.ScrollView.Background.transform, offset, Vector2Int.right, null);
            }

            public override bool CheckScrollItem(object obj)
            {
                if(obj is Spell &&((Spell)obj).ID == 1) 
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
                TutorialArrowUI.Open("選擇目標。", new Vector3(4, 2, 4), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(4, 4))
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
                TutorialArrowUI.Open("再次點選同樣的位置確認。", new Vector3(4, 2, 4), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(4, 4))
                {
                    TutorialArrowUI.Close();
                    BattleController.Instance.EndTutorial();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}