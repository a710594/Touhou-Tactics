using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFileEnemyObject : MonoBehaviour
{
    public int ID;
    public int Lv;
    public SpriteRenderer Sprite;

    public void Init(int id)
    {
        ID = id;
        EnemyModel data = DataTable.Instance.EnemyDic[ID];
        Sprite.sprite = Resources.Load<Sprite>("Image/" + data.Controller + "_F");
    }
}
