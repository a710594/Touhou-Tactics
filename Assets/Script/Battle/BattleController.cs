using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

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
    private Vector2Int _selectedPosition;
    private CameraDraw _cameraController;
    private CameraRotate _cameraRotate;
    private StateContext _context = new StateContext();
    private BattleCharacterInfo _selectedCharacter;
    private BattleUI _battleUI;
    private GameObject _arrow;
    private List<Vector2Int> _skillAreaList = new List<Vector2Int>();
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
            info.TileInfoDic[Utility.ConvertToVector2Int(CharacterList[i].Position)].HasCharacter = true;
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

        _arrow = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/Arrow"), Vector3.zero, Quaternion.identity);

        _context.AddState(new SelectCharacterState(_context));
        _context.AddState(new SelectActionState(_context));
        _context.AddState(new MoveState(_context));
        _context.AddState(new SelectSkillState(_context));
        _context.AddState(new SelectTargetState(_context));
        _context.AddState(new ConfirmState(_context));
        _context.AddState(new EffectState(_context));
        _context.AddState(new EndState(_context));

        _context.SetState<SelectCharacterState>();
    }

    public void Click(Vector2Int position)
    {
        if (!_canClick) 
        {
            return;
        }

        ((BattleControllerState)_context.CurrentState).Click(position);
    }

    public void SetActionState()
    {
        _context.SetState<SelectActionState>();
    }

    public void SetMoveState()
    {
        _context.SetState<MoveState>();
    }

    public void SetSelectSkillState()
    {
        _context.SetState<SelectSkillState>();
    }

    public void Idle()
    {
        _selectedCharacter.ActionCount = 0;
        _context.SetState<EndState>();
    }

    public void SelectSkill(Skill skill)
    {
        if (_context.CurrentState is SelectSkillState)
        {
            _selectedCharacter.SelectedSkill = skill;
            _context.SetState<SelectTargetState>();
        }
    }

    public void SetQuad(List<Vector2Int> list, Color color) 
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
            _skillAreaList = BattleCalculator.GetTroughAreaList(_selectedCharacter.Position, new Vector3(_selectedPosition.x, Instance.BattleInfo.TileInfoDic[_selectedPosition].Height, _selectedPosition.y), BattleInfo.TileInfoDic);
        }
        else
        {
            _skillAreaList = BattleCalculator.GetNormalAreaList(BattleInfo.Width, BattleInfo.Height, _selectedCharacter.SelectedSkill.Effect, _selectedPosition);
        }
        SetQuad(_skillAreaList, _yellow);
    }

    public void ClearQuad() 
    {
        foreach (KeyValuePair<Vector2Int, TileComponent> pair in BattleInfo.TileComponentDic)
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
        _selectedCharacter.ActionCount--;

        if (_selectedCharacter.AI != null)
        {
            _selectedCharacter.AI.OnMoveEnd();
        }
        else
        {
            if (_selectedCharacter.ActionCount > 0)
            {
                _context.SetState<SelectActionState>();
            }
            else 
            {
                _context.SetState<EndState>();
            }
        }
    }

    public void SetCharacterInfoUI_2(Vector2 position) 
    {
        //顯示角色資料
        Instance._battleUI.SetCharacterInfoUI_2(null);
        for (int i = 0; i < CharacterList.Count; i++)
        {
            if (CharacterList[i] != Instance._selectedCharacter && position == new Vector2(CharacterList[i].Position.x, CharacterList[i].Position.z))
            {
                Instance._battleUI.SetCharacterInfoUI_2(CharacterList[i]);
                return;
            }
        }
    }

    private class BattleControllerState : State
    {
        protected BattleCharacterInfo _character;
        protected List<BattleCharacterInfo> _characterList;

        public BattleControllerState(StateContext context) : base(context)
        {
        }

        public virtual void Next() { }

        public virtual void Click(Vector2Int position)
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
                _context.SetState<SelectActionState>();
            });
            Instance._arrow.transform.SetParent(controller.transform);
            Instance._arrow.transform.localPosition = new Vector3(0, 1.3f, 0);
            Instance._arrow.transform.localEulerAngles = Vector3.zero;
        }
    }

    private class SelectActionState : BattleControllerState
    {
        public SelectActionState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            _character = Instance._selectedCharacter;
            _characterList = Instance.CharacterList;
            Instance._battleUI.ActionButtonGroup.gameObject.SetActive(true);
            Instance._battleUI.ActionButtonGroup.SkillButton.interactable = !_character.HasUseSkill;
            Instance._battleUI.SetCharacterInfoUI_1(_character);
            Instance._battleUI.SetCharacterInfoUI_2(null);
            Instance.ClearQuad();

            if (_character.IsAuto)
            {
                _character.AI.Start();
            }
        }

        public override void Click(Vector2Int position)
        {
            Instance.SetCharacterInfoUI_2(position);
        }
    }

    private class MoveState : BattleControllerState
    {
        private Vector2Int _lastSelectedPosition;
        private List<Vector2Int> _stepList;

        public MoveState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            _lastSelectedPosition = new Vector2Int(int.MaxValue, int.MaxValue);
            BattleInfo info = Instance.BattleInfo;
            _character = Instance._selectedCharacter;
            _characterList = Instance.CharacterList;
            Instance._controllerDic[_character.ID].transform.position = _character.Position;
            Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_character.Position)].HasCharacter = false;
            Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
            _stepList = AStarAlgor.Instance.GetStepList(info.Width, info.Height, Utility.ConvertToVector2Int(_character.Position), _character, _characterList, info.TileInfoDic);
            Instance.SetQuad(_stepList, Instance._white);
        }

        public override void Click(Vector2Int position)
        {
            Instance.SetCharacterInfoUI_2(position);

            if (_stepList.Contains(position))
            {
                if (position == _lastSelectedPosition) //確定移動
                {
                    Instance._canClick = false;
                    Instance._controllerDic[_character.ID].transform.position = _character.Position;
                    Instance._battleUI.SetSkillVisible(false);
                    Instance.BattleInfo.TileComponentDic[_lastSelectedPosition].Select.gameObject.SetActive(false);
                    Instance.ClearQuad();
                    List<Vector2Int> path = AStarAlgor.Instance.GetPath(Utility.ConvertToVector2Int(_character.Position), position, _character, _characterList, Instance.BattleInfo.TileInfoDic, false);
                    Instance._controllerDic[_character.ID].Move(path);
                    Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_character.Position)].HasCharacter = false;
                    _character.Position = new Vector3(position.x, Instance.BattleInfo.TileInfoDic[position].Height, position.y);
                    Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_character.Position)].HasCharacter = true;
                }
                else
                {
                    _lastSelectedPosition = position;
                    Instance.BattleInfo.TileComponentDic[_lastSelectedPosition].Select.gameObject.SetActive(true);
                }
            }
            else
            {
                if (Instance.BattleInfo.TileComponentDic.ContainsKey(_lastSelectedPosition))
                {
                    Instance.BattleInfo.TileComponentDic[_lastSelectedPosition].Select.gameObject.SetActive(false);
                }
                Instance._controllerDic[_character.ID].transform.position = _character.Position;
                _context.SetState<SelectActionState>();
            }
        }

        //public void SelectSkill(Skill skill)
        //{
        //    _character.SelectedSkill = skill;
        //    _context.SetState<SelectTargetState>();
        //}
    }

    private class SelectSkillState : BattleControllerState
    {
        public SelectSkillState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
            BattleCharacterInfo character = Instance._selectedCharacter;
            if (!character.IsAuto)
            {
                Instance._battleUI.SetSkillVisible(true);
                Instance._battleUI.SetSkillData(character);
            }
        }

        public override void Click(Vector2Int position)
        {
            Instance.SetCharacterInfoUI_2(position);
        }
    }

    private class SelectTargetState : BattleControllerState
    {
        private List<Vector2Int> _rangeList;
        private BattleCharacterInfo _character;

        public SelectTargetState(StateContext context) : base(context)
        {
        }

        public override void Begin()
        {
            BattleInfo info = Instance.BattleInfo;
            _character = Instance._selectedCharacter;
            if (_character.SelectedSkill != null)
            {
                _rangeList = Utility.GetRange(_character.SelectedSkill.Effect.Data.Range, info.Width, info.Height, Utility.ConvertToVector2Int(_character.Position));
                Instance.SetQuad(_rangeList, Instance._white);
            }
            Instance._battleUI.SetSkillVisible(false);
        }

        public override void Click(Vector2Int position)
        {
            if (_rangeList.Contains(position))
            {
                //顯示角色資料
                List<BattleCharacterInfo> characterList = Instance.CharacterList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    if (characterList[i] != Instance._selectedCharacter && position == Utility.ConvertToVector2Int(characterList[i].Position))
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

                Dictionary<Vector2Int, TileInfo> tileDic = Instance.BattleInfo.TileInfoDic;
                Vector3 p = new Vector3(position.x, tileDic[position].Height, position.y);
                if (_character.SelectedSkill.Effect.Data.Track == EffectModel.TrackEnum.Straight)
                {
                    BattleCalculator.CheckLine(_character.Position, p, tileDic, out bool isBlock, out Vector3 result);
                    Instance._cameraController.DrawLine(_character.Position, result, isBlock);
                    Instance._selectedPosition = Utility.ConvertToVector2Int(result);
                }
                else if (_character.SelectedSkill.Effect.Data.Track == EffectModel.TrackEnum.Parabola)
                {
                    BattleCalculator.CheckParabola(_character.Position, p, 4, tileDic, out bool isBlock, out List<Vector3> result); //要補拋物線的高度
                    Instance._cameraController.DrawParabola(result, isBlock);
                    Instance._selectedPosition = Utility.ConvertToVector2Int(result.Last());
                }
                else
                {
                    Instance._selectedPosition = position;
                }

                Instance._controllerDic[_character.ID].SetDirection(Instance._selectedPosition);

                _context.SetState<ConfirmState>();
            }
            else
            {
                Instance.ClearQuad();
                _context.SetState<SelectSkillState>();
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
            if (Instance._selectedCharacter.SelectedSkill.Effect.Data.Area == "Through")
            {
                Vector3 to = new Vector3(Instance._selectedPosition.x, Instance.BattleInfo.TileInfoDic[Instance._selectedPosition].Height, Instance._selectedPosition.y);
                Instance._skillAreaList = BattleCalculator.GetTroughAreaList(Instance._selectedCharacter.Position, to, Instance.BattleInfo.TileInfoDic);
            }
            else
            {
                Instance._skillAreaList = BattleCalculator.GetNormalAreaList(Instance.BattleInfo.Width, Instance.BattleInfo.Height, Instance._selectedCharacter.SelectedSkill.Effect, Instance._selectedPosition);
            }
            Instance.SetSkillArea();
            Instance.BattleInfo.TileComponentDic[Instance._selectedPosition].Select.gameObject.SetActive(true);
        }

        public override void Click(Vector2Int position)
        {
            Instance._battleUI.StopHpPrediction();

            if (position == Instance._selectedPosition)
            {
                _context.SetState<EffectState>();
            }
            else
            {
                Instance._battleUI.SetCharacterInfoUI_2(null);
                Instance.ClearQuad();
                _context.SetState<SelectSkillState>();
            }
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
            _character = Instance._selectedCharacter;
            _targetList = new List<BattleCharacterInfo>();
            for (int i = 0; i < _characterList.Count; i++)
            {
                if (BattleCalculator.CheckEffectArea(Instance._skillAreaList, Utility.ConvertToVector2Int(_characterList[i].Position)))
                {
                    _targetList.Add(_characterList[i]);
                }

            }
            if (_targetList.Count > 0)
            {
                for (int i = 0; i < _targetList.Count; i++)
                {
                    List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                    _character.SelectedSkill.SetEffect(_character, _targetList[i], floatingList, _characterList);
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
                    Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_targetList[i].Position)].HasCharacter = false;
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
                _character.HasUseSkill = true;
                _character.ActionCount--;
                if (_character.ActionCount > 0)
                {
                    _context.SetState<SelectActionState>();
                }
                else
                {
                    _context.SetState<EndState>();
                }
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
            _characterList = Instance.CharacterList;
            _character = Instance._selectedCharacter;
            Dictionary<Vector2Int, TileInfo> tileDic = Instance.BattleInfo.TileInfoDic;

            if (_characterList.Contains(_character))
            {
                if (_character.ActionCount == 0)
                {
                    List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                    _character.CheckStatus(floatingList);
                    for (int j = 0; j < floatingList.Count; j++)
                    {
                        Instance._battleUI.PlayFloatingNumberPool(_character.ID, floatingList[j].Type, floatingList[j].Text);
                    }
                    for(int i=0; i<_character.SkillList.Count; i++)
                    {
                        _character.SkillList[i].CheckCD();
                    }
                    _character.CurrentWT = _character.WT;
                    _character.ActionCount = 2;
                    _character.HasUseSkill = false;
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

                    _context.SetState<SelectCharacterState>();
                }
                else
                {
                    _context.SetState<SelectActionState>();
                }
            }
            else 
            {
                _context.SetState<SelectCharacterState>();
            }
        }
    }
}
