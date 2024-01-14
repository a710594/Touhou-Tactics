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

        //�Ĥ@�B:��ܲ���
        private class State_1 : TutorialState 
        {
            public State_1(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Vector3 offset = new Vector3(-200, 0, 0);
                TutorialArrowUI.Open("��ܲ��ʡC", Instance._battleUI.ActionButtonGroup.MoveButton.transform, offset, Vector2Int.right, null);
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

        //���w��m
        private class State_2 : TutorialState 
        {
            public State_2(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialUI.Open("�a�ϤW�զ⪺�ϰ�N��i���ʪ��d��C\n���I��b�Y���ܪ���m�C", "Tutorial_2", ()=> 
                {
                    TutorialArrowUI.Open("��ܲ��ʡC", new Vector3(4, 1, 3), Vector2Int.down, null);
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

        //�T�{��m
        private class State_3 : TutorialState
        {
            public State_3(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialArrowUI.Open("�A���I��P�˪���m�N��T�w��m�C", new Vector3(4, 1, 3), Vector2Int.down, null);
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

        //��ܧޯ�
        private class State_4 : TutorialState
        {
            public State_4(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Vector3 offset = new Vector3(-200, 0, 0);
                TutorialArrowUI.Open("��ܧޯ�C", Instance._battleUI.ActionButtonGroup.SkillButton.transform, offset, Vector2Int.right, null);
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

        //����
        private class State_5 : TutorialState
        {
            public State_5(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Vector3 offset = new Vector3(-200, 160, 0);
                TutorialArrowUI.Open("��ܧ����C", Instance._battleUI.ActionButtonGroup.ScrollView.Background.transform, offset, Vector2Int.right, null);
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

        //��ܧ�����m
        private class State_6 : TutorialState
        {
            public State_6(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialUI.Open("�{�b�a�ϤW�զ⪺�d��O�i�H��ޯ઺�a��C\n���I��b�Y���ܪ���m�C", "Tutorial_3", () =>
                {
                    TutorialArrowUI.Open("��ܥؼСC", new Vector3(4, 2, 4), Vector2Int.down, null);
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

        //�T�{������m
        private class State_7 : TutorialState
        {
            public State_7(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                TutorialArrowUI.Open("�A���I��P�˪���m�T�{�C", new Vector3(4, 2, 4), Vector2Int.down, null);
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
                TutorialUI.Open("�H�W�N�O���ʻP�������򥻬y�{�C\n�@�Ө���@�^�X���i�H���⦸��ʪ����|�C���F���ʩM�ϥΧޯ�~�٦���L���ﶵ�C", ()=> 
                {
                    TutorialUI.Open("1.���ʡG�@�^�X�̦h�i�H���ʨ⦸\n2.�ޯ�G�@�^�X�u��ϥΤ@��\n3.�䴩�G�P�ޯ������A�j�h�O�j�Ʀۤv���ĪG�C�����Ӧ�ʦ��ơA���@�ˤ@�^�X�u��ϥΤ@��\n4.�D��G�ϥγW�h�M�ޯ�ۦP�A���O�ݭn���ӹD��\n5.�ݾ��G�p�G�Ӧ^�X�S����L�Q�����ơA�i�H��ܫݾ������Ӧ^�X�C", null);
                });

                BattleController.Instance.Info.IsTutorial = false;
                BattleController.Instance.Arrow.SetActive(true);
            }
        }
    }
}