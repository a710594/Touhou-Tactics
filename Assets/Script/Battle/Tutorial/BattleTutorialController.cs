using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{ 
    public class BattleTutorialController
    {
        private static BattleTutorialController _instance;
        public static BattleTutorialController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleTutorialController();
                }
                return _instance;
            }
        }

        private bool _hasStarted = false;
        private StateContext _context = new StateContext();
        private BattleUI _battleUI;

        public BattleTutorialController() 
        {
            _context.ClearState();
            _context.AddState(new State_1(_context));
            _context.AddState(new State_2(_context));
            _context.AddState(new State_3(_context));
            _context.AddState(new State_4(_context));
            _context.AddState(new State_5(_context));
            _context.AddState(new State_6(_context));
            _context.AddState(new State_7(_context));
            _context.AddState(new State_8(_context));
        }

        public void Start() 
        {
            if (!_hasStarted)
            {
                _hasStarted = true;
                _battleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
                _context.SetState<State_1>();
                BattleController.Instance.Arrow.SetActive(false);
            }
        }

        public bool CanMove() 
        {
            return ((TutorialState)_context.CurrentState).CanMove();
        }

        public bool CanSkill()
        {
            return ((TutorialState)_context.CurrentState).CanSkill();
        }

        public bool CanSupport()
        {
            return ((TutorialState)_context.CurrentState).CanSupport();
        }

        public bool CanItem()
        {
            return ((TutorialState)_context.CurrentState).CanItem();
        }

        public bool CanIdle()
        {
            return ((TutorialState)_context.CurrentState).CanIdle();
        }

        public bool CanReset()
        {
            return ((TutorialState)_context.CurrentState).CanReset();
        }

        public bool CheckClick(Vector2Int position)
        {
            return ((TutorialState)_context.CurrentState).CheckClick(position);
        }

        public bool CheckScrollItem(object obj) 
        {
            return ((TutorialState)_context.CurrentState).CheckScrollItem(obj);
        }

        public void ToState_4() 
        {
            if (_context.CurrentState is State_3)
            {
                _context.SetState<State_4>();
            }
        }

        public void ToState_8()
        {
            if (_context.CurrentState is State_7)
            {
                _context.SetState<State_8>();
            }
        }

        private class TutorialState : State
        {
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

        //第一步:選擇移動
        private class State_1 : TutorialState 
        {
            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Vector3 offset = new Vector3(-200, 0, 0);
                TutorialArrowUI.Open("選擇移動。", Instance._battleUI.ActionButtonGroup.MoveButton.transform, offset, Vector2Int.right, null);
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
                _context.SetState<State_2>();
            }
        }

        //指定位置
        private class State_2 : TutorialState 
        {
            public State_2(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialUI.Open("地圖上白色的區域代表可移動的範圍。\n請點選箭頭指示的位置。", "Tutorial_2", ()=> 
                {
                    TutorialArrowUI.Open("選擇移動。", new Vector3(4, 1, 3), Vector2Int.down, null);
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
                _context.SetState<State_3>();
            }
        }

        //確認位置
        private class State_3 : TutorialState
        {
            public State_3(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialArrowUI.Open("再次點選同樣的位置代表確定位置。", new Vector3(4, 1, 3), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(4, 3))
                {
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
                _context.SetState<State_4>();
            }
        }

        //選擇技能
        private class State_4 : TutorialState
        {
            public State_4(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Vector3 offset = new Vector3(-200, 0, 0);
                TutorialArrowUI.Open("選擇技能。", Instance._battleUI.ActionButtonGroup.SkillButton.transform, offset, Vector2Int.right, null);
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
                _context.SetState<State_5>();
            }
        }

        //攻擊
        private class State_5 : TutorialState
        {
            public State_5(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Vector3 offset = new Vector3(-200, 160, 0);
                TutorialArrowUI.Open("選擇攻擊。", Instance._battleUI.ActionButtonGroup.ScrollView.Background.transform, offset, Vector2Int.right, null);
            }

            public override void End()
            {
                TutorialArrowUI.Close();
            }

            public override bool CheckScrollItem(object obj)
            {
                if(obj is Skill &&((Skill)obj).Data.ID == 1) 
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
                _context.SetState<State_6>();
            }
        }

        //選擇攻擊位置
        private class State_6 : TutorialState
        {
            public State_6(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialUI.Open("現在地圖上白色的範圍是可以放技能的地方。\n請點選箭頭指示的位置。", "Tutorial_3", () =>
                {
                    TutorialArrowUI.Open("選擇目標。", new Vector3(4, 2, 4), Vector2Int.down, null);
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
                _context.SetState<State_7>();
            }
        }

        //確認攻擊位置
        private class State_7 : TutorialState
        {
            public State_7(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialArrowUI.Open("再次點選同樣的位置確認。", new Vector3(4, 2, 4), Vector2Int.down, null);
            }

            public override bool CheckClick(Vector2Int position)
            {
                if (position == new Vector2Int(4, 4))
                {
                    TutorialArrowUI.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private class State_8 : TutorialState
        {
            public State_8(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialUI.Open("以上就是移動與攻擊的基本流程。\n一個角色一回合中可以有兩次行動的機會。除了移動和使用技能外還有其他的選項。", ()=> 
                {
                    TutorialUI.Open("1.移動：一回合最多可以移動兩次\n2.技能：一回合只能使用一次\n3.支援：與技能類似，大多是強化自己的效果。不消耗行動次數，但一樣一回合只能使用一次\n4.道具：使用規則和技能相同，但是需要消耗道具\n5.待機：如果該回合沒有其他想做的事，可以選擇待機結束該回合。", null);
                });

                BattleController.Instance.Info.IsTutorial = false;
                BattleController.Instance.Arrow.SetActive(true);
            }
        }
    }
}