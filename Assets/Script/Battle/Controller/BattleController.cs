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

        public BattleTutorial Tutorial = null;
        public BattleCharacterController SelectedCharacter;
        public List<BattleCharacterController> CharacterList = new List<BattleCharacterController>(); //�s��������
        public List<BattleCharacterController> DyingList = new List<BattleCharacterController>(); //�x�����ڤ訤��
        public List<BattleCharacterController> DeadList = new List<BattleCharacterController>(); //���`���ڤ訤��
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

        private readonly Color _white = new Color(1, 1, 1, 0.5f);
        private readonly Color _yellow = new Color(1, 1, 0, 0.5f);

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
        private List<CharacterInfo> _candidateList = new List<CharacterInfo>();
        private Dictionary<Command, List<BattleCharacterController>> _commandTargetDic = new Dictionary<Command, List<BattleCharacterController>>();

        public List<Log> LogList = new List<Log>();

        public void Init(string tutorial, string map) 
        {
            GetGameObject();
            _fileLoader.Load<BattleFileFixed>(map, FileManager.PathEnum.MapBattleFixed, (obj)=> 
            {
                BattleFileFixed file = (BattleFileFixed)obj;
                PlayerPositionList = file.PlayerPositionList;
                Exp = file.Exp;
                MinPlayerCount = file.PlayerCount;
                MaxPlayerCount = file.PlayerCount;
                _maxIndex = 0;
                _canClick = true;
                CharacterList.Clear();
                DyingList.Clear();
                DeadList.Clear();
                EnemyDataList.Clear();

                for (int i = 0; i < file.EnemyList.Count; i++)
                {
                    EnemyDataList.Add(DataTable.Instance.EnemyDic[file.EnemyList[i].ID]);
                    CreateEnemy(file.EnemyList[i].ID, file.EnemyList[i].Lv, file.EnemyList[i].Position);
                }

                BattleInfoTile tile;
                TileDic.Clear();
                for (int i = 0; i < file.TileList.Count; i++)
                {
                    tile = new BattleInfoTile();
                    tile.TileData = DataTable.Instance.TileDic[file.TileList[i].ID];
                    TileDic.Add(file.TileList[i].Position, tile);
                }

                SetMinAndMax();
                CreateObject();
                InitState();
                SetTutorial(tutorial);
            });
        }

        public void Init(string tutorial, EnemyGroupModel enemyGroup) 
        {
            GetGameObject();
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
                _maxIndex = 0;
                _canClick = true;
                CharacterList.Clear();
                DyingList.Clear();
                DeadList.Clear();
                EnemyDataList.Clear();

                Vector2Int position;
                TileModel tileData;
                BattleInfoTile tileInfo;
                AttachModel attachData;
                Queue<Vector2Int> queue = new Queue<Vector2Int>();

                BattleInfoTile tile;
                TileDic.Clear();
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
                    if (pair.Value.MoveCost == 0)
                    {
                        invalidList.Add(pair.Key);
                    }
                }

                for (int i = 0; i < PlayerPositionList.Count; i++)
                {
                    invalidList.Add(PlayerPositionList[i]);
                }

                int height;
                for (int i = 0; i < enemyGroup.EnemyList.Count; i++)
                {
                    if (Utility.GetRandomPosition(file.MinX, file.MaxX, file.MinY, file.MaxY, invalidList, out Vector2Int result))
                    {
                        EnemyDataList.Add(DataTable.Instance.EnemyDic[enemyGroup.EnemyList[i]]);
                        height = TileDic[result].TileData.Height;
                        CreateEnemy(enemyGroup.EnemyList[i], enemyGroup.Lv, new Vector3(result.x, height, result.y));
                        invalidList.Add(result);
                    }
                }

                SetMinAndMax();
                CreateObject();
                InitState();
                SetTutorial(tutorial);
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
            _context.AddState(new TargetState(_context));
            _context.AddState(new ConfirmState(_context));
            _context.AddState(new EffectState(_context));
            _context.AddState(new EndState(_context));
            _context.AddState(new WinState(_context));
            _context.AddState(new LoseState(_context));
            _context.AddState(new DirectionState(_context));
        }

        private void SetTutorial(string tutorial)
        {
            if (tutorial!=null && tutorial!="")
            {
                var objectType = Type.GetType("Battle." + tutorial);
                Tutorial = (BattleTutorial)Activator.CreateInstance(objectType);
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

        public bool PlaceCharacter(BattleCharacterController character)
        {
            if(_context.CurrentState is PrepareState) 
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

        public void SetSelectedCommand(Command command) 
        {
            SelectedCharacter.Info.SelectedCommand = command;
        }

        public void SetQuad(List<Vector2Int> list, Color color)
        {
            for (int i = 0; i < list.Count; i++)
            {
                TileDic[list[i]].TileObject.Quad.gameObject.SetActive(true);
                TileDic[list[i]].TileObject.Quad.material.SetColor("_Color", color);
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
                    List<BattleCharacterController> tempList = new List<BattleCharacterController>();
                    for(int i=0; i<CharacterList.Count; i++)
                    {
                        if(SelectedCharacter.Info.Faction == CharacterList[i].Info.Faction)
                        {
                            tempList.Add(CharacterList[i]);  
                        }  
                    }

                    if(tempList.Count>0)
                    {
                        BattleCharacterController minHPCharacter = tempList[0];
                        for(int i=1; i<tempList.Count; i++)
                        {
                            if(tempList[i].Info.CurrentHP < minHPCharacter.Info.CurrentHP)
                            {
                                minHPCharacter = tempList[i];
                            }
                        }
                        commandPositionList.Add(Utility.ConvertToVector2Int(minHPCharacter.transform.position)); 
                    }
                }
                else
                {
                    for(int i=0; i<CharacterList.Count; i++)
                    {
                        if(command.AreaTarget == TargetEnum.Us && SelectedCharacter.Info.Faction == CharacterList[i].Info.Faction)
                        {
                            commandPositionList.Add(Utility.ConvertToVector2Int(CharacterList[i].transform.position));  
                        }   
                        else if(command.AreaTarget == TargetEnum.Them && SelectedCharacter.Info.Faction != CharacterList[i].Info.Faction)
                        {
                            commandPositionList.Add(Utility.ConvertToVector2Int(CharacterList[i].transform.position)); 
                        }
                        else if(command.AreaTarget == TargetEnum.All)
                        {
                            commandPositionList.Add(Utility.ConvertToVector2Int(CharacterList[i].transform.position));  
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
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic)
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
            SelectedCharacter.Info.ActionCount--;

            if (SelectedCharacter.AI != null)
            {
                SelectedCharacter.AI.OnMoveEnd();
            }
            else
            {
                if (SelectedCharacter.Info.ActionCount > 0)
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
                ((DirectionState)_context.CurrentState).SetDirection(direction);
            }
        }

        public bool SetCharacterInfoUI_2(Vector2 position)
        {
            //��ܨ�����
            Instance.CharacterInfoUIGroup.SetCharacterInfoUI_2(null);
            for (int i = 0; i < CharacterList.Count; i++)
            {
                if (CharacterList[i] != Instance.SelectedCharacter && position == new Vector2(CharacterList[i].transform.position.x, CharacterList[i].transform.position.z))
                {
                    CharacterInfoUIGroup.SetCharacterInfoUI_2(CharacterList[i].Info);
                    ShowTileBuff(CharacterList[i]);
                    ShowRange(CharacterList[i]);
                    return true;
                }
            }
            return false;
        }

        public void ResetAction()
        {
            SelectedCharacter.Info.HasMove = false;
            SelectedCharacter.Info.ActionCount = 2;
            SelectedCharacter.transform.position = SelectedCharacter.LastPosition;
            SelectedCharacter.LastPosition = BattleCharacterInfo.DefaultLastPosition;
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
            BattleCharacterController controller = obj.GetComponent<BattleCharacterController>();
            controller.Init(lv, enemyData);
            controller.MoveEndHandler += OnMoveEnd;
            controller.Outline.OutlineColor = Color.red;
            CharacterList.Add(controller);
            _maxIndex++;
            controller.Index = _maxIndex;
            BattleUI.CharacterListGroup.Add(controller);
            BattleUI.SetLittleHpBarAnchor(controller);
            BattleUI.SetLittleHpBarValue(controller);
            BattleUI.SetFloatingNumberPoolAnchor(controller);
        }

        public void SortCharacterList(bool isStart)
        {
            CharacterList.Sort((x, y) =>
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

        private void ShowTileBuff(BattleCharacterController controller) 
        {
            Vector2Int position;
            for (int i = 0; i < controller.Info.StatusList.Count; i++)
            {
                if (controller.Info.StatusList[i].AreaList.Count > 1)
                {
                    for (int j = 0; j < controller.Info.StatusList[i].AreaList.Count; j++)
                    {
                        position = Utility.ConvertToVector2Int(controller.transform.position) + controller.Info.StatusList[i].AreaList[j];
                        if (TileDic.ContainsKey(position))
                        {
                            TileDic[position].TileObject.Buff.SetActive(true);
                        }
                    }
                }
            }
        }

        public void ShowRange(BattleCharacterController controller) 
        {
            List<Vector2Int> stepList = GetStepList(controller);
            Instance.SetQuad(stepList, _white);
            if (controller.AI != null)
            {
                List<Vector2Int> tempList = new List<Vector2Int>();
                List<Vector2Int> rangeList = new List<Vector2Int>();
                for (int i = 0; i < stepList.Count; i++) //�ڥi�H���ʪ��d��
                {
                    tempList = GetRange(controller.AI.SelectedSkill.Range, stepList[i]);
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

        public class BattleControllerState : State
        {
            protected BattleCharacterController _character;
            protected List<BattleCharacterController> _characterList;
            protected List<BattleCharacterController> _allCharacterList; //Include dying character

            public BattleControllerState(StateContext context) : base(context)
            {
            }

            public virtual void Next() { }

            public virtual void Click(Vector2Int position)
            {
            }

            public List<BattleCharacterController> GetAllCharacterList() 
            {
                List<BattleCharacterController> list = new List<BattleCharacterController>(Instance.CharacterList);
                list.AddRange(Instance.DyingList);
                return list;
            }
        }
    }
}