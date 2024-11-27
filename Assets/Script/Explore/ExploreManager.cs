using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public partial class ExploreManager
    {
        public Action ReloadHandler;

        private readonly int _maxFloor = 4;
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
        public ExploreCharacterController Player;
        //public ExploreInfo Info;
        public Dictionary<Vector2Int, ExploreFileTile> TileDic;
        public List<ExploreEnemyController> EnemyList;
        private LayerMask _mapLayer = LayerMask.NameToLayer("Map");
        private LayerMask _transparentFXLayer = LayerMask.NameToLayer("TransparentFX");

        private bool _hasCollision = false;
        private Timer _timer = new Timer();
        private Generator2D _generator2D = new Generator2D();

        public void Init() 
        {
            ExploreMapBuilder exploreMapBuilder = GameObject.Find("ExploreMapBuilder").GetComponent<ExploreMapBuilder>();
            if (SystemManager.Instance.Info.MaxFloor == 0) 
            {
                File = DataContext.Instance.Load<ExploreFile>("Floor_1", DataContext.PrePathEnum.MapExplore);
                SystemManager.Instance.Info.CurrentFloor = File.Floor;
                SystemManager.Instance.Info.MaxFloor = 1;
                exploreMapBuilder.GetFixed(File, out TileDic, out EnemyList);
            }
            else
            {
                File = DataContext.Instance.Load<ExploreFile>(_fileName, DataContext.PrePathEnum.Save);
                SystemManager.Instance.Info.CurrentFloor = File.Floor;
                exploreMapBuilder.GetFixed(File, out TileDic, out EnemyList);
            }
        }

        public void Init(int floor) 
        {
            SystemManager.Instance.Info.CurrentFloor = floor;
            ExploreMapBuilder exploreMapBuilder = GameObject.Find("ExploreMapBuilder").GetComponent<ExploreMapBuilder>();
            if (DataContext.Instance.FixedFloorDic.ContainsKey(floor))
            {
                FixedFloorModel data = DataContext.Instance.FixedFloorDic[floor];
                File = DataContext.Instance.Load<ExploreFile>(data.Name, DataContext.PrePathEnum.MapExplore);
                exploreMapBuilder.GetFixed(File, out TileDic, out EnemyList);
            }
            else
            {
                RandomFloorModel data = DataContext.Instance.RandomFloorDic[floor];
                File = ExploreFileRandomGenerator.Instance.Create(data);
                exploreMapBuilder.GetRandom(File, out TileDic, out EnemyList);
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

        public bool CheckEnemyCollision() 
        {
            ExploreFileEnemy enemy;
            for (int i = 0; i < File.EnemyList.Count; i++)
            {
                if (File.EnemyList[i].Position == File.PlayerPosition)
                {
                    _hasCollision = true;
                    InputMamager.Instance.Lock();
                    TileDic[File.PlayerPosition].IsWalkable = true;
                    enemy = File.EnemyList[i];
                    File.EnemyList.RemoveAt(i);
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
                                    battleInfo = battleMapBuilder.Generate(enemyGroup);
                                }
                            }
                            else
                            {
                                EnemyGroupModel enemyGroup = DataContext.Instance.EnemyGroupDic[enemy.EnemyGroupId];
                                battleInfo = battleMapBuilder.Generate(enemyGroup);
                            
                                if(File.Floor>1 && !FlowController.Instance.Info.HasSanaeTutorial)
                                {
                                    FlowController.Instance.Info.HasSanaeTutorial = true;
                                    tutorial = "SanaeTutorial";
                                }
                                else if (ItemManager.Instance.GetAmount(ItemManager.CardID) > 0 && !FlowController.Instance.Info.HasUseCard) 
                                {
                                    FlowController.Instance.Info.HasUseCard = true;
                                    tutorial = "UseCardTutorial";
                                }
                            }
                            PathManager.Instance.LoadData(battleInfo.TileDic);
                            BattleController.Instance.Init(File.Floor, File.Floor, tutorial, battleInfo, battleMapBuilder.transform);
                        });
                    });

                    return true;
                }
            }
            return false;
        }

        private bool CheckGoal(Vector2Int v2) 
        {
            if (v2 == File.Goal)
            {
                InputMamager.Instance.Lock();
                ConfirmUI.Open("要前往下一層嗎？", "確定", "取消", () =>
                {
                    File.Floor++;
                    if (File.Floor > SystemManager.Instance.Info.MaxFloor)
                    {
                        SystemManager.Instance.Info.MaxFloor = File.Floor;
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
            if(TileDic[v2].Event != null && !FlowController.Instance.Info.EventConditionList.Contains(TileDic[v2].Event)) 
            {
                FlowController.Instance.Info.EventConditionList.Add(TileDic[v2].Event);
                Type objectType = Type.GetType(TileDic[v2].Event);
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

            for (int i=0; i<EnemyList.Count; i++) 
            {
                v2 = Utility.ConvertToVector2Int(EnemyList[i].transform.position);
                if (TileDic[v2].IsVisited) 
                {
                    EnemyList[i].Arrow.layer = _mapLayer;
                }
                else
                {
                    EnemyList[i].Arrow.layer = _transparentFXLayer;
                }
            }
        }

        private void CheckVidsit(Vector2Int v2) 
        {
            if (TileDic.ContainsKey(v2)) 
            {
                TileDic[v2].IsVisited = true;
                TileDic[v2].Object.Quad.layer = _mapLayer;
                if(TileDic[v2].Object.Icon != null) 
                {
                    TileDic[v2].Object.Icon.layer = _mapLayer;
                }
            }
        }

        public void OpenAllMap() //debug
        {
            foreach(KeyValuePair<Vector2Int, ExploreFileTile> pair in TileDic) 
            {
                pair.Value.IsVisited = true;
                pair.Value.Object.Quad.layer = _mapLayer;
            }
            TileDic[File.Goal].Object.Icon.layer = _mapLayer;

            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].Arrow.layer = _mapLayer;
            }
        }

        public void Reload() 
        {
            ExploreMapBuilder exploreMapBuilder = GameObject.Find("ExploreMapBuilder").GetComponent<ExploreMapBuilder>();
            exploreMapBuilder.GetFixed(File, out TileDic, out EnemyList);
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
                        if (File.Floor > SystemManager.Instance.Info.MaxFloor)
                        {
                            SystemManager.Instance.Info.MaxFloor = File.Floor;
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
            if (TileDic.ContainsKey(position))
            {
                bool result = TileDic[position].Treasure != null;
                if (!FlowController.Instance.Info.HasGetItem && result)
                {
                    GetItemEvent getItemEvent = new GetItemEvent();
                    getItemEvent.Start();
                    FlowController.Instance.Info.HasGetItem = true;
                }
                return result;
            }

            return false;
        }

        public ExploreFileTreasure GetTreasure() 
        {
            ExploreFileTreasure treasure = null;
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            if (CheckTreasure(v2)) 
            {
               treasure = TileDic[v2].Treasure;

                bool bagIsFull = false;
                // if (treasure.Type == TreasureModel.TypeEnum.Equip)
                // {
                //     bagIsFull = ItemManager.Instance.AddEquip(treasure.ItemID);
                // }
                if (!bagIsFull) 
                {
                    TileDic[v2].IsWalkable = true;
                    ItemManager.Instance.AddItem(treasure.ItemID, 1);
                    GameObject.Destroy(TileDic[v2].Treasure.Object.gameObject);
                    File.TreasureList.Remove(treasure);
                    TileDic[v2].Treasure = null;

                    if(!FlowController.Instance.Info.HasGetCard && treasure.ItemID == ItemManager.CardID)
                    {
                        CardEvent cardEvent = new CardEvent();
                        cardEvent.Start();
                        FlowController.Instance.Info.HasGetCard = true;
                    }
                }
            }

            return treasure;
        }

        public void CreateObject() 
        {
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.transform.position = new Vector3(File.PlayerPosition.x, 1, File.PlayerPosition.y);
            Player.transform.eulerAngles = new Vector3(0, File.PlayerRotationY, 0);

            float x = 1;
            float y = 1;
            if (File.Size.x > 60)
            {
                x = File.Size.x / 60f;
            }
            if (File.Size.y > 60)
            {
                y = File.Size.y / 60f;
            }
            ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
            exploreUI.SetCameraPosition(File.Size.x / 2, File.Size.y / 2, x);

            Vector2Int v2 = Utility.ConvertToVector2Int(Player.transform.position);
            CheckEvent(v2);
            CheckVidsit(Player.transform);
        }

        private int _enemyMoveCount;
        public void EnemyMove() 
        {
            _enemyMoveCount = 0;
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].Move();
            }
        }

        public void WaitForAllMoveComplete() 
        {
            _enemyMoveCount++;
            if (_enemyMoveCount == File.EnemyList.Count + 1) 
            {
                CheckVidsit(Player.transform);
                Vector2Int v2 = Utility.ConvertToVector2Int(Player.transform.position);
                if (!_hasCollision)
                {
                    if (CheckEnemyCollision())
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
                else
                {
                    _hasCollision = false;
                }
            }
        }
    }
}
