using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreManager
    {
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
        private ExploreInfo _info;
        private List<ExploreEnemyController> _enemyList = new List<ExploreEnemyController>();

        public void Init() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(_fileName, DataContext.PrePathEnum.Save);
            if (file != null)
            {
                _info = new ExploreInfo(file);
            }
            else if(SystemManager.Instance.SystemInfo.MaxFloor == 1)
            {
                file = DataContext.Instance.Load<ExploreFile>("Explore/Floor_1", DataContext.PrePathEnum.Map);
                _info = new ExploreInfo(file);
            }
            else
            {
                Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
                _info = generator2D.Generate(SystemManager.Instance.SystemInfo.MaxFloor);
            }

            SetObject();
        }

        public void Init(int floor) 
        {
            Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
            _info = generator2D.Generate(floor);
            SetObject();
        }

        public void Test() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>("Explore/Floor_1", DataContext.PrePathEnum.Map);
            _info = new ExploreInfo(file);
            SetObject();
        }

        public void Save()
        {
            if (SceneController.Instance.CurrentScene == "Explore")
            {
                SetInfo();
                ExploreFile file = new ExploreFile(_info);
                DataContext.Instance.Save(file, _fileName, DataContext.PrePathEnum.Save);
            }
        }

        public bool IsWalkable(Vector3 position) //for player
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(position);
            if (InBound(v2))
            {
                if (_info.WalkableList.Contains(v2) && !_info.TreasureDic.ContainsKey(v2))
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
                if (_info.WalkableList.Contains(v2) && !_info.TreasureDic.ContainsKey(v2))
                {
                    for(int i=0; i<_enemyList.Count; i++)
                    {
                        if (enemy != _enemyList[i] && Utility.ComparePosition(position, _enemyList[i].MoveTo))
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

        public bool InBound(Vector2Int position) 
        {
            if (position.x >= 0 && position.x < _info.Size.x && position.y >= 0 && position.y < _info.Size.y)
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
                    _info.EnemyInfoList.Remove(_enemyList[i].Info);
                    _timer.Start(1f, ()=> 
                    {
                        SceneController.Instance.ChangeScene("Battle", () =>
                        {
                            InputMamager.Instance.Unlock();
                            BattleMapBuilder battleMapBuilder = GameObject.Find("BattleMapGenerator").GetComponent<BattleMapBuilder>();
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
                            BattleController.Instance.Init(_info.Floor, _info.Floor, battleInfo);
                        });
                    });

                    return;
                }
            }

            if(Utility.ConvertToVector2Int(Player.MoveTo) == _info.Start) 
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
            else if(Utility.ConvertToVector2Int(Player.MoveTo) == _info.Goal) 
            {
                InputMamager.Instance.Lock();
                ConfirmUI.Open("要前往下一層並回到營地嗎？", "確定", "取消", () =>
                {
                    _info.Floor++;
                    if (_info.Floor > SystemManager.Instance.SystemInfo.MaxFloor)
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = _info.Floor;
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
        }

        public void CheckVidsit(Transform transform) 
        {
            Vector2Int v2;
            for (int i = 0; i <= 1; i++)
            {
                v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i);
                if (!_info.VisitedList.Contains(v2))
                {
                    _info.VisitedList.Add(v2);
                    _info.TileDic[v2].Quad.layer = MapLayer;
                    if (_info.TileDic[v2].Icon != null)
                    {
                        _info.TileDic[v2].Icon.layer = MapLayer;
                    }
                }
                v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i + transform.right);
                if (!_info.VisitedList.Contains(v2))
                {
                    _info.VisitedList.Add(v2);
                    _info.TileDic[v2].Quad.layer = MapLayer;
                    if (_info.TileDic[v2].Icon != null)
                    {
                        _info.TileDic[v2].Icon.layer = MapLayer;
                    }
                }
                v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i - transform.right);
                if (!_info.VisitedList.Contains(v2))
                {
                    _info.VisitedList.Add(v2);
                    _info.TileDic[v2].Quad.layer = MapLayer;
                    if (_info.TileDic[v2].Icon != null)
                    {
                        _info.TileDic[v2].Icon.layer = MapLayer;
                    }
                }
            }

            for (int i=0; i<_enemyList.Count; i++) 
            {
                if (_info.VisitedList.Contains(Utility.ConvertToVector2Int(_enemyList[i].MoveTo))) 
                {
                    _enemyList[i].Arrow.layer = MapLayer;
                }
                else
                {
                    _enemyList[i].Arrow.layer = TransparentFXLayer;
                }
            }
        }

        public void Reload() 
        {
            SetObject();

            //foreach (KeyValuePair<Vector2Int, TileObject> pair in _info.TileDic)
            //{
            //    if (_info.VisitedList.Contains(pair.Key))
            //    {
            //        if (pair.Value.Quad != null)
            //        {
            //            pair.Value.Quad.layer = MapLayer;
            //        }
            //        if (pair.Value.Icon != null)
            //        {
            //            pair.Value.Icon.layer = MapLayer;
            //        }
            //    }
            //}

            if (_info.PlayerPosition == _info.Goal) 
            {
                _info.TileDic[_info.Goal].Icon.transform.GetChild(0).gameObject.SetActive(true); //顯示粒子
                ConfirmUI.Open("你已經打倒了出口的守衛！要前往下一層並回到營地嗎？", "確定", "取消", ()=> 
                {
                    _info.Floor++;
                    if(_info.Floor > SystemManager.Instance.SystemInfo.MaxFloor) 
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = _info.Floor;
                    }

                    SceneController.Instance.ChangeScene("Camp", () =>
                    {
                        CharacterManager.Instance.RecoverAllHP();
                    });
                }, null);
            }
        }

        public Treasure GetTreasure() 
        {
            Treasure treasure = null;
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            Vector3Int v3 = new Vector3Int(v2.x, 1, v2.y);
            if (_info.TreasureDic.ContainsKey(v2)) 
            {
                treasure = _info.TreasureDic[v2];
                _info.TreasureDic.Remove(v2);

                if (treasure.Type == TreasureModel.TypeEnum.Item)
                {
                    ItemManager.Instance.AddItem(treasure.ID, 1);
                    GameObject.Destroy(_info.TileDic[v2].Treasure);
                }
                else
                {
                    bool bagIsFull = ItemManager.Instance.AddEquip(treasure.ID);
                    if (!bagIsFull) 
                    {
                        GameObject.Destroy(_info.TileDic[v2].Treasure);
                    }
                }
            }

            return treasure;
        }

        private void SetObject() 
        {
            GameObject obj = null;
            Transform parent = GameObject.Find("Generator2D").transform;
            foreach (KeyValuePair<Vector2Int, TileObject> pair in _info.TileDic)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + pair.Value.Name), Vector3.zero, Quaternion.identity);

                if (obj != null)
                {
                    obj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
                    obj.transform.SetParent(parent);
                    _info.TileDic[pair.Key].Cube = obj;
                    _info.TileDic[pair.Key].Quad = obj.transform.GetChild(0).gameObject;

                    if (_info.VisitedList.Contains(pair.Key))
                    {
                        pair.Value.Quad.layer = MapLayer;
                    }
                }
            }

            foreach (KeyValuePair<Vector2Int, Treasure> pair in _info.TreasureDic)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + pair.Value.Prefab), Vector3.zero, Quaternion.identity);
                obj.transform.position = new Vector3(pair.Key.x, pair.Value.Height, pair.Key.y);
                _info.TileDic[pair.Key].Treasure = obj;
                if (obj.transform.childCount > 0)
                {
                    _info.TileDic[pair.Key].Icon = obj.transform.GetChild(0).gameObject;
                    if (_info.VisitedList.Contains(pair.Key))
                    {
                        _info.TileDic[pair.Key].Icon.layer = MapLayer;
                    }
                }
            }

            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Start"), Vector3.zero, Quaternion.identity);
            obj.transform.position = new Vector3(_info.Start.x, 0, _info.Start.y);
            obj.transform.eulerAngles = new Vector3(90, 0, 0);
            obj.transform.SetParent(parent);
            _info.TileDic[_info.Start].Icon = obj;
            if (_info.VisitedList.Contains(_info.Start))
            {
                _info.TileDic[_info.Start].Icon.layer = MapLayer;
            }

            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
            obj.transform.position = new Vector3(_info.Goal.x, 0, _info.Goal.y);
            obj.transform.eulerAngles = new Vector3(90, 0, 0);
            obj.transform.SetParent(parent);
            _info.TileDic[_info.Goal].Icon = obj;
            if (_info.VisitedList.Contains(_info.Goal))
            {
                _info.TileDic[_info.Goal].Icon.layer = MapLayer;
            }

            _enemyList.Clear();
            ExploreEnemyInfo info;
            ExploreEnemyController controller;
            if (_info.EnemyInfoList.Count == 0)
            {
                int random;
                List<Vector2Int> walkableList = new List<Vector2Int>(_info.WalkableList);
                walkableList.Remove(_info.Start);
                walkableList.Remove(_info.Goal);
                foreach (KeyValuePair<Vector2Int, Treasure> pair in _info.TreasureDic)
                {
                    walkableList.Remove(pair.Key);
                }

                for (int i = 0; i < 5; i++)
                {
                    obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Mashroom"), Vector3.zero, Quaternion.identity);
                    controller = obj.GetComponent<ExploreEnemyController>();
                    random = UnityEngine.Random.Range(0, walkableList.Count);
                    info = new ExploreEnemyInfo("Mashroom", null, walkableList[random], 0);
                    controller.Init(info);
                    walkableList.RemoveAt(random);
                    _enemyList.Add(controller);
                }
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Mashroom_NotMove"), Vector3.zero, Quaternion.identity);
                controller = obj.GetComponent<ExploreEnemyController>();
                info = new ExploreEnemyInfo("FloorBOSS", null, _info.Goal, 0);
                controller.Init(info);
                _enemyList.Add(controller);
            }
            else 
            {
                for (int i=0; i< _info.EnemyInfoList.Count; i++) 
                {
                    obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + _info.EnemyInfoList[i].Prefab), Vector3.zero, Quaternion.identity);
                    controller = obj.GetComponent<ExploreEnemyController>();
                    controller.Init(_info.EnemyInfoList[i]);
                    _enemyList.Add(controller);
                }
            }

            Camera.main.transform.position = new Vector3(_info.PlayerPosition.x, 1, _info.PlayerPosition.y);
            Camera.main.transform.eulerAngles = new Vector3(0, _info.PlayerRotation, 0);
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.MoveTo = new Vector3(_info.PlayerPosition.x, 1, _info.PlayerPosition.y);
            Player.MoveHandler += OnPlayerMove;
            Player.RotateHandler += OnPlayerRotate;
            CheckVidsit(Player.transform);
        }

        private void SetInfo() 
        {
            _info.PlayerPosition = Utility.ConvertToVector2Int(Player.MoveTo);
            _info.PlayerRotation = Mathf.RoundToInt(Player.transform.eulerAngles.y);

            _info.EnemyInfoList.Clear();
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].Info.Prefab = _enemyList[i].Info.Prefab;
                _enemyList[i].Info.Map = _enemyList[i].Info.Map;
                _enemyList[i].Info.Position = Utility.ConvertToVector2Int(_enemyList[i].MoveTo);
                _enemyList[i].Info.Rotation = Mathf.RoundToInt(_enemyList[i].transform.eulerAngles.y);
                _info.EnemyInfoList.Add(_enemyList[i].Info);
            }
        }

        private void OnPlayerMove() 
        {
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
