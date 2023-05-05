using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController
{
    private static BattleController _instance;
    public static BattleController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BattleController();
            }
            return _instance;
        }
    }

    public void Init(Dictionary<Vector3, TileComponent> tileComponentDic, Dictionary<Vector3, TileInfo> tileInfoDic, Dictionary<Vector3, GameObject> attachDic, List<Vector3> noAttachList) 
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Character/" + "Reimu_S"), Vector3.zero, Quaternion.identity);
        obj.transform.position = noAttachList[0] + Vector3.up;
        Camera.main.transform.parent = obj.transform;
        Camera.main.transform.localPosition = obj.transform.position + new Vector3(0, 8, -13);
        Camera.main.transform.localEulerAngles = new Vector3(30, 0, 0);

        PathPlanner.Instance.LoadData(tileInfoDic);
        List<Vector2> list = PathPlanner.Instance.GetPath(new Vector2(obj.transform.position.x, obj.transform.position.z), new Vector2(5, 5));
        for (int i=0; i<list.Count; i++) 
        {
            tileComponentDic[new Vector3(list[i].x, 0, list[i].y)].Quad.gameObject.SetActive(true);
        }
    }
}
