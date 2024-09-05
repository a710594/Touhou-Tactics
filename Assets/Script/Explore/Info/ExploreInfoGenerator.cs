using System.Collections;
using System.Collections.Generic;
using Explore;
using UnityEngine;

/*public class ExploreInfoGenerator
{
    private static ExploreInfoGenerator _instance;
    public static ExploreInfoGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ExploreInfoGenerator();
            }
            return _instance;
        }
    }

    private LayerMask _mapLayer = LayerMask.NameToLayer("Map");

    public ExploreInfo Generate(ExploreFile file)
    {
        ExploreInfo info = new ExploreInfo(file);
        Transform parent = GameObject.Find("Generator2D").transform;
                    
        ExploreInfoTile tile;
        for (int i = 0; i < file.TileList.Count; i++)
        {
            tile = new ExploreInfoTile(file.TileList[i]);
            info.TileDic.Add(file.TileList[i].Position, tile);
        }

        ExploreInfoTreasure treasure;
        for (int i = 0; i < file.TreasureList.Count; i++)
        {
            treasure = new ExploreInfoTreasure(file.TreasureList[i]);

            tile = info.TileDic[file.TreasureList[i].Position];
            tile.Treasure = treasure;
            tile.IsWalkable = false;
        }

        for (int i = 0; i < file.TriggerList.Count; i++)
        {
            info.TileDic[file.TriggerList[i].Position].Event = file.TriggerList[i].Name;
        }           

        ExploreInfoEnemy enemy;
        for (int i = 0; i < file.EnemyList.Count; i++)
        {
            enemy = new ExploreInfoEnemy(file.EnemyList[i]);
            info.EnemyList.Add(enemy);
        }

        return info;
    }
}*/
