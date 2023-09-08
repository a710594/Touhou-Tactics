using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public RandomMapGenerator RandomMapGenerator;

    // Start is called before the first frame update
    void Start()
    {
        DataContext.Instance.Init();
        MapReader mapReader = new MapReader();
        //mapReader.LoadData();
        //mapReader.Read(Application.streamingAssetsPath + "/Map.txt", out BattleInfo battleInfo);
        RandomMapGenerator.Generate(out BattleInfo battleInfo);
        PathManager.Instance.LoadData(battleInfo.TileInfoDic);
        //ItemManager.Instance.Init();
        BattleController.Instance.Init(1, 1, battleInfo);

        ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Medicine, 1, 1);
        ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 1, 1);
        ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 2, 1);
        ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 3, 1);
        ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 4, 1);
        ItemManager.Instance.AddItem(ItemModel.CategoryEnum.Card, 5, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
