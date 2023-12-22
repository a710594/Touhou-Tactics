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
        private List<AI> _enemyList = new List<AI>();

        public void Init() 
        {
            ExploreFile file = DataContext.Instance.Load<ExploreFile>(_fileName, DataContext.PrePathEnum.Save);
            if (file != null)
            {
                _info = new ExploreInfo(file);
                Reload();
            }
            else
            {
                Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
                generator2D.Generate(3);
            }
        }

        public void SetData(ExploreInfo info)
        {
            _info = info;
            _info.PlayerPosition = info.Start;
            _info.PlayerRotation = 0;
            _info.VisitedList.Clear();
            SetObject();
        }

        public void Save()
        {
            if (SceneController.Instance.CurrentScene == "Explore")
            {
                _info.PlayerPosition = Utility.ConvertToVector2Int(Player.MoveTo);
                _info.PlayerRotation = Mathf.RoundToInt(Player.transform.eulerAngles.y);
                for (int i = 0; i < _enemyList.Count; i++)
                {
                    ExploreEnemyInfo info = new ExploreEnemyInfo();
                    info.Prefab = _enemyList[i].Prefab;
                    info.Position = Utility.ConvertToVector2Int(_enemyList[i].MoveTo);
                    info.Rotation = Mathf.RoundToInt(_enemyList[i].transform.eulerAngles.y);
                    _info.EnemyInfoList.Add(info);
                }
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

        public bool IsWalkable(AI enemy, Vector3 position) //for enemy
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(position);
            if (InBound(Utility.ConvertToVector2Int(position)))
            {
                if (_info.WalkableList.Contains(v2) && !_info.TreasureDic.ContainsKey(v2))
                {
                    for (int i = 0; i < _enemyList.Count; i++)
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
                    Player.CanMove = false;
                    _info.PlayerPosition = Utility.ConvertToVector2Int(Player.MoveTo);
                    _info.PlayerRotation = Mathf.RoundToInt(Player.transform.eulerAngles.y);
                    for (int j=0; j<_enemyList.Count; j++) 
                    {
                        if (_enemyList[j] != _enemyList[i]) 
                        {
                            ExploreEnemyInfo info = new ExploreEnemyInfo();
                            info.Prefab = _enemyList[j].Prefab;
                            info.Position = Utility.ConvertToVector2Int(_enemyList[j].MoveTo);
                            info.Rotation = Mathf.RoundToInt(_enemyList[j].transform.eulerAngles.y);
                            _info.EnemyInfoList.Add(info);
                        }
                    }
                    _timer.Start(1f, ()=> 
                    {
                        SceneController.Instance.ChangeScene("Battle", () =>
                        {
                            BattleMapGenerator randomMapGenerator = GameObject.Find("Tilemap").GetComponent<BattleMapGenerator>();
                            randomMapGenerator.Generate(out BattleInfo battleInfo);
                            PathManager.Instance.LoadData(battleInfo.TileInfoDic);
                            BattleController.Instance.Init(_info.Floor, _info.Floor, battleInfo);
                        });
                    });

                    return;
                }
            }

            if(Utility.ConvertToVector2Int(Player.MoveTo) == _info.Start) 
            {
                _timer.Start(1f, () => 
                { 
                    SceneController.Instance.ChangeScene("Camp", () =>
                    {
                    });
                });
            }
            else if(Utility.ConvertToVector2Int(Player.MoveTo) == _info.Goal) 
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
                    });
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
                    _info.CellDic[v2].Quad.layer = MapLayer;
                    if (_info.CellDic[v2].Icon != null)
                    {
                        _info.CellDic[v2].Icon.layer = MapLayer;
                    }
                }
                v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i + transform.right);
                if (!_info.VisitedList.Contains(v2))
                {
                    _info.VisitedList.Add(v2);
                    _info.CellDic[v2].Quad.layer = MapLayer;
                    if (_info.CellDic[v2].Icon != null)
                    {
                        _info.CellDic[v2].Icon.layer = MapLayer;
                    }
                }
                v2 = Utility.ConvertToVector2Int(transform.position + transform.forward * i - transform.right);
                if (!_info.VisitedList.Contains(v2))
                {
                    _info.VisitedList.Add(v2);
                    _info.CellDic[v2].Quad.layer = MapLayer;
                    if (_info.CellDic[v2].Icon != null)
                    {
                        _info.CellDic[v2].Icon.layer = MapLayer;
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
            Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
            generator2D.Relod(_info);
            SetObject();
            if(_info.PlayerPosition == _info.Goal) 
            {
                _info.CellDic[_info.Goal].Icon.transform.GetChild(0).gameObject.SetActive(true); //顯示粒子
                ConfirmUI.Open("你已經打倒了出口的守衛！要前往下一層樓嗎？", "確定", "取消", ()=> 
                {
                    _info.Floor++;
                    if(_info.Floor > SystemManager.Instance.SystemInfo.MaxFloor) 
                    {
                        SystemManager.Instance.SystemInfo.MaxFloor = _info.Floor;
                    }

                    SceneController.Instance.ChangeScene("Camp", () =>
                    {
                    });

                    //SceneController.Instance.ChangeScene("Explore", ()=> 
                    //{
                    //    Generator2D generator2D = (GameObject.Find("Generator2D")).GetComponent<Generator2D>();
                    //    generator2D.Generate(_info.Floor + 1);
                    //});
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
                    GameObject.Destroy(_info.CellDic[v2].Treasure);
                }
                else
                {
                    bool bagIsFull = ItemManager.Instance.AddEquip(treasure.ID);
                    if (!bagIsFull) 
                    {
                        GameObject.Destroy(_info.CellDic[v2].Treasure);
                    }
                }
            }

            return treasure;
        }

        private void SetObject() 
        {
            _enemyList.Clear();
            GameObject obj;
            AI mashroom;
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
                    mashroom = obj.GetComponent<AI>();
                    random = UnityEngine.Random.Range(0, walkableList.Count);
                    mashroom.Init("Mashroom", walkableList[random], 0);
                    walkableList.RemoveAt(random);
                    _enemyList.Add(mashroom);
                }
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Mashroom_NotMove"), Vector3.zero, Quaternion.identity);
                mashroom = obj.GetComponent<AI>();
                mashroom.Init("Mashroom_NotMove", _info.Goal, 0);
                _enemyList.Add(mashroom);
            }
            else 
            {
                for (int i=0; i< _info.EnemyInfoList.Count; i++) 
                {
                    obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + _info.EnemyInfoList[i].Prefab), Vector3.zero, Quaternion.identity);
                    mashroom = obj.GetComponent<AI>();
                    mashroom.Init(_info.EnemyInfoList[i].Prefab, _info.EnemyInfoList[i].Position, _info.EnemyInfoList[i].Rotation);
                    _enemyList.Add(mashroom);
                }
                _info.EnemyInfoList.Clear();
            }

            Camera.main.transform.position = new Vector3(_info.PlayerPosition.x, 1, _info.PlayerPosition.y);
            Camera.main.transform.eulerAngles = new Vector3(0, _info.PlayerRotation, 0);
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.MoveTo = new Vector3(_info.PlayerPosition.x, 1, _info.PlayerPosition.y);
            Player.MoveHandler += OnPlayerMove;
            Player.RotateHandler += OnPlayerRotate;
            CheckVidsit(Player.transform);
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
