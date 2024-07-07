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
        public NewExploreFile File;
        private List<ExploreEnemyController> _enemyList = new List<ExploreEnemyController>();

        private Generator2D _generator2D = new Generator2D();
        private List<Vector2Int> _tilePositionList;
        private Dictionary<Vector2Int, TileObject> _tileDic = new Dictionary<Vector2Int, TileObject>();
        public Dictionary<Vector2Int, Treasure> TreasureDic = new Dictionary<Vector2Int, Treasure>();

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
            FloorModel data = DataContext.Instance.FloorDic[floor];
            if (data.Name != "x")
            {
                LoadFile(data.Name, DataContext.PrePathEnum.MapExplore);
            }
            else
            {
                CreateNewFile(new Vector2Int(data.Width, data.Height), data.RoomCount, new Vector2Int(5, 7));
            }
            CreateObject();
            SetCamera();
        }
        private void LoadFile(string name, DataContext.PrePathEnum pathEnum)
        {
            File = DataContext.Instance.Load<NewExploreFile>(name, pathEnum);
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
            }
        }

        public void CreateNewFile(Vector2Int size, int roomCount, Vector2Int roomMaxSize)
        {
            File = new NewExploreFile();
            _tilePositionList = new List<Vector2Int>();
            _generator2D.Generate(size, roomCount, roomMaxSize, out List<Generator2D.Room> roomList, out List<List<Vector2Int>> pathList);

            for(int i=0; i<roomList.Count; i++)
            {
                foreach (var pos in roomList[i].bounds.allPositionsWithin) 
                {
                    File.WalkableList.Add(pos);
                    File.TileList.Add(new NewExploreFile.TileInfo(pos, "Ground", ""));
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
                        File.TileList.Add(new NewExploreFile.TileInfo(pathList[i][j], "Ground", ""));
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

            //PlaceStartAndGoal
            Generator2D.Room startRoom = roomList[0];
            File.Start = startRoom.bounds.position + new Vector2Int(UnityEngine.Random.Range(0, startRoom.bounds.size.x),UnityEngine.Random.Range(0, startRoom.bounds.size.y));
            File.WalkableList.Remove(File.Start);

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

            //Set Treasure
            int treasureCount = 10;
            Generator2D.Room room;
            Treasure treasure;
            Vector2Int position;
            TreasureDic.Clear();
            for (int i = 0; i < treasureCount; i++)
            {
                room = roomList[UnityEngine.Random.Range(0, roomList.Count)];
                position = room.GetRandomPosition();
                File.WalkableList.Remove(position);
                TreasureModel treasureModel = new TreasureModel();
                treasureModel.ID = 1;
                treasureModel.Type = TreasureModel.TypeEnum.Item;
                treasureModel.Prefab = "TreasureBox";
                treasureModel.Height = 0.85f;
                treasureModel.IDList = new List<int>(){21, 22, 23, 24};
                treasure = new Treasure(position, treasureModel);
                File.TreasureList.Add(treasure);
                TreasureDic.Add(position, treasure);
            }

            //Set Enemy
            int enemyCount = 10;
            NewExploreFile.EnemyInfo enemyInfo;
            for (int i = 0; i < enemyCount; i++)
            {
                room = roomList[UnityEngine.Random.Range(0, roomList.Count)];
                position = room.GetRandomPosition();
                enemyInfo = new NewExploreFile.EnemyInfo("NormalAI", null, null, position, 0);
                File.EnemyInfoList.Add(enemyInfo);
                File.WalkableList.Remove(position);
            }
            enemyInfo = new NewExploreFile.EnemyInfo("FloorBOSS", null, null, File.Goal, 0);
            File.EnemyInfoList.Add(enemyInfo);
        }

        private bool CheckWall(Vector2Int position)
        {
            if(!_tilePositionList.Contains(position))
            {
                File.TileList.Add(new NewExploreFile.TileInfo(position, "Wall", "Wall"));
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

        public void CheckCollision() 
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(Player.MoveTo);
            File.PlayerPosition = v2;
            File.PlayerRotation = Mathf.RoundToInt(Camera.main.transform.eulerAngles.y);
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (Utility.ComparePosition(_enemyList[i].MoveTo, Player.MoveTo))
                {
                    InputMamager.Instance.Lock();
                    File.EnemyInfoList.Remove(_enemyList[i].Info);
                    _timer.Start(1f, ()=> 
                    {
                        SceneController.Instance.ChangeScene("Battle", () =>
                        {
                            InputMamager.Instance.Unlock();
                            BattleMapBuilder battleMapBuilder = GameObject.Find("BattleMapBuilder").GetComponent<BattleMapBuilder>();
                            BattleInfo battleInfo;
                            if (_enemyList[i].Info.Map != null && _enemyList[i].Info.Map != "")
                            {
                                battleInfo = battleMapBuilder.Get(_enemyList[i].Info.Map);
                            }
                            else
                            {
                                battleInfo = battleMapBuilder.Generate();
                            }
                            PathManager.Instance.LoadData(battleInfo.TileAttachInfoDic);
                            BattleController.Instance.Init(File.Floor, File.Floor, _enemyList[i].Info.Tutorial, battleInfo, battleMapBuilder.transform);
                        });
                    });

                    return;
                }
            }

            if (v2 == File.Start) 
            {
                InputMamager.Instance.Lock();
                _timer.Start(1f, () => 
                {
                    ConfirmUI.Open("要回到營地嗎？", "確定", "取消", () =>
                    {
                        SceneController.Instance.ChangeScene("Camp", () =>
                        {
                            CharacterManager.Instance.RecoverAllHP();
                            InputMamager.Instance.Unlock();
                        });
                    }, ()=> 
                    {
                        InputMamager.Instance.Unlock();
                    });
                });
            }
            else if(v2 == File.Goal) 
            {
                InputMamager.Instance.Lock();
                ConfirmUI.Open("要前往下一層並回到營地嗎？", "確定", "取消", () =>
                {
                    File.Floor++;
                    if (File.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = File.Floor;
                    }

                    _timer.Start(1f, () =>
                    {
                        SceneController.Instance.ChangeScene("Camp", () =>
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
        }

        private void CheckEvent() 
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(Player.MoveTo);
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
                if (File.VisitedList.Contains(Utility.ConvertToVector2Int(_enemyList[i].MoveTo))) 
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
            CreateObject();
            SetCamera();

            if (File.PlayerPosition == File.Goal) 
            {
                if (File.Floor == _maxFloor)
                {
                    ConfirmUI.Open("�P�¹C���I", "�T�{", () =>
                    {
                        Application.Quit();
                    });
                }
                else
                {
                    _tileDic[File.Goal].Icon.transform.GetChild(0).gameObject.SetActive(true); //��ܲɤl
                    ConfirmUI.Open("�A�w�g���ˤF�X�f���u�áI�n�e���U�@�h�æ^����a�ܡH", "�T�w", "����", () =>
                    {
                        File.Floor++;
                        if (File.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                        {
                            SystemManager.Instance.SystemInfo.MaxFloor = File.Floor;
                        }

                        SceneController.Instance.ChangeScene("Camp", () =>
                        {
                            CharacterManager.Instance.RecoverAllHP();
                        });
                    }, null);
                }
            }
        }

        public Treasure GetTreasure() 
        {
            /*Treasure treasure = null;
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            if (Info.TreasureDic.ContainsKey(v2)) 
            {
                treasure = Info.TreasureDic[v2];
                Info.TreasureDic.Remove(v2);
                Info.WalkableList.Add(v2);

                if (treasure.Type == TreasureModel.TypeEnum.Item)
                {
                    ItemManager.Instance.AddItem(treasure.ID, 1);
                    GameObject.Destroy(Info.TileDic[v2].Treasure);
                }
                else
                {
                    bool bagIsFull = ItemManager.Instance.AddEquip(treasure.ID);
                    if (!bagIsFull) 
                    {
                        GameObject.Destroy(Info.TileDic[v2].Treasure);
                    }
                }
            }

            return treasure;*/
            return null;
        }

        public void CreateObject() 
        {
            NewExploreFile.TileInfo tile;
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
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + treasure.Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(treasure.Position.x, treasure.Height, treasure.Position.y);
                gameObj.transform.eulerAngles = new Vector3(0, 0, treasure.RotationZ);
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

            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Start"), Vector3.zero, Quaternion.identity);
            gameObj.transform.position = new Vector3(File.Start.x, 0, File.Start.y);
            gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
            gameObj.transform.SetParent(parent);
            _tileDic[File.Start].Icon = gameObj;
            if (File.VisitedList.Contains(File.Start))
            {
                _tileDic[File.Start].Icon.layer = MapLayer;
            }

            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
            gameObj.transform.position = new Vector3(File.Goal.x, 0, File.Goal.y);
            gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
            gameObj.transform.SetParent(parent);
            _tileDic[File.Goal].Icon = gameObj;
            if (File.VisitedList.Contains(File.Goal))
            {
                _tileDic[File.Goal].Icon.layer = MapLayer;
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
            Camera.main.transform.position = new Vector3(File.PlayerPosition.x, 1, File.PlayerPosition.y);
            Camera.main.transform.eulerAngles = new Vector3(0, File.PlayerRotation, 0);
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.MoveTo = new Vector3(File.PlayerPosition.x, 1, File.PlayerPosition.y);
            Player.MoveHandler += OnPlayerMove;
            Player.RotateHandler += OnPlayerRotate;
            CheckVidsit(Player.transform);
            CheckEvent();

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
        }

        /*private void SetInfo() 
        {
            Info.PlayerPosition = Utility.ConvertToVector2Int(Player.MoveTo);
            Info.PlayerRotation = Mathf.RoundToInt(Player.transform.eulerAngles.y);

            Info.EnemyInfoList.Clear();
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Info.Prefab = _enemyList[i].Info.Prefab;
                _enemyList[i].Info.Map = _enemyList[i].Info.Map;
                _enemyList[i].Info.Position = Utility.ConvertToVector2Int(_enemyList[i].MoveTo);
                _enemyList[i].Info.Rotation = Mathf.RoundToInt(_enemyList[i].transform.eulerAngles.y);
                Info.EnemyInfoList.Add(_enemyList[i].Info);
            }
        }*/

        private void OnPlayerMove() 
        {
            CheckCollision();
            for (int i=0; i<_enemyList.Count; i++) 
            {
                _enemyList[i].Move();
            }
            CheckCollision();
        }

        private void OnPlayerRotate()
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Rotate();
            }
        }
    }
}
