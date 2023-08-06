using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataContext.Instance.Init();
        MapReader mapReader = new MapReader();
        //mapReader.LoadData();
        mapReader.Read(Application.streamingAssetsPath + "/Map.txt", out int width, out int height, out Dictionary<Vector2Int, TileInfo> tileInfoDic, out Dictionary<Vector2Int, TileComponent> tileComponentDic, out Dictionary<Vector2Int, GameObject> attachDic);
        PathManager.Instance.LoadData(tileInfoDic);
        ItemManager.Instance.Init();
        BattleInfo battleInfo = new BattleInfo();
        battleInfo.Width = width;
        battleInfo.Height = height;
        battleInfo.TileInfoDic = tileInfoDic;
        battleInfo.TileComponentDic = tileComponentDic;
        battleInfo.AttachDic = attachDic;
        BattleController.Instance.Init(battleInfo);

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
