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

        public float PlayerSpeed;
        public ExploreFile File;
        //public ExploreCharacterController Player;
        public FPSController Player;
        public Dictionary<Vector2Int, ExploreFileTile> TileDic = new Dictionary<Vector2Int, ExploreFileTile>();

        private bool _hasCollision = false;
        private Transform _parent;
        private Timer _timer = new Timer();
        private ExploreUI _exploreUI;
        private TreasureTrigger _treasure = null;
        private DoorTrigger _door = null;

        public void LoadFile() 
        {
            SaveManager.Instance.LoadExploreFile((file)=> 
            {
                File = file;
                Init();
                CreateObject();
            });
        }

        public void CreateFile(int floor) 
        {
            SceneController.Instance.Info.CurrentFloor = floor;

            SaveManager.Instance.CreateExploreFile(floor, (file)=> 
            {
                File = file;
                Init();
                CreateObject();
            });
        }

        public void UpdateFile() 
        {
            File.PlayerPositionX = Player.transform.position.x;
            File.PlayerPositionZ = Player.transform.position.z;
            File.PlayerRotationY = Player.transform.eulerAngles.y;
            for (int i = 0; i < File.EnemyList.Count; i++)
            {
                File.EnemyList[i].PositionX = File.EnemyList[i].Controller.transform.position.x;
                File.EnemyList[i].PositionZ = File.EnemyList[i].Controller.transform.position.z;
                File.EnemyList[i].RotationY = File.EnemyList[i].Controller.transform.eulerAngles.y;
            }
        }

        public void EnterBattle(ExploreFileEnemy enemy) 
        {
            _hasCollision = true;
            InputMamager.Instance.IsLock = true;
            File.EnemyList.Remove(enemy);
            UpdateFile();

            DeInit();
            SceneController.Instance.ChangeScene("Battle", ChangeSceneUI.TypeEnum.Fade, (sceneName) =>
            {
                Cursor.lockState = CursorLockMode.None;
                InputMamager.Instance.IsLock = false;
                string tutorial = null;
                if (enemy.Type == ExploreFileEnemy.TypeEnum.Fixed)
                {
                    BattleController.Instance.Init();
                    BattleController.Instance.SetFixed(enemy.Tutorial, enemy.Map);
                }
                else
                {
                    EnemyGroupModel enemyGroup = DataTable.Instance.EnemyGroupDic[enemy.EnemyGroupId];
                    if (File.Floor > 1 && !EventManager.Instance.Info.SanaeTutorial)
                    {
                        EventManager.Instance.Info.SanaeTutorial = true;
                        tutorial = "SanaeTutorial";
                    }

                    BattleController.Instance.Init();
                    BattleController.Instance.SetRandom(tutorial, enemyGroup);
                }
            });
        }

        public void ArriveGoal() 
        {
            InputMamager.Instance.IsLock = true;
            ConfirmUI.Open("要前往下一層嗎？", "確定", "取消", () =>
            {
                File.Floor++;
                if (File.Floor > SceneController.Instance.Info.MaxFloor)
                {
                    SceneController.Instance.Info.MaxFloor = File.Floor;
                }

                DeInit();
                SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Loading, (sceneName) =>
                {
                    CharacterManager.Instance.RecoverAllHP();
                    ItemManager.Instance.Info.Key = 0;
                    InputMamager.Instance.IsLock = false;
                });
            }, () =>
            {
                InputMamager.Instance.IsLock = false;
            });         
        }

        //public bool CheckEvent(Vector2Int v2) 
        //{
        //    if (TileDic.ContainsKey(v2))
        //    {
        //        string trigger = TileDic[v2].Event;
        //        if (trigger != null && !EventManager.Instance.Info.TriggerList.Contains(trigger))
        //        {
        //            EventManager.Instance.Info.TriggerList.Add(trigger);
        //            Type objectType = Type.GetType(trigger);
        //            MyEvent myEvent = (MyEvent)Activator.CreateInstance(objectType);
        //            myEvent.Start();
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public void CheckEvent(EventTrigger eventTrigger)
        {
            if (!EventManager.Instance.Info.TriggeredEventList.Contains(eventTrigger.Event.Name))
            {
                EventManager.Instance.Info.TriggeredEventList.Add(eventTrigger.Event.Name);
                File.EventList.Remove(eventTrigger.Event);
                GameObject.Destroy(eventTrigger.gameObject);
                Type objectType = Type.GetType(eventTrigger.Event.Name);
                MyEvent myEvent = (MyEvent)Activator.CreateInstance(objectType);
                myEvent.Start();
            }
        }

        public void CheckVisit(Vector2Int v2, Color color) 
        {
            if (!TileDic[v2].IsVisited)
            {
                TileDic[v2].IsVisited = true;
                _exploreUI.SetMap(v2, color);
            }
        }

        public void OpenAllMap() //debug
        {
            foreach(KeyValuePair<Vector2Int, ExploreFileTile> pair in TileDic) 
            {
                pair.Value.IsVisited = true;
                //pair.Value.Object.Quad.layer = _mapLayer;
            }

            for (int i = 0; i < File.EnemyList.Count; i++)
            {
                //File.EnemyList[i].Controller.Arrow.gameObject.layer = _mapLayer;
            }
        }

        public void ShowTreasure(TreasureTrigger treasure) 
        {
            _treasure = treasure;
            _exploreUI.ShowObjectInfoLabel("[空白鍵]");
            if (!treasure.File.IsVisited)
            {
                treasure.File.IsVisited = true;
                _exploreUI.SetIcon(Utility.ConvertToVector2Int(treasure.transform.position), treasure.File.Prefab);
            }
        }

        public void GetTreasure() 
        {
            //bool bagIsFull = false;
            //if (!bagIsFull)
            //{

            if (_treasure != null)
            {
                _exploreUI.OpenTreasure(_treasure.File.ItemID);
                if (_treasure.File.ItemID == ItemManager.KeyID)
                {
                    ItemManager.Instance.Info.Key++;
                }
                else
                {
                    ItemManager.Instance.AddItem(_treasure.File.ItemID, 1);
                }

                GameObject.Destroy(_treasure.gameObject);
                File.TreasureList.Remove(_treasure.File);
                _exploreUI.ClearIcon(Utility.ConvertToVector2Int(_treasure.transform.position));

                _treasure = null;
                _exploreUI.HideObjectInfoLabel();
            }
            //}

        }


        public void ShowDoor(DoorTrigger door)
        {
            _door = door;
            if (!door.File.IsVisited)
            {
                door.File.IsVisited = true;
                _exploreUI.SetIcon(Utility.ConvertToVector2Int(door.transform.position), "Door");
            }
            if (ItemManager.Instance.Info.Key > 0)
            {
                _exploreUI.ShowObjectInfoLabel("按空白鍵使用鑰匙開門");
            }
            else
            {
                _exploreUI.ShowObjectInfoLabel("需要鑰匙開門");
            }
        }

        public void OpenDoor() 
        {
            if (_door != null) 
            {
                if (ItemManager.Instance.Info.Key > 0) 
                {
                    _exploreUI.ClearIcon(Utility.ConvertToVector2Int(_door.transform.position));
                    _exploreUI.HideObjectInfoLabel();
                    File.DoorList.Remove(_door.File);
                    GameObject.Destroy(_door.gameObject);
                    _door = null;
                    ItemManager.Instance.Info.Key--;
                }
                else
                {
                    _exploreUI.TipLabel.SetLabel("需要鑰匙才能開鎖");
                }
            }
        }

        public void ShowEnemy(Vector3 position, ExploreEnemyController enemy)
        {
            _exploreUI.ShowEnemy(position, enemy);
        }

        public void HideEnemy(ExploreEnemyController enemy)
        {
            _exploreUI.HideEnemy(enemy);
        }

        public void ClearObjectInfo()
        {
            _treasure = null;
            _door = null;
            _exploreUI.HideObjectInfoLabel();
        }

        public void Reload()
        {
            Init();
            CreateObject();

            if (ReloadHandler != null)
            {
                ReloadHandler();
            }
        }

        public void SetPlayerPosition(Vector2 position) 
        {
            _exploreUI.SetPlayerPosition(position);
        }

        private void CreateObject() 
        {
            GameObject gameObj;
            _parent = GameObject.Find("Generator2D").transform;

            _exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
            _exploreUI.InitMap(File.Size.x, File.Size.y);

            for (int i = _parent.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(_parent.GetChild(0).gameObject);
            }

            TileDic.Clear();
            string name = "";
            for (int i = 0; i < File.TileList.Count; i++)
            {
                name = File.TileList[i].Prefab;
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + File.TileList[i].Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(File.TileList[i].Position.x, 0, File.TileList[i].Position.y);
                gameObj.transform.SetParent(_parent);
                TileDic.Add(File.TileList[i].Position, File.TileList[i]);
                if (File.TileList[i].IsVisited) 
                {
                    if (File.TileList[i].Prefab == "Wall")
                    {
                        _exploreUI.SetMap(File.TileList[i].Position, Color.gray);
                    }
                    if (File.TileList[i].Prefab == "Ground")
                    {
                        _exploreUI.SetMap(File.TileList[i].Position, Color.green);
                    }
                }
            }

            DoorTrigger door;
            for (int i = 0; i < File.DoorList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Door_Cube"), Vector3.zero, Quaternion.identity);
                door = gameObj.GetComponent<DoorTrigger>();
                door.transform.position = new Vector3(File.DoorList[i].Position.x, 1, File.DoorList[i].Position.y);
                door.transform.SetParent(_parent);
                door.File = File.DoorList[i];

                if (File.DoorList[i].IsVisited)
                {
                    _exploreUI.SetIcon(File.DoorList[i].Position, "Door");
                }
            }

            TreasureTrigger treasureTrigger;
            for (int i=0; i<File.TreasureList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + File.TreasureList[i].Prefab), Vector3.zero, Quaternion.identity);
                treasureTrigger = gameObj.GetComponent<TreasureTrigger>();
                treasureTrigger.transform.position = new Vector3(File.TreasureList[i].Position.x, 0, File.TreasureList[i].Position.y);
                treasureTrigger.transform.SetParent(_parent);
                treasureTrigger.File = File.TreasureList[i];

                if (File.TreasureList[i].IsVisited)
                {
                    _exploreUI.SetIcon(File.TreasureList[i].Position, File.TreasureList[i].Prefab);
                }
            }

            EventTrigger eventTrigger;
            for (int i=0; i<File.EventList.Count; i++) 
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/EventTrigger"), Vector3.zero, Quaternion.identity);
                eventTrigger = gameObj.GetComponent<EventTrigger>();
                eventTrigger.transform.position = new Vector3(File.EventList[i].Position.x, 0, File.EventList[i].Position.y);
                eventTrigger.transform.SetParent(_parent);
                eventTrigger.Event = File.EventList[i];
            }

            gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Goal"), Vector3.zero, Quaternion.identity);
            gameObj.transform.position = new Vector3(File.Goal.x, 0, File.Goal.y);
            gameObj.transform.SetParent(_parent);
            Goal goal = gameObj.GetComponent<Goal>();
            File.GoalObj = goal;
            if (TileDic[File.Goal].IsVisited)
            {
                //File.GoalObj.Quad.layer = _mapLayer;
            }

            ExploreEnemyController controller;
            for (int i = 0; i < File.EnemyList.Count; i++)
            {
                gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/" + File.EnemyList[i].Prefab), Vector3.zero, Quaternion.identity);
                gameObj.transform.position = new Vector3(File.EnemyList[i].PositionX, 1, File.EnemyList[i].PositionZ);
                gameObj.transform.eulerAngles = new Vector3(0, File.EnemyList[i].RotationY, 0);
                gameObj.transform.SetParent(_parent);
                controller = gameObj.GetComponent<ExploreEnemyController>();
                controller.Init(File.EnemyList[i]);
                File.EnemyList[i].Controller = controller;
            }

            UnityEngine.Object obj = Resources.Load("Prefab/Explore/Player");
            Vector3 position = new Vector3(File.PlayerPositionX, 1, File.PlayerPositionZ);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, File.PlayerRotationY, 0));
            Player = ((GameObject)GameObject.Instantiate(obj, position, rotation)).GetComponent<FPSController>();

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

            //CheckVisit(Player.transform);
            //CheckEvent(Utility.ConvertToVector2Int(Player.transform.position));
            SetPlayerPosition(new Vector2(Player.transform.position.x, Player.transform.position.z));
        }

        private void Init() 
        {
            InputMamager.Instance.SpaceHandler += SpaceOnClick;
        }

        private void DeInit() 
        {
            InputMamager.Instance.SpaceHandler -= SpaceOnClick;
        }

        private void SpaceOnClick() 
        {
            GetTreasure();
            OpenDoor();
        }
    }
}
