using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public ExploreInfo Info;
        private List<ExploreEnemyController> _enemyList = new List<ExploreEnemyController>();

        public void Init() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(_fileName, DataContext.PrePathEnum.Save);
            if (file != null)
            {
                Info = new ExploreInfo(file);
            }
            else if(SystemManager.Instance.SystemInfo.MaxFloor == 1) //��l�ɮ�
            {
                file = DataContext.Instance.Load<ExploreFile>("Explore/Floor_1", DataContext.PrePathEnum.Map);
                Info = new ExploreInfo(file);
            }
            else
            {
                Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
                Info = generator2D.Generate(SystemManager.Instance.SystemInfo.MaxFloor);
            }
            Info.SetWalkableList();

            SetObject();
        }

        public void Init(int floor) 
        {
            FloorModel data = DataContext.Instance.FloorDic[floor];
            if (data.Name != "x")
            {
                ExploreFile file = DataContext.Instance.Load<ExploreFile>("Explore/" + data.Name, DataContext.PrePathEnum.Map);
                Info = new ExploreInfo(file);
            }
            else
            {
                Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
                Info = generator2D.Generate(floor);
            }
            Info.SetWalkableList();
            SetObject();
        }

        public void Test() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>("Explore/Floor_1", DataContext.PrePathEnum.Map);
            Info = new ExploreInfo(file);
            SetObject();
        }

        public void Save()
        {
            if (SceneController.Instance.CurrentScene == "Explore")
            {
                SetInfo();
                ExploreFile file = new ExploreFile(Info);
                DataContext.Instance.Save(file, _fileName, DataContext.PrePathEnum.Save);
            }
        }

        public void Delete()
        {
            DataContext.Instance.DeleteData(_fileName, DataContext.PrePathEnum.Save);
        }

        public bool IsWalkable(Vector3 position) //for player
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(position);
            if (InBound(v2))
            {
                if (Info.GroundList.Contains(v2) && !Info.TreasureDic.ContainsKey(v2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsWalkable(ExploreEnemyController enemy, Vector3 position) //for enemy
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(position);
            if (InBound(Utility.ConvertToVector2Int(position)))
            {
                if (Info.GroundList.Contains(v2) && !Info.TreasureDic.ContainsKey(v2))
                {
                    List<ExploreEnemyController> temList = new List<ExploreEnemyController>(_enemyList);
                    temList.Remove(enemy);
                    for(int i=0; i< temList.Count; i++)
                    {
                        if (Utility.ComparePosition(position, temList[i].MoveTo))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsWalkableNew(Vector2Int position)
        {
            return Info.WalkableList.Contains(position);
        }
        public bool IsBlocked(Vector3 position)
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(position);
            if (InBound(v2))
            {
                if (Info.GroundList.Contains(v2))
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

        public void CheckCollision() 
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (Utility.ComparePosition(_enemyList[i].MoveTo, Player.MoveTo))
                {
                    InputMamager.Instance.Lock();
                    SetInfo();
                    Info.EnemyInfoList.Remove(_enemyList[i].Info);
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
                            BattleController.Instance.Init(Info.Floor, Info.Floor, _enemyList[i].Info.Tutorial, battleInfo, battleMapBuilder.transform);
                        });
                    });

                    return;
                }
            }

            Vector2Int v2 = Utility.ConvertToVector2Int(Player.MoveTo);
            if (v2 == Info.Start) 
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
            else if(v2 == Info.Goal) 
            {
                InputMamager.Instance.Lock();
                ConfirmUI.Open("要前往下一層並回到營地嗎？", "確定", "取消", () =>
                {
                    Info.Floor++;
                    if (Info.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = Info.Floor;
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
            foreach (KeyValuePair<Vector2Int, string> pair in Info.TriggerDic)
            {
                if (v2 == pair.Key)
                {
                    var objectType = Type.GetType(pair.Value);
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
                //v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i);
                //CheckVidsit(v2);
                //v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i + transform.right);
                //CheckVidsit(v2);
                //v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i - transform.right);
                //CheckVidsit(v2);
            }

            for (int i=0; i<_enemyList.Count; i++) 
            {
                if (Info.VisitedList.Contains(Utility.ConvertToVector2Int(_enemyList[i].MoveTo))) 
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
            if (!Info.VisitedList.Contains(v2) && Info.TileDic.ContainsKey(v2))
            {
                Info.VisitedList.Add(v2);
                Info.TileDic[v2].Quad.layer = MapLayer;
                if (Info.TileDic[v2].Icon != null)
                {
                    Info.TileDic[v2].Icon.layer = MapLayer;
                }
            }
        }

        public void OpenAllMap() //debug
        {
            foreach (KeyValuePair<Vector2Int, TileObject> pair in Info.TileDic) 
            {
                CheckVidsit(pair.Key);
            }

            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Arrow.layer = MapLayer;
            }
        }

        public void Reload() 
        {
            SetObject();

            if (Info.PlayerPosition == Info.Goal) 
            {
                if (Info.Floor == _maxFloor)
                {
                    ConfirmUI.Open("�P�¹C���I", "�T�{", () =>
                    {
                        Application.Quit();
                    });
                }
                else
                {
                    Info.TileDic[Info.Goal].Icon.transform.GetChild(0).gameObject.SetActive(true); //��ܲɤl
                    ConfirmUI.Open("�A�w�g���ˤF�X�f���u�áI�n�e���U�@�h�æ^����a�ܡH", "�T�w", "����", () =>
                    {
                        Info.Floor++;
                        if (Info.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                        {
                            SystemManager.Instance.SystemInfo.MaxFloor = Info.Floor;
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
            Treasure treasure = null;
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

            return treasure;
        }

        private void SetObject() 
        {
            GameObject obj = null;
            Transform parent = GameObject.Find("Generator2D").transform;
            foreach (KeyValuePair<Vector2Int, TileObject> pair in Info.TileDic)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + pair.Value.Name), Vector3.zero, Quaternion.identity);

                if (obj != null)
                {
                    obj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
                    obj.transform.SetParent(parent);
                    Info.TileDic[pair.Key].Cube = obj;
                    Info.TileDic[pair.Key].Quad = obj.transform.GetChild(0).gameObject;

                    if (Info.VisitedList.Contains(pair.Key))
                    {
                        pair.Value.Quad.layer = MapLayer;
                    }
                }
            }

            foreach (KeyValuePair<Vector2Int, Treasure> pair in Info.TreasureDic)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + pair.Value.Prefab), Vector3.zero, Quaternion.identity);
                obj.transform.position = new Vector3(pair.Key.x, pair.Value.Height, pair.Key.y);
                obj.transform.eulerAngles = new Vector3(0, 0, pair.Value.RotationZ);
                if(pair.Value.Prefab == "SpellCard") 
                {
                    Debug.Log(obj.transform.eulerAngles);
                }
                Info.TileDic[pair.Key].Treasure = obj;
                if (obj.transform.childCount > 0)
                {
                    Info.TileDic[pair.Key].Icon = obj.transform.GetChild(0).gameObject;
                    if (Info.VisitedList.Contains(pair.Key))
                    {
                        Info.TileDic[pair.Key].Icon.layer = MapLayer;
                    }
                }
            }

            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Start"), Vector3.zero, Quaternion.identity);
            obj.transform.position = new Vector3(Info.Start.x, 0, Info.Start.y);
            obj.transform.eulerAngles = new Vector3(90, 0, 0);
            obj.transform.SetParent(parent);
            Info.TileDic[Info.Start].Icon = obj;
            if (Info.VisitedList.Contains(Info.Start))
            {
                Info.TileDic[Info.Start].Icon.layer = MapLayer;
            }

            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
            obj.transform.position = new Vector3(Info.Goal.x, 0, Info.Goal.y);
            obj.transform.eulerAngles = new Vector3(90, 0, 0);
            obj.transform.SetParent(parent);
            Info.TileDic[Info.Goal].Icon = obj;
            if (Info.VisitedList.Contains(Info.Goal))
            {
                Info.TileDic[Info.Goal].Icon.layer = MapLayer;
            }

            _enemyList.Clear();
            ExploreEnemyController controller;
            for (int i = 0; i < Info.EnemyInfoList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + Info.EnemyInfoList[i].Prefab), Vector3.zero, Quaternion.identity);
                //obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/TraceAI"), Vector3.zero, Quaternion.identity);
                controller = obj.GetComponent<ExploreEnemyController>();
                controller.Init(Info.EnemyInfoList[i]);
                _enemyList.Add(controller);
            }

            Camera.main.transform.position = new Vector3(Info.PlayerPosition.x, 1, Info.PlayerPosition.y);
            Camera.main.transform.eulerAngles = new Vector3(0, Info.PlayerRotation, 0);
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.MoveTo = new Vector3(Info.PlayerPosition.x, 1, Info.PlayerPosition.y);
            Player.MoveHandler += OnPlayerMove;
            Player.RotateHandler += OnPlayerRotate;
            CheckVidsit(Player.transform);
            CheckEvent();

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
        }

        private void SetInfo() 
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
        }

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
