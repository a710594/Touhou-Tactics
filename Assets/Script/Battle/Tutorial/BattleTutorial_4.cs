using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public class BattleTutorial_4 : BattleTutorial
    {
        public BattleTutorial_4() 
        {
            _context.Parent = this;
            _context.AddState(new State_1(_context));
            _context.AddState(new State_2(_context));
            _context.AddState(new State_3(_context));
            _context.AddState(new State_4(_context));
            _context.AddState(new State_5(_context));
        }

        public override void Start()
        {
            List<CharacterInfo> list = new List<CharacterInfo>();
            list.Add(CharacterManager.Instance.Info.CharacterList[0]);
            list.Add(CharacterManager.Instance.Info.CharacterList[3]);
            BattleController.Instance.SetCandidateList(list);
            BattleController.Instance.SetState<BattleController.PrepareState>();

            BattleController.Instance.ActionStateBeginHandler += ()=> 
            {
                _context.SetState<State_1>();
            };
        }

        private class State_1 : TutorialState
        {
            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                BattleController.Instance.ActionStateBeginHandler = null;
                BattleUI.Instance.SetArrowVisible(false);
                ((BattleTutorial)_context.Parent).ConversationUI = ConversationUI.Open(7, null, () => 
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
                if (obj is Support && ((Support)obj).ID == 3)
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
                Vector3 v = BattleController.Instance.SelectedCharacter.Position;
                TutorialArrowUI.Open("選擇目標。", new Vector3(v.x, v.y + 1, v.z), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                Vector3 v = BattleController.Instance.SelectedCharacter.Position;
                if (position == new Vector2Int((int)v.x, (int)v.z))
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
                Vector3 v = BattleController.Instance.SelectedCharacter.Position;
                TutorialArrowUI.Open("再次點選同樣的位置確認。", new Vector3(v.x, v.y + 1, v.z), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                Vector3 v = BattleController.Instance.SelectedCharacter.Position;
                if (position == new Vector2Int((int)v.x, (int)v.z))
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
                ((BattleTutorial)_context.Parent).ConversationUI.Continue(() =>
                {
                    BattleController.Instance.EndTutorial();
                    BattleUI.Instance.SetArrowVisible(true);
                });
            }
        }
    }
}