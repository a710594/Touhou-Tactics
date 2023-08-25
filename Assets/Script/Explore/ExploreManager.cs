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

        private Grid2D<Generator2D.CellType> _grid;
        private ExploreCharacterController _player;
        private List<MashroomAI> enemyList = new List<MashroomAI>();

        public void SetData(Grid2D<Generator2D.CellType> grid, List<Generator2D.Room> rooms)
        {
            _grid = grid;
            _player = Camera.main.GetComponent<ExploreCharacterController>();
            _player.MoveHandler += OnPlayerMove;

            GameObject obj;
            MashroomAI mashroom;
            for (int i = 0; i < 5; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Mashroom"), Vector3.zero, Quaternion.identity);
                mashroom = obj.GetComponent<MashroomAI>();
                mashroom.Init(rooms);
                enemyList.Add(mashroom);
            }
        }

        public bool IsWalkable(Vector3 position)
        {
            if (position.x >= 0 && position.x < _grid.Size.x && position.z >= 0 && position.z < _grid.Size.y && _grid[(int)position.x, (int)position.z] != Generator2D.CellType.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnPlayerMove() 
        {
            for (int i=0; i<enemyList.Count; i++) 
            {
                enemyList[i].Step();
            }
        }
    }
}
