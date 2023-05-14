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
        mapReader.Read(Application.streamingAssetsPath + "/Map.txt", out int width, out int height, out Dictionary<Vector3, TileInfo> tileInfoDic, out Dictionary<Vector3, TileComponent> tileComponentDic, out Dictionary<Vector3, GameObject> attachDic);

        BattleController.Instance.Init(width, height, tileComponentDic, tileInfoDic, attachDic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
