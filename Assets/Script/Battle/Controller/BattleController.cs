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
        public Action CharacterStateBeginHandler;
        public Action MoveStateBeginHandler;
        public Action RangeStateBeginHandler;
        public Action DirectionStateBeginHandler;
        public Action WinStateBeginHandler;

        public Action PlaceCharacterHandler;
        public Action AfterMoveHandler;

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
        private static readonly Color _red = new Color(1, 1, 0, 0.5f);

        private int _maxIndex = 0;
        private Vector2Int _cameraDefaultPosition; //戰鬥一開始的時候攝影機的位置
        private CameraController _cameraController;
        private StateContext _context = new StateContext();
        public BattleUI BattleUI;
        public CharacterInfoUIGroup CharacterInfoUIGroup;
        public BattleResultUI BattleResultUI;
        public SelectCharacterUI SelectCharacterUI;
        private Transform _root;
        private FileManager _fileLoader;
        private LineSetting _line;
        private List<CharacterInfo> _candidateList = new List<CharacterInfo>();
        private List<BattleCharacterController> _targetList = new List<BattleCharacterController>();
        private List<Vector2Int> _areaList = new List<Vector2Int>();

        public List<Log> LogList = new List<Log>();

        public void Init()
        {
            _maxIndex = 0;
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
            PrepareStateBeginHandler = null;
            CharacterStateBeginHandler = null;
            MoveStateBeginHandler = null;
            RangeStateBeginHandler = null;
            DirectionStateBeginHandler = null;
            WinStateBeginHandler = null;
        }

        public void SetFixed(string tutorial, string map)
        {
            _fileLoader.Load<BattleFileFixed>(map, FileManager.PathEnum.MapBattleFixed, (obj) =>
            {
                BattleFileFixed file = (BattleFileFixed)obj;
                PlayerPositionList = file.PlayerPositionList;
                _cameraDefaultPosition = file.CameraDefaultPosition;
                Exp = file.Exp;
                MinPlayerCount = file.PlayerCount;
                MaxPlayerCount = file.PlayerCount;

                BattleInfoTile tile;
                for (int i = 0; i < file.TileList.Count; i++)
                {
                    tile = new BattleInfoTile();
                    tile.TileData = DataTable.Instance.TileDic[file.TileList[i].ID];
                    TileDic.Add(file.TileList[i].Position, tile);
                }

                for (int i = 0; i < file.EnemyList.Count; i++)
                {
                    EnemyDataList.Add(DataTable.Instance.EnemyDic[file.EnemyList[i].ID]);
                    CreateEnemy(file.EnemyList[i].ID, file.EnemyList[i].Lv, file.EnemyList[i].Position, file.EnemyList[i].Angle);
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

                List<Vector2Int> positionList = new List<Vector2Int>();
                List<Vector2Int> invalidList = new List<Vector2Int>();
                foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic)
                {
                    if (pair.Value.MoveCost == -1)
                    {
                        invalidList.Add(pair.Key);
                    }
                    else if (PlayerPositionList.Contains(pair.Key))
                    {
                        invalidList.Add(pair.Key);
                    }
                    else
                    {
                        positionList.Add(pair.Key);
                    }
                }
                float height;
                Vector2Int center = file.EnemyCenterPosition;
                for (int i = 0; i < enemyGroup.EnemyList.Count; i++)
                {
                    EnemyDataList.Add(DataTable.Instance.EnemyDic[enemyGroup.EnemyList[i]]);
                    position = Utility.GenerateNormalPoints(center, file.StdDev, file.MinX, file.MaxX, file.MinY, file.MaxY, invalidList);
                    height = TileDic[position].TileData.Height * 0.5f + 0.5f;
                    CreateEnemy(enemyGroup.EnemyList[i], enemyGroup.Lv, position, Utility.GetRandomAngle());
                    invalidList.Add(position);
                }

                Start(tutorial);
            });
        }

        private void GetGameObject()
        {
            BattleUI = GameObject.Find("BattleUI").GetComponent<BattleUI>();
            BattleResultUI = GameObject.Find("BattleResultUI").GetComponent<BattleResultUI>();
            SelectCharacterUI = GameObject.Find("SelectBattleCharacterUI").GetComponent<SelectCharacterUI>();
            CharacterInfoUIGroup = GameObject.Find("CharacterInfoUIGroup").GetComponent<CharacterInfoUIGroup>();
            BattleUI.gameObject.SetActive(false);
            BattleResultUI.gameObject.SetActive(false);
            _root = GameObject.Find("BattleController").transform;
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
            //_context.AddState(new CommandState(_context));
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

            for (int i = 0; i < _candidateList.Count; i++)
            {
                if (_candidateList[i].CurrentHP == 0)
                {
                    _candidateList.RemoveAt(i);
                    i--;
                }
            }
            _context.SetState<PrepareState>();

            LogList.Clear();
        }


        public bool PlaceCharacter(BattleCharacterController character)
        {
            if (((PrepareState)_context.CurrentState).PlaceCharacter(character))
            {
                if (PlaceCharacterHandler != null)
                {
                    PlaceCharacterHandler();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void PrepareStateStopDrag()
        {
            ((PrepareState)_context.CurrentState).CanDrag = false;
        }

        public void SetState<T>()
        {
            _context.SetState<T>();
        }

        public void SetMoveState()
        {
            if (Tutorial == null || !Tutorial.IsActive || Tutorial.CanMove)
            {
                _context.SetState<MoveState>();
            }
        }

        public void Move(Vector2Int position)
        {
            ((MoveState)_context.CurrentState).Move(position);
        }

        public void SetSelectedCommand(Skill skill)
        {
            if (Tutorial == null || !Tutorial.IsActive || skill.ID == Tutorial.SkillID)
            {
                SelectedCharacter.Info.SelectedCommand = skill;
                _context.SetState<RangeState>();
            }
        }

        public void SetSelectedCommand(Sub sub)
        {
            if (Tutorial == null || !Tutorial.IsActive || sub.ID == Tutorial.SubID)
            {
                SelectedCharacter.Info.SelectedCommand = sub;
                _context.SetState<RangeState>();
            }
        }

        public void SetSelectedCommand(ItemCommand item)
        {
            if (Tutorial == null || !Tutorial.IsActive || item.ID == Tutorial.ItemID)
            {
                SelectedCharacter.Info.SelectedCommand = item;
                _context.SetState<RangeState>();
            }
        }

        public void SetSelectedCommand(Spell spell)
        {
            if (Tutorial == null || !Tutorial.IsActive || spell.ID == Tutorial.SpellID)
            {
                SelectedCharacter.Info.SelectedCommand = spell;
                _context.SetState<RangeState>();
            }
        }

        public void UseCommand(Vector2Int position, List<Vector2Int> areaList)
        {
            _areaList = areaList;
            if (Tutorial == null || !Tutorial.IsActive || position == Tutorial.CommandPosition)
            {
                SetTargetList(areaList);

                Vector2Int v2 = position - Utility.ConvertToVector2Int(SelectedCharacter.transform.position);
                Vector2Int forward = Utility.ConvertToVector2Int(SelectedCharacter.transform.forward);
                float angle = Mathf.RoundToInt(Vector2.Angle(v2, forward) / 90f) * 90; //取最接近90的倍數的數
                Vector3 cross = Vector3.Cross((Vector2)v2, (Vector2)forward);
                if (cross.z > 0)
                {
                    angle = 360 - angle;
                }
                SelectedCharacter.SetDirection(angle);

                if (SelectedCharacter.Info.SelectedCommand is Sub)
                {
                    _context.SetState<SubState>();
                }
                else if (SelectedCharacter.Info.SelectedCommand is Skill)
                {
                    _context.SetState<SkillState>();
                }
                else if (SelectedCharacter.Info.SelectedCommand is ItemCommand)
                {
                    _context.SetState<ItemState>();
                }
                else if (SelectedCharacter.Info.SelectedCommand is Spell)
                {
                    _context.SetState<SpellState>();
                }
            }
        }

        public BattleCharacterDetailUI OpenCharacterDetail(BattleCharacterInfo character, Vector2Int position)
        {
            if (Tutorial == null || !Tutorial.IsActive)
            {
                return BattleCharacterDetailUI.Open(character, position);
            }
            return null;
        }

        public void SetDirectionState()
        {
            if (Tutorial == null || !Tutorial.IsActive || Tutorial.CanFinish)
            {
                _context.SetState<DirectionState>();
            }
        }

        public void SetDirection(Vector2Int direction)
        {
            if (Tutorial == null || !Tutorial.IsActive || direction == Tutorial.Direction)
            {
                ((DirectionState)_context.CurrentState).SetDirection(direction);
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
            List<Vector2Int> commandPositionList = new List<Vector2Int>();
            if (command.AreaType == AreaTypeEnum.Point)
            {
                commandPositionList.Add(to);
            }
            else if (command.AreaType == AreaTypeEnum.Array)
            {
                commandPositionList = GetNormalAreaList(from, to, command.Target, command.ArrayList);
            }
            else if (command.AreaType == AreaTypeEnum.Global)
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

            if (command.Track == TrackEnum.Through)
            {
                List<Vector2Int> line = Utility.DrawLine2D(from, to);
                line.Remove(from);
                for (int i = 0; i < line.Count; i++)
                {
                    if (!commandPositionList.Contains(line[i]))
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
            for (int i = 0; i < list.Count; i++)
            {
                ClearQuad(list[i]);
            }
        }

        public void Skip()
        {
            _context.SetState<EndState>();
        }

        public void CreateEnemy(int id, int lv, Vector2Int position, float angle)
        {
            EnemyModel enemyData = DataTable.Instance.EnemyDic[id];
            CreateEnemy(enemyData, lv, position, angle);
        }

        public void CreateEnemy(EnemyModel enemyData, int lv, Vector2Int position, float angle)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Battle/" + enemyData.Controller), Vector3.zero, Quaternion.identity);
            obj.transform.position = new Vector3(position.x, TileDic[position].TileData.Height * 0.5f + 0.5f, position.y);
            obj.transform.localEulerAngles = new Vector3(0, angle, 0);
            BattleCharacterController controller = obj.GetComponent<BattleCharacterController>();
            controller.Init(lv, enemyData);
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
                    SetSelect((Vector2Int)_lastPosition, false);
                    _lastPosition = null;
                    BattleUI.SetTileLabel(null);
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
                Instance.SetSelect((Vector2Int)currentPosition, true);
                if (_lastPosition == null || ((Vector2Int)currentPosition != (Vector2Int)_lastPosition))
                {
                    HideLastPosition();
                    _lastPosition = currentPosition;
                    BattleUI.SetTileLabel(TileDic[(Vector2Int)currentPosition]);
                    return true;
                }
            }
            else if (_lastPosition != null)
            {
                SetSelect((Vector2Int)_lastPosition, false);
                _lastPosition = null;
                BattleUI.SetTileLabel(null);
                return true;
            }
            return false;
        }

        public void SetSelect(Vector2Int position, bool show)
        {
            TileDic[position].TileObject.Select.SetActive(show);
        }

        public void HideLastPosition() 
        {
            if (_lastPosition != null)
            {
                SetSelect((Vector2Int)_lastPosition, false);
            }
        }

        public void UseEffect()
        {
            int originalHP = 0;
            BattleCharacterController target = null;
            if (_targetList.Count > 0 && _targetList[0] != SelectedCharacter)
            {
                target = _targetList[0];
                originalHP = target.Info.CurrentHP;
            }

            HitType hitType;
            BattleCharacterController user = SelectedCharacter;
            Command command = user.Info.SelectedCommand;
            Dictionary<BattleCharacterController, List<FloatingNumberData>> _floatingNumberDic = new Dictionary<BattleCharacterController, List<FloatingNumberData>>();
            for (int i = 0; i < _targetList.Count; i++)
            {
                hitType = CheckHit(command.Hit, user, _targetList[i], Tutorial);
                command.Effect.Use(hitType, user, _targetList[i], _floatingNumberDic);
                CheckDeath(_targetList[i]);

                if (command.Shake)
                {
                    _targetList[i].transform.DOShakePosition(0.5f, 0.1f);
                }
            }

            if (command.SubEffect != null)
            {
                command.SubEffect.Use(user, command.SubTarget, _floatingNumberDic, out BattleCharacterController taregt);
                CheckDeath(taregt);
            }

            if (Passive.Contains<MiraclePassive>(user.Info.PassiveList) && command.Effect is RecoverEffect)
            {
                int recover = (int)(user.Info.MaxHP / 10f);
                user.Info.SetRecover(recover);

                if (!_floatingNumberDic.ContainsKey(user))
                {
                    _floatingNumberDic.Add(user, new List<FloatingNumberData>());
                }
                _floatingNumberDic[user].Add(new FloatingNumberData(recover.ToString(), EffectModel.TypeEnum.Recover, HitType.Hit));
            }

            float height;
            GameObject partilce;
            for (int i = 0; i < _areaList.Count; i++)
            {
                partilce = GameObject.Instantiate(Resources.Load("Particle/" + command.Particle), Vector3.zero, Quaternion.identity) as GameObject;
                height = TileDic[_areaList[i]].TileData.Height * 0.5f + 0.5f;
                partilce.transform.position = new Vector3(_areaList[i].x, height, _areaList[i].y);
            }

            int maxCount = -1;
            foreach (KeyValuePair<BattleCharacterController, List<FloatingNumberData>> pair in _floatingNumberDic)
            {
                Instance.BattleUI.PlayFloatingNumberPool(pair.Key, pair.Value);
                if (pair.Value.Count > maxCount)
                {
                    maxCount = pair.Value.Count;
                }
            }
            Timer timer = new Timer();
            timer.Start(maxCount * 0.5f, Instance.CheckResult);

            if (target != null)
            {
                Instance.CharacterInfoUIGroup.SetCharacterInfoUIWithTween_2(target, originalHP, Utility.ConvertToVector2Int(target.transform.position));
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
            Tutorial = null;
            BattleUI.CommandGroup.MainButton.Lock = false;
            BattleUI.CommandGroup.SubButton.Lock = false;
            BattleUI.CommandGroup.ItemButton.Lock = false;
        }

        public void ShowStepList(BattleCharacterController character)
        {
            int distance;
            Vector2Int start = Utility.ConvertToVector2Int(character.transform.position);
            List<Vector2Int> stepList = GetPositionList(character.Info.CurrentMOV, start);
            for (int i = 0; i < stepList.Count; i++)
            {
                distance = GetDistance(start, stepList[i], character.Info.Faction);
                if (distance > character.Info.CurrentMOV || distance == -1)
                {
                    stepList.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < CharacterAliveList.Count; i++)
            {
                if (CharacterAliveList[i] != character)
                {
                    stepList.Remove(Utility.ConvertToVector2Int(CharacterAliveList[i].transform.position));
                }
            }

            for (int i = 0; i < CharacterDyingList.Count; i++)
            {
                if (CharacterDyingList[i] != character)
                {
                    stepList.Remove(Utility.ConvertToVector2Int(CharacterDyingList[i].transform.position));
                }
            }

            character.Info.StepList = stepList;

            SetQuad(character.Info.StepList, _white);
        }

        public void ClearStepList(BattleCharacterController character) 
        {
            ClearQuad(character.Info.StepList);
            character.Info.StepList.Clear();
        }

        public void SetRangeList(BattleCharacterController character, Command command)
        {
            List<Vector2Int> rangeList;
            if (Passive.Contains<ArcherPassive>(character.Info.PassiveList))
            {
                rangeList = ArcherPassive.GetRange(command.Range, Utility.ConvertToVector2Int(character.transform.position));
            }
            else
            {
                rangeList = GetPositionList(command.Range, Utility.ConvertToVector2Int(character.transform.position));
            }

            RemoveRange(command.Target, rangeList);
            SetQuad(rangeList, _yellow);
            character.Info.RangeList = rangeList;
        }

        public void ClearRangeList(BattleCharacterController character) 
        {
            ClearQuad(character.Info.RangeList);
            character.Info.RangeList.Clear();
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

            public virtual void Update() { }
        }
    }
}