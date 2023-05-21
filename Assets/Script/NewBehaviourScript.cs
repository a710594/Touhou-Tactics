using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MapReader mapReader = new MapReader();
        mapReader.LoadData();
        mapReader.Read(Application.streamingAssetsPath + "/Map.txt", out int width, out int height, out Dictionary<Vector2, TileInfo> tileInfoDic, out Dictionary<Vector2, TileComponent> tileComponentDic, out Dictionary<Vector2, GameObject> attachDic);

        DataContext.Instance.Init();

        BattleInfo battleInfo = new BattleInfo();
        battleInfo.Width = width;
        battleInfo.Height = height;
        battleInfo.tileInfoDic = tileInfoDic;
        battleInfo.tileComponentDic = tileComponentDic;
        battleInfo.attachDic = attachDic;
        BattleController.Instance.Init(battleInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
