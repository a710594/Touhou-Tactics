using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQuickStarter : MonoBehaviour
{
    public enum ModeEnum 
    {
        Fixed,
        Random,
    }

    public ModeEnum CurrentMode;
    public int EnemyGroupId;
    public string Map;
    public string Tutorial;

    void Start()
    {
        DataContext.Instance.Init();
        CharacterManager.Instance.Init();

        if(CurrentMode == ModeEnum.Fixed) 
        {
            BattleController.Instance.Init(Tutorial, Map);
        }
        else
        {
            EnemyGroupModel enemyGroup = DataContext.Instance.EnemyGroupDic[EnemyGroupId];
            BattleController.Instance.Init("", enemyGroup);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
