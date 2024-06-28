using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
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

        public bool IsTutorial
        {
            get
            {
                return Tutorial != null;
            }
        }

        public Action PrepareStateBeginHandler;
        public Action ActionStateBeginHandler;
        public Action MoveStateEndHandler;
        public Action DirectionStateBeginHandler;
        public Action CharacterStateBeginHandler;

        public BattleInfo Info;
        public BattleTutorial Tutorial = null;
        public BattleCharacterInfo SelectedCharacter;
        public List<BattleCharacterInfo> CharacterList = new List<BattleCharacterInfo>(); //�s��������
        public List<BattleCharacterInfo> DyingList = new List<BattleCharacterInfo>(); //�x�����ڤ訤��
        public List<BattleCharacterInfo> DeadList = new List<BattleCharacterInfo>(); //���`���ڤ訤��

        private readonly Color _white = new Color(1, 1, 1, 0.5f);
        private readonly Color _yellow = new Color(1, 1, 0, 0.5f);

        private bool _canClick = true;
        private int _maxIndex = 0;
        private Vector2Int _selectedPosition;
        private CameraDraw _cameraController;
        public CameraRotate CameraRotate;
        private StateContext _context = new StateContext();
        private BattleUI _battleUI;
        private DragCameraUI _dragCameraUI;
        private BattleResultUI _battleResultUI;
        //public GameObject Arrow;
        public GameObject DirectionGroup;
        private Transform _root;
        private List<int> _enemyList = new List<int>();
        private Dictionary<int, BattleCharacterController> _controllerDic = new Dictionary<int, BattleCharacterController>();
        private Dictionary<Command, List<BattleCharacterInfo>> _commandTargetDic = new Dictionary<Command, List<BattleCharacterInfo>>();

        public List<Log> LogList = new List<Log>();

        public void Init(int floor, int lv, string tutorial, BattleInfo info, Transform root)
        {
            if(tutorial!=null)
            {
                var objectType = Type.GetType("Battle." + tutorial);
                Tutorial = (BattleTutorial)Activator.CreateInstance(objectType);
            }

            _root = root;
            _battleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
            _battleResultUI = GameObject.Find("BattleResultUI").GetComponent<BattleResultUI>();
            _cameraController = Camera.main.GetComponent<CameraDraw>();
            CameraRotate = Camera.main.GetComponent<CameraRotate>();
            Info = info;

            CharacterList.Clear();
            List<Vector3Int> positionList = new List<Vector3Int>();
            if (info.EnemyDic.Count == 0)
            {
                EnemyGroupModel enemyGroup = DataContext.Instance.EnemyGroupDic[floor].ElementAt(UnityEngine.Random.Range(0, DataContext.Instance.EnemyGroupDic[floor].Count)).Value;
                _enemyList = enemyGroup.EnemyList;
                info.Exp = enemyGroup.Exp;
            }
            else
            {
                _enemyList.Clear();
                foreach(KeyValuePair<Vector3Int, int> pair in info.EnemyDic) 
                {
                    _enemyList.Add(pair.Value);
                    positionList.Add(pair.Key);
                }
            }

            EnemyModel enemyData;
            for (int i = 0; i < _enemyList.Count; i++)
            {
                enemyData = DataContext.Instance.EnemyDic[_enemyList[i]];
                CharacterList.Add(new BattleCharacterInfo(lv, enemyData));
                _maxIndex++;
                CharacterList[i].Index = _maxIndex;
                Type t = Type.GetType("Battle." + enemyData.AI);
                CharacterList[i].AI = (BattleAI)Activator.CreateInstance(t);
                CharacterList[i].AI.Init(CharacterList[i]);
                if (positionList.Count == 0)
                {
                    CharacterList[i].Position = RandomCharacterPosition(BattleCharacterInfo.FactionEnum.Enemy);
                }
                else
                {
                    CharacterList[i].Position = positionList[i];
                }
            }

            _dragCameraUI = GameObject.Find("DragCameraUI").GetComponent<DragCameraUI>();
            _dragCameraUI.Init(info);

            GameObject obj;
            _controllerDic.Clear();
            for (int i = 0; i < CharacterList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + CharacterList[i].Controller), Vector3.zero, Quaternion.identity);
                obj.transform.position = CharacterList[i].Position;
                obj.transform.SetParent(_root);
                _controllerDic.Add(CharacterList[i].Index, obj.GetComponent<BattleCharacterController>());
                _controllerDic[CharacterList[i].Index].Init(CharacterList[i].Sprite);
                _controllerDic[CharacterList[i].Index].MoveEndHandler += OnMoveEnd;
                _battleUI.SetLittleHpBarAnchor(CharacterList[i].Index, _controllerDic[CharacterList[i].Index]);
                _battleUI.SetLittleHpBarValue(CharacterList[i].Index, CharacterList[i]);
                _battleUI.SetFloatingNumberPoolAnchor(CharacterList[i].Index, _controllerDic[CharacterList[i].Index]);
            }
            _battleUI.gameObject.SetActive(false);
            _battleResultUI.gameObject.SetActive(false);

            //SortCharacterList(true);

            //_battleUI.CharacterListGroupInit(CharacterList);

            //_arrow = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/Arrow"), Vector3.zero, Quaternion.identity);

            _context.ClearState();
            _context.AddState(new PrepareState(_context));
            _context.AddState(new CharacterState(_context));
            _context.AddState(new ActionState(_context));
            _context.AddState(new MoveState(_context));
            //_context.AddState(new SkillState(_context));
            //_context.AddState(new SupportState(_context));
            //_context.AddState(new ItemState(_context));
            _context.AddState(new TargetState(_context));
            _context.AddState(new ConfirmState(_context));
            _context.AddState(new EffectState(_context));
            _context.AddState(new EndState(_context));
            _context.AddState(new WinState(_context));
            _context.AddState(new LoseState(_context));
            _context.AddState(new DirectionState(_context));

            _context.SetState<PrepareState>();

            LogList.Clear();
        }

        public void Click(Vector2Int position)
        {
            if (!_canClick)
            {
                return;
            }

            ((BattleControllerState)_context.CurrentState).Click(position);
        }

        public GameObject PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
        {
            if(_context.CurrentState is PrepareState) 
            {
                return ((PrepareState)_context.CurrentState).PlaceCharacter(position, characterInfo);
            }
            else
            {
                return null;
            }
        }

        public void SetCharacterSpriteVisible(CharacterInfo characterInfo, bool isVisible)
        {
            if (_context.CurrentState is PrepareState)
            {
                ((PrepareState)_context.CurrentState).SetCharacterSpriteVisible(characterInfo, isVisible);
            }
        }

        public void RemoveCharacterSprite(CharacterInfo characterInfo)
        {
            if (_context.CurrentState is PrepareState)
            {
                ((PrepareState)_context.CurrentState).RemoveCharacterSprite(characterInfo);
            }
        }

        public void SetCharacterState() 
        {
            _context.SetState<CharacterState>();
        }

        public void SetActionState()
        {
            _context.SetState<ActionState>();
        }

        public void SetMoveState()
        {
            _context.SetState<MoveState>();
        }

        public void Idle()
        {
            SelectedCharacter.ActionCount = 0;
            _context.SetState<EndState>();
        }

        public void SetTargetState(Command command)
        {
            SelectedCharacter.SelectedCommand = command;
            _context.SetState<TargetState>();
        }

        public void SetQuad(List<Vector2Int> list, Color color)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Info.TileComponentDic[list[i]].Quad.gameObject.SetActive(true);
                Info.TileComponentDic[list[i]].Quad.material.SetColor("_Color", color);
            }
        }

        // public List<Vector2Int> GetAreaList(Command command) 
        // {
        //     List<Vector2Int> areaList = new List<Vector2Int>();
        //     if (command.Track == TrackEnum.Through)
        //     {
        //         areaList = GetTroughAreaList(SelectedCharacter.Position, new Vector3(_selectedPosition.x, Instance.Info.TileAttachInfoDic[_selectedPosition].Height, _selectedPosition.y));
        //     }
        //     else
        //     {
        //         areaList = GetNormalAreaList(Utility.ConvertToVector2Int(SelectedCharacter.Position), _selectedPosition, command.AreaList);
        //     }

        //     return areaList;
        // }

        public List<Vector2Int> SetCommandPosition(Vector2Int from, Vector2Int to, Command command)
        {
            List<Vector2Int> commandPositionList =new List<Vector2Int>();
            if(command.AreaType == AreaTypeEnum.Point)
            {
                commandPositionList.Add(to);
            }
            else if(command.AreaType == AreaTypeEnum.Array)
            {
                commandPositionList = GetNormalAreaList(from, to, command.AreaTarget, command.ArrayList);
            }
            else
            {
                if(command.AreaTarget == TargetEnum.Self)
                {
                    commandPositionList.Add(from);
                }
                else
                {
                    for(int i=0; i<CharacterList.Count; i++)
                    {
                        if(command.AreaTarget == TargetEnum.Us && SelectedCharacter.Faction == CharacterList[i].Faction)
                        {
                            commandPositionList.Add(Utility.ConvertToVector2Int(CharacterList[i].Position));  
                        }   
                        else if(command.AreaTarget == TargetEnum.Them && SelectedCharacter.Faction != CharacterList[i].Faction)
                        {
                            commandPositionList.Add(Utility.ConvertToVector2Int(CharacterList[i].Position)); 
                        }
                        else if(command.AreaTarget == TargetEnum.All)
                        {
                            commandPositionList.Add(Utility.ConvertToVector2Int(CharacterList[i].Position));  
                        }
                    }
                }
            }

            if(command.Track == TrackEnum.Through)
            {
                List<Vector2Int> line = Utility.DrawLine2D(from, to);
                line.Remove(from);
                for(int i=0; i<line.Count; i++)
                {
                    if(!commandPositionList.Contains(line[i]))
                    {
                        commandPositionList.Add(line[i]);
                    }
                }
            }
            
            return commandPositionList;
        }

        public void ClearQuad()
        {
            foreach (KeyValuePair<Vector2Int, TileComponent> pair in Info.TileComponentDic)
            {
                pair.Value.Quad.gameObject.SetActive(false);
                pair.Value.Buff.gameObject.SetActive(false);
            }
        }

        public void Skip()
        {
            _context.SetState<EndState>();
        }

        public void OnMoveEnd()
        {
            _canClick = true;
            SelectedCharacter.ActionCount--;

            if (SelectedCharacter.AI != null)
            {
                SelectedCharacter.AI.OnMoveEnd();
            }
            else
            {
                if (SelectedCharacter.ActionCount > 0)
                {
                    _context.SetState<ActionState>();
                }
                else
                {
                    _context.SetState<EndState>();
                }
            }
        }

        public void SetDirection(Vector2Int direction) 
        {
            if(_context.CurrentState is DirectionState)
            {
                Vector2Int globalDirection = Vector2Int.RoundToInt(Quaternion.AngleAxis(CameraRotate.Angle, Vector3.back) * (Vector2)direction);
                ((DirectionState)_context.CurrentState).SetDirection(globalDirection);
            }
        }

        public bool SetCharacterInfoUI_2(Vector2 position)
        {
            //��ܨ�����
            Instance._battleUI.SetCharacterInfoUI_2(null);
            for (int i = 0; i < CharacterList.Count; i++)
            {
                if (CharacterList[i] != Instance.SelectedCharacter && position == new Vector2(CharacterList[i].Position.x, CharacterList[i].Position.z))
                {
                    _battleUI.SetCharacterInfoUI_2(CharacterList[i]);
                    ShowTileBuff(CharacterList[i]);
                    ShowRange(CharacterList[i]);
                    return true;
                }
            }
            return false;
        }

        public void ResetAction()
        {
            SelectedCharacter.ActionCount = 2;
            SelectedCharacter.Position = SelectedCharacter.LastPosition;
            SelectedCharacter.HasMove = false;
            _controllerDic[SelectedCharacter.Index].transform.position = SelectedCharacter.LastPosition;
            SelectedCharacter.LastPosition = BattleCharacterInfo.DefaultLastPosition;
        }

        public void CreateCharacter(int id, int lv, Vector3 position) 
        {
            EnemyModel enemyData = DataContext.Instance.EnemyDic[id];
            BattleCharacterInfo info = new BattleCharacterInfo(lv, enemyData);
            CharacterList.Add(info);
            _maxIndex++;
            info.Index = _maxIndex;
            _battleUI.CharacterListGroup.Add(info);
            Type t = Type.GetType("Battle." + enemyData.AI);
            info.AI = (BattleAI)Activator.CreateInstance(t);
            info.AI.Init(info);
            info.Position = position;

            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + info.Controller), Vector3.zero, Quaternion.identity);
            obj.transform.position = info.Position;
            _controllerDic.Add(info.Index, obj.GetComponent<BattleCharacterController>());
            _controllerDic[info.Index].Init(info.Sprite);
            _controllerDic[info.Index].MoveEndHandler += OnMoveEnd;
            _battleUI.SetLittleHpBarAnchor(info.Index, _controllerDic[info.Index]);
            _battleUI.SetLittleHpBarValue(info.Index, info);
            _battleUI.SetFloatingNumberPoolAnchor(info.Index, _controllerDic[info.Index]);
        }

        public void ChangeSprite(int index, string sprite) 
        {
            _controllerDic[index].Init(sprite);
            _battleUI.CharacterListGroup.ChangeSprite(index, sprite);
        }

        private void SortCharacterList(bool isStart)
        {
            CharacterList.Sort((x, y) =>
            {
                if (isStart && !Passive.Contains<TankPassive>(x.PassiveList) && Passive.Contains<TankPassive>(y.PassiveList))
                {
                    return 1;
                }
                else if (isStart && Passive.Contains<TankPassive>(x.PassiveList) && !Passive.Contains<TankPassive>(y.PassiveList))
                {
                    return -1;
                }
                else 
                {
                    if (x.CurrentWT > y.CurrentWT)
                    {
                        return 1;
                    }
                    else
                    {
                        if (x.CurrentWT == y.CurrentWT)
                        {
                            if (x.Index > y.Index)
                            {
                                return 1;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            });
        }

        private void ShowTileBuff(BattleCharacterInfo character) 
        {
            Vector2Int position;
            for (int i = 0; i < character.StatusList.Count; i++)
            {
                if (character.StatusList[i].AreaList.Count > 1)
                {
                    for (int j = 0; j < character.StatusList[i].AreaList.Count; j++)
                    {
                        position = Utility.ConvertToVector2Int(character.Position) + character.StatusList[i].AreaList[j];
                        if (Info.TileComponentDic.ContainsKey(position))
                        {
                            Info.TileComponentDic[position].Buff.SetActive(true);
                        }
                    }
                }
            }
        }

        public void ShowRange(BattleCharacterInfo character) 
        {
            List<Vector2Int> stepList = GetStepList(character);
            Instance.SetQuad(stepList, _white);
            if (character.AI != null)
            {
                List<Vector2Int> tempList = new List<Vector2Int>();
                List<Vector2Int> rangeList = new List<Vector2Int>();
                for (int i = 0; i < stepList.Count; i++) //�ڥi�H���ʪ��d��
                {
                    tempList = Utility.GetRange(character.AI.SelectedSkill.Range, stepList[i], Info);
                    for (int j=0; j<tempList.Count; j++) 
                    {
                        if(!stepList.Contains(tempList[j]) && !rangeList.Contains(tempList[j])) 
                        {
                            rangeList.Add(tempList[j]);
                        }
                    }
                }
                Instance.SetQuad(rangeList, _yellow);
            }
        }

        public void SetWin() 
        {
            _context.SetState<WinState>();
        }

        public void SetLose()
        {
            _context.SetState<LoseState>();
        }

        public void EndTutorial()
        {
            Tutorial.Deregister();
            Tutorial = null;
        }

        private class BattleControllerState : State
        {
            protected BattleInfo _info;
            protected BattleCharacterInfo _character;
            protected List<BattleCharacterInfo> _characterList;
            protected List<BattleCharacterInfo> _allCharacterList; //Include dying character

            public BattleControllerState(StateContext context) : base(context)
            {
            }

            public virtual void Next() { }

            public virtual void Click(Vector2Int position)
            {
            }

            public List<BattleCharacterInfo> GetAllCharacterList() 
            {
                List<BattleCharacterInfo> list = new List<BattleCharacterInfo>(Instance.CharacterList);
                list.AddRange(Instance.DyingList);
                return list;
            }
        }
    }
}