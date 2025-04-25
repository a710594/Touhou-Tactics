using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

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

        public BattleTutorial Tutorial = null;
        public BattleCharacterController SelectedCharacter;
        public List<BattleCharacterController> CharacterAliveList = new List<BattleCharacterController>();
        public List<BattleCharacterController> CharacterDyingList = new List<BattleCharacterController>();
        public List<BattleCharacterController> CharacterDeadList = new List<BattleCharacterController>();
        public List<BattleCharacterController> TempList = new List<BattleCharacterController>(); //for PrepareState

        public int MinX;
        public int MaxX;
        public int MinY;
        public int MaxY;
        public int Exp;
        public int MinPlayerCount;
        public int MaxPlayerCount;
        public List<Vector2Int> PlayerPositionList = new List<Vector2Int>();
        public List<EnemyModel> EnemyDataList = new List<EnemyModel>();
        public Dictionary<Vector2Int, BattleInfoTile> TileDic = new Dictionary<Vector2Int, BattleInfoTile>();

        private static readonly Color _white = new Color(1, 1, 1, 0.5f);
        private static readonly Color _yellow = new Color(1, 1, 0, 0.5f);

        private bool _canClick = true;
        private int _maxIndex = 0;
        private Vector2Int _selectedPosition;
        private Vector2Int _cameraDefaultPosition; //戰鬥一開始的時候攝影機的位置
        private CameraDraw _cameraDraw;
        private CameraController _cameraController;
        private StateContext _context = new StateContext();
        public BattleUI BattleUI;
        public CharacterInfoUIGroup CharacterInfoUIGroup;
        public BattleResultUI BattleResultUI;
        public SelectCharacterUI SelectBattleCharacterUI;
        private Transform _root;
        private FileManager _fileLoader;
        private LineSetting _line;
        private List<CharacterInfo> _candidateList = new List<CharacterInfo>();
        private List<BattleCharacterController> _targetList = new List<BattleCharacterController>();

        public List<Log> LogList = new List<Log>();

        public void Init()
        {
            _maxIndex = 0;
            _canClick = true;
            CharacterAliveList.Clear();
            CharacterDyingList.Clear();
            CharacterDeadList.Clear();
            EnemyDataList.Clear();
            TileDic.Clear();
            GetGameObject();
            TimerUpdater.UpdateHandler += Update;
        }

        public void DeInit()
        {
            Instance.BattleUI.gameObject.SetActive(false);
            Instance.BattleResultUI.gameObject.SetActive(true);
            TimerUpdater.UpdateHandler -= Update;
        }

        public void SetFixed(string tutorial, string map)
        {
            _fileLoader.Load<BattleFileFixed>(map, FileManager.PathEnum.MapBattleFixed, (obj) =>
            {
                BattleFileFixed file = (BattleFileFixed)obj;
                PlayerPositionList = file.PlayerPositionList;
                Exp = file.Exp;
                MinPlayerCount = file.PlayerCount;
                MaxPlayerCount = file.PlayerCount;

                for (int i = 0; i < file.EnemyList.Count; i++)
                {
                    EnemyDataList.Add(DataTable.Instance.EnemyDic[file.EnemyList[i].ID]);
                    CreateEnemy(file.EnemyList[i].ID, file.EnemyList[i].Lv, file.EnemyList[i].Position);
                }

                BattleInfoTile tile;
                for (int i = 0; i < file.TileList.Count; i++)
                {
                    tile = new BattleInfoTile();
                    tile.TileData = DataTable.Instance.TileDic[file.TileList[i].ID];
                    TileDic.Add(file.TileList[i].Position, tile);
                }

                Start(tutorial);
            });
        }

        public void SetRandom(string tutorial, EnemyGroupModel enemyGroup)
        {
            _fileLoader.Load<BattleFileRandom>(enemyGroup.GetMap(), FileManager.PathEnum.MapBattleRandom, (obj) =>
            {
                BattleFileRandom file = (BattleFileRandom)obj;
                PlayerPositionList = file.PlayerPositionList;
                _cameraDefaultPosition = file.CameraDefaultPosition;
                Exp = enemyGroup.Exp;
                MinPlayerCount = enemyGroup.MinPlayerCount;
                MaxPlayerCount = enemyGroup.MaxPlayerCount;
                if (CharacterManager.Instance.SurvivalCount() < MaxPlayerCount)
                {
                    MaxPlayerCount = CharacterManager.Instance.SurvivalCount();
                }

                Vector2Int position;
                TileModel tileData;
                BattleInfoTile tileInfo;
                AttachModel attachData;
                Queue<Vector2Int> queue = new Queue<Vector2Int>();

                BattleInfoTile tile;
                for (int i = 0; i < file.TileList.Count; i++)
                {
                    tile = new BattleInfoTile();
                    tile.TileData = DataTable.Instance.TileDic[file.TileList[i].ID];
                    TileDic.Add(file.TileList[i].Position, tile);
                    if (tile.TileData.Enqueue)
                    {
                        queue.Enqueue(file.TileList[i].Position);
                    }
                }

                //BFS
                while (queue.Count != 0)
                {
                    position = queue.Dequeue();
                    if ((position + Vector2Int.up).y <= file.MaxY && !TileDic.ContainsKey(position + Vector2Int.up))
                    {
                        tileData = GetAdjacentTile(TileDic[position].TileData, Vector2Int.up);
                        tileInfo = new BattleInfoTile();
                        tileInfo.TileData = tileData;
                        TileDic.Add(position + Vector2Int.up, tileInfo);
                        if (tileData.Enqueue)
                        {
                            queue.Enqueue(position + Vector2Int.up);
                        }
                    }
                    if ((position + Vector2Int.down).y >= file.MinY && !TileDic.ContainsKey(position + Vector2Int.down))
                    {
                        tileData = GetAdjacentTile(TileDic[position].TileData, Vector2Int.down);
                        tileInfo = new BattleInfoTile();
                        tileInfo.TileData = tileData;
                        TileDic.Add(position + Vector2Int.down, tileInfo);
                        if (tileData.Enqueue)
                        {
                            queue.Enqueue(position + Vector2Int.down);
                        }
                    }
                    if ((position + Vector2Int.left).x >= file.MinX && !TileDic.ContainsKey(position + Vector2Int.left))
                    {
                        tileData = GetAdjacentTile(TileDic[position].TileData, Vector2Int.left);
                        tileInfo = new BattleInfoTile();
                        tileInfo.TileData = tileData;
                        TileDic.Add(position + Vector2Int.left, tileInfo);
                        if (tileData.Enqueue)
                        {
                            queue.Enqueue(position + Vector2Int.left);
                        }
                    }
                    if ((position + Vector2Int.right).x <= file.MaxX && !TileDic.ContainsKey(position + Vector2Int.right))
                    {
                        tileData = GetAdjacentTile(TileDic[position].TileData, Vector2Int.right);
                        tileInfo = new BattleInfoTile();
                        tileInfo.TileData = tileData;
                        TileDic.Add(position + Vector2Int.right, tileInfo);
                        if (tileData.Enqueue)
                        {
                            queue.Enqueue(position + Vector2Int.right);
                        }
                    }
                }

                //Attach
                foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic)
                {
                    if (!file.PlayerPositionList.Contains(pair.Key))
                    {
                        attachData = GetAttachRandomly(pair.Value.TileData);
                        pair.Value.AttachData = attachData;
                    }
                }

                //Life Game
                int count = 5;
                while (count > 0)
                {
                    NextGeneration(file);
                    count--;
                }

                //Create Enemy
                List<Vector2Int> invalidList = new List<Vector2Int>();
                foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic)
                {
                    if (pair.Value.MoveCost == -1)
                    {
                        invalidList.Add(pair.Key);
                    }
                }

                for (int i = 0; i < PlayerPositionList.Count; i++)
                {
                    invalidList.Add(PlayerPositionList[i]);
                }

                float height;
                for (int i = 0; i < enemyGroup.EnemyList.Count; i++)
                {
                    if (Utility.GetRandomPosition(file.MinX, file.MaxX, file.MinY, file.MaxY, invalidList, out Vector2Int result))
                    {
                        EnemyDataList.Add(DataTable.Instance.EnemyDic[enemyGroup.EnemyList[i]]);
                        height = TileDic[result].TileData.Height * 0.5f + 0.5f;
                        CreateEnemy(enemyGroup.EnemyList[i], enemyGroup.Lv, new Vector3(result.x, height, result.y));
                        invalidList.Add(result);
                    }
                }

                Start(tutorial);
            });
        }

        private void GetGameObject()
        {
            BattleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
            BattleResultUI = GameObject.Find("BattleResultUI").GetComponent<BattleResultUI>();
            SelectBattleCharacterUI = GameObject.Find("SelectBattleCharacterUI").GetComponent<SelectCharacterUI>();
            CharacterInfoUIGroup = GameObject.Find("CharacterInfoUIGroup").GetComponent<CharacterInfoUIGroup>();
            BattleUI.gameObject.SetActive(false);
            BattleResultUI.gameObject.SetActive(false);
            _root = GameObject.Find("BattleController").transform;
            _cameraDraw = Camera.main.GetComponent<CameraDraw>();
            _cameraController = Camera.main.GetComponent<CameraController>();
            _fileLoader = GameObject.Find("FileManager").GetComponent<FileManager>();
            _line = GameObject.Find("Line").GetComponent<LineSetting>();
            _line.Hide();
        }

        private void Start(string tutorial)
        {
            SetMinAndMax();
            CreateObject();
            InitState();
            SetTutorial(tutorial);
        }

        private void SetMinAndMax()
        {
            MinX = int.MaxValue;
            MaxX = int.MinValue;
            MinY = int.MaxValue;
            MaxY = int.MinValue;
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic)
            {
                if (pair.Key.x < MinX)
                {
                    MinX = pair.Key.x;
                }
                if (pair.Key.x > MaxX)
                {
                    MaxX = pair.Key.x;
                }
                if (pair.Key.y < MinY)
                {
                    MinY = pair.Key.y;
                }
                if (pair.Key.y > MaxY)
                {
                    MaxY = pair.Key.y;
                }
            }
        }

        private void InitState()
        {
            _context.ClearState();
            _context.AddState(new PrepareState(_context));
            _context.AddState(new CharacterState(_context));
            _context.AddState(new CommandState(_context));
            _context.AddState(new MoveState(_context));
            _context.AddState(new RangeState(_context));
            //_context.AddState(new TargetState(_context));
            //_context.AddState(new ConfirmState(_context));
            //_context.AddState(new EffectState(_context));
            _context.AddState(new EndState(_context));
            _context.AddState(new WinState(_context));
            _context.AddState(new LoseState(_context));
            _context.AddState(new DirectionState(_context));

            _context.AddState(new SubState(_context));
            _context.AddState(new SkillState(_context));
            _context.AddState(new ItemState(_context));
            _context.AddState(new SpellState(_context));
        }

        private void SetTutorial(string tutorial)
        {
            if (tutorial != null && tutorial != "")
            {
                var objectType = Type.GetType("Battle." + tutorial);
                Tutorial = (BattleTutorial)Activator.CreateInstance(objectType);
            }

            _candidateList = new List<CharacterInfo>(CharacterManager.Instance.Info.CharacterList);

            if (Tutorial != null)
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

        public bool PlaceCharacter(BattleCharacterController character)
        {
            if (_context.CurrentState is PrepareState)
            {
                return ((PrepareState)_context.CurrentState).PlaceCharacter(character);
            }
            else
            {
                return false;
            }
        }

        //public void SetCharacterSpriteVisible(CharacterInfo characterInfo, bool isVisible)
        //{
        //    if (_context.CurrentState is PrepareState)
        //    {
        //        ((PrepareState)_context.CurrentState).SetCharacterSpriteVisible(characterInfo, isVisible);
        //    }
        //}

        //public void RemoveCharacterSprite(int jobId)
        //{
        //    if (_context.CurrentState is PrepareState)
        //    {
        //        ((PrepareState)_context.CurrentState).RemoveCharacterSprite(jobId);
        //    }
        //}

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

        public void Move(Vector2Int position, Action callback) 
        {
            if(_context.CurrentState is MoveState) 
            {
                ((MoveState)_context.CurrentState).Move(position, callback);
            }
        }

        public void SetSelectedCommand(Command command) 
        {
            if(_context.CurrentState is CommandState) 
            {
                ((CommandState)_context.CurrentState).SetCommand(command);
            }
        }

        public void SetQuad(List<Vector2Int> list, Color color)
        {
            for (int i = 0; i < list.Count; i++)
            {
                SetQuad(list[i], color);
            }
        }

        public void SetQuad(Vector2Int position, Color color)
        {
            TileDic[position].TileObject.Quad.gameObject.SetActive(true);
            TileDic[position].TileObject.Quad.material.SetColor("_Color", color);
        }

        public List<Vector2Int> GetAreaList(Vector2Int from, Vector2Int to, Command command)
        {
            List<Vector2Int> commandPositionList =new List<Vector2Int>();
            if(command.AreaType == AreaTypeEnum.Point)
            {
                commandPositionList.Add(to);
            }
            else if(command.AreaType == AreaTypeEnum.Array)
            {
                commandPositionList = GetNormalAreaList(from, to, command.Target, command.ArrayList);
            }
            else if(command.AreaType == AreaTypeEnum.Global)
            {
                for (int i = 0; i < CharacterAliveList.Count; i++)
                {
                    if (command.Target == TargetEnum.Us && SelectedCharacter.Info.Faction == CharacterAliveList[i].Info.Faction)
                    {
                        commandPositionList.Add(Utility.ConvertToVector2Int(CharacterAliveList[i].transform.position));
                    }
                    else if (command.Target == TargetEnum.Them && SelectedCharacter.Info.Faction != CharacterAliveList[i].Info.Faction)
                    {
                        commandPositionList.Add(Utility.ConvertToVector2Int(CharacterAliveList[i].transform.position));
                    }
                    else if (command.Target == TargetEnum.All)
                    {
                        commandPositionList.Add(Utility.ConvertToVector2Int(CharacterAliveList[i].transform.position));
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

        public void SetTargetList(List<Vector2Int> areaList) 
        {
            BattleCharacterController character;
            for (int i = 0; i < areaList.Count; i++)
            {
                character = Instance.GetCharacterByPosition(areaList[i]);
                if (character != null)
                {
                    Instance._targetList.Add(character);
                }
            }
        }

        public void ClearQuad(Vector2Int v2)
        {
            TileDic[v2].TileObject.Quad.gameObject.SetActive(false);
        }

        public void ClearQuad(List<Vector2Int> list)
        {
            for (int i=0; i<list.Count; i++) 
            {
                ClearQuad(list[i]);
            }
        }

        public void Skip()
        {
            _context.SetState<EndState>();
        }

        public void OnMoveEnd()
        {
            _canClick = true;

            //if (SelectedCharacter.AI != null)
            //{
            //    SelectedCharacter.AI.OnMoveEnd();
            //}
        }

        public void SetDirection(Vector2Int direction) 
        {
            if(_context.CurrentState is DirectionState)
            {
                ((DirectionState)_context.CurrentState).SetDirection(direction);
            }
        }

        public void CreateEnemy(int id, int lv, Vector3 position) 
        {
            EnemyModel enemyData = DataTable.Instance.EnemyDic[id];
            CreateEnemy(enemyData, lv, position);
        }

        public void CreateEnemy(EnemyModel enemyData, int lv, Vector3 position)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Battle/" + enemyData.Controller), Vector3.zero, Quaternion.identity);
            obj.transform.position = position;
            obj.transform.localEulerAngles = new Vector3(0, -90, 0);
            BattleCharacterController controller = obj.GetComponent<BattleCharacterController>();
            controller.Init(lv, enemyData);
            controller.MoveEndHandler += OnMoveEnd;
            controller.Outline.OutlineColor = Color.red;
            CharacterAliveList.Add(controller);
            _maxIndex++;
            controller.Index = _maxIndex;
            BattleUI.CharacterListGroup.Add(controller);
            BattleUI.SetFloatingNumberPoolAnchor(controller);
        }

        public void SortCharacterList(bool isStart)
        {
            CharacterAliveList.Sort((x, y) =>
            {
                if (isStart && !Passive.Contains<TankPassive>(x.Info.PassiveList) && Passive.Contains<TankPassive>(y.Info.PassiveList))
                {
                    return 1;
                }
                else if (isStart && Passive.Contains<TankPassive>(x.Info.PassiveList) && !Passive.Contains<TankPassive>(y.Info.PassiveList))
                {
                    return -1;
                }
                else 
                {
                    if (x.Info.CurrentWT > y.Info.CurrentWT)
                    {
                        return 1;
                    }
                    else
                    {
                        if (x.Info.CurrentWT == y.Info.CurrentWT)
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


        protected static LayerMask _battleTileLayer = LayerMask.GetMask("BattleTile");
        private Vector2Int? _lastPosition = null;
        public bool UpdatePosition(out Vector2Int? currentPosition) //位置有更新時回傳 true
        {
            currentPosition = null;

            if (EventSystem.current.IsPointerOverGameObject()) //block by UI
            {
                if (_lastPosition != null)
                {
                    _lastPosition = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, _battleTileLayer))
            {
                currentPosition = Utility.ConvertToVector2Int(hit.transform.position);
                if(_lastPosition == null || ((Vector2Int)currentPosition != (Vector2Int)_lastPosition)) 
                {
                    _lastPosition = currentPosition;
                    return true;
                }
            }
            else if(_lastPosition != null)
            {
                _lastPosition = null;
                return true;
            }
            return false;
        }

        public void SetSelect(Vector2Int position, bool show) 
        {
            TileDic[position].TileObject.Select.SetActive(show);
        }

        public void UseEffect(Command command, BattleCharacterController user, BattleCharacterController target, out int count)
        {
            HitType hitType;

            if (command.Target == TargetEnum.Us || Instance.Tutorial != null)
            {
                hitType = HitType.Hit;
            }
            else
            {
                hitType = Instance.CheckHit(command.Hit, user, target);
            }

            List<Log> logList = new List<Log>();
            command.Effect.Use(hitType, user, target, logList);

            for (int i=0; i<logList.Count; i++) 
            {
                Instance.BattleUI.PlayFloatingNumberPool(logList[i].Target, logList[i]);
            }
            count = logList.Count;

            GameObject partilce;
            if (command.Effect.Status != null)
            {
                for (int i = 0; i < command.Effect.Status.AreaList.Count; i++)
                {
                    partilce = GameObject.Instantiate(Resources.Load("Particle/" + command.Particle), Vector3.zero, Quaternion.identity) as GameObject;
                    partilce.transform.position = target.transform.position + new Vector3(command.Effect.Status.AreaList[i].x, 0, command.Effect.Status.AreaList[i].y);
                }
            }
            else
            {
                partilce = GameObject.Instantiate(Resources.Load("Particle/" + command.Particle), Vector3.zero, Quaternion.identity) as GameObject;
                partilce.transform.position = target.transform.position;
            }

            if (command.Shake)
            {
                target.transform.DOShakePosition(0.5f, 0.1f);
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

        private void Update() 
        {
            ((BattleControllerState)_context.CurrentState).Update();
        }


        public class BattleControllerState : State
        {
            protected BattleCharacterController _selectedCharacter;
            protected List<BattleCharacterController> _characterList;
            protected List<BattleCharacterController> _allCharacterList; //Include dying character

            public BattleControllerState(StateContext context) : base(context)
            {
            }

            public virtual void Next() { }

            public virtual void Click(Vector2Int position) { }

            public virtual void Update() { }
        }
    }
}