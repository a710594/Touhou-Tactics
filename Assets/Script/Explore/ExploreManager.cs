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
        private LayerMask _mapLayer = LayerMask.NameToLayer("Map");
        private LayerMask _transparentFXLayer = LayerMask.NameToLayer("TransparentFX");

        private bool _hasCollision = false;
        private Transform _parent;
        private Timer _timer = new Timer();
        private ExploreUI _exploreUI;
        private ExploreFileTreasure _treasure = null;
        private ExploreFIleDoor _door = null;

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
        }

        public void EnterBattle(ExploreFileEnemy enemy) 
        {
            _hasCollision = true;
            InputMamager.Instance.Lock();
            //TileDic[File.PlayerPosition].IsWalkable = true;
            File.EnemyList.Remove(enemy);
            File.PlayerPositionX = Player.transform.position.x;
            File.PlayerPositionZ = Player.transform.position.z;
            File.PlayerRotationY = Player.transform.eulerAngles.y;

            DeInit();
            SceneController.Instance.ChangeScene("Battle", ChangeSceneUI.TypeEnum.Fade, (sceneName) =>
            {
                Cursor.lockState = CursorLockMode.None;
                InputMamager.Instance.Unlock();
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
                    else if (File.Floor > 1 && !EventManager.Instance.Info.SpellTutorial)
                    {
                        EventManager.Instance.Info.SpellTutorial = true;
                        tutorial = "SpellTutorial";
                    }
                    BattleController.Instance.Init();
                    BattleController.Instance.SetRandom(tutorial, enemyGroup);
                }
            });
        }

        public void CheckGoal() 
        {
            if (File.IsArrive)
            {
                InputMamager.Instance.Lock();
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
                        InputMamager.Instance.Unlock();
                    });
                }, () =>
                {
                    InputMamager.Instance.Unlock();
                });
            }
        }

        public bool CheckEvent(Vector2Int v2) 
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

        public void CheckVisit(Transform transform) 
        {
            bool leftBlock = false;
            bool rightBlock = false;
            Vector2Int v2;

            v2 = Utility.ConvertToVector2Int(transform.position);
            CheckVisit(v2);

            v2 = Utility.ConvertToVector2Int(transform.position - transform.right);
            if (TileDic.ContainsKey(v2))
            {
                leftBlock = true;
                CheckVisit(v2);
            }

            v2 = Utility.ConvertToVector2Int(transform.position + transform.right);
            if (TileDic.ContainsKey(v2))
            {
                rightBlock = true;
                CheckVisit(v2);
            }

            v2 = Utility.ConvertToVector2Int(transform.position + transform.forward);
            if (TileDic.ContainsKey(v2))
            {
                CheckVisit(v2);

                if (leftBlock) 
                {
                    v2 = Utility.ConvertToVector2Int(transform.position + transform.forward - transform.right);
                    CheckVisit(v2);
                }

                if (rightBlock) 
                {
                    v2 = Utility.ConvertToVector2Int(transform.position + transform.forward + transform.right);
                    CheckVisit(v2);
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

        private void CheckVisit(Vector2Int v2) 
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

        public void CheckTreasure(Transform transform) 
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(transform.position + transform.forward);
            if (TileDic.ContainsKey(v2) && TileDic[v2].Treasure != null)
            {
                _treasure = TileDic[v2].Treasure;
                _treasure.Object.Outline.enabled = true;
                _exploreUI.ShowTreasureLabel(true);

                if (!EventManager.Instance.Info.GetItem)
                {
                    GetItemEvent getItemEvent = new GetItemEvent();
                    getItemEvent.Start();
                    EventManager.Instance.Info.GetItem = true;
                }
            }
            else
            {
                if (_treasure != null)
                {
                    _treasure.Object.Outline.enabled = false;
                }
                _treasure = null;
                _exploreUI.ShowTreasureLabel(false);
            }
        }

        public void GetTreasure() 
        {
            if (_treasure != null)
            {
                //bool bagIsFull = false;
                //if (!bagIsFull)
                //{
                _exploreUI.OpenTreasure(_treasure.ItemID);
                TileDic[_treasure.Position].IsWalkable = true;
                if (_treasure.ItemID == ItemManager.KeyID)
                {
                    ItemManager.Instance.Info.Key++;
                }
                else
                {
                    ItemManager.Instance.AddItem(_treasure.ItemID, 1);
                }

                GameObject.Destroy(TileDic[_treasure.Position].Treasure.Object.gameObject);
                File.TreasureList.Remove(_treasure);
                TileDic[_treasure.Position].Treasure = null;
                _treasure = null;
                _exploreUI.ShowTreasureLabel(false);
                //}

            }
        }

        public void CheckDoor(Transform transform)
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(transform.position + transform.forward);
            bool hasDoor = false;
            for (int i=0; i<File.DoorList.Count; i++) 
            {
                if (File.DoorList[i].PositionList.Contains(v2)) 
                {
                    hasDoor = true;
                    _door = File.DoorList[i];
                    break;
                }
            }

            if (!hasDoor) 
            {
                _door = null;
            }

            _exploreUI.ShowDoorLabel(hasDoor);
        }

        public void OpenDoor() 
        {
            if (_door != null && ItemManager.Instance.Info.Key > 0) 
            {
                for (int i=0; i< _door.PositionList.Count; i++) 
                {
                    TileDic[_door.PositionList[i]].IsWalkable = true;
                    GameObject.Destroy(TileDic[_door.PositionList[i]].Door);
                    TileDic[_door.PositionList[i]].Door = null;
                }
                ItemManager.Instance.Info.Key--;
                File.DoorList.Remove(_door);
                _exploreUI.ShowDoorLabel(false);
            }
        }

        public void Reload()
        {
            if (Vector2.Distance(new Vector2(File.PlayerPositionX, File.PlayerPositionZ), File.Goal) < 1)
            {
                File.IsArrive = true;
            }
            Init();
            CreateObject();

            if (ReloadHandler != null)
            {
                ReloadHandler();
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
                controller.Init(File.EnemyList[i]);
                TileDic[File.EnemyList[i].Position].IsWalkable = false;
                File.EnemyList[i].Controller = controller;
            }

            Player = GameObject.Find("Player").GetComponent<FPSController>();
            Player.transform.position = new Vector3(File.PlayerPositionX, Player.transform.position.y, File.PlayerPositionZ);
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

            _exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
            _exploreUI.SetCameraPosition(File.Size.x / 2, File.Size.y / 2, x);

            CheckVisit(Player.transform);
            ExploreManager.Instance.CheckEvent(Utility.ConvertToVector2Int(Player.transform.position));
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
