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

        public bool IsTutorialActive
        {
            get
            {
                if (Tutorial == null) 
                {
                    return false;
                }
                else
                {
                    return Tutorial.IsActive;
                }
            }
        }

        public Action PrepareStateBeginHandler;
        public Action CommandStateBeginHandler;
        public Action MoveStateEndHandler;
        public Action DirectionStateBeginHandler;
        public Action CharacterStateBeginHandler;
        public Action<State> ChangeStateHandler;

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
        public BattleUI BattleUI;
        public DragCameraUI DragCameraUI;
        public BattleResultUI BattleResultUI;
        public SelectCharacterUI SelectBattleCharacterUI;
        private Transform _root;
        private List<CharacterInfo> _candidateList = new List<CharacterInfo>();
        //private Dictionary<int, BattleCharacterController> _controllerDic = new Dictionary<int, BattleCharacterController>();
        private Dictionary<Command, List<BattleCharacterInfo>> _commandTargetDic = new Dictionary<Command, List<BattleCharacterInfo>>();

        public List<Log> LogList = new List<Log>();

        public void Init(int floor, int lv, string tutorial, BattleInfo info, Transform root)
        {
            Info = info;

            if (tutorial!=null && tutorial!="")
            {
                var objectType = Type.GetType("Battle." + tutorial);
                Tutorial = (BattleTutorial)Activator.CreateInstance(objectType);
            }

            _root = root;
            BattleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
            BattleResultUI = GameObject.Find("BattleResultUI").GetComponent<BattleResultUI>();
            _cameraController = Camera.main.GetComponent<CameraDraw>();
            SelectBattleCharacterUI = GameObject.Find("SelectBattleCharacterUI").GetComponent<SelectCharacterUI>();
            CameraRotate = Camera.main.GetComponent<CameraRotate>();
            DragCameraUI = GameObject.Find("DragCameraUI").GetComponent<DragCameraUI>();
            DragCameraUI.Init(info);
            BattleUI.gameObject.SetActive(false);
            BattleResultUI.gameObject.SetActive(false);

            _context.ClearState();
            _context.AddState(new PrepareState(_context));
            _context.AddState(new CharacterState(_context));
            _context.AddState(new CommandState(_context));
            _context.AddState(new MoveState(_context));
            _context.AddState(new TargetState(_context));
            _context.AddState(new ConfirmState(_context));
            _context.AddState(new EffectState(_context));
            _context.AddState(new EndState(_context));
            _context.AddState(new WinState(_context));
            _context.AddState(new LoseState(_context));
            _context.AddState(new DirectionState(_context));
            _context.ChangeStateHandler = OnStateChange;

            CharacterList.Clear();
            for (int i = 0; i < info.EnemyList.Count; i++)
            {
                CharacterList.Add(info.EnemyList[i]);
                BattleUI.SetLittleHpBarAnchor(info.EnemyList[i]);
                BattleUI.SetLittleHpBarValue(info.EnemyList[i]);
                BattleUI.SetFloatingNumberPoolAnchor(info.EnemyList[i]);
                info.EnemyList[i].Controller.MoveEndHandler += OnMoveEnd;
            }

            _candidateList = new List<CharacterInfo>(CharacterManager.Instance.Info.CharacterList);

            if (Tutorial!=null) 
            {
                Tutorial.Start();
            }
            else
            {
                for (int i = 0; i < _candidateList.Count; i++)
                {
                    if (_candidateList[i].CurrentHP == 0)
                    {
                        _candidateList.RemoveAt(i);
                        i--;
                    }
                }
                _context.SetState<PrepareState>();
            }

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

        public BattleCharacterController PlaceCharacter(Vector2Int position, CharacterInfo characterInfo)
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

        //public void SetCharacterState() 
        //{
        //    _context.SetState<CharacterState>();
        //}

        //public void SetActionState()
        //{
        //    _context.SetState<ActionState>();
        //}

        //public void SetMoveState()
        //{
        //    _context.SetState<MoveState>();
        //}

        //public void Idle()
        //{
        //    SelectedCharacter.ActionCount = 0;
        //    _context.SetState<EndState>();
        //}

        //public void SetTargetState(Command command)
        //{
        //    SelectedCharacter.SelectedCommand = command;
        //    _context.SetState<TargetState>();
        //}

        public void SetState<T>() 
        {
            _context.SetState<T>();
        }

        public void SetSelectedCommand(Command command) 
        {
            SelectedCharacter.SelectedCommand = command;
        }

        public void SetQuad(List<Vector2Int> list, Color color)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Info.TileDic[list[i]].TileObject.Quad.gameObject.SetActive(true);
                Info.TileDic[list[i]].TileObject.Quad.material.SetColor("_Color", color);
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
                else if(command.AreaTarget == TargetEnum.UsMinHP)
                {
                    List<BattleCharacterInfo> tempList = new List<BattleCharacterInfo>();
                    for(int i=0; i<CharacterList.Count; i++)
                    {
                        if(SelectedCharacter.Faction == CharacterList[i].Faction)
                        {
                            tempList.Add(CharacterList[i]);  
                        }  
                    }

                    if(tempList.Count>0)
                    {
                        BattleCharacterInfo minHPCharacter = tempList[0];
                        for(int i=1; i<tempList.Count; i++)
                        {
                            if(tempList[i].CurrentHP < minHPCharacter.CurrentHP)
                            {
                                minHPCharacter = tempList[i];
                            }
                        }
                        commandPositionList.Add(Utility.ConvertToVector2Int(minHPCharacter.Position)); 
                    }
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
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in Info.TileDic)
            {
                pair.Value.TileObject.Quad.gameObject.SetActive(false);
                pair.Value.TileObject.Buff.gameObject.SetActive(false);
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
                    _context.SetState<CommandState>();
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
            Instance.BattleUI.SetCharacterInfoUI_2(null);
            for (int i = 0; i < CharacterList.Count; i++)
            {
                if (CharacterList[i] != Instance.SelectedCharacter && position == new Vector2(CharacterList[i].Position.x, CharacterList[i].Position.z))
                {
                    BattleUI.SetCharacterInfoUI_2(CharacterList[i]);
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
            SelectedCharacter.Controller.transform.position = SelectedCharacter.LastPosition;
            SelectedCharacter.LastPosition = BattleCharacterInfo.DefaultLastPosition;
        }

        public void CreateCharacter(int id, int lv, Vector3 position) 
        {
            EnemyModel enemyData = DataContext.Instance.EnemyDic[id];
            BattleCharacterInfo info = new BattleCharacterInfo(lv, enemyData);
            CharacterList.Add(info);
            _maxIndex++;
            info.Index = _maxIndex;
            BattleUI.CharacterListGroup.Add(info);
            Type t = Type.GetType("Battle." + enemyData.AI);
            info.AI = (BattleAI)Activator.CreateInstance(t);
            info.AI.Init(info);
            info.Position = position;
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/Enemy/" + info.Enemy.Controller), Vector3.zero, Quaternion.identity);
            obj.transform.position = info.Position;
            info.Controller = obj.GetComponent<BattleCharacterController>();
            info.Controller.Init(info.Sprite);
            info.Controller.SetAngle();
            info.Controller.MoveEndHandler += OnMoveEnd;
            BattleUI.SetLittleHpBarAnchor(info);
            BattleUI.SetLittleHpBarValue(info);
            BattleUI.SetFloatingNumberPoolAnchor(info);
        }

        public void CreateCharacter(BattleCharacterInfo info, Vector2Int position) 
        {
            info.Position = new Vector3(position.x, Info.TileDic[position].TileData.Height, position.y);
            BattleCharacterController controller = ((GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + info.Job.Controller), Vector3.zero, Quaternion.identity)).GetComponent<BattleCharacterController>();
            controller.transform.position = new Vector3(position.x, Info.TileDic[position].TileData.Height, position.y);
            controller.transform.SetParent(Instance._root);
            controller.Init(info.Sprite);
            controller.SetAngle();
            controller.MoveEndHandler += OnMoveEnd;
            _maxIndex++;
            info.Controller = controller;
            BattleUI.SetLittleHpBarAnchor(info);
            BattleUI.SetLittleHpBarValue(info);
            BattleUI.SetFloatingNumberPoolAnchor(info);
        }

        public void ChangeSprite(BattleCharacterInfo info, string sprite) 
        {
            info.Controller.Init(sprite);
            BattleUI.CharacterListGroup.ChangeSprite(info, sprite);
        }

        public void SortCharacterList(bool isStart)
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

        public void CharacterListGroupInit() 
        {
            BattleUI.CharacterListGroupInit(CharacterList);
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
                        if (Info.TileDic.ContainsKey(position))
                        {
                            Info.TileDic[position].TileObject.Buff.SetActive(true);
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
            if (Tutorial != null)
            {
                Tutorial.Deregister();
                Tutorial = null;
            }
        }

        //public void SetCandidateList(List<CharacterInfo> list) 
        //{
        //    _candidateList = list;
        //}

        private void OnStateChange(State state) 
        {
            if (ChangeStateHandler != null) 
            {
                ChangeStateHandler(state);
            }
        }

        public class BattleControllerState : State
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