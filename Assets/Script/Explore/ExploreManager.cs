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

        public ExploreFile File;
        public ExploreInfo Info;
        private LayerMask _mapLayer = LayerMask.NameToLayer("Map");
        private LayerMask _transparentFXLayer = LayerMask.NameToLayer("TransparentFX");

        private Vector3 _playerPosition;
        private Vector3 _playerRotation;
        private Timer _timer = new Timer();
        private Generator2D _generator2D = new Generator2D();

        public void Init() 
        {
            if(SystemManager.Instance.SystemInfo.MaxFloor == 0) 
            {
                File = DataContext.Instance.Load<ExploreFile>("Floor_1", DataContext.PrePathEnum.MapExplore);
                SystemManager.Instance.SystemInfo.CurrentFloor = File.Floor;
                SystemManager.Instance.SystemInfo.MaxFloor = 1;
            }
            else
            {
                File = DataContext.Instance.Load<ExploreFile>(_fileName, DataContext.PrePathEnum.Save);
                SystemManager.Instance.SystemInfo.CurrentFloor = File.Floor;
            }

            Info=ExploreInfoGenerator.Instance.Generate(File);
            CreateObject();
            SetCamera();
        }

        public void Init(int floor) 
        {
            if (DataContext.Instance.FixedFloorDic.ContainsKey(floor))
            {
                FixedFloorModel data = DataContext.Instance.FixedFloorDic[floor];
                File = DataContext.Instance.Load<ExploreFile>(data.Name, DataContext.PrePathEnum.MapExplore);
                SystemManager.Instance.SystemInfo.CurrentFloor = File.Floor;
            }
            else
            {
                RandomFloorModel data = DataContext.Instance.RandomFloorDic[floor];
                File = ExploreFileRandomGenerator.Instance.Create(data);
            }
            
            Info = ExploreInfoGenerator.Instance.Generate(File);
            CreateObject();
            SetCamera();
        }


        public void Save()
        {
            File.PlayerPosition = Utility.ConvertToVector2Int(Info.Player.transform.position);
            File.PlayerRotation = (int)Info.Player.transform.eulerAngles.y;
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
                return !Info.TileDic[v2].IsWalkable;
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
            ExploreInfoEnemy enemy;
            for (int i = 0; i < Info.EnemyList.Count; i++)
            {
                Info.EnemyList[i].Position = Info.EnemyList[i].Controller.transform.position;
                Info.EnemyList[i].Rotation = Info.EnemyList[i].Controller.transform.eulerAngles;
                if (Utility.ConvertToVector2Int(Info.EnemyList[i].Controller.transform.position) == playerPosition)
                {
                    InputMamager.Instance.Lock();
                    _playerPosition = Info.Player.transform.position;
                    _playerRotation = Info.Player.transform.eulerAngles;
                    Info.TileDic[playerPosition].IsWalkable = true;
                    enemy = Info.EnemyList[i];
                    Info.EnemyList.RemoveAt(i);
                    _timer.Start(1f, () =>
                    {
                        SceneController.Instance.ChangeScene("Battle", (sceneName) =>
                        {
                            InputMamager.Instance.Unlock();
                            BattleMapBuilder battleMapBuilder = GameObject.Find("BattleMapBuilder").GetComponent<BattleMapBuilder>();
                            BattleInfo battleInfo;
                            string tutorial = null;
                            if (enemy.Type == ExploreFileEnemy.TypeEnum.Fixed)
                            {
                                if (enemy.Map != "")
                                {
                                    battleInfo = battleMapBuilder.Get(enemy.Map);
                                    tutorial = enemy.Tutorial;
                                }
                                else
                                {
                                    EnemyGroupModel enemyGroup = DataContext.Instance.EnemyGroupDic[enemy.EnemyGroupId];
                                    battleInfo = battleMapBuilder.Generate(enemyGroup.GetMap(), enemyGroup.EnemyList, enemyGroup.Exp);
                                }
                            }
                            else
                            {
                                EnemyGroupModel enemyGroup = DataContext.Instance.EnemyGroupDic[enemy.EnemyGroupId];
                                battleInfo = battleMapBuilder.Generate(enemyGroup.GetMap(), enemyGroup.EnemyList, enemyGroup.Exp);
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
            if(Info.TileDic[v2].Event != null) 
            {
                var objectType = Type.GetType(Info.TileDic[v2].Event);
                MyEvent myEvent = (MyEvent)Activator.CreateInstance(objectType);
                myEvent.Start();
                return true;
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

            for (int i=0; i<Info.EnemyList.Count; i++) 
            {
                v2 = Utility.ConvertToVector2Int(Info.EnemyList[i].Controller.transform.position);
                if (Info.TileDic[v2].IsVisited) 
                {
                    Info.EnemyList[i].Controller.Arrow.layer = _mapLayer;
                }
                else
                {
                    Info.EnemyList[i].Controller.Arrow.layer = _transparentFXLayer;
                }
            }
        }

        private void CheckVidsit(Vector2Int v2) 
        {
            if (Info.TileDic.ContainsKey(v2)) 
            {
                Info.TileDic[v2].IsVisited = true;
                Info.TileDic[v2].Object.Quad.layer = _mapLayer;
                if(Info.TileDic[v2].Object.Icon != null) 
                {
                    Info.TileDic[v2].Object.Icon.layer = _mapLayer;
                }
            }
        }

        public void OpenAllMap() //debug
        {
            foreach(KeyValuePair<Vector2Int, ExploreInfoTile> pair in Info.TileDic) 
            {
                pair.Value.IsVisited = true;
                pair.Value.Object.Quad.layer = _mapLayer;
            }
            Info.TileDic[Info.Goal].Object.Icon.layer = _mapLayer;

            for (int i = 0; i < Info.EnemyList.Count; i++)
            {
                Info.EnemyList[i].Controller.Arrow.layer = _mapLayer;
            }
        }

        public void Reload() 
        {
            CreateObject();
            SetCamera();
            Info.Player.transform.position = _playerPosition;
            Info.Player.transform.eulerAngles = _playerRotation;
            if (Utility.ConvertToVector2Int(_playerPosition) == Info.Goal) 
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
        }

        public bool CheckTreasure(Vector2Int position)
        {
            return Info.TileDic[position].Treasure != null;
        }

        public ExploreInfoTreasure GetTreasure() 
        {
            ExploreInfoTreasure treasure = null;
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            if (CheckTreasure(v2)) 
            {
                treasure = Info.TileDic[v2].Treasure;

                bool bagIsFull = false;
                // if (treasure.Type == TreasureModel.TypeEnum.Equip)
                // {
                //     bagIsFull = ItemManager.Instance.AddEquip(treasure.ItemID);
                // }
                if (!bagIsFull) 
                {
                    Info.TileDic[v2].IsWalkable = true;
                    ItemManager.Instance.AddItem(treasure.ItemID, 1);
                    GameObject.Destroy(Info.TileDic[v2].Treasure.Object.gameObject);
                }
            }

            return treasure;
        }

        public void CreateObject() 
        {
            GameObject gameObj;
            TileObject tileObj;
            TreasureObject treasureObj;
            Transform parent = GameObject.Find("Generator2D").transform;

            for (int i = parent.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
            }

            foreach(KeyValuePair<Vector2Int, ExploreInfoTile> pair in Info.TileDic)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.Prefab), Vector3.zero, Quaternion.identity);
                tileObj = gameObj.GetComponent<TileObject>();
                tileObj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
                tileObj.transform.SetParent(parent);
                if (pair.Value.IsVisited)
                {
                    tileObj.Quad.layer = _mapLayer;
                }
                pair.Value.Object = tileObj;

                if(pair.Value.Treasure!=null)
                {
                    gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + pair.Value.Treasure.Prefab), Vector3.zero, Quaternion.identity);
                    treasureObj = gameObj.GetComponent<TreasureObject>();
                    treasureObj.transform.position = new Vector3(pair.Key.x, pair.Value.Treasure.Height, pair.Key.y);
                    treasureObj.transform.SetParent(parent);
                    if (pair.Value.IsVisited)
                    {
                        treasureObj.Icon.layer = _mapLayer;
                    }
                    pair.Value.Treasure.Object = treasureObj;
                }
            }

            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
            gameObj.transform.position = new Vector3(Info.Goal.x, 0, Info.Goal.y);
            gameObj.transform.eulerAngles = new Vector3(90, 0, 0);
            gameObj.transform.SetParent(parent);
            Info.TileDic[Info.Goal].Object.Icon = gameObj;
            if (Info.TileDic[Info.Goal].IsVisited)
            {
                Info.TileDic[Info.Goal].Object.Icon.layer = _mapLayer;
            }
            Info.TileDic[Info.Goal].Object.Icon.GetComponent<Goal>().Red.SetActive(!Info.IsArrive);
            Info.TileDic[Info.Goal].Object.Icon.GetComponent<Goal>().Blue.SetActive(Info.IsArrive);

            ExploreEnemyController controller;
            for (int i = 0; i < Info.EnemyList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + Info.EnemyList[i].Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = Info.EnemyList[i].Position;
                gameObj.transform.eulerAngles = Info.EnemyList[i].Rotation;
                gameObj.transform.SetParent(parent);
                controller = gameObj.GetComponent<ExploreEnemyController>();
                controller.SetAI(Info.EnemyList[i].AI);
                Info.EnemyList[i].Controller = controller;
            }
        }

        public void SetCamera()
        {
            Info.Player = Camera.main.GetComponent<ExploreCharacterController>();
            Info.Player.transform.position = new Vector3(Info.Start.x, 1, Info.Start.y);
            Info.Player.transform.eulerAngles = new Vector3(0, 0, 0);

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

            Vector2Int v2 = Utility.ConvertToVector2Int(Info.Player.transform.position);
            CheckEvent(v2);
            CheckVidsit(Info.Player.transform);
        }

        private int _enemyMoveCount;
        public void EnemyMove() 
        {
            _enemyMoveCount = 0;
            for (int i = 0; i < Info.EnemyList.Count; i++)
            {
                Info.EnemyList[i].Controller.Move();
            }
        }

        public void WaitForAllMoveComplete() 
        {
            _enemyMoveCount++;
            if (_enemyMoveCount == Info.EnemyList.Count + 1) 
            {
                CheckVidsit(Info.Player.transform);
                Vector2Int v2 = Utility.ConvertToVector2Int(Info.Player.transform.position);
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
    }
}
