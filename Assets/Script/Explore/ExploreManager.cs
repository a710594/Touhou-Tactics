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
        public Dictionary<Vector2Int, ExploreFileTile> TileDic = new Dictionary<Vector2Int, ExploreFileTile>();
        private LayerMask _mapLayer = LayerMask.NameToLayer("Map");
        private LayerMask _transparentFXLayer = LayerMask.NameToLayer("TransparentFX");

        private bool _hasCollision = false;
        private Transform _parent;
        private Timer _timer = new Timer();
        private FileLoader _fileLoader;


        public void SetFileLoader(FileLoader fileLoader) 
        {
            _fileLoader = fileLoader;
        }

        public void LoadFile() 
        {
            SaveManager.Instance.LoadExploreFile((file)=> 
            {
                File = file;
                CreateObject();
            });
        }

        public void CreateFile(int floor) 
        {
            SceneController.Instance.Info.CurrentFloor = floor;

            SaveManager.Instance.CreateExploreFile(floor, (file)=> 
            {
                File = file;
                CreateObject();
            });
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

                        SceneController.Instance.ChangeScene("Battle", ChangeSceneUI.TypeEnum.Fade, (sceneName) =>
                        {
                            InputMamager.Instance.Unlock();
                            string tutorial = null;
                            if (enemy.Type == ExploreFileEnemy.TypeEnum.Fixed)
                            {
                                BattleController.Instance.Init(enemy.Tutorial, enemy.Map);
                            }
                            else
                            {
                                EnemyGroupModel enemyGroup = DataTable.Instance.EnemyGroupDic[enemy.EnemyGroupId];
                                if (File.Floor > 1 && !EventManager.Instance.Info.SanaeTutorial)
                                {
                                    EventManager.Instance.Info.SanaeTutorial = true;
                                    tutorial = "SanaeTutorial";
                                }
                                else if (File.Floor > 1 && !EventManager.Instance.Info.SpellTutorial) 
                                {
                                    EventManager.Instance.Info.SpellTutorial = true;
                                    tutorial = "SpellTutorial";
                                }
                                BattleController.Instance.Init(tutorial, enemyGroup);
                            }
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
                    if (File.Floor > SceneController.Instance.Info.MaxFloor)
                    {
                        SceneController.Instance.Info.MaxFloor = File.Floor;
                    }

                    _timer.Start(1f, () =>
                    {
                        SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
                        {
                            CharacterManager.Instance.RecoverAllHP();
                            ItemManager.Instance.Info.Key = 0;
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
            string trigger = TileDic [v2].Event;
            if(trigger != null && !EventManager.Instance.Info.TriggerList.Contains(trigger)) 
            {
                EventManager.Instance.Info.TriggerList.Add(trigger);
                Type objectType = Type.GetType(trigger);
                MyEvent myEvent = (MyEvent)Activator.CreateInstance(objectType);
                myEvent.Start();
                return true;
            }
            return false;
        }

        public void CheckVidsit(Transform transform) 
        {
            bool leftBlock = false;
            bool rightBlock = false;
            Vector2Int v2;

            v2 = Utility.ConvertToVector2Int(transform.position);
            CheckVidsit(v2);

            v2 = Utility.ConvertToVector2Int(transform.position - transform.right);
            if (TileDic.ContainsKey(v2) && !TileDic[v2].IsWalkable)
            {
                leftBlock = true;
                CheckVidsit(v2);
            }

            v2 = Utility.ConvertToVector2Int(transform.position + transform.right);
            if (TileDic.ContainsKey(v2) && !TileDic[v2].IsWalkable)
            {
                rightBlock = true;
                CheckVidsit(v2);
            }

            v2 = Utility.ConvertToVector2Int(transform.position + transform.forward);
            if (TileDic.ContainsKey(v2) && !TileDic[v2].IsWalkable)
            {
                CheckVidsit(v2);

                if (leftBlock) 
                {
                    v2 = Utility.ConvertToVector2Int(transform.position + transform.forward - transform.right);
                    CheckVidsit(v2);
                }

                if (rightBlock) 
                {
                    v2 = Utility.ConvertToVector2Int(transform.position + transform.forward + transform.right);
                    CheckVidsit(v2);
                }
            }

            for (int i=0; i<File.EnemyList.Count; i++) 
            {
                v2 = Utility.ConvertToVector2Int(File.EnemyList[i].Controller.transform.position);
                if (TileDic[v2].IsVisited) 
                {
                    File.EnemyList[i].Controller.Arrow.layer = _mapLayer;
                }
                else
                {
                    File.EnemyList[i].Controller.Arrow.layer = _transparentFXLayer;
                }
            }
        }

        private void CheckVidsit(Vector2Int v2) 
        {
            if (TileDic.ContainsKey(v2)) 
            {
                TileDic[v2].IsVisited = true;
                TileDic[v2].Object.Quad.layer = _mapLayer;
                if (TileDic[v2].Treasure != null) 
                {
                    TileDic[v2].Treasure.Object.Icon.layer = _mapLayer;
                }
            }

            if(v2 == File.Goal) 
            {
                File.GoalObj.Quad.layer = _mapLayer;
            }
        }

        public void OpenAllMap() //debug
        {
            foreach(KeyValuePair<Vector2Int, ExploreFileTile> pair in TileDic) 
            {
                pair.Value.IsVisited = true;
                pair.Value.Object.Quad.layer = _mapLayer;
            }

            for (int i = 0; i < File.EnemyList.Count; i++)
            {
                File.EnemyList[i].Controller.Arrow.layer = _mapLayer;
            }
        }

        public void Reload() 
        {
            if (File.PlayerPosition == File.Goal)
            {
                File.IsArrive = true;
            }
            CreateObject();
            if (File.IsArrive) 
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
                    InputMamager.Instance.Lock();
                    ConfirmUI.Open("你打倒了出口的守衛！要前往下一層嗎？", "確定", "取消", () =>
                    {
                        File.Floor++;
                        if (File.Floor > SceneController.Instance.Info.MaxFloor)
                        {
                            SceneController.Instance.Info.MaxFloor = File.Floor;
                        }

                        SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
                        {
                            CharacterManager.Instance.RecoverAllHP();
                            ItemManager.Instance.Info.Key = 0;
                            InputMamager.Instance.Unlock();
                        });
                    }, ()=> 
                    {
                        InputMamager.Instance.Unlock();
                    });
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
                if (!EventManager.Instance.Info.GetItem && result)
                {
                    GetItemEvent getItemEvent = new GetItemEvent();
                    getItemEvent.Start();
                    EventManager.Instance.Info.GetItem = true;
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

                if (!bagIsFull) 
                {
                    TileDic[v2].IsWalkable = true;
                    if (treasure.ItemID == ItemManager.KeyID)
                    {
                        ItemManager.Instance.Info.Key++;
                    }
                    else
                    {
                         ItemManager.Instance.AddItem(treasure.ItemID, 1);
                    }
                    
                    GameObject.Destroy(TileDic[v2].Treasure.Object.gameObject);
                    File.TreasureList.Remove(treasure);
                    TileDic[v2].Treasure = null;
                }
            }

            return treasure;
        }

        public ExploreFIleDoor CheckDoor(Vector2Int position)
        {
            for (int i=0; i<File.DoorList.Count; i++) 
            {
                if (File.DoorList[i].PositionList.Contains(position)) 
                {
                    return File.DoorList[i];
                }
            }
            return null;
        }

        public void OpenDoor() 
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(Camera.main.transform.position + Camera.main.transform.forward);
            ExploreFIleDoor door = CheckDoor(v2);
            if (door != null && ItemManager.Instance.Info.Key > 0) 
            {
                for (int i=0; i<door.PositionList.Count; i++) 
                {
                    TileDic[door.PositionList[i]].IsWalkable = true;
                    GameObject.Destroy(TileDic[door.PositionList[i]].Door);
                    TileDic[door.PositionList[i]].Door = null;
                }
                ItemManager.Instance.Info.Key--;
                File.DoorList.Remove(door);
            }
        }

        public void CreateObject() 
        {
            GameObject gameObj;
            TileObject tileObj;
            TreasureObject treasureObj;
            _parent = GameObject.Find("Generator2D").transform;

            for (int i = _parent.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(_parent.GetChild(0).gameObject);
            }

            TileDic.Clear();
            string name = "";
            try
            {
                for (int i = 0; i < File.TileList.Count; i++)
                {
                    name = File.TileList[i].Prefab;
                    gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + File.TileList[i].Prefab), Vector3.zero, Quaternion.identity);
                    tileObj = gameObj.GetComponent<TileObject>();
                    tileObj.transform.position = new Vector3(File.TileList[i].Position.x, 0, File.TileList[i].Position.y);
                    tileObj.transform.SetParent(_parent);
                    if (File.TileList[i].IsVisited)
                    {
                        tileObj.Quad.layer = _mapLayer;
                    }
                    File.TileList[i].Object = tileObj;
                    TileDic.Add(File.TileList[i].Position, File.TileList[i]);
                }
            }
            catch(Exception ex) 
            {
                Debug.Log(ex);
            }

            for (int i = 0; i < File.DoorList.Count; i++)
            {
                for (int j=0; j<File.DoorList[i].PositionList.Count; j++) 
                {
                    gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Door_Cube"), Vector3.zero, Quaternion.identity);
                    gameObj.transform.position = new Vector3(File.DoorList[i].PositionList[j].x, 1, File.DoorList[i].PositionList[j].y);
                    gameObj.transform.SetParent(_parent);
                    TileDic[File.DoorList[i].PositionList[j]].Door = gameObj;
                    TileDic[File.DoorList[i].PositionList[j]].IsWalkable = false;
                }
            }

            for (int i=0; i<File.TreasureList.Count; i++)
            {
                TileDic[File.TreasureList[i].Position].Treasure = File.TreasureList[i];
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + File.TreasureList[i].Prefab), Vector3.zero, Quaternion.identity);
                treasureObj = gameObj.GetComponent<TreasureObject>();
                treasureObj.transform.position = new Vector3(File.TreasureList[i].Position.x, File.TreasureList[i].Height, File.TreasureList[i].Position.y);
                treasureObj.transform.SetParent(_parent);
                TileDic[File.TreasureList[i].Position].Treasure.Object = treasureObj;
                TileDic[File.TreasureList[i].Position].IsWalkable = false;
                if (TileDic[File.TreasureList[i].Position].IsVisited && treasureObj.Icon != null)
                {
                    treasureObj.Icon.layer = _mapLayer;
                }

            }

            for (int i=0; i<File.TriggerList.Count; i++) 
            {
                TileDic[File.TriggerList[i].Position].Event = File.TriggerList[i].Name;
            }

            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
            gameObj.transform.position = new Vector3(File.Goal.x, 0, File.Goal.y);
            gameObj.transform.SetParent(_parent);
            Goal goal = gameObj.GetComponent<Goal>();
            goal.Red.SetActive(!File.IsArrive);
            goal.Blue.SetActive(File.IsArrive);
            File.GoalObj = goal;

            ExploreEnemyController controller;
            for (int i = 0; i < File.EnemyList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + File.EnemyList[i].Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(File.EnemyList[i].Position.x, 1, File.EnemyList[i].Position.y);
                gameObj.transform.eulerAngles = new Vector3(0, File.EnemyList[i].RotationY, 0);
                gameObj.transform.SetParent(_parent);
                controller = gameObj.GetComponent<ExploreEnemyController>();
                controller.SetAI(File.EnemyList[i]);
                TileDic[File.EnemyList[i].Position].IsWalkable = false;
                File.EnemyList[i].Controller = controller;
            }

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
            for (int i = 0; i < File.EnemyList.Count; i++)
            {
                File.EnemyList[i].Controller.Move();
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
