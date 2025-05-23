using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*namespace Battle
{
    public class BattleTutorial_1 : BattleTutorial
    {

        public BattleTutorial_1()
        {
            _context.Parent = this;
            _context.ClearState();
            _context.AddState(new State_1(_context));
            _context.AddState(new State_2(_context));
            _context.AddState(new State_3(_context));
            _context.AddState(new State_4(_context));
            _context.AddState(new State_5(_context));
            _context.AddState(new State_6(_context));
            _context.AddState(new State_7(_context));
            _context.AddState(new State_8(_context));
            _context.AddState(new State_9(_context));
            _context.AddState(new State_10(_context));
            _context.AddState(new State_11(_context));
            _context.AddState(new State_12(_context));
            _context.AddState(new State_13(_context));
            _context.AddState(new State_14(_context));
            _context.AddState(new State_15(_context));
            _context.AddState(new State_16(_context));
            _context.AddState(new State_17(_context));
            _context.AddState(new State_18(_context));
        }

        public override void Start()
        {
            BattleController.Instance.SetState<BattleController.PrepareState>();
            _context.SetState<State_1>();
        }

        public override void Deregister()
        {
            ((TutorialState)_context.CurrentState).Deregister();
        }

        private class State_1 : TutorialState
        {
            private Timer _timer = new Timer();
            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _timer.Start(0.5f, () =>
                {
                    ((BattleTutorial_1)_context.Parent)._conversationUI = ConversationUI.Open(3, false, null, null);
                    //TutorialUI.Open("將角色從下方拖曳至場景的白色區域中。\n白色的區域代表可放置角色的位置。\n將角色配置完後按開始戰鬥。", "Tutorial_1", null);
                });
                BattleController.Instance.CommandStateBeginHandler += Next;
            }

            public override void Next()
            {
                BattleController.Instance.CommandStateBeginHandler -= Next;
                _context.SetState<State_2>();
            }

            public override void Deregister()
            {
                BattleController.Instance.CommandStateBeginHandler -= Next;
            }
        }

        //第一步:選擇移動
        private class State_2 : TutorialState
        {
            public State_2(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 offset = new Vector3(-200, 0, 0);
                TutorialArrowUI.Open("選擇移動。", BattleUI.Instance.ActionButtonGroup.MoveButton.transform, offset, Vector2Int.right);
            }

            public override void End()
            {
                TutorialArrowUI.Close();
            }

            public override bool CanMove()
            {
                Next();
                return true;
            }

            public override void Next()
            {
                _context.SetState<State_3>();
            }
        }

        //指定位置
        private class State_3 : TutorialState
        {
            public State_3(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                ((BattleTutorial_1)_context.Parent)._conversationUI.Continue(()=> 
                {
                    TutorialArrowUI.Open("選擇移動。", new Vector3(4, 1, 3), Vector2Int.down);
                });
            }

            public override void End()
            {
                TutorialArrowUI.Close();
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(4, 3))
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
                _context.SetState<State_4>();
            }
        }

        //確認位置
        private class State_4 : TutorialState
        {
            public State_4(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _clickable = true;
                TutorialArrowUI.Open("再次點選同樣的位置代表確定位置。", new Vector3(4, 1, 3), Vector2Int.down);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (_clickable && position == new Vector2Int(4, 3))
                {
                    _clickable = false;
                    TutorialArrowUI.Close();
                    BattleController.Instance.MoveStateEndHandler += Next;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Next()
            {
                BattleController.Instance.MoveStateEndHandler -= Next;
                _context.SetState<State_5>();
            }

            public override void Deregister()
            {
                BattleController.Instance.MoveStateEndHandler -= Next;
            }
        }

        //選擇技能
        private class State_5 : TutorialState
        {
            public State_5(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 offset = new Vector3(-200, 0, 0);
                TutorialArrowUI.Open("選擇技能。", BattleUI.Instance.ActionButtonGroup.SkillButton.transform, offset, Vector2Int.right);
            }

            public override void End()
            {
                TutorialArrowUI.Close();
            }

            public override bool CanSkill()
            {
                Next();
                return true;
            }

            public override void Next()
            {
                _context.SetState<State_6>();
            }
        }

        //攻擊
        private class State_6 : TutorialState
        {
            public State_6(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 offset = new Vector3(-200, 170, 0);
                TutorialArrowUI.Open("選擇攻擊。", BattleUI.Instance.ActionButtonGroup.ScrollView.Background.transform, offset, Vector2Int.right);
            }

            public override void End()
            {
                TutorialArrowUI.Close();
            }

            public override bool CheckScrollItem(object obj)
            {
                if (obj is Skill && ((Skill)obj).ID == 1)
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
                _context.SetState<State_7>();
            }
        }

        //選擇攻擊位置
        private class State_7 : TutorialState
        {
            public State_7(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                ((BattleTutorial_1)_context.Parent)._conversationUI.Continue(() =>
                {
                    TutorialArrowUI.Open("選擇目標。", new Vector3(4, 2, 4), Vector2Int.down);
                });
            }

            public override void End()
            {
                TutorialArrowUI.Close();
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
                _context.SetState<State_8>();
            }
        }

        //確認攻擊位置
        private class State_8 : TutorialState
        {
            public State_8(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _clickable = true;
                TutorialArrowUI.Open("再次點選同樣的位置確認。", new Vector3(4, 2, 4), Vector2Int.down);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (_clickable && position == new Vector2Int(4, 4))
                {
                    _clickable = false;
                    TutorialArrowUI.Close();
                    BattleController.Instance.DirectionStateBeginHandler += Next;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Next()
            {
                BattleController.Instance.DirectionStateBeginHandler -= Next;
                _context.SetState<State_9>();
            }

            public override void Deregister()
            {
                BattleController.Instance.DirectionStateBeginHandler -= Next;
            }
        }

        //選擇方向
        private class State_9 : TutorialState
        {
            public State_9(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _clickable = true;
                ((BattleTutorial_1)_context.Parent)._conversationUI.Continue(() =>
                {
                    TutorialArrowUI.Open("選擇方向。", new Vector3(4, 2, 4), Vector2Int.down);
                });
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (_clickable && position == new Vector2Int(0, 1))
                {
                    _clickable = false;
                    BattleController.Instance.CharacterStateBeginHandler += Next;
                    TutorialArrowUI.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override void Next()
            {
                if (BattleController.Instance.SelectedCharacter.Info.Faction == BattleCharacterInfo.FactionEnum.Player)
                {
                    BattleController.Instance.CharacterStateBeginHandler -= Next;
                    _context.SetState<State_10>();
                }
            }

            public override void Deregister()
            {
                BattleController.Instance.CharacterStateBeginHandler -= Next;
            }
        }

        private class State_10 : TutorialState
        {
            public State_10(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                ((BattleTutorial_1)_context.Parent)._conversationUI.Continue(() =>
                {
                    Vector3 offset = new Vector3(-200, 0, 0);
                    TutorialArrowUI.Open("選擇支援。", BattleUI.Instance.ActionButtonGroup.SupportButton.transform, offset, Vector2Int.right);
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
                _context.SetState<State_11>();
            }
        }

        private class State_11 : TutorialState
        {
            public State_11(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 offset = new Vector3(-200, 160, 0);
                TutorialArrowUI.Open("選擇猛力一擊。", BattleUI.Instance.ActionButtonGroup.ScrollView.Background.transform, offset, Vector2Int.right);
            }

            public override bool CheckScrollItem(object obj)
            {
                if (obj is Support && ((Support)obj).ID == 2)
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
                _context.SetState<State_12>();
            }
        }

        private class State_12 : TutorialState
        {
            public State_12(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 v = BattleController.Instance.SelectedCharacter.transform.position;
                TutorialArrowUI.Open("選擇目標。", new Vector3(v.x, v.y + 1, v.z), Vector2Int.down);
            }

            public override bool CheckClick(Vector2Int position)
            {
                Vector3 v = BattleController.Instance.SelectedCharacter.transform.position;
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
                _context.SetState<State_13>();
            }
        }

        private class State_13 : TutorialState
        {
            public State_13(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Vector3 v = BattleController.Instance.SelectedCharacter.transform.position;
                TutorialArrowUI.Open("再次點選同樣的位置確認。", new Vector3(v.x, v.y + 1, v.z), Vector2Int.down);
            }

            public override bool CheckClick(Vector2Int position)
            {
                Vector3 v = BattleController.Instance.SelectedCharacter.transform.position;
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
                _context.SetState<State_14>();
            }
        }

        private class State_14 : TutorialState
        {
            public State_14(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                ((BattleTutorial_1)_context.Parent)._conversationUI.Continue(() =>
                {
                    Vector3 offset = new Vector3(-200, 0, 0);
                    TutorialArrowUI.Open("選擇技能。", BattleUI.Instance.ActionButtonGroup.SkillButton.transform, offset, Vector2Int.right);
                });
            }

            public override bool CanSkill()
            {
                Next();
                return true;
            }

            public override void Next()
            {
                TutorialArrowUI.Close();
                _context.SetState<State_15>();
            }
        }

        private class State_15 : TutorialState
        {
            public State_15(StateContext context) : base(context)
            {
            }
            public override void Begin()
            {
                Vector3 offset = new Vector3(-200, 170, 0);
                TutorialArrowUI.Open("選擇攻擊。", BattleUI.Instance.ActionButtonGroup.ScrollView.Background.transform, offset, Vector2Int.right);
            }

            public override void End()
            {
                TutorialArrowUI.Close();
            }

            public override bool CheckScrollItem(object obj)
            {
                if (obj is Skill && ((Skill)obj).ID == 1)
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
                _context.SetState<State_16>();
            }
        }

        private class State_16 : TutorialState
        {
            public State_16(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                TutorialArrowUI.Open("選擇目標。", new Vector3(4, 2, 4), Vector2Int.down);
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
                _context.SetState<State_17>();
            }
        }

        private class State_17 : TutorialState
        {
            public State_17(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                TutorialArrowUI.Open("再次點選同樣的位置確認。", new Vector3(4, 2, 4), Vector2Int.down);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(4, 4))
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
                _context.SetState<State_18>();
            }
        }

        private class State_18 : TutorialState
        {
            public State_18(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                BattleController.Instance.EndTutorial();
            }
        }
    }
}*/