using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController
{
    private static BattleController _instance;
    public static BattleController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BattleController();
            }
            return _instance;
        }
    }

    private Vector2 _selectedPosition;
    private BattleInfo _battleInfo;
    private StateContext _context = new StateContext();
    public BattleCharacterInfo _selectedCharacter;
    private List<BattleCharacterInfo> _characterList = new List<BattleCharacterInfo>();
    private Dictionary<int, BattleCharacterController> _controllerDic = new Dictionary<int, BattleCharacterController>();

    public void Init(BattleInfo info)
    {
        _battleInfo = info;
        _characterList.Add(new BattleCharacterInfo(DataContext.Instance.JobDic[1]));
        _characterList[0].ID = 1;
        _characterList[0].Position = new Vector3(0, 1, 0);
        _characterList.Add(new BattleCharacterInfo(DataContext.Instance.EnemyDic[1]));
        _characterList[1].ID = 2;
        _characterList[1].Position = new Vector3(2, 1, 2);
        _characterList.Add(new BattleCharacterInfo(DataContext.Instance.EnemyDic[1]));
        _characterList[2].ID = 3;
        _characterList[2].Position = new Vector3(5, 1, 5);

        GameObject obj;
        for (int i = 0; i < _characterList.Count; i++)
        {
            info.tileInfoDic[_characterList[i].Position].HasCharacter = true;
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Character/" + "Reimu"), Vector3.zero, Quaternion.identity);
            obj.transform.position = _characterList[i].Position;
            _controllerDic.Add(_characterList[i].ID, obj.GetComponent<BattleCharacterController>());
        }

        _characterList.Sort((x, y) =>
        {
            if (x.CurrentWT > y.CurrentWT)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        });

        _context.AddState(new SelectCharacterState(_context));
        _context.AddState(new MoveState(_context));
        _context.AddState(new SelectTargetState(_context));
        _context.AddState(new ConfirmState(_context));
        _context.AddState(new EffectState(_context));

        _context.SetState<SelectCharacterState>();
    }

    public void Next()
    {
        ((BattleControllerState)_context.CurrentState).Next();
    }

    public void Click(Vector2 position)
    {
        ((BattleControllerState)_context.CurrentState).Click(position);
    }

    public void SelectSkill(Skill skill)
    {
        if (_context.CurrentState is MoveState)
        {
            ((MoveState)_context.CurrentState).SelectSkill(skill);
        }
    }

    public void Confirm()
    {
        if (_context.CurrentState is ConfirmState)
        {
            ((ConfirmState)_context.CurrentState).Confirm();
        }
    }

    public void SetQuad(List<Vector2> list) 
    {
        foreach(KeyValuePair<Vector2, TileComponent> pair in _battleInfo.tileComponentDic) 
        {
            pair.Value.Quad.gameObject.SetActive(false);
        }

        for (int i = 0; i < list.Count; i++)
        {
            _battleInfo.tileComponentDic[list[i]].Quad.gameObject.SetActive(true);
        }
    }

    private class BattleControllerState : State
    {
        public BattleControllerState(StateContext context) : base(context)
        {
        }

        public virtual void Next() { }

        public virtual void Click(Vector2 position) { }
    }

    private class SelectCharacterState : BattleControllerState
    {
        public SelectCharacterState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            Instance._selectedCharacter = Instance._characterList[0];
            int wt = Instance._selectedCharacter.CurrentWT;
            List<BattleCharacterInfo> characterList = Instance._characterList;
            for (int i = 0; i < characterList.Count; i++)
            {
                characterList[i].CurrentWT -= wt;
            }

            BattleCharacterController controller = Instance._controllerDic[Instance._selectedCharacter.ID];
            Camera.main.transform.parent = controller.transform;
            Camera.main.transform.localPosition = controller.transform.position + new Vector3(0, 8, -13);
            Camera.main.transform.localEulerAngles = new Vector3(30, 0, 0);

            _context.SetState<MoveState>();
        }
    }

    private class MoveState : BattleControllerState
    {
        private List<Vector2> _stepList;
        private BattleCharacterInfo _character = Instance._selectedCharacter;

        public MoveState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            BattleInfo info = Instance._battleInfo;
            _character = Instance._selectedCharacter;
            _character.MoveTo = _character.Position;
            Instance._battleInfo.tileInfoDic[_character.Position].HasCharacter = false;
            _stepList = Utility.GetStepList(_character.MOV, info.Width, info.Height, new Vector2(_character.Position.x, _character.Position.z));
            Instance.SetQuad(_stepList);
        }

        public override void Click(Vector2 position)
        {
            List<BattleCharacterInfo> characterList = Instance._characterList;
            for (int i = 0; i < characterList.Count; i++)
            {
                if (characterList[i] != Instance._selectedCharacter &&  position == new Vector2(characterList[i].Position.x, characterList[i].Position.z))
                {
                    //顯示角色資料
                    return;
                }
            }

            if (_stepList.Contains(position))
            {
                Vector3 p3 = new Vector3(position.x, Instance._battleInfo.tileInfoDic[position].Height, position.y);
                _character.MoveTo = p3;
                Instance._controllerDic[_character.ID].transform.position = p3;
                //move
            }
            else
            {
                _character.MoveTo = _character.Position;
                //Console.WriteLine("不在可移動的範圍內");
            }
        }

        public void SelectSkill(Skill skill)
        {
            _character.SelectedSkill = skill;
            _context.SetState<SelectTargetState>();
        }
    }

    private class SelectTargetState : BattleControllerState
    {
        private List<Vector2> _rangeList;
        private BattleCharacterInfo _character = Instance._selectedCharacter;

        public SelectTargetState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            BattleInfo info = Instance._battleInfo;
            _character = Instance._selectedCharacter;
            _rangeList = Utility.GetRange(_character.SelectedSkill.Effect.Data.Range, info.Width, info.Height, new Vector2(_character.MoveTo.x, _character.MoveTo.z));
            Instance.SetQuad(_rangeList);
        }

        public override void Click(Vector2 position)
        {
            if (_rangeList.Contains(position))
            {
                Instance._selectedPosition = position;
                _context.SetState<ConfirmState>();
                //Console.WriteLine("Select " + position);
            }
            else
            {
                _context.SetState<MoveState>();
                //Console.WriteLine("Return to move state");
            }
        }
    }

    private class ConfirmState : BattleControllerState
    {
        public ConfirmState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            BattleCharacterInfo character = Instance._selectedCharacter;
            Skill skill = character.SelectedSkill;
            int width = Instance._battleInfo.Width;
            int height = Instance._battleInfo.Height;
            //show effect area
        }

        public override void Click(Vector2 position)
        {
            _context.SetState<MoveState>();
            //Console.WriteLine("Return to move state");
        }

        public void Confirm()
        {
            //Console.WriteLine("Confirm");
            _context.SetState<EffectState>();
        }
    }

    private class EffectState : BattleControllerState
    {
        private BattleCharacterInfo _character;
        List<BattleCharacterInfo> _characterList;

        public EffectState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            base.Begin();
            _characterList = Instance._characterList;
            _character = _characterList[0];
            List<BattleCharacterInfo> targrtList = new List<BattleCharacterInfo>();
            for (int i = 0; i < _characterList.Count; i++)
            {
                if (_character.SelectedSkill.Effect.CheckArea(Instance._selectedPosition, new Vector2(_characterList[i].Position.z, _characterList[i].Position.z)))
                {
                    //SetEffect
                    targrtList.Add(_characterList[i]);
                    //Console.WriteLine(_characterList[i].Name);
                }
            }
            _character.SelectedSkill.Use(targrtList);

            //after animation
            _context.SetState<SelectCharacterState>();
        }

        public override void End()
        {
            Instance._battleInfo.tileInfoDic[_character.Position].HasCharacter = false;
            Instance._battleInfo.tileInfoDic[_character.MoveTo].HasCharacter = true;
            _character.CurrentWT = _characterList[0].WT;
            _character.Position = _character.MoveTo;
            _characterList.RemoveAt(0);
            _characterList.Add(_character);
            _characterList.Sort((x, y) =>
            {
                if (x.CurrentWT > y.CurrentWT)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
        }
    }
}
