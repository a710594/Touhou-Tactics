using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFileEnemyObject : MonoBehaviour
{
    public int ID;
    public SpriteRenderer Sprite;

    public void Init(int id)
    {
        ID = id;
        EnemyModel data = DataContext.Instance.EnemyDic[ID];
        Sprite.sprite = Resources.Load<Sprite>("Image/" + data.Controller + "_F");
    }
}
