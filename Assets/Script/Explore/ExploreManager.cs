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
        public ExploreInfo Info;
        private Generator2D _generator2D = new Generator2D();

        public void Init() 
        {
            if(SystemManager.Instance.SystemInfo.MaxFloor == 0) 
            {
                LoadFile("Floor_1", DataContext.PrePathEnum.MapExplore);
                SystemManager.Instance.SystemInfo.MaxFloor = 1;
            }
            else
            {
                LoadFile(_fileName, DataContext.PrePathEnum.Save);
            }

            //LoadFile(_fileName, DataContext.PrePathEnum.Save);
            //if(File == null && SystemManager.Instance.SystemInfo.MaxFloor == 1) //��l�ɮ�
            //{
            //    LoadFile("Floor_1", DataContext.PrePathEnum.MapExplore);
            //}

            CreateObject();
            SetCamera();
        }

        public void Init(int floor) 
        {
            if (DataContext.Instance.FixedFloorDic.ContainsKey(floor))
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
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(name, pathEnum);
            SystemManager.Instance.SystemInfo.CurrentFloor = file.Floor;
            Info = new ExploreInfo(file);

            ExploreInfoTile tile;
            for (int i = 0; i < file.TileList.Count; i++)
            {
                tile = new ExploreInfoTile();
                if(file.TileList[i].Tag != "Wall") 
                {
                    tile.IsWalkable = false;
                }
                else
                {
                    tile.IsWalkable = true;
                }
                Info.TileDic.Add(file.TileList[i].Position, tile);
            }

            for (int i=0; i<file.TreasureList.Count; i++) 
            {
                Info.TileDic[file.TreasureList[i].Position].Treasure = new ExploreInfoTreasure(file.TreasureList[i]);
                Info.TileDic[file.TreasureList[i].Position].IsWalkable = false;
            }
            
        }

        public void CreateNewFile(int floor, Vector2Int size, int roomCount, Vector2Int roomMaxSize, RandomFloorModel data)
        {
            Info = new ExploreFile();
            Info.Floor = floor;
            _tilePositionList = new List<Vector2Int>();
            _generator2D.Generate(size, roomCount, roomMaxSize, out List<Generator2D.Room> roomList, out List<List<Vector2Int>> pathList);

            for(int i=0; i<roomList.Count; i++)
            {
                foreach (var pos in roomList[i].bounds.allPositionsWithin) 
                {
                    Info.WalkableList.Add(pos);
                    Info.TileList.Add(new ExploreFileTile(pos, "Ground", ""));
                    _tilePositionList.Add(pos);
                }
            }

            for(int i=0; i<pathList.Count; i++)
            {
                for(int j=0; j<pathList[i].Count; j++)
                {
                    if(!_tilePositionList.Contains(pathList[i][j]))
                    {
                        Info.WalkableList.Add(pathList[i][j]);
                        Info.TileList.Add(new ExploreFileTile(pathList[i][j], "Ground", ""));
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
            Info.Size = new Vector2Int(maxX - minX, maxY - minY);

            //PlaceStartAndGoal
            Generator2D.Room startRoom = roomList[0];
            Info.Start = startRoom.bounds.position + new Vector2Int(UnityEngine.Random.Range(0, startRoom.bounds.size.x),UnityEngine.Random.Range(0, startRoom.bounds.size.y));
            startRoom.WalkableList.Remove(Info.Start);

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

            Info.Goal = goalRoom.bounds.position + new Vector2Int(UnityEngine.Random.Range(0, goalRoom.bounds.size.x),UnityEngine.Random.Range(0, goalRoom.bounds.size.y));
            Info.WalkableList.Remove(Info.Goal);
            goalRoom.WalkableList.Remove(Info.Goal);

            //Set Treasure
            int treasureCount = 10;
            Generator2D.Room room;
            ExploreFileTreasure treasure;
            _treasureDic.Clear();
            for (int i = 0; i < treasureCount; i++)
            {
                room = roomList[UnityEngine.Random.Range(0, roomList.Count)];
                if (room.GetRandomPosition(out position))
                {
                    if (!_treasureDic.ContainsKey(position))
                    {
                        room.WalkableList.Remove(position);
                        Info.WalkableList.Remove(position);
                        TreasureModel treasureModel = new TreasureModel();
                        treasureModel.ID = 1;
                        treasureModel.Type = TreasureModel.TypeEnum.Item;
                        treasureModel.Prefab = "TreasureBox";
                        treasureModel.Height = 0.85f;
                        treasureModel.IDList = new List<int>() { 21, 22, 23, 24 };
                        treasureModel.Rotation = "(0, 0, 0)";
                        treasure = new ExploreFileTreasure(position, treasureModel);
                        Info.TreasureList.Add(treasure);
                        _treasureDic.Add(position, treasure);
                    }
                }
            }

            //Set Player
            Info.PlayerPosition = Info.Start;
            Info.PlayerRotation = 0;

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
                    enemy.Prefab = groupData.Explorer;
                    enemy.Position = position;
                    enemy.RotationY = 0;
                    enemy.MapSeed = groupData.MapPool[UnityEngine.Random.Range(0, groupData.MapPool.Count)];
                    enemy.Lv = groupData.Lv;
                    enemy.Exp = groupData.Exp;
                    enemy.EnemyList = groupData.EnemyList;
                    Info.EnemyInfoList.Add(enemy);
                    Info.WalkableList.Remove(position);
                    room.WalkableList.Remove(position);
                }
            }
            groupId = data.BossEnemyGroup;
            groupData = DataContext.Instance.EnemyGroupDic[groupId];
            enemy = new ExploreFileEnemy();
            enemy.AI = ExploreFileEnemy.AiEnum.NotMove;
            enemy.Prefab = groupData.Explorer;
            enemy.Position = Info.Goal;
            enemy.RotationY = 0;
            enemy.MapSeed = groupData.MapPool[UnityEngine.Random.Range(0, groupData.MapPool.Count)];
            enemy.Lv = groupData.Lv;
            enemy.Exp = groupData.Exp;
            enemy.EnemyList = groupData.EnemyList;
            Info.EnemyInfoList.Add(enemy);
        }

        private bool CheckWall(Vector2Int position)
        {
            if(!_tilePositionList.Contains(position))
            {
                Info.TileList.Add(new ExploreFileTile(position, "Wall", "Wall"));
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
                DataContext.Instance.Save(Info, _fileName, DataContext.PrePathEnum.Save);
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
                if (Info.WalkableList.Contains(v2))
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
            if (position.x >= 0 && position.x < Info.Size.x && position.y >= 0 && position.y < Info.Size.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckEnemyCollision(Vector2Int playerPosition) 
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (Utility.ConvertToVector2Int(_enemyList[i].transform.position) == playerPosition)
                {
                    InputMamager.Instance.Lock();
                    Info.EnemyInfoList.Remove(_enemyList[i].File);
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
                                if (_enemyList[i].File.Map != "")
                                {
                                    battleInfo = battleMapBuilder.Get(_enemyList[i].File.Map);
                                    tutorial = _enemyList[i].File.Tutorial;
                                }
                                else
                                {
                                    battleInfo = battleMapBuilder.Generate(_enemyList[i].File.MapSeed, _enemyList[i].File.EnemyGroupId, _enemyList[i].File.Exp);
                                }
                            }
                            else
                            {
                                battleInfo = battleMapBuilder.Generate(_enemyList[i].File.MapSeed, _enemyList[i].File.EnemyList, _enemyList[i].File.Exp);
                            }
                            PathManager.Instance.LoadData(battleInfo.TileDic);
                            BattleController.Instance.Init(Info.Floor, Info.Floor, tutorial, battleInfo, battleMapBuilder.transform);
                        });
                    });

                    return true;
                }
            }
            return false;
        }

        private bool CheckGoal(Vector2Int v2) 
        {
            if (v2 == Info.Goal)
            {
                InputMamager.Instance.Lock();
                ConfirmUI.Open("要前往下一層嗎？", "確定", "取消", () =>
                {
                    Info.Floor++;
                    if (Info.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = Info.Floor;
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
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckEvent(Vector2Int v2) 
        {
            for(int i=0; i<Info.TriggerList.Count; i++)
            {
                if (v2 == Info.TriggerList[i].Position)
                {
                    var objectType = Type.GetType(Info.TriggerList[i].Name);
                    MyEvent myEvent = (MyEvent)Activator.CreateInstance(objectType);
                    myEvent.Start();
                    return true;
                }
            }
            return false;
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
                if (Info.VisitedList.Contains(Utility.ConvertToVector2Int(_enemyList[i].transform.position))) 
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
            if (!Info.VisitedList.Contains(v2) && _tilePositionList.Contains(v2))
            {
                Info.VisitedList.Add(v2);
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
            if (Info.PlayerPosition == Info.Goal) 
            {
                if (Info.Floor == _maxFloor)
                {
                    ConfirmUI.Open("感謝遊玩！", "確認", () =>
                    {
                        Application.Quit();
                    });
                }
                else
                {
                    Info.IsArrive = true;
                    ConfirmUI.Open("你打倒了出口的守衛！要前往下一層嗎？", "確定", "取消", () =>
                    {
                        Info.Floor++;
                        if (Info.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                        {
                            SystemManager.Instance.SystemInfo.MaxFloor = Info.Floor;
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

        public ExploreFileTreasure GetTreasure() 
        {
            ExploreFileTreasure treasure = null;
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
                    Info.TreasureList.Remove(treasure);
                    Info.WalkableList.Add(v2);
                    ItemManager.Instance.AddItem(treasure.ItemID, 1);
                    GameObject.Destroy(_tileDic[v2].Treasure);
                }
            }

            return treasure;
        }

        public void CreateObject() 
        {
            ExploreFileTile tile;
            GameObject gameObj;
            TileObject tileObject;
            Transform parent = GameObject.Find("Generator2D").transform;
            _tileDic.Clear();

            for (int i = parent.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
            }

            for(int i=0; i<Info.TileList.Count; i++)
            {
                tile = Info.TileList[i];
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + tile.Prefab), Vector3.zero, Quaternion.identity);

                if (gameObj != null)
                {
                    gameObj.transform.position = new Vector3(tile.Position.x, 0, tile.Position.y);
                    gameObj.transform.SetParent(parent);
                    tileObject = new TileObject();
                    tileObject.Cube = gameObj;
                    tileObject.Quad = gameObj.transform.GetChild(0).gameObject;

                    if (Info.VisitedList.Contains(tile.Position))
                    {
                        tileObject.Quad.layer = MapLayer;
                    }

                    _tileDic.Add(tile.Position, tileObject);
                }
            }

            ExploreFileTreasure treasure;
            for(int i=0; i<Info.TreasureList.Count; i++)
            {
                treasure = Info.TreasureList[i];
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + treasure.Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(treasure.Position.x, treasure.Height, treasure.Position.y);
                gameObj.transform.eulerAngles = treasure.Rotation;
                gameObj.transform.SetParent(parent);
                _tileDic[treasure.Position].Treasure = gameObj;
                if (gameObj.transform.childCount > 0)
                {
                    _tileDic[treasure.Position].Icon = gameObj.transform.GetChild(0).gameObject;
                    if (Info.VisitedList.Contains(treasure.Position))
                    {
                        _tileDic[treasure.Position].Icon.layer = MapLayer;
                    }
                }
            }

            if (Info.Goal.x != int.MinValue && Info.Goal.y != int.MinValue)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(Info.Goal.x, 0, Info.Goal.y);
                gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
                gameObj.transform.SetParent(parent);
                _tileDic[Info.Goal].Icon = gameObj;
                if (Info.VisitedList.Contains(Info.Goal))
                {
                    _tileDic[Info.Goal].Icon.layer = MapLayer;
                }
                _tileDic[Info.Goal].Icon.GetComponent<Goal>().Red.SetActive(!Info.IsArrive);
                _tileDic[Info.Goal].Icon.GetComponent<Goal>().Blue.SetActive(Info.IsArrive);
            }

            _enemyList.Clear();
            ExploreEnemyController controller;
            for (int i = 0; i < Info.EnemyInfoList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + Info.EnemyInfoList[i].Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(Info.EnemyInfoList[i].Position.x, 1, Info.EnemyInfoList[i].Position.y);
                gameObj.transform.SetParent(parent);
                controller = gameObj.GetComponent<ExploreEnemyController>();
                controller.Init(Info.EnemyInfoList[i]);
                _enemyList.Add(controller);
            }
        }

        public void SetCamera()
        {
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.transform.position = new Vector3(Info.PlayerPosition.x, 1, Info.PlayerPosition.y);
            Player.transform.eulerAngles = new Vector3(0, Info.PlayerRotation, 0);

            float x = 1;
            float y = 1;
            if(Info.Size.x > 60) 
            {
                x = Info.Size.x / 60f;
            }
            if (Info.Size.y > 60)
            {
                y = Info.Size.y / 60f;
            }
            Camera bigMapCamera = GameObject.Find("BigMapCamera").GetComponent<Camera>();
            bigMapCamera.orthographicSize = (bigMapCamera.orthographicSize * x) + 2;
            bigMapCamera.gameObject.SetActive(false);

            ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
            exploreUI.SetCameraPosition(Info.Size.x / 2, Info.Size.y / 2 - 2, x);

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
                if (CheckEnemyCollision(v2))
                {
                    return;
                }
                else if (CheckGoal(v2))
                {
                    return;
                }
                else
                {
                    CheckEvent(v2);
                }
            }
        }

        private void OnPlayerRotate()
        {
            Info.PlayerRotation = (int)Player.transform.eulerAngles.z;
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Rotate();
            }
        }
    }
}
