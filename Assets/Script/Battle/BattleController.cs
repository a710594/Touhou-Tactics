using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public BattleInfo BattleInfo;
    public BattleCharacterInfo SelectedCharacter;
    public List<BattleCharacterInfo> CharacterList = new List<BattleCharacterInfo>();

    private readonly Color _white = new Color(1, 1, 1, 0.5f);
    private readonly Color _yellow = new Color(1, 1, 0, 0.5f);

    private bool _canClick = true;
    private Vector2 _selectedPosition;
    private CameraDraw _cameraController;
    private CameraRotate _cameraRotate;
    private StateContext _context = new StateContext();
    private BattleCharacterInfo _selectedCharacter;
    private BattleUI _battleUI;
    private List<Vector2> _skillAreaList = new List<Vector2>();
    private Dictionary<int, BattleCharacterController> _controllerDic = new Dictionary<int, BattleCharacterController>();

    public void Init(BattleInfo info)
    {
        _battleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
        _cameraController = Camera.main.GetComponent<CameraDraw>();
        _cameraRotate = Camera.main.GetComponent<CameraRotate>();

        BattleInfo = info;
        CharacterList.Add(new BattleCharacterInfo(DataContext.Instance.JobDic[1]));
        CharacterList[0].ID = 1;
        CharacterList[0].Position = new Vector3(0, 1, 0);
        CharacterList.Add(new BattleCharacterInfo(DataContext.Instance.JobDic[2]));
        CharacterList[1].ID = 2;
        CharacterList[1].Position = new Vector3(1, 1, 0);
        CharacterList.Add(new BattleCharacterInfo(DataContext.Instance.JobDic[3]));
        CharacterList[2].ID = 3;
        CharacterList[2].Position = new Vector3(0, 1, 1);
        CharacterList.Add(new BattleCharacterInfo(DataContext.Instance.EnemyDic[1]));
        CharacterList[3].ID = 4;
        CharacterList[3].AI = new MashroomAI(CharacterList[3]);
        CharacterList[3].Position = new Vector3(3, 1, 3);
        CharacterList.Add(new BattleCharacterInfo(DataContext.Instance.EnemyDic[1]));
        CharacterList[4].ID = 5;
        CharacterList[4].AI = new MashroomAI(CharacterList[4]);
        CharacterList[4].Position = new Vector3(4, 1, 4);

        CameraDrag camera = Camera.main.GetComponent<CameraDrag>();
        _battleUI.SetMapInfo(info.Width, info.Height);

        GameObject obj;
        for (int i = 0; i < CharacterList.Count; i++)
        {
            info.TileInfoDic[Utility.ConvertToVector2(CharacterList[i].Position)].HasCharacter = true;
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + CharacterList[i].Controller), Vector3.zero, Quaternion.identity);
            obj.transform.position = CharacterList[i].Position;
            _controllerDic.Add(CharacterList[i].ID, obj.GetComponent<BattleCharacterController>());
            _controllerDic[CharacterList[i].ID].MoveEndHandler += OnMoveEnd;
            _battleUI.SetLittleHpBarAnchor(CharacterList[i].ID, _controllerDic[CharacterList[i].ID]);
            _battleUI.SetLittleHpBarValue(CharacterList[i].ID, CharacterList[i]);
            _battleUI.SetFloatingNumberPoolAnchor(CharacterList[i].ID, _controllerDic[CharacterList[i].ID]);
        }

        CharacterList.Sort((x, y) =>
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
        _context.AddState(new EndState(_context));

        _context.SetState<SelectCharacterState>();
    }

    public void Click(Vector2 position)
    {
        if (!_canClick) 
        {
            return;
        }

        ((BattleControllerState)_context.CurrentState).Click(position);
    }

    public void SelectSkill(Skill skill)
    {
        if (_context.CurrentState is MoveState)
        {
            ((MoveState)_context.CurrentState).SelectSkill(skill);
        }
    }

    public void Idle() 
    {
        _context.SetState<EndState>();
    }

    public void SetQuad(List<Vector2> list, Color color) 
    {
        ClearQuad();

        for (int i = 0; i < list.Count; i++)
        {
            BattleInfo.TileComponentDic[list[i]].Quad.gameObject.SetActive(true);
            BattleInfo.TileComponentDic[list[i]].Quad.material.SetColor("_Color", color);
        }
    }

    public void SetSkillArea() 
    {
        if (_selectedCharacter.SelectedSkill.Effect.Data.Area == "Through")
        {
            _skillAreaList = BattleCalculator.GetTroughAreaList(_selectedCharacter.MoveTo, new Vector3(_selectedPosition.x, Instance.BattleInfo.TileInfoDic[_selectedPosition].Height, _selectedPosition.y), BattleInfo.TileInfoDic);
        }
        else
        {
            _skillAreaList = BattleCalculator.GetNormalAreaList(BattleInfo.Width, BattleInfo.Height, _selectedCharacter.SelectedSkill.Effect, _selectedPosition);
        }
        SetQuad(_skillAreaList, _yellow);
    }

    public void ClearQuad() 
    {
        foreach (KeyValuePair<Vector2, TileComponent> pair in BattleInfo.TileComponentDic)
        {
            pair.Value.Quad.gameObject.SetActive(false);
        }
    }

    public void Skip() 
    {
        _context.SetState<EndState>();
    }

    public void OnMoveEnd() 
    {
        _canClick = true;

        if (_selectedCharacter.AI != null)
        {
            _selectedCharacter.AI.OnMoveEnd();
        }
        else
        {
            Instance._battleUI.SetSkillVisible(true);
        }
    }

    private class BattleControllerState : State
    {
        public BattleControllerState(StateContext context) : base(context)
        {
        }

        public virtual void Next() { }

        public virtual void Click(Vector2 position)
        {
        }
    }

    private class SelectCharacterState : BattleControllerState
    {
        private BattleCharacterInfo info;

        public SelectCharacterState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            info = Instance.CharacterList[0];
            Instance._selectedCharacter = info;

            int wt = info.CurrentWT;
            List<BattleCharacterInfo> characterList = Instance.CharacterList;
            for (int i = 0; i < characterList.Count; i++)
            {
                characterList[i].CurrentWT -= wt;
            }

            Vector3 position = new Vector3();
            BattleCharacterController controller = Instance._controllerDic[info.ID];
            if (Instance._cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
            {
                position = controller.transform.position + new Vector3(-10, 10, -10);
            }
            else
            {
                position = controller.transform.position + new Vector3(0, 10, 0);
            }
            Camera.main.transform.DOMove(position, 1f).OnComplete(()=> 
            {
                _context.SetState<MoveState>();
            });
            //Camera.main.transform.localEulerAngles = new Vector3(30, 45, 0);
        }
    }

    private class MoveState : BattleControllerState
    {
        private List<Vector2> _stepList;
        private BattleCharacterInfo _character;
        private List<BattleCharacterInfo> _characterList;

        public MoveState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            BattleInfo info = Instance.BattleInfo;
            _character = Instance._selectedCharacter;
            _character.MoveTo = _character.Position;
            _characterList = Instance.CharacterList;
            Instance._controllerDic[_character.ID].transform.position = _character.Position;
            Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2(_character.Position)].HasCharacter = false;
            Instance._battleUI.SetCharacterInfoUI_1(_character);
            Instance._battleUI.SetCharacterInfoUI_2(null);
            if (!_character.IsAuto)
            {
                Instance._battleUI.SetSkillVisible(true);
                Instance._battleUI.SetSkillData(_character);
            }
            _stepList = AStarAlgor.Instance.GetStepList(info.Width, info.Height, Utility.ConvertToVector2(_character.Position), _character, _characterList, info.TileInfoDic);
            Instance.SetQuad(_stepList, Instance._white);

            if (_character.IsAuto)
            {
                _character.AI.Start(_stepList);
            }
        }

        public override void Click(Vector2 position)
        {
            //顯示角色資料
            Instance._battleUI.SetCharacterInfoUI_2(null);
            for (int i = 0; i < _characterList.Count; i++)
            {
                if (_characterList[i] != Instance._selectedCharacter && position == new Vector2(_characterList[i].Position.x, _characterList[i].Position.z))
                {
                    Instance._battleUI.SetCharacterInfoUI_2(_characterList[i]);
                    return;
                }
            }

            if (_stepList.Contains(position))
            {
                Instance._canClick = false;
                Instance._controllerDic[_character.ID].transform.position = _character.Position;
                Instance._battleUI.SetSkillVisible(false);
                _character.MoveTo = new Vector3(position.x, Instance.BattleInfo.TileInfoDic[position].Height, position.y);
                List<Vector2> path = AStarAlgor.Instance.GetPath(Utility.ConvertToVector2(_character.Position), position, _character, _characterList, Instance.BattleInfo.TileInfoDic, false);
                Instance._controllerDic[_character.ID].Move(path);
            }
            else
            {
                _character.MoveTo = _character.Position;
                Instance._controllerDic[_character.ID].transform.position = _character.Position;
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
            BattleInfo info = Instance.BattleInfo;
            _character = Instance._selectedCharacter;
            if (_character.SelectedSkill != null)
            {
                _rangeList = Utility.GetRange(_character.SelectedSkill.Effect.Data.Range, info.Width, info.Height, new Vector2(_character.MoveTo.x, _character.MoveTo.z));
                Instance.SetQuad(_rangeList, Instance._white);
            }
            Instance._battleUI.SetSkillVisible(false);
        }

        public override void Click(Vector2 position)
        {
            if (_rangeList.Contains(position))
            {
                //顯示角色資料
                List<BattleCharacterInfo> characterList = Instance.CharacterList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    if (characterList[i] != Instance._selectedCharacter && position == new Vector2(characterList[i].Position.x, characterList[i].Position.z))
                    {
                        Instance._battleUI.SetCharacterInfoUI_2(characterList[i]);
                        int predictionHp = BattleCalculator.GetPredictionHp(characterList[i].CurrentHP, _character.SelectedSkill.Effect, _character, characterList[i], characterList);
                        if (predictionHp != -1)
                        {
                            Instance._battleUI.SetHpPrediction(characterList[i].CurrentHP, predictionHp, characterList[i].MaxHP);
                        }
                        break;
                    }
                }

                if (_character.SelectedSkill.Effect.Data.Track == EffectModel.TrackEnum.Straight && Instance.BattleInfo.TileInfoDic.ContainsKey(position))
                {
                    Dictionary<Vector2, TileInfo> tileDic = Instance.BattleInfo.TileInfoDic;
                    Vector3 p = new Vector3(position.x, tileDic[position].Height, position.y);
                    BattleCalculator.CheckLine(_character.MoveTo, p, tileDic, out bool isBlock, out Vector3 result);
                    Instance._cameraController.DrawLine(_character.MoveTo, result, isBlock);
                    Instance._selectedPosition = new Vector2(result.x, result.z);
                }
                else
                {
                    Instance._selectedPosition = position;
                }

                Instance._controllerDic[_character.ID].SetDirection(Instance._selectedPosition);

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
            //if (Instance._selectedCharacter.SelectedSkill.Effect.Data.Area == "Through")
            //{
            //    Instance._skillAreaList = BattleCalculator.GetTroughAreaList(Instance._selectedCharacter.MoveTo, Instance._selectedPosition, Instance.BattleInfo.TileInfoDic);
            //}
            //else
            //{
            //    Instance._skillAreaList = BattleCalculator.GetNormalAreaList(Instance._selectedCharacter.SelectedSkill.Effect, Instance._selectedPosition);
            //}
            Instance.SetSkillArea();
            Instance.BattleInfo.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(true);
        }

        public override void Click(Vector2 position)
        {
            Instance._battleUI.StopHpPrediction();

            if (position == Instance._selectedPosition)
            {
                _context.SetState<EffectState>();
            }
            else
            {
                Instance._battleUI.SetCharacterInfoUI_2(null);
                _context.SetState<MoveState>();
            }
        }

        public void Confirm()
        {
            _context.SetState<EffectState>();
        }

        public override void End()
        {
            Instance._cameraController.Clear();
            Instance.BattleInfo.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(false);
        }
    }

    private class EffectState : BattleControllerState
    {
        private BattleCharacterInfo _character;
        private List<BattleCharacterInfo> _characterList;
        private List<BattleCharacterInfo> _targetList;
        private Timer _timer = new Timer();

        public EffectState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            base.Begin();
            Instance._battleUI.SetCharacterInfoUI_2(null);
            Instance.ClearQuad();
            _characterList = Instance.CharacterList;
            _character = _characterList[0];
            _targetList = new List<BattleCharacterInfo>();
            for (int i = 0; i < _characterList.Count; i++)
            {
                if (BattleCalculator.CheckEffectArea(Instance._skillAreaList, Utility.ConvertToVector2(_characterList[i].MoveTo)))
                {
                    _targetList.Add(_characterList[i]);
                }

            }
            if (_targetList.Count > 0)
            {
                for (int i = 0; i < _targetList.Count; i++)
                {
                    List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                    _character.SelectedSkill.Effect.SetEffect(_character, _targetList[i], floatingList, _characterList);
                    Instance._battleUI.SetLittleHpBarValue(_targetList[i].ID, _targetList[i]);
                    for (int j = 0; j < floatingList.Count; j++)
                    {
                        Instance._battleUI.PlayFloatingNumberPool(_targetList[i].ID, floatingList[j].Type, floatingList[j].Text);
                    }
                }
                _character.CurrentMP -= _character.SelectedSkill.Data.MP;
                _timer.Start(0.5f, CheckResult);
            }
            else 
            {
                Debug.Log("毫無反應...");
                _context.SetState<EndState>();
            }
        }

        private void CheckResult()
        {
            for (int i = 0; i < _targetList.Count; i++)
            {
                if (_targetList[i].CurrentHP <= 0)
                {
                    _characterList.Remove(_targetList[i]);
                    Instance._controllerDic[_targetList[i].ID].gameObject.SetActive(false);
                    Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2(_targetList[i].Position)].HasCharacter = false;
                }
            }

            int playerCount = 0;
            int enemyCount = 0;
            for (int i = 0; i < _characterList.Count; i++)
            {
                if (_characterList[i].Faction == BattleCharacterInfo.FactionEnum.Player)
                {
                    playerCount++;
                }
                else
                {
                    enemyCount++;
                }
            }

            if (playerCount == 0)
            {
                Debug.Log("You Lose");
            }
            else if (enemyCount == 0)
            {
                Debug.Log("You Win");
            }
            else
            {
                _context.SetState<EndState>();
            }
        }
    }

    private class EndState : BattleControllerState 
    {
        public EndState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            List<BattleCharacterInfo> characterList = Instance.CharacterList;
            BattleCharacterInfo character = characterList[0];
            Dictionary<Vector2, TileInfo> tileDic = Instance.BattleInfo.TileInfoDic;

            Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2(character.Position)].HasCharacter = false;
            Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2(character.MoveTo)].HasCharacter = true;
            character.CheckStatus();
            character.CurrentWT = character.WT;
            character.Position = character.MoveTo;
            characterList.RemoveAt(0);
            characterList.Add(character);
            characterList.Sort((x, y) =>
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

            _context.SetState<SelectCharacterState>();
        }
    }
}
