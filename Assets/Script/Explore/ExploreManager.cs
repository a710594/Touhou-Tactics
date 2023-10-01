using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreManager
    {
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

        public ExploreCharacterController Player;

        private Timer _timer = new Timer();
        private Grid2D<Generator2D.CellType> _grid;
        private List<Generator2D.Room> _rooms;
        private List<MashroomAI> enemyList = new List<MashroomAI>();

        public void SetData(Grid2D<Generator2D.CellType> grid, List<Generator2D.Room> rooms)
        {
            _grid = grid;
            _rooms = rooms;

            SetObject();
        }

        public bool IsWalkable(Vector3 position) //for player
        {
            if (InBound(position) && _grid[(int)position.x, (int)position.z] != Generator2D.CellType.None && _grid[(int)position.x, (int)position.z] != Generator2D.CellType.Wall)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsWalkable(AI enemy, Vector3 position) //for enemy
        {
            Debug.Log(_grid[(int)position.x, (int)position.z]);
            if (InBound(position) && _grid[(int)position.x, (int)position.z] != Generator2D.CellType.None && _grid[(int)position.x, (int)position.z] != Generator2D.CellType.Wall)
            {
                for (int i = 0; i < enemyList.Count; i++) 
                {
                    if(enemy != enemyList[i] && Utility.ComparePosition(position, enemyList[i].MoveTo)) 
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

        public bool InBound(Vector3 position) 
        {
            if (position.x >= 0 && position.x < _grid.Size.x && position.z >= 0 && position.z < _grid.Size.y)
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
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (Utility.ComparePosition(enemyList[i].MoveTo, Player.MoveTo))
                {
                    Debug.Log("Start Battle!");
                    Player.CanMove = false;
                    _timer.Start(1f, ()=> 
                    {
                        SceneController.Instance.ChangeScene("Battle", () =>
                        {
                            BattleMapGenerator randomMapGenerator = GameObject.Find("Tilemap").GetComponent<BattleMapGenerator>();
                            randomMapGenerator.Generate(out BattleInfo battleInfo);
                            PathManager.Instance.LoadData(battleInfo.TileInfoDic);
                            BattleController.Instance.Init(1, 1, battleInfo);
                        });
                    });
                }
            }
        }

        public void Reload() 
        {
            Generator2D generator2D = GameObject.Find("Generator2D").GetComponent<Generator2D>();
            generator2D.Relod(_grid, _rooms);
            SetObject();
        }

        private void SetObject() 
        {
            Camera.main.transform.position = new Vector3((int)_rooms[0].bounds.center.x, 1, (int)_rooms[0].bounds.center.y);
            Player = Camera.main.GetComponent<ExploreCharacterController>();
            Player.MoveHandler += OnPlayerMove;
            Player.RotateHandler += OnPlayerRotate;

            GameObject obj;
            MashroomAI mashroom;
            for (int i = 0; i < 1; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Mashroom"), Vector3.zero, Quaternion.identity);
                mashroom = obj.GetComponent<MashroomAI>();
                mashroom.Init(_rooms);
                enemyList.Add(mashroom);
            }
        }

        private void OnPlayerMove() 
        {
            for (int i=0; i<enemyList.Count; i++) 
            {
                enemyList[i].Move();
            }

            CheckCollision();
        }

        private void OnPlayerRotate()
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Rotate();
            }
        }
    }
}
