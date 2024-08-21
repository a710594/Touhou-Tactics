using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Explore
{
    public partial class ExploreManager
    {
        public Action ReloadHandler;

        private readonly int _maxFloor = 5;
        private readonly string _fileName = "ExploreFile";

        private static ExploreManager _instance;
        public static ExploreManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExploreManager();
                }
                return _instance;
            }
        }

        public Action<Vector2Int> VisitedHandler;

        public ExploreCharacterController Player;
        public LayerMask MapLayer = LayerMask.NameToLayer("Map");
        public LayerMask TransparentFXLayer = LayerMask.NameToLayer("TransparentFX");

        private Timer _timer = new Timer();
        //public ExploreInfo Info;
        public ExploreFile File;
        private List<ExploreEnemyController> _enemyList = new List<ExploreEnemyController>();

        private Generator2D _generator2D = new Generator2D();
        private List<Vector2Int> _tilePositionList;
        private Dictionary<Vector2Int, TileObject> _tileDic = new Dictionary<Vector2Int, TileObject>();
        private Dictionary<Vector2Int, Treasure> _treasureDic = new Dictionary<Vector2Int, Treasure>();

        public void Init() 
        {
            LoadFile(_fileName, DataContext.PrePathEnum.Save);
            if(File == null && SystemManager.Instance.SystemInfo.MaxFloor == 1) //��l�ɮ�
            {
                LoadFile("Floor_1", DataContext.PrePathEnum.MapExplore);
            }
            else
            {
                // Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
                //Info = generator2D.Generate(SystemManager.Instance.SystemInfo.MaxFloor);
            }
            //Info.SetWalkableList();

            CreateObject();
            SetCamera();
        }

        public void Init(int floor) 
        {
            if(DataContext.Instance.FixedFloorDic.ContainsKey(floor))
            {
                FixedFloorModel data = DataContext.Instance.FixedFloorDic[floor];
                LoadFile(data.Name, DataContext.PrePathEnum.MapExplore);                
            }
            else
            {
                RandomFloorModel data = DataContext.Instance.RandomFloorDic[floor];
                CreateNewFile(floor, new Vector2Int(data.Width, data.Height), data.RoomCount, new Vector2Int(5, 7), data);
            }
            
            CreateObject();
            SetCamera();
        }
        private void LoadFile(string name, DataContext.PrePathEnum pathEnum)
        {
            File = DataContext.Instance.Load<ExploreFile>(name, pathEnum);
            if (File != null)
            {
                _tilePositionList = new List<Vector2Int>();
                for (int i = 0; i < File.TileList.Count; i++)
                {
                    _tilePositionList.Add(File.TileList[i].Position);
                    if(File.TileList[i].Tag != "Wall") 
                    {
                        File.WalkableList.Add(File.TileList[i].Position);
                    }
                }

                for (int i=0; i<File.TreasureList.Count; i++) 
                {
                    _treasureDic.Add(File.TreasureList[i].Position, File.TreasureList[i]);
                    File.WalkableList.Remove(File.TreasureList[i].Position);
                }
            }
        }

        public void CreateNewFile(int floor, Vector2Int size, int roomCount, Vector2Int roomMaxSize, RandomFloorModel data)
        {
            File = new ExploreFile();
            File.Floor = floor;
            _tilePositionList = new List<Vector2Int>();
            _generator2D.Generate(size, roomCount, roomMaxSize, out List<Generator2D.Room> roomList, out List<List<Vector2Int>> pathList);

            for(int i=0; i<roomList.Count; i++)
            {
                foreach (var pos in roomList[i].bounds.allPositionsWithin) 
                {
                    File.WalkableList.Add(pos);
                    File.TileList.Add(new ExploreFile.TileInfo(pos, "Ground", ""));
                    _tilePositionList.Add(pos);
                }
            }

            for(int i=0; i<pathList.Count; i++)
            {
                for(int j=0; j<pathList[i].Count; j++)
                {
                    if(!_tilePositionList.Contains(pathList[i][j]))
                    {
                        File.WalkableList.Add(pathList[i][j]);
                        File.TileList.Add(new ExploreFile.TileInfo(pathList[i][j], "Ground", ""));
                        _tilePositionList.Add(pathList[i][j]);     
                    }
                }   
            }

            //PlaceWall
            int x;
            int y;
            int groundCount = _tilePositionList.Count;
            for(int i=0; i<groundCount; i++)
            {
                //左
                x = _tilePositionList[i].x - 1;
                y = _tilePositionList[i].y;
                CheckWall(new Vector2Int(x, y));

                //右
                x = _tilePositionList[i].x + 1;
                y = _tilePositionList[i].y;
                CheckWall(new Vector2Int(x, y));

                //下
                x = _tilePositionList[i].x;
                y = _tilePositionList[i].y - 1;
                CheckWall(new Vector2Int(x, y));

                //上
                x = _tilePositionList[i].x;
                y = _tilePositionList[i].y + 1;
                CheckWall(new Vector2Int(x, y));

                //左下
                x = _tilePositionList[i].x - 1;
                y = _tilePositionList[i].y - 1;
                CheckWall(new Vector2Int(x, y));

                //左上
                x = _tilePositionList[i].x - 1;
                y = _tilePositionList[i].y + 1;
                CheckWall(new Vector2Int(x, y));

                //右下
                x = _tilePositionList[i].x + 1;
                y = _tilePositionList[i].y - 1;
                CheckWall(new Vector2Int(x, y));

                //右上
                x = _tilePositionList[i].x + 1;
                y = _tilePositionList[i].y + 1;
                CheckWall(new Vector2Int(x, y));
            }

            //get size
            int minX = int.MinValue;
            int maxX = int.MinValue;
            int minY = int.MinValue;
            int maxY = int.MinValue;
            Vector2Int position;
            for(int i=0; i< _tilePositionList.Count; i++)
            {
                position = _tilePositionList[i];
                if (minX == int.MinValue || position.x < minX)
                {
                    minX = position.x;
                }
                if (maxX == int.MinValue || position.x > maxX)
                {
                    maxX = position.x;
                }
                if (minY == int.MinValue || position.y < minY)
                {
                    minY = position.y;
                }
                if (maxY == int.MinValue || position.y > maxY)
                {
                    maxY = position.y;
                }
            }
            File.Size = new Vector2Int(maxX - minX, maxY - minY);

            //PlaceStartAndGoal
            Generator2D.Room startRoom = roomList[0];
            File.Start = startRoom.bounds.position + new Vector2Int(UnityEngine.Random.Range(0, startRoom.bounds.size.x),UnityEngine.Random.Range(0, startRoom.bounds.size.y));
            startRoom.WalkableList.Remove(File.Start);

            List<Generator2D.Room> tempList = new List<Generator2D.Room>(roomList);
            tempList.Remove(startRoom);
            Generator2D.Room goalRoom = null;
            for (int i = 0; i < tempList.Count; i++)
            {
                if (goalRoom == null || Vector3.Distance(tempList[i].bounds.center, startRoom.bounds.center) > Vector3.Distance(goalRoom.bounds.center, startRoom.bounds.center))
                {
                    goalRoom = tempList[i];
                }
            }

            File.Goal = goalRoom.bounds.position + new Vector2Int(UnityEngine.Random.Range(0, goalRoom.bounds.size.x),UnityEngine.Random.Range(0, goalRoom.bounds.size.y));
            File.WalkableList.Remove(File.Goal);
            goalRoom.WalkableList.Remove(File.Goal);

            //Set Treasure
            int treasureCount = 10;
            Generator2D.Room room;
            Treasure treasure;
            _treasureDic.Clear();
            for (int i = 0; i < treasureCount; i++)
            {
                room = roomList[UnityEngine.Random.Range(0, roomList.Count)];
                if (room.GetRandomPosition(out position))
                {
                    if (!_treasureDic.ContainsKey(position))
                    {
                        room.WalkableList.Remove(position);
                        File.WalkableList.Remove(position);
                        TreasureModel treasureModel = new TreasureModel();
                        treasureModel.ID = 1;
                        treasureModel.Type = TreasureModel.TypeEnum.Item;
                        treasureModel.Prefab = "TreasureBox";
                        treasureModel.Height = 0.85f;
                        treasureModel.IDList = new List<int>() { 21, 22, 23, 24 };
                        treasureModel.Rotation = "(0, 0, 0)";
                        treasure = new Treasure(position, treasureModel);
                        File.TreasureList.Add(treasure);
                        _treasureDic.Add(position, treasure);
                    }
                }
            }

            //Set Player
            File.PlayerPosition = File.Start;
            File.PlayerRotation = 0;

            //Set Enemy
            int groupId;
            EnemyGroupModel groupData;
            ExploreFileEnemy enemy;
            for (int i = 0; i < data.EnemyCount; i++)
            {
                room = roomList[UnityEngine.Random.Range(0, roomList.Count)];
                if (room.GetRandomPosition(out position))
                {
                    groupId = data.EnemyGroupPool[UnityEngine.Random.Range(0, data.EnemyGroupPool.Count)];
                    groupData = DataContext.Instance.EnemyGroupDic[groupId];
                    enemy = new ExploreFileEnemy();
                    enemy.AI = ExploreFileEnemy.AiEnum.Default;
                    enemy.Prefab = "DefaultAI";
                    enemy.Position = position;
                    enemy.RotationY = 0;
                    enemy.MapSeed = groupData.MapPool[UnityEngine.Random.Range(0, groupData.MapPool.Count)];
                    enemy.Lv = groupData.Lv;
                    enemy.Exp = groupData.Exp;
                    enemy.EnemyList = groupData.EnemyList;
                    File.EnemyInfoList.Add(enemy);
                    File.WalkableList.Remove(position);
                    room.WalkableList.Remove(position);
                }
            }
            groupId = data.BossEnemyGroup;
            groupData = DataContext.Instance.EnemyGroupDic[groupId];
            enemy = new ExploreFileEnemy();
            enemy.AI = ExploreFileEnemy.AiEnum.NotMove;
            enemy.Prefab = "NotMoveAI";
            enemy.Position = File.Goal;
            enemy.RotationY = 0;
            enemy.MapSeed = groupData.MapPool[UnityEngine.Random.Range(0, groupData.MapPool.Count)];
            enemy.Lv = groupData.Lv;
            enemy.Exp = groupData.Exp;
            enemy.EnemyList = groupData.EnemyList;
            File.EnemyInfoList.Add(enemy);
        }

        private bool CheckWall(Vector2Int position)
        {
            if(!_tilePositionList.Contains(position))
            {
                File.TileList.Add(new ExploreFile.TileInfo(position, "Wall", "Wall"));
                _tilePositionList.Add(position);  
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Save()
        {
            if (SceneController.Instance.CurrentScene == "Explore")
            {
                DataContext.Instance.Save(File, _fileName, DataContext.PrePathEnum.Save);
            }
        }

        public void Delete()
        {
            DataContext.Instance.DeleteData(_fileName, DataContext.PrePathEnum.Save);
        }

        public bool IsBlocked(Vector3 position)
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(position);
            if (InBound(v2))
            {
                if (File.WalkableList.Contains(v2))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public bool InBound(Vector2Int position) 
        {
            if (position.x >= 0 && position.x < File.Size.x && position.y >= 0 && position.y < File.Size.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public void CheckCollision(Vector2Int v2) 
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (Utility.ConvertToVector2Int(_enemyList[i].MoveTo) == v2)
                {
                    InputMamager.Instance.Lock();
                    File.EnemyInfoList.Remove(_enemyList[i].Info);
                    _timer.Start(1f, ()=> 
                    {
                        SceneController.Instance.ChangeScene("Battle", (sceneName) =>
                        {
                            InputMamager.Instance.Unlock();
                            BattleMapBuilder battleMapBuilder = GameObject.Find("BattleMapBuilder").GetComponent<BattleMapBuilder>();
                            BattleInfo battleInfo;
                            if (!_enemyList[i].Info.IsSeed)
                            {
                                battleInfo = battleMapBuilder.Get(_enemyList[i].Info.Map);
                            }
                            else
                            {
                                battleInfo = battleMapBuilder.Generate(_enemyList[i].Info.Map);
                            }
                            PathManager.Instance.LoadData(battleInfo.TileAttachInfoDic);
                            BattleController.Instance.Init(File.Floor, File.Floor, _enemyList[i].Info.Tutorial, battleInfo, battleMapBuilder.transform);
                        });
                    });

                    return;
                }
            }

            if (v2 == File.Goal) 
            {
                InputMamager.Instance.Lock();
                ConfirmUI.Open("要前往下一層嗎？", "確定", "取消", () =>
                {
                    File.Floor++;
                    if (File.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = File.Floor;
                    }

                    _timer.Start(1f, () =>
                    {
                        SceneController.Instance.ChangeScene("Camp", (sceneName) =>
                        {
                            CharacterManager.Instance.RecoverAllHP();
                            InputMamager.Instance.Unlock();
                        });
                    });
                }, ()=> 
                {
                    InputMamager.Instance.Unlock();
                });
            }
            else 
            {
                CheckEvent();
            }
        }*/

        public void CheckEnemyCollision(Vector2Int playerPosition) 
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (Utility.ConvertToVector2Int(_enemyList[i].transform.position) == playerPosition)
                {
                    InputMamager.Instance.Lock();
                    File.EnemyInfoList.Remove(_enemyList[i].File);
                    _timer.Start(1f, () =>
                    {
                        SceneController.Instance.ChangeScene("Battle", (sceneName) =>
                        {
                            InputMamager.Instance.Unlock();
                            BattleMapBuilder battleMapBuilder = GameObject.Find("BattleMapBuilder").GetComponent<BattleMapBuilder>();
                            BattleInfo battleInfo;
                            string tutorial = null;
                            if (_enemyList[i].File.Type == ExploreFileEnemy.TypeEnum.Fixed)
                            {
                                battleInfo = battleMapBuilder.Get(_enemyList[i].File.Map);
                                tutorial = _enemyList[i].File.Tutorial;
                            }
                            else
                            {
                                battleInfo = battleMapBuilder.Generate(_enemyList[i].File.MapSeed, _enemyList[i].File.EnemyList, _enemyList[i].File.Lv, _enemyList[i].File.Exp);
                            }
                            PathManager.Instance.LoadData(battleInfo.TileAttachInfoDic);
                            BattleController.Instance.Init(File.Floor, File.Floor, tutorial, battleInfo, battleMapBuilder.transform);
                        });
                    });

                    return;
                }
            }
        }

        private void CheckGoal(Vector2Int v2) 
        {
            if (v2 == File.Goal)
            {
                InputMamager.Instance.Lock();
                ConfirmUI.Open("要前往下一層嗎？", "確定", "取消", () =>
                {
                    File.Floor++;
                    if (File.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = File.Floor;
                    }

                    _timer.Start(1f, () =>
                    {
                        SceneController.Instance.ChangeScene("Camp", (sceneName) =>
                        {
                            CharacterManager.Instance.RecoverAllHP();
                            InputMamager.Instance.Unlock();
                        });
                    });
                }, () =>
                {
                    InputMamager.Instance.Unlock();
                });
            }
        }

        private void CheckEvent(Vector2Int v2) 
        {
            for(int i=0; i<File.TriggerList.Count; i++)
            {
                if (v2 == File.TriggerList[i].Position)
                {
                    var objectType = Type.GetType(File.TriggerList[i].Name);
                    MyEvent myEvent = (MyEvent)Activator.CreateInstance(objectType);
                    myEvent.Start();
                    break;
                }
            }
        }

        public void CheckVidsit(Transform transform) 
        {
            Vector2Int v2;
            for (int i = 0; i <= 2; i++)
            {
                for (int j=-2; j<=2; j++) 
                {
                    v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i + transform.right * j);
                    CheckVidsit(v2);
                }
            }

            for (int i=0; i<_enemyList.Count; i++) 
            {
                if (File.VisitedList.Contains(Utility.ConvertToVector2Int(_enemyList[i].transform.position))) 
                {
                    _enemyList[i].Arrow.layer = MapLayer;
                }
                else
                {
                    _enemyList[i].Arrow.layer = TransparentFXLayer;
                }
            }
        }

        private void CheckVidsit(Vector2Int v2) 
        {
            if (!File.VisitedList.Contains(v2) && _tilePositionList.Contains(v2))
            {
                File.VisitedList.Add(v2);
                _tileDic[v2].Quad.layer = MapLayer;
                if (_tileDic[v2].Icon != null)
                {
                    _tileDic[v2].Icon.layer = MapLayer;
                }
            }
        }

        public void OpenAllMap() //debug
        {
            for(int i=0; i<_tilePositionList.Count; i++)
            {
                CheckVidsit(_tilePositionList[i]);
            }

            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Arrow.layer = MapLayer;
            }
        }

        public void Reload() 
        {
            if (File.PlayerPosition == File.Goal) 
            {
                if (File.Floor == _maxFloor)
                {
                    ConfirmUI.Open("感謝遊玩！", "確認", () =>
                    {
                        Application.Quit();
                    });
                }
                else
                {
                    File.IsArrive = true;
                    ConfirmUI.Open("你打倒了出口的守衛！要前往下一層嗎？", "確定", "取消", () =>
                    {
                        File.Floor++;
                        if (File.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                        {
                            SystemManager.Instance.SystemInfo.MaxFloor = File.Floor;
                        }

                        SceneController.Instance.ChangeScene("Camp", (sceneName) =>
                        {
                            CharacterManager.Instance.RecoverAllHP();
                        });
                    }, null);
                }
            }

            if (ReloadHandler != null) 
            {
                ReloadHandler();
            }

            CreateObject();
            SetCamera();
        }

        public bool CheckTreasure(Vector2Int position)
        {
            return _treasureDic.ContainsKey(position);
        }

        public Treasure GetTreasure() 
        {
            Treasure treasure = null;
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            if (_treasureDic.ContainsKey(v2)) 
            {
                treasure = _treasureDic[v2];

                bool bagIsFull = false;
                if (treasure.Type == TreasureModel.TypeEnum.Equip)
                {
                    bagIsFull = ItemManager.Instance.AddEquip(treasure.ItemID);
                }
                if (!bagIsFull) 
                {
                    _treasureDic.Remove(v2);
                    File.TreasureList.Remove(treasure);
                    File.WalkableList.Add(v2);
                    ItemManager.Instance.AddItem(treasure.ItemID, 1);
                    GameObject.Destroy(_tileDic[v2].Treasure);
                }
            }

            return treasure;
        }

        public void CreateObject() 
        {
            ExploreFile.TileInfo tile;
            GameObject gameObj;
            TileObject tileObject;
            Transform parent = GameObject.Find("Generator2D").transform;
            _tileDic.Clear();

            for (int i = parent.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
            }

            for(int i=0; i<File.TileList.Count; i++)
            {
                tile = File.TileList[i];
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + tile.Prefab), Vector3.zero, Quaternion.identity);

                if (gameObj != null)
                {
                    gameObj.transform.position = new Vector3(tile.Position.x, 0, tile.Position.y);
                    gameObj.transform.SetParent(parent);
                    tileObject = new TileObject();
                    tileObject.Cube = gameObj;
                    tileObject.Quad = gameObj.transform.GetChild(0).gameObject;

                    if (File.VisitedList.Contains(tile.Position))
                    {
                        tileObject.Quad.layer = MapLayer;
                    }

                    _tileDic.Add(tile.Position, tileObject);
                }
            }

            Treasure treasure;
            for(int i=0; i<File.TreasureList.Count; i++)
            {
                treasure = File.TreasureList[i];
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + treasure.Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(treasure.Position.x, treasure.Height, treasure.Position.y);
                gameObj.transform.eulerAngles = treasure.Rotation;
                gameObj.transform.SetParent(parent);
                _tileDic[treasure.Position].Treasure = gameObj;
                if (gameObj.transform.childCount > 0)
                {
                    _tileDic[treasure.Position].Icon = gameObj.transform.GetChild(0).gameObject;
                    if (File.VisitedList.Contains(treasure.Position))
                    {
                        _tileDic[treasure.Position].Icon.layer = MapLayer;
                    }
                }
            }

            if (File.Goal.x != int.MinValue && File.Goal.y != int.MinValue)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(File.Goal.x, 0, File.Goal.y);
                gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
                gameObj.transform.SetParent(parent);
                _tileDic[File.Goal].Icon = gameObj;
                if (File.VisitedList.Contains(File.Goal))
                {
                    _tileDic[File.Goal].Icon.layer = MapLayer;
                }
                _tileDic[File.Goal].Icon.GetComponent<Goal>().Red.SetActive(!File.IsArrive);
                _tileDic[File.Goal].Icon.GetComponent<Goal>().Blue.SetActive(File.IsArrive);
            }

            _enemyList.Clear();
            ExploreEnemyController controller;
            for (int i = 0; i < File.EnemyInfoList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + File.EnemyInfoList[i].Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(File.EnemyInfoList[i].Position.x, 1, File.EnemyInfoList[i].Position.y);
                gameObj.transform.SetParent(parent);
                controller = gameObj.GetComponent<ExploreEnemyController>();
                controller.Init(File.EnemyInfoList[i]);
                _enemyList.Add(controller);
            }
        }

        public void SetCamera()
        {
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.transform.position = new Vector3(File.PlayerPosition.x, 1, File.PlayerPosition.y);
            Player.transform.eulerAngles = new Vector3(0, File.PlayerRotation, 0);

            float x = 1;
            float y = 1;
            if(File.Size.x > 60) 
            {
                x = File.Size.x / 60f;
            }
            if (File.Size.y > 60)
            {
                y = File.Size.y / 60f;
            }
            Camera bigMapCamera = GameObject.Find("BigMapCamera").GetComponent<Camera>();
            bigMapCamera.orthographicSize = (bigMapCamera.orthographicSize * x) + 2;
            bigMapCamera.gameObject.SetActive(false);

            ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
            exploreUI.SetCameraPosition(File.Size.x / 2, File.Size.y / 2 - 2, x);

            Vector2Int v2 = Utility.ConvertToVector2Int(Player.transform.position);
            CheckEvent(v2);
            CheckVidsit(Player.transform);
        }

        private int _enemyMoveCount;
        public void EnemyMove() 
        {
            _enemyMoveCount = 0;
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Move();
            }
        }

        public void WaitForAllMoveComplete() 
        {
            _enemyMoveCount++;
            if (_enemyMoveCount == _enemyList.Count + 1) 
            {
                CheckVidsit(Player.transform);
                Vector2Int v2 = Utility.ConvertToVector2Int(Player.transform.position);
                CheckEnemyCollision(v2);
                CheckGoal(v2);
                CheckEvent(v2);
            }
        }

        private void OnPlayerRotate()
        {
            File.PlayerRotation = (int)Player.transform.eulerAngles.z;
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Rotate();
            }
        }
    }
}
