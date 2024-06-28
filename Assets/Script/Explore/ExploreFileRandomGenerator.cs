using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFileRandomGenerator : MonoBehaviour
{
    private Generator2D _generator2D = new Generator2D();
    private NewExploreFile _file;
    private List<Vector2Int> _tilePositionList;

    public void Build(Vector2Int size, int roomCount, Vector2Int roomMaxSize)
    {
        _file = new NewExploreFile();
        _tilePositionList = new List<Vector2Int>();
        _generator2D.Generate(size, roomCount, roomMaxSize, out List<Generator2D.Room> roomList, out List<List<Vector2Int>> pathList);

        for(int i=0; i<roomList.Count; i++)
        {
            foreach (var pos in roomList[i].bounds.allPositionsWithin) 
            {
                _file.TileList.Add(new NewExploreFile.TileInfo(pos, "Ground", ""));
                _tilePositionList.Add(pos);
            }
        }

        for(int i=0; i<pathList.Count; i++)
        {
            for(int j=0; j<pathList[i].Count; j++)
            {
                _file.TileList.Add(new NewExploreFile.TileInfo(pathList[i][j], "Ground", ""));
                _tilePositionList.Add(pathList[i][j]);     
            }   
        }
    }

    void PlaceWall()
    {
        int x;
        int y;
        int groundCount = _tilePositionList.Count;
        for(int i=0; i<groundCount; i++)
        {
            //左
            x = _tilePositionList[i].x - 1;
            y = _tilePositionList[i].y;
            CheckWall(new Vector2Int(x, y));

            //右
            x = _tilePositionList[i].x + 1;
            y = _tilePositionList[i].y;
            CheckWall(new Vector2Int(x, y));

            //下
            x = _tilePositionList[i].x;
            y = _tilePositionList[i].y - 1;
            CheckWall(new Vector2Int(x, y));

            //上
            x = _tilePositionList[i].x;
            y = _tilePositionList[i].y + 1;
            CheckWall(new Vector2Int(x, y));

            //左下
            x = _tilePositionList[i].x - 1;
            y = _tilePositionList[i].y - 1;
            CheckWall(new Vector2Int(x, y));

            //左上
            x = _tilePositionList[i].x - 1;
            y = _tilePositionList[i].y + 1;
            CheckWall(new Vector2Int(x, y));

            //右下
            x = _tilePositionList[i].x + 1;
            y = _tilePositionList[i].y - 1;
            CheckWall(new Vector2Int(x, y));

            //右上
            x = _tilePositionList[i].x + 1;
            y = _tilePositionList[i].y + 1;
            CheckWall(new Vector2Int(x, y));
        }
    }

    private bool CheckWall(Vector2Int position)
    {
        if(!_tilePositionList.Contains(position))
        {
            _file.TileList.Add(new NewExploreFile.TileInfo(position, "Wall", "Wall"));
            _tilePositionList.Add(position);  
            return true;
        }
        else
        {
            return false;
        }
    }
}
